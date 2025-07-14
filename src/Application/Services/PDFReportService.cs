
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using NPOI.SS.Formula.Functions;
using QuestPDF.Companion;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using QuestPDF.Previewer;
using Shared.Contracts.ReportRequest;
using Shared.DTOs.FormDtos;
namespace Application.Services;

public class PDFReportService
{
  private readonly ReportService reportService;
  private readonly SubSidaryDailyService _subsidaryDailyService;
  private readonly IWebHostEnvironment _webHostEnvironment;
  private readonly IConfiguration _config;



  public PDFReportService(ReportService reportService, SubSidaryDailyService subsidaryDailyService, IConfiguration config, IWebHostEnvironment webHostEnvironment)
  {
    this.reportService = reportService;
    this._subsidaryDailyService = subsidaryDailyService;
    this._webHostEnvironment = webHostEnvironment;

    this._config = config;
  }

  public async Task<byte[]> GenerateReport(GetAccountsBalanceBy request, CancellationToken cancellationToken)
  {
    var report = await reportService.GetFormDetailsReportAsync(request, cancellationToken);
    if (report == null)
    {
      return Array.Empty<byte>();
    }


    using var stream = new MemoryStream();

    var openingColor = Color.FromHex("#f0f0f0");
    var monthlyColor = Color.FromHex("#ffffffff");
    var closingColor = Color.FromHex("#fafafa");
    var accountColor = Color.FromHex("#e1e1e1");
    var monthlyCellColor2 = Color.FromARGB(200, 255, 255, 255); // Color.FromHex("#e1e1e1");


    Document.Create(Container =>
    {

      // Set Content Right To Left

      Container.Page(page =>
          {
            //page.Foreground().PaddingTop(10).PaddingBottom(10).PaddingRight(10).PaddingLeft(10).Border(3).BorderColor("#444444");
            page.Size(PageSizes.A4.Portrait());


            page.Margin(.5f, Unit.Centimetre);
            page.PageColor(Colors.White);
            page.DefaultTextStyle(x => x.FontSize(8).FontFamily("Arial"));
            page.Header().Column(c =>
            {
              //   c.Item().Row(r =>
              //  {
              //    var imagePath = Path.Combine(_webHostEnvironment.ContentRootPath, "Content", "images", "logo.png");
              //    r.AutoItem().AlignCenter().Width(8, Unit.Centimetre).Height(4, Unit.Centimetre).Image(imagePath);
              //  });

              c.Item().Column(c2 =>
              {
                c2.Item().AlignLeft().Text("تاريخ الطباعة : " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")).FontSize(8).FontFamily("Cairo");
                c2.Item().AlignRight().Text("جامعة الاسكندريه").FontSize(10).Bold().FontFamily("Cairo");
                c2.Item().AlignRight().Text("الوحدة الحسابيه المركزيه للمجمع الطبى").FontSize(10).Bold().Underline().FontFamily("Cairo");
                c2.Item().AlignCenter().Text("تقرير أرصدة الحسابات").FontSize(16).FontFamily("Cairo").Bold();
                c2.Item().AlignCenter().Text($"من : {request.StartDate:yyyy-MM-dd} إلى : {request.EndDate:yyyy-MM-dd}").FontSize(12).FontFamily("Cairo").Underline().Bold();
                c2.Item().AlignRight().Text($"الكلية : {report.CollageName}").FontSize(8).Bold().FontFamily("Cairo");
                c2.Item().AlignRight().Text($"الصندوق : {report.FundName}").FontSize(8).Bold().FontFamily("Cairo");
                c2.Item().AlignRight().Text($"نوع الحساب : {report.AccountType}").FontSize(8).Bold().FontFamily("Cairo");
              });



            });

            page.Content().Column(page2 =>
                {
                  var imagePath = Path.Combine(_webHostEnvironment.ContentRootPath, "Content", "images", "logo.png");
                  page.Background().AlignBottom().Image(imagePath);
                  //i need table with borders
                  page2.Item().Table(table =>
                      {

                        table.ColumnsDefinition(columns =>
                        {
                          columns.RelativeColumn(.5f);
                          columns.RelativeColumn(1);
                          columns.RelativeColumn(2);
                          columns.RelativeColumn(1); columns.RelativeColumn(1);
                        });
                        table.Header(header =>
                        {
                          //i need 2 2 hader rows

                          header.Cell().Row(1).Column(3).Element(cell => HeaderStyle(cell)).Text(" اسم الحساب ").FontSize(12).FontFamily("Cairo").ExtraBold().ExtraBlack();
                          header.Cell().Row(1).Column(1).Element(cell => HeaderStyle(cell)).Text("كود ").FontSize(12).FontFamily("Cairo").ExtraBold().ExtraBlack();
                          header.Cell().Row(1).Column(2).Element(cell => HeaderStyle(cell)).Text(" مدين ").FontSize(12).FontFamily("Cairo").ExtraBold().ExtraBlack();
                          header.Cell().Row(1).Column(4).Element(cell => HeaderStyle(cell)).Text(" دائن ").FontSize(12).FontFamily("Cairo").ExtraBold().ExtraBlack();
                          header.Cell().Row(1).Column(5).Element(cell => HeaderStyle(cell)).Text(" التوقيع ").FontSize(12).FontFamily("Cairo").ExtraBold().ExtraBlack();
                        });

                        report.ReportDetailsDtos.ForEach(x =>
                        {

                          table.Cell().Element(cell => MonthlyCellStyle(cell, monthlyCellColor2)).Scale(1.2f).Text(x.AccountId.ToString()).FontSize(8).Bold();
                          table.Cell().Element(cell => MonthlyCellStyle(cell, monthlyCellColor2)).Text(x.MonthlyTransAction?.Debit.ToString() ?? "0").FontSize(8).Bold();
                          table.Cell().Element(cell => MonthlyCellStyle(cell, monthlyCellColor2)).Scale(1.2f).Text(x.AccountName).FontFamily("Cairo").FontSize(8).Bold();
                          table.Cell().Element(cell => MonthlyCellStyle(cell, monthlyCellColor2)).Text(x.MonthlyTransAction?.Credit.ToString() ?? "0").FontSize(8).Bold();
                          table.Cell().Element(cell => MonthlyCellStyle(cell, monthlyCellColor2)).Text(string.Empty).FontSize(8).Bold();
                        });



                        table.Footer(footer =>
                        {

                          footer.Cell().Element(cell => FooterCellStyle(cell, accountColor)).Text(string.Empty).ExtraBold().ExtraBlack().FontSize(10);
                          footer.Cell().Element(cell => FooterCellStyle(cell, accountColor)).Text(report.ReportDetailsDtos.Sum(x => x.MonthlyTransAction?.Debit ?? 0).ToString()).ExtraBold().ExtraBlack().FontSize(10);
                          footer.Cell().Element(cell => FooterCellStyle(cell, accountColor)).Text("المجموع").ExtraBold().ExtraBlack().FontFamily("Cairo").FontSize(10);
                          footer.Cell().Element(cell => FooterCellStyle(cell, accountColor)).Text(report.ReportDetailsDtos.Sum(x => x.MonthlyTransAction?.Credit ?? 0).ToString()).ExtraBold().ExtraBlack().FontSize(10);
                          footer.Cell().Element(cell => FooterCellStyle(cell, accountColor)).Text(string.Empty).ExtraBold().ExtraBlack().FontSize(10);

                        });



                        static IContainer HeaderStyle(IContainer container)
                          => container.Border(1).Background(Color.FromHex("#e1e1e1")).PaddingHorizontal(1).PaddingVertical(4).AlignCenter().AlignMiddle();


                        static IContainer MainHeaderStyle(IContainer container, Color color)
                          => container.Border(0).Background(color).Width(100).PaddingHorizontal(1).PaddingVertical(1).AlignCenter().AlignMiddle();

                        static IContainer FooterCellStyle(IContainer container, Color color)
                          => container.Border(1).Background(color).PaddingHorizontal(1).PaddingVertical(4).AlignCenter().AlignMiddle();

                        // static IContainer OpeningCellStyle(IContainer container, Color color)
                        //     => container.Border(1).Background(color).PaddingHorizontal(1).PaddingVertical(1).AlignCenter().AlignMiddle();


                        static IContainer MonthlyCellStyle(IContainer container, Color color)
                          => container.Border(1).Background(color).PaddingHorizontal(1).PaddingVertical(3).AlignCenter().AlignMiddle();


                        // static IContainer ClosingCellStyle(IContainer container, Color color)
                        //     => container.Border(1).Background(color).PaddingHorizontal(1).PaddingVertical(1).AlignCenter().AlignMiddle();

                        static IContainer AccountCellStyle(IContainer container, Color color)
                          => container.Border(1).Background(color).PaddingHorizontal(1).PaddingVertical(1).AlignCenter().AlignMiddle();

                        page.Footer().AlignCenter().Text(x =>
                        {
                          x.CurrentPageNumber();
                          x.Span(" / ");
                          x.TotalPages();
                        });
                      });
                });
          });




    }).GeneratePdf(stream);
    return stream.ToArray();
  }


  public async Task<byte[]> GenerateSubsidaryReport(GetSubsidartDailyRequest request, CancellationToken cancellationToken)
  {
    var report = await _subsidaryDailyService.GetSubsidaryDaily(request);
    if (report == null)
    {
      return Array.Empty<byte>();
    }


    using var stream = new MemoryStream();

    var openingColor = Color.FromHex("#f0f0f0");
    var monthlyColor = Color.FromHex("#d1d1d1");
    var closingColor = Color.FromHex("#fafafa");
    var accountColor = Color.FromHex("#fff");
    var totalColor = Color.FromHex("#a9a9a9");
    var GrandTotalColor = Color.FromHex("#708090");
    var subtotalColor = Color.FromHex("#d3d3d3");


    Document.Create(Container =>
    {

      // Set Content Right To Left

      Container.Page(page =>
          {

            page.ContentFromRightToLeft();

            page.Size(PageSizes.A4.Landscape());
            // page.orientation(Orientation.Portrait);


            //page.Content().Image(_config["ApiContent"] + "images/logo.png"); ;
            page.Margin(.5f, Unit.Centimetre);
            page.PageColor(Colors.White);
            page.DefaultTextStyle(x => x.FontSize(8).FontFamily("Arial"));

            page.Header().Column(c =>
                {
                  c.Item().AlignLeft().Text("تاريخ الطباعة : " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")).FontSize(10);
                  c.Item().AlignRight().Text("جامعة الاسكندريه").FontSize(12).Bold();

                  c.Item().AlignRight().Text("الوحدة الحسابيه المركزيه للمجمع الطبى").FontSize(12).Bold().Underline();


                  c.Item().AlignCenter().Text(report.Daily).FontSize(16).Bold();
                  c.Item().AlignCenter().Text($"من : {request.StartDate:yyyy-MM-dd} إلى : {request.EndDate:yyyy-MM-dd}").FontSize(12).Underline().Bold();
                  c.Item().AlignRight().Text($"الكلية : {report.CollageName}").FontSize(12).Bold();
                  c.Item().AlignRight().Text($"الصندوق : {report.FundName}").FontSize(12).Bold();
                  c.Item().AlignRight().Text($"اسم الحساب : {report.AccountName}").FontSize(12).Bold();
                  c.Item().AlignRight().Text($"نوع اليوميه : {report.AccountType}").FontSize(12).Bold();


                });


            page.Content().Table(table =>
                {
                  //i need table with borders



                  table.ColumnsDefinition(columns =>
                    {

                      columns.RelativeColumn(1);
                      columns.RelativeColumn(1);
                      columns.RelativeColumn(1);
                      columns.RelativeColumn(1.2f);
                      columns.RelativeColumn(1);


                    });
                  table.Header(header =>
                    {
                      //i need 2 2 hader rows

                      header.Cell().Row(1).Column(1).AlignCenter().Scale(1.4f).Element(cell => MainHeaderStyle(cell, Colors.Transparent)).Text("الكليه").Bold().FontSize(10);
                      header.Cell().Row(1).Column(2).AlignCenter().Scale(1.4f).Element(cell => MainHeaderStyle(cell, Colors.Transparent)).Text("الصندوق").Bold().FontSize(10);
                      header.Cell().Row(1).Column(3).AlignCenter().Scale(1.4f).Element(cell => MainHeaderStyle(cell, Colors.Transparent)).Text("مدين").Bold().FontSize(10);
                      header.Cell().Row(1).Column(4).AlignCenter().Scale(1.4f).Element(cell => MainHeaderStyle(cell, Colors.Transparent)).Text("البيان").Bold().FontSize(10);
                      header.Cell().Row(1).Column(5).AlignCenter().Scale(1.4f).Element(cell => MainHeaderStyle(cell, Colors.Transparent)).Text("دائن").Bold().FontSize(10);

                    });

                  report.Collages.ForEach(c =>
                    {
                      c.Funds.ForEach(f =>
                      {

                        f.SubsidaryDetails.ForEach(s =>
                        {
                          table.Cell().Element(cell => AccountCellStyle(cell, accountColor)).Scale(1.2f).Text(c.CollageName.ToString());
                          table.Cell().Element(cell => AccountCellStyle(cell, accountColor)).Scale(1.2f).Text(f.FundName?.ToString() ?? string.Empty);
                          table.Cell().Element(cell => AccountCellStyle(cell, accountColor)).Scale(1.2f).Text(s.Debit.ToString());
                          table.Cell().Element(cell => AccountCellStyle(cell, accountColor)).Scale(1.2f).Text(s.SubsidaryName?.ToString() ?? string.Empty);
                          table.Cell().Element(cell => AccountCellStyle(cell, accountColor)).Scale(1.2f).Text(s.Credit.ToString());
                        });
                        table.Cell().Element(cell => subtotalStyle(cell, subtotalColor)).Scale(1.2f).Text(c.CollageName.ToString()).SemiBold();
                        table.Cell().Element(cell => subtotalStyle(cell, subtotalColor)).Scale(1.2f).Text(string.Empty);
                        table.Cell().Element(cell => subtotalStyle(cell, subtotalColor)).Scale(1.2f).Text(f.SubsidaryDetails.Sum(t => t.Debit).ToString()).SemiBold();
                        table.Cell().Element(cell => subtotalStyle(cell, subtotalColor)).Scale(1.2f).Text(" أجمالى " + f.FundName).SemiBold();
                        table.Cell().Element(cell => subtotalStyle(cell, subtotalColor)).Scale(1.2f).Text(f.SubsidaryDetails.Sum(t => t.Credit).ToString()).SemiBold();

                      });


                    });
                  table.Cell().Element(cell => totalStyle(cell, GrandTotalColor)).Scale(1.2f).Text(string.Empty);
                  table.Cell().Element(cell => totalStyle(cell, GrandTotalColor)).Scale(1.2f).Text(string.Empty);
                  table.Cell().Element(cell => totalStyle(cell, GrandTotalColor)).Scale(1.2f).Text(report.Collages.Sum(f => f.Funds.Sum(a => a.SubsidaryDetails.Sum(j => j.Debit))).ToString()).FontColor(Colors.White).ExtraBold(); // report.TotalDebit.ToString().Funds.Sum(a => a.SubsidaryDetails.Sum(j => j.Debit)).ToString());
                  table.Cell().Element(cell => totalStyle(cell, GrandTotalColor)).Scale(1.2f).Text(" أجمالى كلى").FontColor(Colors.White).ExtraBold();
                  table.Cell().Element(cell => totalStyle(cell, GrandTotalColor)).Scale(1.2f).Text(report.Collages.Sum(f => f.Funds.Sum(a => a.SubsidaryDetails.Sum(j => j.Credit))).ToString()).FontColor(Colors.White).ExtraBold();

                  table.Footer(footer =>
                    {
                      //  report.TotalSubsidaries = report.TotalSubsidaries.OrderBy(x => x.SubsidaryNumber).ToList();



                      report.TotalSubsidaries.ForEach(total =>
                      {
                        footer.Cell().Element(cell => AccountCellStyle(cell, closingColor)).Scale(1.2f).Text(string.Empty);
                        footer.Cell().Element(cell => AccountCellStyle(cell, closingColor)).Scale(1.2f).Text(total.SubsidaryNumber?.ToString() ?? string.Empty);
                        footer.Cell().Element(cell => AccountCellStyle(cell, closingColor)).Scale(1.2f).Text(total.Debit.ToString());
                        footer.Cell().Element(cell => AccountCellStyle(cell, closingColor)).Scale(1.2f).Text("أجماليات " + total.SubsidaryName);
                        footer.Cell().Element(cell => AccountCellStyle(cell, closingColor)).Scale(1.2f).Text(total.Credit.ToString());

                      });
                      footer.Cell().Element(cell => AccountCellStyle(cell, monthlyColor)).Scale(1.2f).Text(string.Empty);
                      footer.Cell().Element(cell => AccountCellStyle(cell, monthlyColor)).Scale(1.2f).Text(string.Empty);
                      footer.Cell().Element(cell => AccountCellStyle(cell, monthlyColor)).Scale(1.2f).Text(report.TotalSubsidaries.Sum(x => x.Debit).ToString());
                      footer.Cell().Element(cell => AccountCellStyle(cell, monthlyColor)).Scale(1.2f).Text("أجمالى كلى  ");
                      footer.Cell().Element(cell => AccountCellStyle(cell, monthlyColor)).Scale(1.2f).Text(report.TotalSubsidaries.Sum(x => x.Credit).ToString());
                    });

                  // static IContainer CellStyle(IContainer container, Color color)
                  //   => container.Border(1).Background(color).PaddingHorizontal(1).PaddingVertical(2).AlignCenter().AlignMiddle();


                  // static IContainer HeaderStyle(IContainer container, Color color)
                  //     => container.Border(1).Background(color).PaddingHorizontal(1).PaddingVertical(3).AlignCenter().AlignMiddle();


                  static IContainer MainHeaderStyle(IContainer container, Color color)
                      => container.Border(0).Background(color).Width(100).PaddingHorizontal(1).PaddingVertical(1).AlignCenter().AlignMiddle();

                  // static IContainer FooterCellStyle(IContainer container, Color color)
                  //     => container.Border(1).Background(color).PaddingHorizontal(1).PaddingVertical(2).AlignCenter().AlignMiddle();

                  // static IContainer OpeningCellStyle(IContainer container, Color color)
                  //     => container.Border(1).Background(color).PaddingHorizontal(1).PaddingVertical(1).AlignCenter().AlignMiddle();


                  static IContainer subtotalStyle(IContainer container, Color color)
                      => container.Height(20).Border(1.2f).Background(color).PaddingHorizontal(1).PaddingVertical(1).AlignCenter().AlignMiddle();


                  static IContainer totalStyle(IContainer container, Color color)
                      => container.Height(25).Border(3).Background(Color.FromHex(color)).PaddingHorizontal(1).PaddingVertical(1).AlignCenter().AlignMiddle();

                  static IContainer AccountCellStyle(IContainer container, Color color)
                      => container.Border(1).Background(color).PaddingHorizontal(1).PaddingVertical(1).AlignCenter().AlignMiddle();

                  page.Footer().AlignCenter().Text(x =>
                    {
                      x.CurrentPageNumber();
                      x.Span(" / ");
                      x.TotalPages();
                    });
                });
          })

              ;
    }).GeneratePdf(stream);
    return stream.ToArray();
  }

}