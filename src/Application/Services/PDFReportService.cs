
using System.Threading.Tasks;
using Application.Services.PDFProviders;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NPOI.SS.Formula.Functions;
using QuestPDF.Companion;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using QuestPDF.Previewer;
using Shared.Contracts.ReportRequest;
using Shared.DTOs.FormDtos;
using Shared.DTOs.ReportDtos;
namespace Application.Services;

public partial class PDFReportService
{
  private readonly ReportService reportService;
  private readonly SubSidaryDailyService _subsidaryDailyService;
  private readonly IWebHostEnvironment _webHostEnvironment;
  private readonly ILogger<PDFReportService> _logger;
  private readonly IConfiguration _config;



  public PDFReportService(ReportService reportService, SubSidaryDailyService subsidaryDailyService, IConfiguration config, IWebHostEnvironment webHostEnvironment, ILogger<PDFReportService> logger)
  {
    this.reportService = reportService;
    this._subsidaryDailyService = subsidaryDailyService;
    this._webHostEnvironment = webHostEnvironment;
    this._logger = logger;
    this._config = config;
  }


  public async Task<byte[]> GenerateReport(GetAccountsBalanceBy request, CancellationToken cancellationToken)
  {
    // Input validation
    if (request == null)
      throw new ArgumentNullException(nameof(request));

    if (request.StartDate > request.EndDate)
      throw new ArgumentException("Start date cannot be greater than end date");

    try
    {
      var report = await reportService.GetFormDetailsReportAsync(request, cancellationToken);
      if (report == null || !report.ReportDetailsDtos.Any())
      {
        return Array.Empty<byte>();
      }
      var imagePath = Path.Combine(_webHostEnvironment.ContentRootPath, "Content", "images", "logo.png");
      using var stream = new MemoryStream();

      // Document.Create(Container =>
      // {
      //   Container.Page(page =>
      //     {
      //       // page.Size(PageSizes.A4.Portrait());
      //       // page.ContentFromRightToLeft();
      //       // page.Margin(.5f, Unit.Centimetre);
      //       // page.PageColor(Colors.White);
      //       // page.DefaultTextStyle(x => x.FontSize(ReportConstants.DataFontSize).FontFamily(ReportConstants.EnglishFont));

      //       page.Header().Column(c =>
      //         {
      //           c.Item().Column(c2 =>
      //             {
      //               c2.Item().AlignLeft().Text("تاريخ الطباعة : " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))
      //                     .FontSize(ReportConstants.DataFontSize).FontFamily(ReportConstants.ArabicFont);
      //               c2.Item().AlignRight().Text("جامعة الاسكندريه")
      //                     .FontSize(10).Bold().FontFamily(ReportConstants.ArabicFont);
      //               c2.Item().AlignRight().Text("الوحدة الحسابيه المركزيه للمجمع الطبى")
      //                     .FontSize(10).Bold().Underline().FontFamily(ReportConstants.ArabicFont);
      //               c2.Item().AlignCenter().Text("تقرير أرصدة الحسابات")
      //                     .FontSize(ReportConstants.TitleFontSize).FontFamily(ReportConstants.ArabicFont).Bold();
      //               c2.Item().AlignCenter().Text($"من : {request.StartDate:yyyy-MM-dd} إلى : {request.EndDate:yyyy-MM-dd}")
      //                     .FontSize(ReportConstants.SubtitleFontSize).FontFamily(ReportConstants.ArabicFont).Underline().Bold();
      //               c2.Item().AlignRight().Text($"الكلية : {report.CollageName}")
      //                     .FontSize(ReportConstants.DataFontSize).Bold().FontFamily(ReportConstants.ArabicFont);
      //               c2.Item().AlignRight().Text($"الصندوق : {report.FundName}")
      //                     .FontSize(ReportConstants.DataFontSize).Bold().FontFamily(ReportConstants.ArabicFont);
      //               c2.Item().AlignRight().Text($"نوع الحساب : {report.AccountType}")
      //                     .FontSize(ReportConstants.DataFontSize).Bold().FontFamily(ReportConstants.ArabicFont);
      //             });
      //         });

      //       page.Content().Column(page2 =>
      //         {
      //           var imagePath = Path.Combine(_webHostEnvironment.ContentRootPath, "Content", "images", "logo.png");
      //           page.Background().AlignBottom().Image(imagePath);

      //           page2.Item().Table(table =>
      //             {
      //               table.ColumnsDefinition(columns =>
      //                 {
      //                   columns.RelativeColumn(.5f);  // كود
      //                   columns.RelativeColumn(2);    // اسم الحساب
      //                   columns.RelativeColumn(1);    // مدين
      //                   columns.RelativeColumn(1);    // دائن
      //                   columns.RelativeColumn(1);    // التوقيع
      //                 });

      //               table.Header(header =>
      //                 {
      //                   header.Cell().Row(1).Column(1).Element(cell => CellStyles.HeaderCellStyle(cell))
      //                         .Text("كود").FontSize(ReportConstants.HeaderFontSize).FontFamily(ReportConstants.ArabicFont).ExtraBold().ExtraBlack();
      //                   header.Cell().Row(1).Column(2).Element(cell => CellStyles.HeaderCellStyle(cell))
      //                         .Text("اسم الحساب").FontSize(ReportConstants.HeaderFontSize).FontFamily(ReportConstants.ArabicFont).ExtraBold().ExtraBlack();
      //                   header.Cell().Row(1).Column(3).Element(cell => CellStyles.HeaderCellStyle(cell))
      //                         .Text("مدين").FontSize(ReportConstants.HeaderFontSize).FontFamily(ReportConstants.ArabicFont).ExtraBold().ExtraBlack();
      //                   header.Cell().Row(1).Column(4).Element(cell => CellStyles.HeaderCellStyle(cell))
      //                         .Text("دائن").FontSize(ReportConstants.HeaderFontSize).FontFamily(ReportConstants.ArabicFont).ExtraBold().ExtraBlack();
      //                   header.Cell().Row(1).Column(5).Element(cell => CellStyles.HeaderCellStyle(cell))
      //                         .Text("التوقيع").FontSize(ReportConstants.HeaderFontSize).FontFamily(ReportConstants.ArabicFont).ExtraBold().ExtraBlack();
      //                 });

      //               report.ReportDetailsDtos.ForEach(x =>
      //                 {
      //                   var debitAmount = x.MonthlyTransAction?.Debit?.ToString("N2") ?? "0.00";
      //                   var creditAmount = x.MonthlyTransAction?.Credit?.ToString("N2") ?? "0.00";

      //                   table.Cell().Element(cell => CellStyles.DataCellStyle(cell))
      //                         .Text(x.AccountId.ToString()).FontSize(ReportConstants.DataFontSize).Bold();
      //                   table.Cell().Element(cell => CellStyles.DataCellStyle(cell))
      //                         .Text(x.AccountName ?? string.Empty).FontFamily(ReportConstants.ArabicFont).FontSize(ReportConstants.DataFontSize).Bold();
      //                   table.Cell().Element(cell => CellStyles.DataCellStyle(cell))
      //                         .Text(debitAmount).FontSize(ReportConstants.DataFontSize).Bold();
      //                   table.Cell().Element(cell => CellStyles.DataCellStyle(cell))
      //                         .Text(creditAmount).FontSize(ReportConstants.DataFontSize).Bold();
      //                   table.Cell().Element(cell => CellStyles.DataCellStyle(cell))
      //                         .Text(string.Empty).FontSize(ReportConstants.DataFontSize).Bold();
      //                 });

      //               table.Footer(footer =>
      //                 {
      //                   var totalDebit = report.ReportDetailsDtos.Sum(x => x.MonthlyTransAction?.Debit ?? 0);
      //                   var totalCredit = report.ReportDetailsDtos.Sum(x => x.MonthlyTransAction?.Credit ?? 0);

      //                   footer.Cell().Element(cell => CellStyles.FooterCellStyle(cell))
      //                         .Text(string.Empty).ExtraBold().ExtraBlack().FontSize(10);
      //                   footer.Cell().Element(cell => CellStyles.FooterCellStyle(cell))
      //                         .Text("المجموع").ExtraBold().ExtraBlack().FontFamily(ReportConstants.ArabicFont).FontSize(10);
      //                   footer.Cell().Element(cell => CellStyles.FooterCellStyle(cell))
      //                         .Text(totalDebit.ToString("N2")).ExtraBold().ExtraBlack().FontSize(10);
      //                   footer.Cell().Element(cell => CellStyles.FooterCellStyle(cell))
      //                         .Text(totalCredit.ToString("N2")).ExtraBold().ExtraBlack().FontSize(10);
      //                   footer.Cell().Element(cell => CellStyles.FooterCellStyle(cell))
      //                         .Text(string.Empty).ExtraBold().ExtraBlack().FontSize(10);
      //                 });
      //             });
      //         });

      //       page.Footer().AlignCenter().Text(x =>
      //         {
      //           x.CurrentPageNumber();
      //           x.Span(" / ");
      //           x.TotalPages();
      //         });
      //     });
      // }).GeneratePdf(stream);

      //(report, request, imagePath);
      var document = CreateReportDocument(report, request, imagePath);
      document.GeneratePdf(stream);

      return stream.ToArray();

    }
    catch (Exception ex)
    {
      // Log the exception if you have a logger
      // _logger.LogError(ex, "Error generating account balance report");
      return Array.Empty<byte>();
    }
  }


