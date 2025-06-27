
using System.Threading.Tasks;
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
  private readonly IConfiguration _config;

  public PDFReportService(ReportService reportService, SubSidaryDailyService subsidaryDailyService, IConfiguration config)
  {
    this.reportService = reportService;
    this._subsidaryDailyService = subsidaryDailyService;

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
    var monthlyColor = Color.FromHex("#d1d1d1");
    var closingColor = Color.FromHex("#fafafa");
    var accountColor = Color.FromHex("#e1e1e1");


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


                  c.Item().AlignCenter().Text("تقرير أرصدة الحسابات").FontSize(16).Bold();
                  c.Item().AlignCenter().Text($"من : {request.StartDate:yyyy-MM-dd} إلى : {request.EndDate:yyyy-MM-dd}").FontSize(12).Underline().Bold();
                  c.Item().AlignRight().Text($"الكلية : {report.CollageName}").FontSize(12).Bold();
                  c.Item().AlignRight().Text($"الصندوق : {report.FundName}").FontSize(12).Bold();
                  c.Item().AlignRight().Text($"نوع الحساب : {report.AccountType}").FontSize(12).Bold();

                });


            page.Content().Table(table =>
                {
                  //i need table with borders



                  table.ColumnsDefinition(columns =>
                    {
                      columns.RelativeColumn(.5f);
                      columns.RelativeColumn(1.5f);

                      columns.RelativeColumn(1);
                      columns.RelativeColumn(1);
                      columns.RelativeColumn(1);
                      columns.RelativeColumn(1);
                      columns.RelativeColumn(1);
                      columns.RelativeColumn(1);
                      columns.RelativeColumn(1);
                      columns.RelativeColumn(1);
                      columns.RelativeColumn(1);


                    });
                  table.Header(header =>
                    {
                      //i need 2 2 hader rows


                      header.Cell().Row(1).Column(3).ColumnSpan(3).AlignCenter().Scale(1.4f).Element(cell => MainHeaderStyle(cell, Colors.Transparent)).Text("رصيد افتتاحى").Bold().FontSize(10);
                      header.Cell().Row(1).Column(6).ColumnSpan(3).AlignCenter().Scale(1.4f).Element(cell => MainHeaderStyle(cell, Colors.Transparent)).Text("عمليات الشهر").Bold().FontSize(10);
                      header.Cell().Row(1).Column(9).ColumnSpan(3).AlignCenter().Scale(1.4f).Element(cell => MainHeaderStyle(cell, Colors.Transparent)).Text("أخر الفترة").Bold().FontSize(10);






                      header.Cell().Row(2).Column(2).Element(cell => AccountCellStyle(cell, accountColor)).Text("الحساب ").ExtraBold();
                      header.Cell().Row(2).Column(1).Element(cell => AccountCellStyle(cell, accountColor)).Text("كود ").ExtraBold();


                      header.Cell().Column(3).Row(2).Element(cell => HeaderStyle(cell, openingColor)).Text(" مدين").ExtraBold();
                      header.Cell().Row(2).Column(3 + 1).Element(cell => HeaderStyle(cell, openingColor)).Text(" دائن").ExtraBold();
                      header.Cell().Row(2).Column(4 + 1).Element(cell => HeaderStyle(cell, openingColor)).Text(" رصيد").ExtraBold();

                      header.Cell().Row(2).Column(5 + 1).Element(cell => HeaderStyle(cell, monthlyColor)).Text(" مدين ").ExtraBold();
                      header.Cell().Row(2).Column(6 + 1).Element(cell => HeaderStyle(cell, monthlyColor)).Text(" دائن ").ExtraBold();
                      header.Cell().Row(2).Column(7 + 1).Element(cell => HeaderStyle(cell, monthlyColor)).Text(" رصيد ").ExtraBold();




                      header.Cell().Row(2).Column(8 + 1).Element(cell => HeaderStyle(cell, closingColor)).Text(" مدين").ExtraBold();
                      header.Cell().Row(2).Column(9 + 1).Element(cell => HeaderStyle(cell, closingColor)).Text(" دائن").ExtraBold();
                      header.Cell().Row(2).Column(10 + 1).Element(cell => HeaderStyle(cell, closingColor)).Text(" رصيد").ExtraBold();
                    });

                  report.ReportDetailsDtos.ForEach(x =>
                    {

                      table.Cell().Element(cell => AccountCellStyle(cell, accountColor)).Scale(1.2f).Text(x.AccountNumber);
                      table.Cell().Element(cell => AccountCellStyle(cell, accountColor)).Scale(1.2f).Text(x.AccountName);
                      table.Cell().Element(cell => OpeningCellStyle(cell, openingColor)).Text(x.OpeningBalance.Debit.ToString());
                      table.Cell().Element(cell => OpeningCellStyle(cell, openingColor)).Text(x.OpeningBalance.Credit.ToString());
                      table.Cell().Element(cell => OpeningCellStyle(cell, openingColor)).Text(x.OpeningBalance.Balance.ToString());
                      table.Cell().Element(cell => MonthlyCellStyle(cell, monthlyColor)).Text(x.MonthlyTransAction.Debit.ToString());
                      table.Cell().Element(cell => MonthlyCellStyle(cell, monthlyColor)).Text(x.MonthlyTransAction.Credit.ToString());
                      table.Cell().Element(cell => MonthlyCellStyle(cell, monthlyColor)).Text(x.MonthlyTransAction?.Balance.ToString() ?? "0");
                      table.Cell().Element(cell => ClosingCellStyle(cell, closingColor)).Text(x.ClosingBalance?.Debit.ToString() ?? "0");
                      table.Cell().Element(cell => ClosingCellStyle(cell, closingColor)).Text(x.ClosingBalance?.Credit.ToString() ?? "0");
                      table.Cell().Element(cell => ClosingCellStyle(cell, closingColor)).Text(x.ClosingBalance?.Balance.ToString() ?? "0");



                      //table.Cell().Text(x.ClosingBalance?.Balance.ToString() ?? "0");
                    });
                  table.Footer(footer =>
                    {
                      footer.Cell().ColumnSpan(2).Element(cell => FooterCellStyle(cell, accountColor)).Text("المجموع").Bold().FontSize(10);
                      footer.Cell().Element(cell => FooterCellStyle(cell, openingColor)).Text(report.ReportDetailsDtos.Sum(x => x.OpeningBalance.Debit).ToString()).Bold().FontSize(10);
                      footer.Cell().Element(cell => FooterCellStyle(cell, openingColor)).Text(report.ReportDetailsDtos.Sum(x => x.OpeningBalance.Credit).ToString()).Bold().FontSize(10);
                      footer.Cell().Element(cell => FooterCellStyle(cell, openingColor)).Text(report.ReportDetailsDtos.Sum(x => x.OpeningBalance.Balance).ToString()).Bold().FontSize(10);
                      footer.Cell().Element(cell => FooterCellStyle(cell, monthlyColor)).Text(report.ReportDetailsDtos.Sum(x => x.MonthlyTransAction.Debit).ToString()).Bold().FontSize(10);
                      footer.Cell().Element(cell => FooterCellStyle(cell, monthlyColor)).Text(report.ReportDetailsDtos.Sum(x => x.MonthlyTransAction.Credit).ToString()).Bold().FontSize(10);
                      footer.Cell().Element(cell => FooterCellStyle(cell, monthlyColor)).Text(report.ReportDetailsDtos.Sum(x => x.MonthlyTransAction?.Balance ?? 0).ToString()).Bold().FontSize(10);
                      footer.Cell().Element(cell => FooterCellStyle(cell, closingColor)).Text(report.ReportDetailsDtos.Sum(x => x.ClosingBalance?.Debit ?? 0).ToString()).Bold().FontSize(10);
                      footer.Cell().Element(cell => FooterCellStyle(cell, closingColor)).Text(report.ReportDetailsDtos.Sum(x => x.ClosingBalance?.Credit ?? 0).ToString()).Bold().FontSize(10);
                      footer.Cell().Element(cell => FooterCellStyle(cell, closingColor)).Text(report.ReportDetailsDtos.Sum(x => x.ClosingBalance?.Balance ?? 0).ToString()).Bold().FontSize(10);

                    });

                  static IContainer CellStyle(IContainer container, Color color)
                    => container.Border(1).Background(color).PaddingHorizontal(1).PaddingVertical(2).AlignCenter().AlignMiddle();


                  static IContainer HeaderStyle(IContainer container, Color color)
                      => container.Border(1).Background(color).PaddingHorizontal(1).PaddingVertical(3).AlignCenter().AlignMiddle();


                  static IContainer MainHeaderStyle(IContainer container, Color color)
                      => container.Border(0).Background(color).Width(100).PaddingHorizontal(1).PaddingVertical(1).AlignCenter().AlignMiddle();

                  static IContainer FooterCellStyle(IContainer container, Color color)
                      => container.Border(1).Background(color).PaddingHorizontal(1).PaddingVertical(2).AlignCenter().AlignMiddle();

                  static IContainer OpeningCellStyle(IContainer container, Color color)
                      => container.Border(1).Background(color).PaddingHorizontal(1).PaddingVertical(1).AlignCenter().AlignMiddle();


                  static IContainer MonthlyCellStyle(IContainer container, Color color)
                      => container.Border(1).Background(color).PaddingHorizontal(1).PaddingVertical(1).AlignCenter().AlignMiddle();


                  static IContainer ClosingCellStyle(IContainer container, Color color)
                      => container.Border(1).Background(color).PaddingHorizontal(1).PaddingVertical(1).AlignCenter().AlignMiddle();

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
                          table.Cell().Element(cell => AccountCellStyle(cell, accountColor)).Scale(1.2f).Text(f.FundName.ToString());
                          table.Cell().Element(cell => AccountCellStyle(cell, accountColor)).Scale(1.2f).Text(s.Debit.ToString());
                          table.Cell().Element(cell => AccountCellStyle(cell, accountColor)).Scale(1.2f).Text(s.SubsidaryName.ToString());
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
                        footer.Cell().Element(cell => AccountCellStyle(cell, closingColor)).Scale(1.2f).Text(total.SubsidaryNumber.ToString());
                        footer.Cell().Element(cell => AccountCellStyle(cell, closingColor)).Scale(1.2f).Text(total.Debit.ToString());
                        footer.Cell().Element(cell => AccountCellStyle(cell, closingColor)).Scale(1.2f).Text("أجماليات " + total.SubsidaryName);
                        footer.Cell().Element(cell => AccountCellStyle(cell, closingColor)).Scale(1.2f).Text(total.Credit.ToString());

                      });
                      footer.Cell().Element(cell => AccountCellStyle(cell, monthlyColor)).Scale(1.2f).Text(string.Empty);
                      footer.Cell().Element(cell => AccountCellStyle(cell, monthlyColor)).Scale(1.2f).Text(string.Empty);
                      footer.Cell().Element(cell => AccountCellStyle(cell, monthlyColor)).Scale(1.2f).Text(report.TotalSubsidaries.Sum(x => x.Debit));
                      footer.Cell().Element(cell => AccountCellStyle(cell, monthlyColor)).Scale(1.2f).Text("أجمالى كلى  ");
                      footer.Cell().Element(cell => AccountCellStyle(cell, monthlyColor)).Scale(1.2f).Text(report.TotalSubsidaries.Sum(x => x.Credit));
                    });

                  static IContainer CellStyle(IContainer container, Color color)
                    => container.Border(1).Background(color).PaddingHorizontal(1).PaddingVertical(2).AlignCenter().AlignMiddle();


                  static IContainer HeaderStyle(IContainer container, Color color)
                      => container.Border(1).Background(color).PaddingHorizontal(1).PaddingVertical(3).AlignCenter().AlignMiddle();


                  static IContainer MainHeaderStyle(IContainer container, Color color)
                      => container.Border(0).Background(color).Width(100).PaddingHorizontal(1).PaddingVertical(1).AlignCenter().AlignMiddle();

                  static IContainer FooterCellStyle(IContainer container, Color color)
                      => container.Border(1).Background(color).PaddingHorizontal(1).PaddingVertical(2).AlignCenter().AlignMiddle();

                  static IContainer OpeningCellStyle(IContainer container, Color color)
                      => container.Border(1).Background(color).PaddingHorizontal(1).PaddingVertical(1).AlignCenter().AlignMiddle();


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