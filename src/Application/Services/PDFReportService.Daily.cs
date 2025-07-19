using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Services.PDFProviders;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using Shared.Contracts.ReportRequest;
using Shared.DTOs.ReportDtos;

namespace Application.Services
{
    public partial class PDFReportService
    {
        public Document CreateDailyReportDocument(ReportDto report, GetAccountsBalanceBy request, string backgroundPath)
        {
            return Document.Create(container =>
            {
                container.Page(page =>
                {
                    PdfProvider.ConfigurePageSettings(page, PageSizes.A4.Portrait());
                    page.Background().AlignCenter().Image(backgroundPath).FitHeight().FitArea();
                    AddDailyReportHeader(page, report, request);
                    // AddReportTable(page, report);
                    // AddPageFooter(page);
                    page.Footer().AlignCenter().Text(x =>
                                  {
                                      x.CurrentPageNumber();
                                      x.Span(" / ");
                                      x.TotalPages();
                                  });
                });

            });
        }

        private static void AddDailyReportHeader(PageDescriptor page, ReportDto report, GetAccountsBalanceBy request)
        {
            page.Header().Column(column =>
            {
                column.Item().Column(c2 =>
                {
                    AddDailyHeaderItems(c2, report, request);
                });

            });
            page.Content().Column(p =>
            {
                AddDailyPageContent(p, report, request);
            });
        }