  // Styling methods - add these as static methods in your class

  // public async Task<byte[]> GenerateReport(GetAccountsBalanceBy request, CancellationToken cancellationToken)
  // {
  //   var report = await reportService.GetFormDetailsReportAsync(request, cancellationToken);
  //   if (report == null)
  //   {
  //     return Array.Empty<byte>();
  //   }


  //   using var stream = new MemoryStream();

  //   var openingColor = Color.FromHex("#f0f0f0");
  //   var monthlyColor = Color.FromHex("#ffffffff");
  //   var closingColor = Color.FromHex("#fafafa");
  //   var accountColor = Color.FromHex("#e1e1e1");
  //   var monthlyCellColor2 = Color.FromARGB(200, 255, 255, 255); // Color.FromHex("#e1e1e1");


  //   Document.Create(Container =>
  //   {

  //     // Set Content Right To Left

  //     Container.Page(page =>
  //         {
  //           //page.Foreground().PaddingTop(10).PaddingBottom(10).PaddingRight(10).PaddingLeft(10).Border(3).BorderColor("#444444");
  //           page.Size(PageSizes.A4.Portrait());
  //           page.ContentFromRightToLeft();


  //           page.Margin(.5f, Unit.Centimetre);
  //           page.PageColor(Colors.White);
  //           page.DefaultTextStyle(x => x.FontSize(8).FontFamily("Arial"));
  //           page.Header().Column(c =>
  //           {


