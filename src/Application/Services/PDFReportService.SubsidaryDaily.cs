using System;
using System.Collections.Generic;
using System.Linq;

using System.Threading.Tasks;
using Application.Services.PDFProviders;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using Shared.DTOs.FormDtos;

namespace Application.Services
{
    public partial class PDFReportService
    {
        public Document CreateReportDocument(SubsidaryDailyReportDto report, GetSubsidartDailyRequest request)
        {
            return Document.Create(container =>
            {
                container.Page(page =>
        {
            PdfProvider.ConfigurePageSettings(page, PageSizes.A4.Landscape());
            AddReportHeader(page, report, request);
            AddReportTable(page, report);
            // AddPageFooter(page);
        });
            });
        }

        private static void AddReportHeader(PageDescriptor page, SubsidaryDailyReportDto report, GetSubsidartDailyRequest request)
        {
            page.Header().Column(column =>
            {
                AddHeaderItems(column, report, request);
            });
        }

        private static void AddHeaderItems(ColumnDescriptor column, SubsidaryDailyReportDto report, GetSubsidartDailyRequest request)
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



            column.Item().AlignLeft().Text($"تاريخ الطباعة : {printDate}").FontSize(ReportConstants.HeaderFontSize);
            column.Item().AlignRight().Text("جامعة الاسكندريه").FontSize(ReportConstants.TitleFontSize).Bold();
            column.Item().AlignRight().Text("الوحدة الحسابيه المركزيه للمجمع الطبى").FontSize(ReportConstants.TitleFontSize).Bold().Underline();
            column.Item().AlignCenter().Text(report.Daily).FontSize(ReportConstants.MainTitleFontSize).Bold();
            column.Item().AlignCenter().Text(dateRange).FontSize(ReportConstants.TitleFontSize).Underline().Bold();

            AddReportDetails(column, report);
        }

        private static void AddReportDetails(ColumnDescriptor column, SubsidaryDailyReportDto report)
        {
            var details = new[]
            {
        ("الكلية", report.CollageName),
        ("الصندوق", report.FundName),
        ("اسم الحساب", report.AccountName),
        ("نوع اليوميه", report.AccountType)
    };

            foreach (var (label, value) in details)
            {
                column.Item().AlignRight().Text($"{label} : {value}").FontSize(ReportConstants.TitleFontSize).Bold();
            }
        }
        private static void AddReportTable(PageDescriptor page, SubsidaryDailyReportDto report)
        {
            page.Content().Table(table =>
            {
                ConfigureTableColumns(table);
                AddTableHeader(table);
                AddTableContent(table, report);
                AddTableFooter(table, report);
            });
        }

        private static void ConfigureTableColumns(TableDescriptor table)
        {
            table.ColumnsDefinition(columns =>
            {
                columns.RelativeColumn(1);    // الكلية
                columns.RelativeColumn(1);    // الصندوق
                columns.RelativeColumn(1);    // مدين
                columns.RelativeColumn(1.2f); // البيان
                columns.RelativeColumn(1);    // دائن
            });
        }

        private static void AddTableHeader(TableDescriptor table)
        {
            table.Header(header =>
            {
                var headers = new[] { "الكليه", "الصندوق", "مدين", "البيان", "دائن" };

                for (int i = 0; i < headers.Length; i++)
                {
                    header.Cell().Row(1).Column((uint)(i + 1))
                  .AlignCenter()
                  .Scale(ReportConstants.HeaderScale)
                  .Element(cell => CellStyles.MainHeaderStyle(cell, Colors.Transparent))
                  .Text(headers[i])
                  .Bold()
                  .FontSize(ReportConstants.HeaderFontSize);
                }
            });
        }
        private static void AddTableContent(TableDescriptor table, SubsidaryDailyReportDto report)
        {
            foreach (var college in report.Collages)
            {
                AddCollegeData(table, college);
                AddCollegeTotal(table, college);
            }

            AddGrandTotal(table, report);
        }

        private static void AddCollegeData(TableDescriptor table, SubsidaryDailyCollageReportDto college)
        {
            foreach (var fund in college.Funds)
            {
                AddFundDetails(table, college, fund);
                AddFundSubtotal(table, college, fund);
            }
        }

        private static void AddFundDetails(TableDescriptor table, SubsidaryDailyCollageReportDto college, SubsidaryDailyFundsReportDto fund)
        {
            foreach (var detail in fund.SubsidaryDetails)
            {
                AddTableRow(table, ReportColors.Account,
                    college.CollageName?.ToString() ?? string.Empty,
                    fund.FundName?.ToString() ?? string.Empty,
                    detail.Debit.ToString(),
                    detail.SubsidaryName?.ToString() ?? string.Empty,
                    detail.Credit.ToString());
            }
        }
        private static void AddFundSubtotal(TableDescriptor table, SubsidaryDailyCollageReportDto college, SubsidaryDailyFundsReportDto fund)
        {
            var totalDebit = fund.SubsidaryDetails.Sum(s => s.Debit);
            var totalCredit = fund.SubsidaryDetails.Sum(s => s.Credit);

            table.Cell().Element(cell => CellStyles.SubtotalStyle(cell, ReportColors.Subtotal))
                 .Scale(ReportConstants.DefaultScale).Text(college.CollageName?.ToString() ?? string.Empty).SemiBold();
            table.Cell().Element(cell => CellStyles.SubtotalStyle(cell, ReportColors.Subtotal))
                 .Scale(ReportConstants.DefaultScale).Text(string.Empty);
            table.Cell().Element(cell => CellStyles.SubtotalStyle(cell, ReportColors.Subtotal))
                 .Scale(ReportConstants.DefaultScale).Text(totalDebit.ToString()).SemiBold();
            table.Cell().Element(cell => CellStyles.SubtotalStyle(cell, ReportColors.Subtotal))
                 .Scale(ReportConstants.DefaultScale).Text($"أجمالى {fund.FundName}").SemiBold();
            table.Cell().Element(cell => CellStyles.SubtotalStyle(cell, ReportColors.Subtotal))
                 .Scale(ReportConstants.DefaultScale).Text(totalCredit.ToString()).SemiBold();
        }