        private static void AddDailyHeaderItems(ColumnDescriptor c2, ReportDto report, GetAccountsBalanceBy request)
        {
            var printDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            var dateRange = string.Empty;
            if (request.StartDate.HasValue && request.EndDate.HasValue)
            {
                dateRange = $"من : {request.StartDate:yyyy-MM-dd} إلى : {request.EndDate:yyyy-MM-dd}";
            }
            else
            {

                dateRange = "لم يتم تحديد نطاق التقرير";

            }

            c2.Item().AlignLeft().Text("تاريخ الطباعة : " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")).BackgroundColor(ReportColors.TransparentWhite)
                                     .FontSize(ReportConstants.DataFontSize).FontFamily(ReportConstants.ArabicFont);
            c2.Item().AlignRight().Text("جامعة الاسكندريه")
                  .FontSize(10).Bold().FontFamily(ReportConstants.ArabicFont);
            c2.Item().AlignRight().Text("الوحدة الحسابيه المركزيه للمجمع الطبى")
                  .FontSize(10).Bold().Underline().FontFamily(ReportConstants.ArabicFont);
            // c2.Item().AlignCenter().Text("تقرير أرصدة الحسابات").BackgroundColor(ReportColors.TransparentWhite)
            //        .FontSize(ReportConstants.TitleFontSize).FontFamily(ReportConstants.ArabicFont).Bold();
            // c2.Item().AlignCenter().Text($"من : {request.StartDate:yyyy-MM-dd} إلى : {request.EndDate:yyyy-MM-dd}").BackgroundColor(ReportColors.TransparentWhite)
            //       .FontSize(ReportConstants.SubtitleFontSize).FontFamily(ReportConstants.ArabicFont).Underline().Bold();
            c2.Item().AlignRight().Text($"الكلية : {report.CollageName}")
                  .FontSize(ReportConstants.DataFontSize).Bold().FontFamily(ReportConstants.ArabicFont);
            c2.Item().AlignRight().Text($"الصندوق : {report.FundName}")
                  .FontSize(ReportConstants.DataFontSize).Bold().FontFamily(ReportConstants.ArabicFont);
            c2.Item().AlignRight().Text($"نوع الحساب : {report.AccountType}")
                  .FontSize(ReportConstants.DataFontSize).Bold().FontFamily(ReportConstants.ArabicFont);

        }

        private static void AddDailyPageContent(ColumnDescriptor column, ReportDto report, GetAccountsBalanceBy request)
        {
            //  var imagePath = Path.Combine(_webHostEnvironment.ContentRootPath, "Content", "images", "logo.png");
            column.Item().Table(table =>
            {
                table.ColumnsDefinition(columns =>
                {
                    columns.RelativeColumn(.5f);  // كود
                    columns.RelativeColumn(1);    // مدين
                    columns.RelativeColumn(2);    // اسم الحساب

                    columns.RelativeColumn(1);    // دائن
                    columns.RelativeColumn(1);    // رصيد
                    columns.RelativeColumn(1);    // التوقيع

                });
                AddDailyTableDailyHeader(table, request);
                foreach (var item in report.ReportDetailsDtos)
                {
                    AddDailyTableDetails(item, table);
                }
                table.Footer(footer =>
                {
                    AddDailyTableFooter(footer, report);
                });


            });
        }
        private static void AddDailyTableDailyHeader(TableDescriptor table, GetAccountsBalanceBy request)
        {
            table.Header(header =>
            {
                header.Cell().Row(1).Column(1).ColumnSpan(6).Element(cell => CellStyles.DataCellStyle(cell)).Text($"تقرير أرصدة الحسابات عن يوميه : {request.StartDate:yyyy-MM-dd}  ").AlignCenter().FontSize(ReportConstants.MainTitleFontSize).ExtraBold().ExtraBlack().FontFamily(ReportConstants.ArabicFont);



                header.Cell().Row(2).Column(1).Element(cell => CellStyles.HeaderCellStyle(cell))
                        .Text("كود").FontSize(ReportConstants.HeaderFontSize).FontFamily(ReportConstants.ArabicFont).ExtraBold().ExtraBlack();
                header.Cell().Row(2).Column(2).Element(cell => CellStyles.HeaderCellStyle(cell))
                        .Text("مدين").FontSize(ReportConstants.HeaderFontSize).FontFamily(ReportConstants.ArabicFont).ExtraBold().ExtraBlack();
                header.Cell().Row(2).Column(3).Element(cell => CellStyles.HeaderCellStyle(cell))
                      .Text("اسم الحساب").FontSize(ReportConstants.HeaderFontSize).FontFamily(ReportConstants.ArabicFont).ExtraBold().ExtraBlack();


                header.Cell().Row(2).Column(4).Element(cell => CellStyles.HeaderCellStyle(cell))
                      .Text("دائن").FontSize(ReportConstants.HeaderFontSize).FontFamily(ReportConstants.ArabicFont).ExtraBold().ExtraBlack();
                header.Cell().Row(2).Column(5).Element(cell => CellStyles.HeaderCellStyle(cell))
                      .Text("رصيد").FontSize(ReportConstants.HeaderFontSize).FontFamily(ReportConstants.ArabicFont).ExtraBold().ExtraBlack();
                header.Cell().Row(2).Column(6).Element(cell => CellStyles.HeaderCellStyle(cell))
                      .Text("التوقيع").FontSize(ReportConstants.HeaderFontSize).FontFamily(ReportConstants.ArabicFont).ExtraBold().ExtraBlack();


            });
        }

        private static void AddDailyTableDetails(ReportDetailsDto reportDetailsDto, TableDescriptor table)
        {


            var debitMonthAmount = reportDetailsDto.MonthlyTransAction?.Debit?.ToString("N2") ?? "0.00";
            var creditMonthAmount = reportDetailsDto.MonthlyTransAction?.Credit?.ToString("N2") ?? "0.00";
            var balanceMonth = reportDetailsDto.MonthlyTransAction?.Balance?.ToString("N2") ?? "0.00";


            table.Cell().Element(cell => CellStyles.DataCellStyle(cell))
                  .Text(reportDetailsDto.AccountId.ToString()).FontSize(ReportConstants.DataFontSize).Bold();





            table.Cell().Element(cell => CellStyles.DataCellStyle(cell))
                  .Text(debitMonthAmount).FontSize(ReportConstants.DataFontSize).Bold();

            table.Cell().Element(cell => CellStyles.DataCellStyle(cell))
                .Text(reportDetailsDto.AccountName ?? string.Empty).FontFamily(ReportConstants.ArabicFont).FontSize(ReportConstants.DataFontSize).Bold();
            table.Cell().Element(cell => CellStyles.DataCellStyle(cell))
                  .Text(creditMonthAmount).FontSize(ReportConstants.DataFontSize).Bold();
            table.Cell().Element(cell => CellStyles.DataCellStyle(cell))
                  .Text(balanceMonth).FontSize(ReportConstants.DataFontSize).Bold();

            table.Cell().Element(cell => CellStyles.DataCellStyle(cell))
     .Text(string.Empty).FontSize(ReportConstants.DataFontSize).Bold();




        }
        private static void AddDailyTableFooter(TableCellDescriptor footer, ReportDto report)
        {



            var totalMonthDebit = report.ReportDetailsDtos.Sum(x => x.MonthlyTransAction?.Debit ?? 0);
            var totalMonthCredit = report.ReportDetailsDtos.Sum(x => x.MonthlyTransAction?.Credit ?? 0);
            var totalMonthBalance = report.ReportDetailsDtos.Sum(x => x.MonthlyTransAction?.Balance ?? 0);


            footer.Cell().Element(cell => CellStyles.FooterCellStyle(cell))
                  .Text(string.Empty).ExtraBold().ExtraBlack().FontSize(10);





            footer.Cell().Element(cell => CellStyles.FooterCellStyle(cell))
                  .Text(totalMonthDebit.ToString("N2")).ExtraBold().ExtraBlack().FontSize(8);
            footer.Cell().Element(cell => CellStyles.FooterCellStyle(cell))
           .Text("المجموع").ExtraBold().ExtraBlack().FontFamily(ReportConstants.ArabicFont).FontSize(10);
            footer.Cell().Element(cell => CellStyles.FooterCellStyle(cell))
                  .Text(totalMonthCredit.ToString("N2")).ExtraBold().ExtraBlack().FontSize(8);
            footer.Cell().Element(cell => CellStyles.FooterCellStyle(cell))
                  .Text(totalMonthBalance.ToString("N2")).ExtraBold().ExtraBlack().FontSize(8);

            footer.Cell().Element(cell => CellStyles.FooterCellStyle(cell))
                            .Text(string.Empty).ExtraBold().ExtraBlack().FontSize(8);


        }


    }
}