  //             c.Item().Column(c2 =>
  //             {
  //               c2.Item().AlignLeft().Text("تاريخ الطباعة : " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")).FontSize(8).FontFamily("Cairo");
  //               c2.Item().AlignRight().Text("جامعة الاسكندريه").FontSize(10).Bold().FontFamily("Cairo");
  //               c2.Item().AlignRight().Text("الوحدة الحسابيه المركزيه للمجمع الطبى").FontSize(10).Bold().Underline().FontFamily("Cairo");
  //               // c2.Item().AlignCenter().Row(r =>
  //               // {
  //               //   var imagePath = Path.Combine(_webHostEnvironment.ContentRootPath, "Content", "images", "logo.png");
  //               //   r.AutoItem().AlignMiddle().AlignRight().Width(6, Unit.Centimetre).Height(3, Unit.Centimetre).Image(imagePath);
  //               // });
  //               c2.Item().AlignCenter().Text("تقرير أرصدة الحسابات").FontSize(16).FontFamily("Cairo").Bold();
  //               c2.Item().AlignCenter().Text($"من : {request.StartDate:yyyy-MM-dd} إلى : {request.EndDate:yyyy-MM-dd}").FontSize(12).FontFamily("Cairo").Underline().Bold();
  //               c2.Item().AlignRight().Text($"الكلية : {report.CollageName}").FontSize(8).Bold().FontFamily("Cairo");
  //               c2.Item().AlignRight().Text($"الصندوق : {report.FundName}").FontSize(8).Bold().FontFamily("Cairo");
  //               c2.Item().AlignRight().Text($"نوع الحساب : {report.AccountType}").FontSize(8).Bold().FontFamily("Cairo");
  //             });



  //           });

  //           page.Content().Column(page2 =>
  //               {
  //                 var imagePath = Path.Combine(_webHostEnvironment.ContentRootPath, "Content", "images", "logo.png");
  //                 page.Background().AlignBottom().Image(imagePath);
  //                 //i need table with borders
  //                 page2.Item().Table(table =>
  //                     {

  //                       table.ColumnsDefinition(columns =>
  //                       {
  //                         columns.RelativeColumn(.5f);
  //                         columns.RelativeColumn(1);
  //                         columns.RelativeColumn(2);
  //                         columns.RelativeColumn(1); columns.RelativeColumn(1);
  //                       });
  //                       table.Header(header =>
  //                       {
  //                         //i need 2 2 hader rows

  //                         header.Cell().Row(1).Column(3).Element(cell => HeaderStyle(cell)).Text(" اسم الحساب ").FontSize(12).FontFamily("Cairo").ExtraBold().ExtraBlack();
  //                         header.Cell().Row(1).Column(1).Element(cell => HeaderStyle(cell)).Text("كود ").FontSize(12).FontFamily("Cairo").ExtraBold().ExtraBlack();
  //                         header.Cell().Row(1).Column(2).Element(cell => HeaderStyle(cell)).Text(" مدين ").FontSize(12).FontFamily("Cairo").ExtraBold().ExtraBlack();
  //                         header.Cell().Row(1).Column(4).Element(cell => HeaderStyle(cell)).Text(" دائن ").FontSize(12).FontFamily("Cairo").ExtraBold().ExtraBlack();
  //                         header.Cell().Row(1).Column(5).Element(cell => HeaderStyle(cell)).Text(" التوقيع ").FontSize(12).FontFamily("Cairo").ExtraBold().ExtraBlack();
  //                       });

