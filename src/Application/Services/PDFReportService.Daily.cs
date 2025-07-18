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
        public Document CreateReportDocument(ReportDto report, GetAccountsBalanceBy request, string backgroundPath)
        {
            return Document.Create(container =>
            {
                container.Page(page =>
                {
                    PdfProvider.ConfigurePageSettings(page, PageSizes.A4.Landscape());
                    page.Background().AlignCenter().Image(backgroundPath).FitHeight().FitArea();
                    AddReportHeader(page, report, request);
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


        private static void AddReportHeader(PageDescriptor page, ReportDto report, GetAccountsBalanceBy request)
        {
            page.Header().Column(column =>
            {
                column.Item().Column(c2 =>
                {
                    AddHeaderItems(c2, report, request);
                });

            });
            page.Content().Column(p =>
            {
                AddPageContent(p, report, request);
            });
        }


        private static void AddHeaderItems(ColumnDescriptor c2, ReportDto report, GetAccountsBalanceBy request)
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

        private static void AddPageContent(ColumnDescriptor column, ReportDto report, GetAccountsBalanceBy request)
        {
            //  var imagePath = Path.Combine(_webHostEnvironment.ContentRootPath, "Content", "images", "logo.png");
            column.Item().Table(table =>
            {
                table.ColumnsDefinition(columns =>
                {
                    columns.RelativeColumn(.5f);  // كود
                    columns.RelativeColumn(2);    // اسم الحساب
                    columns.RelativeColumn(1);    // مدين
                    columns.RelativeColumn(1);    // دائن
                    columns.RelativeColumn(1);    // رصيد
                    columns.RelativeColumn(1);    // مدين
                    columns.RelativeColumn(1);    // دائن
                    columns.RelativeColumn(1);    // رصيد
                    columns.RelativeColumn(1);    // مدين
                    columns.RelativeColumn(1);    // دائن
                    columns.RelativeColumn(1);    // رصيد
                });
                AddTableDailyHeader(table, request);
                foreach (var item in report.ReportDetailsDtos)
                {
                    AddTableDetails(item, table);
                }
                table.Footer(footer =>
                {
                    AddTableFooter(footer, report);
                });


            });
        }
        private static void AddTableDailyHeader(TableDescriptor table, GetAccountsBalanceBy request)
        {
            table.Header(header =>
            {
                header.Cell().Row(1).Column(1).ColumnSpan(11).Element(cell => CellStyles.TitleStyle(cell, ReportColors.Opening)).Text($"تقرير أرصدة الحسابات عن الفترة من : {request.StartDate:yyyy-MM-dd} إلى : {request.EndDate:yyyy-MM-dd} ").AlignCenter().FontSize(ReportConstants.MainTitleFontSize).ExtraBold().ExtraBlack().FontFamily(ReportConstants.ArabicFont);

                header.Cell().Row(2).Column(1).ColumnSpan(2).Element(cell => CellStyles.HeaderCellStyle(cell)).Text(string.Empty);
                header.Cell().Row(2).Column(3).ColumnSpan(3).Element(cell => CellStyles.HeaderCellStyle(cell)).PaddingBottom(5).Text("رصيد أول").AlignCenter().FontSize(ReportConstants.SubtitleFontSize).Bold().ExtraBlack().FontFamily(ReportConstants.ArabicFont);
                header.Cell().Row(2).Column(6).ColumnSpan(3).Element(cell => CellStyles.HeaderCellStyle(cell)).PaddingBottom(5).Text("حركة الشهر").AlignCenter().FontSize(ReportConstants.SubtitleFontSize).Bold().ExtraBlack().FontFamily(ReportConstants.ArabicFont);
                header.Cell().Row(2).Column(9).ColumnSpan(3).Element(cell => CellStyles.HeaderCellStyle(cell)).PaddingBottom(5).Text("رصيد اخر").AlignCenter().FontSize(ReportConstants.SubtitleFontSize).Bold().ExtraBlack().FontFamily(ReportConstants.ArabicFont);


                header.Cell().Row(3).Column(1).Element(cell => CellStyles.HeaderCellStyle(cell))
                        .Text("كود").FontSize(ReportConstants.HeaderFontSize).FontFamily(ReportConstants.ArabicFont).ExtraBold().ExtraBlack();
                header.Cell().Row(3).Column(2).Element(cell => CellStyles.HeaderCellStyle(cell))
                      .Text("اسم الحساب").FontSize(ReportConstants.HeaderFontSize).FontFamily(ReportConstants.ArabicFont).ExtraBold().ExtraBlack();
                header.Cell().Row(3).Column(3).Element(cell => CellStyles.HeaderCellStyle(cell))
                      .Text("مدين").FontSize(ReportConstants.HeaderFontSize).FontFamily(ReportConstants.ArabicFont).ExtraBold().ExtraBlack();
                header.Cell().Row(3).Column(4).Element(cell => CellStyles.HeaderCellStyle(cell))
                      .Text("دائن").FontSize(ReportConstants.HeaderFontSize).FontFamily(ReportConstants.ArabicFont).ExtraBold().ExtraBlack();
                header.Cell().Row(3).Column(5).Element(cell => CellStyles.HeaderCellStyle(cell))
                    .Text("رصيد").FontSize(ReportConstants.HeaderFontSize).FontFamily(ReportConstants.ArabicFont).ExtraBold().ExtraBlack();
                header.Cell().Row(3).Column(6).Element(cell => CellStyles.HeaderCellStyle(cell))
                .Text("مدين").FontSize(ReportConstants.HeaderFontSize).FontFamily(ReportConstants.ArabicFont).ExtraBold().ExtraBlack();
                header.Cell().Row(3).Column(7).Element(cell => CellStyles.HeaderCellStyle(cell))
                      .Text("دائن").FontSize(ReportConstants.HeaderFontSize).FontFamily(ReportConstants.ArabicFont).ExtraBold().ExtraBlack();
                header.Cell().Row(3).Column(8).Element(cell => CellStyles.HeaderCellStyle(cell))
                    .Text("رصيد").FontSize(ReportConstants.HeaderFontSize).FontFamily(ReportConstants.ArabicFont).ExtraBold().ExtraBlack();
                header.Cell().Row(3).Column(9).Element(cell => CellStyles.HeaderCellStyle(cell))
                .Text("مدين").FontSize(ReportConstants.HeaderFontSize).FontFamily(ReportConstants.ArabicFont).ExtraBold().ExtraBlack();
                header.Cell().Row(3).Column(10).Element(cell => CellStyles.HeaderCellStyle(cell))
                      .Text("دائن").FontSize(ReportConstants.HeaderFontSize).FontFamily(ReportConstants.ArabicFont).ExtraBold().ExtraBlack();
                header.Cell().Row(3).Column(11).Element(cell => CellStyles.HeaderCellStyle(cell))
                    .Text("رصيد").FontSize(ReportConstants.HeaderFontSize).FontFamily(ReportConstants.ArabicFont).ExtraBold().ExtraBlack();

            });
        }

        private static void AddTableDetails(ReportDetailsDto reportDetailsDto, TableDescriptor table)
        {
            var debitOpeningBalance = reportDetailsDto.OpeningBalance?.Debit?.ToString("N2") ?? "0.00";
            var creditOpeningBalance = reportDetailsDto.OpeningBalance?.Credit?.ToString("N2") ?? "0.00";
            var balanceOpening = reportDetailsDto.OpeningBalance?.Balance?.ToString("N2") ?? "0.00";

            var debitMonthAmount = reportDetailsDto.MonthlyTransAction?.Debit?.ToString("N2") ?? "0.00";
            var creditMonthAmount = reportDetailsDto.MonthlyTransAction?.Credit?.ToString("N2") ?? "0.00";
            var balanceMonth = reportDetailsDto.MonthlyTransAction?.Balance?.ToString("N2") ?? "0.00";

            var debitEndBalance = reportDetailsDto.ClosingBalance?.Debit?.ToString("N2") ?? "0.00";
            var creditEndBalance = reportDetailsDto.ClosingBalance?.Credit?.ToString("N2") ?? "0.00";
            var balanceEnd = reportDetailsDto.ClosingBalance?.Balance?.ToString("N2") ?? "0.00";

            table.Cell().Element(cell => CellStyles.DataCellStyle(cell))
                  .Text(reportDetailsDto.AccountId.ToString()).FontSize(ReportConstants.DataFontSize).Bold();
            table.Cell().Element(cell => CellStyles.DataCellStyle(cell))
                  .Text(reportDetailsDto.AccountName ?? string.Empty).FontFamily(ReportConstants.ArabicFont).FontSize(ReportConstants.DataFontSize).Bold();

            table.Cell().Element(cell => CellStyles.DataCellStyle(cell))
                  .Text(debitOpeningBalance).FontSize(ReportConstants.DataFontSize).Bold();
            table.Cell().Element(cell => CellStyles.DataCellStyle(cell))
                  .Text(creditOpeningBalance).FontSize(ReportConstants.DataFontSize).Bold();
            table.Cell().Element(cell => CellStyles.DataCellStyle(cell))
                  .Text(balanceOpening).FontSize(ReportConstants.DataFontSize).Bold();


            table.Cell().Element(cell => CellStyles.DataCellStyle(cell))
                  .Text(debitMonthAmount).FontSize(ReportConstants.DataFontSize).Bold();
            table.Cell().Element(cell => CellStyles.DataCellStyle(cell))
                  .Text(creditMonthAmount).FontSize(ReportConstants.DataFontSize).Bold();
            table.Cell().Element(cell => CellStyles.DataCellStyle(cell))
                  .Text(balanceMonth).FontSize(ReportConstants.DataFontSize).Bold();


            table.Cell().Element(cell => CellStyles.DataCellStyle(cell))
                  .Text(debitEndBalance).FontSize(ReportConstants.DataFontSize).Bold();
            table.Cell().Element(cell => CellStyles.DataCellStyle(cell))
                  .Text(creditEndBalance).FontSize(ReportConstants.DataFontSize).Bold();
            table.Cell().Element(cell => CellStyles.DataCellStyle(cell))
                  .Text(balanceEnd).FontSize(ReportConstants.DataFontSize).Bold();

        }
        private static void AddTableFooter(TableCellDescriptor footer, ReportDto report)
        {
            var totalOpeningDebit = report.ReportDetailsDtos.Sum(x => x.OpeningBalance?.Debit ?? 0);
            var totalOpeningCredit = report.ReportDetailsDtos.Sum(x => x.OpeningBalance?.Credit ?? 0);
            var totalOpeningBalance = report.ReportDetailsDtos.Sum(x => x.OpeningBalance?.Balance ?? 0);


            var totalMonthDebit = report.ReportDetailsDtos.Sum(x => x.MonthlyTransAction?.Debit ?? 0);
            var totalMonthCredit = report.ReportDetailsDtos.Sum(x => x.MonthlyTransAction?.Credit ?? 0);
            var totalMonthBalance = report.ReportDetailsDtos.Sum(x => x.MonthlyTransAction?.Balance ?? 0);


            var totalEndDebit = report.ReportDetailsDtos.Sum(x => x.ClosingBalance?.Debit ?? 0);
            var totalEndCredit = report.ReportDetailsDtos.Sum(x => x.ClosingBalance?.Credit ?? 0);
            var totalEndBalance = report.ReportDetailsDtos.Sum(x => x.ClosingBalance?.Balance ?? 0);

            footer.Cell().Element(cell => CellStyles.FooterCellStyle(cell))
                  .Text(string.Empty).ExtraBold().ExtraBlack().FontSize(10);
            footer.Cell().Element(cell => CellStyles.FooterCellStyle(cell))
                  .Text("المجموع").ExtraBold().ExtraBlack().FontFamily(ReportConstants.ArabicFont).FontSize(10);


            footer.Cell().Element(cell => CellStyles.FooterCellStyle(cell))
                  .Text(totalOpeningDebit.ToString("N2")).ExtraBold().ExtraBlack().FontSize(8);
            footer.Cell().Element(cell => CellStyles.FooterCellStyle(cell))
                  .Text(totalOpeningCredit.ToString("N2")).ExtraBold().ExtraBlack().FontSize(8);
            footer.Cell().Element(cell => CellStyles.FooterCellStyle(cell))
                  .Text(totalOpeningBalance.ToString("N2")).ExtraBold().ExtraBlack().FontSize(8);

            footer.Cell().Element(cell => CellStyles.FooterCellStyle(cell))
                  .Text(totalMonthDebit.ToString("N2")).ExtraBold().ExtraBlack().FontSize(8);
            footer.Cell().Element(cell => CellStyles.FooterCellStyle(cell))
                  .Text(totalMonthCredit.ToString("N2")).ExtraBold().ExtraBlack().FontSize(8);
            footer.Cell().Element(cell => CellStyles.FooterCellStyle(cell))
                  .Text(totalMonthBalance.ToString("N2")).ExtraBold().ExtraBlack().FontSize(8);



            footer.Cell().Element(cell => CellStyles.FooterCellStyle(cell))
                   .Text(totalEndDebit.ToString("N2")).ExtraBold().ExtraBlack().FontSize(8);
            footer.Cell().Element(cell => CellStyles.FooterCellStyle(cell))
                  .Text(totalEndCredit.ToString("N2")).ExtraBold().ExtraBlack().FontSize(8);
            footer.Cell().Element(cell => CellStyles.FooterCellStyle(cell))
                  .Text(totalEndBalance.ToString("N2")).ExtraBold().ExtraBlack().FontSize(8);
        }


    }
}