        private static void AddCollegeTotal(TableDescriptor table, SubsidaryDailyCollageReportDto college)
        {
            var totalDebit = college.Funds.Sum(f => f.SubsidaryDetails.Sum(s => s.Debit));
            var totalCredit = college.Funds.Sum(f => f.SubsidaryDetails.Sum(s => s.Credit));

            table.Cell().Element(cell => CellStyles.TotalStyle(cell, ReportColors.Total))
                 .Scale(ReportConstants.DefaultScale).Text(college.CollageName?.ToString() ?? string.Empty).SemiBold();
            table.Cell().Element(cell => CellStyles.TotalStyle(cell, ReportColors.Total))
                 .Scale(ReportConstants.DefaultScale).Text(string.Empty).SemiBold();
            table.Cell().Element(cell => CellStyles.TotalStyle(cell, ReportColors.Total))
                 .Scale(ReportConstants.DefaultScale).Text(totalDebit.ToString()).SemiBold();
            table.Cell().Element(cell => CellStyles.TotalStyle(cell, ReportColors.Total))
                 .Scale(ReportConstants.DefaultScale).Text($"أجمالى {college.CollageName}").SemiBold();
            table.Cell().Element(cell => CellStyles.TotalStyle(cell, ReportColors.Total))
                 .Scale(ReportConstants.DefaultScale).Text(totalCredit.ToString()).SemiBold();
        }

        private static void AddTableRow(TableDescriptor table, Color backgroundColor, params string[] values)
        {
            foreach (var value in values)
            {
                table.Cell()
                     .Element(cell => CellStyles.AccountCellStyle(cell, backgroundColor))
                     .Scale(ReportConstants.DefaultScale)
                     .Text(value);
            }
        }
        private static void AddTableFooter(TableDescriptor table, SubsidaryDailyReportDto report)
        {
            table.Footer(footer =>
            {
                AddTotalSubsidariesRows(footer, report);
                AddFinalTotalRow(footer, report);
            });
        }


        private static void AddTotalSubsidariesRows(TableCellDescriptor footer, SubsidaryDailyReportDto report)
        {
            var sortedTotalSubsidiaries = report.TotalSubsidaries?.OrderBy(x => x.SubsidaryNumber).ToList();

            if (sortedTotalSubsidiaries?.Any() != true)
                return;

            foreach (var total in sortedTotalSubsidiaries)
            {
                // الطريقة الصحيحة: استخدام footer مباشرة لكل خلية
                footer.Cell().Element(cell => CellStyles.AccountCellStyle(cell, ReportColors.Closing))
                      .Scale(ReportConstants.DefaultScale).Text(string.Empty);

                footer.Cell().Element(cell => CellStyles.AccountCellStyle(cell, ReportColors.Closing))
                      .Scale(ReportConstants.DefaultScale).Text(total.SubsidaryNumber?.ToString() ?? string.Empty);

                footer.Cell().Element(cell => CellStyles.AccountCellStyle(cell, ReportColors.Closing))
                      .Scale(ReportConstants.DefaultScale).Text(total.Debit.ToString());

                footer.Cell().Element(cell => CellStyles.AccountCellStyle(cell, ReportColors.Closing))
                      .Scale(ReportConstants.DefaultScale).Text($"أجماليات {total.SubsidaryName}");

                footer.Cell().Element(cell => CellStyles.AccountCellStyle(cell, ReportColors.Closing))
                      .Scale(ReportConstants.DefaultScale).Text(total.Credit.ToString());
            }
        }


        private static void AddFinalTotalRow(TableCellDescriptor footer, SubsidaryDailyReportDto report)
        {
            if (report.TotalSubsidaries?.Any() != true)
                return;

            var totalDebit = report.TotalSubsidaries.Sum(x => x.Debit);
            var totalCredit = report.TotalSubsidaries.Sum(x => x.Credit);

            AddFooterRow(footer, ReportColors.Monthly,
                string.Empty,
                string.Empty,
                totalDebit.ToString(),
                "أجمالى كلى",
                totalCredit.ToString());
        }

        private static void AddFooterRow(TableCellDescriptor footer, Color backgroundColor, params string[] values)
        {
            foreach (var value in values)
            {
                footer.Cell()
                      .Element(cell => CellStyles.AccountCellStyle(cell, backgroundColor))
                      .Scale(ReportConstants.DefaultScale)
                      .Text(value);
            }
        }

        private static void AddGrandTotal(TableDescriptor table, SubsidaryDailyReportDto report)
        {
            // Pre-calculate totals to avoid repeated calculations
            var totalDebit = report.Collages.Sum(c => c.Funds.Sum(f => f.SubsidaryDetails.Sum(s => s.Debit)));
            var totalCredit = report.Collages.Sum(c => c.Funds.Sum(f => f.SubsidaryDetails.Sum(s => s.Credit)));

            AddTableRow(table, ReportColors.GrandTotal,
                string.Empty,
                string.Empty,
                totalDebit.ToString(),
                "أجمالى كلى",
                totalCredit.ToString());
        }


    }
}