  //                       report.ReportDetailsDtos.ForEach(x =>
  //                       {

  //                         table.Cell().Element(cell => MonthlyCellStyle(cell, monthlyCellColor2)).Scale(1.2f).Text(x.AccountId.ToString()).FontSize(8).Bold();
  //                         table.Cell().Element(cell => MonthlyCellStyle(cell, monthlyCellColor2)).Text(x.MonthlyTransAction?.Debit.ToString() ?? "0").FontSize(8).Bold();
  //                         table.Cell().Element(cell => MonthlyCellStyle(cell, monthlyCellColor2)).Scale(1.2f).Text(x.AccountName).FontFamily("Cairo").FontSize(8).Bold();
  //                         table.Cell().Element(cell => MonthlyCellStyle(cell, monthlyCellColor2)).Text(x.MonthlyTransAction?.Credit.ToString() ?? "0").FontSize(8).Bold();
  //                         table.Cell().Element(cell => MonthlyCellStyle(cell, monthlyCellColor2)).Text(string.Empty).FontSize(8).Bold();
  //                       });



  //                       table.Footer(footer =>
  //                       {

  //                         footer.Cell().Element(cell => FooterCellStyle(cell, accountColor)).Text(string.Empty).ExtraBold().ExtraBlack().FontSize(10);
  //                         footer.Cell().Element(cell => FooterCellStyle(cell, accountColor)).Text(report.ReportDetailsDtos.Sum(x => x.MonthlyTransAction?.Debit ?? 0).ToString()).ExtraBold().ExtraBlack().FontSize(10);
  //                         footer.Cell().Element(cell => FooterCellStyle(cell, accountColor)).Text("المجموع").ExtraBold().ExtraBlack().FontFamily("Cairo").FontSize(10);
  //                         footer.Cell().Element(cell => FooterCellStyle(cell, accountColor)).Text(report.ReportDetailsDtos.Sum(x => x.MonthlyTransAction?.Credit ?? 0).ToString()).ExtraBold().ExtraBlack().FontSize(10);
  //                         footer.Cell().Element(cell => FooterCellStyle(cell, accountColor)).Text(string.Empty).ExtraBold().ExtraBlack().FontSize(10);

  //                       });



  //                       static IContainer HeaderStyle(IContainer container)
  //                         => container.Border(1).Background(Color.FromHex("#e1e1e1")).PaddingHorizontal(1).PaddingVertical(4).AlignCenter().AlignMiddle();


  //                       static IContainer MainHeaderStyle(IContainer container, Color color)
  //                         => container.Border(0).Background(color).Width(100).PaddingHorizontal(1).PaddingVertical(1).AlignCenter().AlignMiddle();

  //                       static IContainer FooterCellStyle(IContainer container, Color color)
  //                         => container.Border(1).Background(color).PaddingHorizontal(1).PaddingVertical(4).AlignCenter().AlignMiddle();

  //                       // static IContainer OpeningCellStyle(IContainer container, Color color)
  //                       //     => container.Border(1).Background(color).PaddingHorizontal(1).PaddingVertical(1).AlignCenter().AlignMiddle();


  //                       static IContainer MonthlyCellStyle(IContainer container, Color color)
  //                         => container.Border(1).Background(color).PaddingHorizontal(1).PaddingVertical(3).AlignCenter().AlignMiddle();


  //                       // static IContainer ClosingCellStyle(IContainer container, Color color)
  //                       //     => container.Border(1).Background(color).PaddingHorizontal(1).PaddingVertical(1).AlignCenter().AlignMiddle();

  //                       static IContainer AccountCellStyle(IContainer container, Color color)
  //                         => container.Border(1).Background(color).PaddingHorizontal(1).PaddingVertical(1).AlignCenter().AlignMiddle();

  //                       page.Footer().AlignCenter().Text(x =>
  //                       {
  //                         x.CurrentPageNumber();
  //                         x.Span(" / ");
  //                         x.TotalPages();
  //                       });
  //                     });
  //               });
  //         });




  //   }).GeneratePdf(stream);
  //   return stream.ToArray();
  // }



  public async Task<byte[]> GenerateSubsidaryReport(GetSubsidartDailyRequest request, CancellationToken cancellationToken)
  {
    try
    {
      var report = await _subsidaryDailyService.GetSubsidaryDaily(request);
      if (report?.Collages == null || !report.Collages.Any())
        return Array.Empty<byte>();

      using var stream = new MemoryStream();
      var document = CreateReportDocument(report, request);
      document.GeneratePdf(stream);

      return stream.ToArray();
    }
    catch (Exception ex)
    {
      // Log the exception
      _logger.LogError(ex, "Error generating subsidiary report");
      throw;
    }
  }








}