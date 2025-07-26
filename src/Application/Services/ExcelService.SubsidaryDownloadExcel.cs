using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.SS.UserModel.Charts;
using NPOI.SS.Util;

using Shared.DTOs.FormDtos;

namespace Application.Services
{
    public partial class ExcelService
    {
        /*Subsidary*/
        public async Task<byte[]> GenerateSubsidiaryExcelFile(SubsidaryToExcelRequest request, CancellationToken cancellationToken = default)
        {
            // Get the data from BuildSubsidaryToExcelData method
            var data = await _subsidaryDailyService.BuildSubsidaryToExcelData(request, cancellationToken);
            if (data == null || !data.Any())
            {
                throw new Exception("No data found to export to Excel");
            }

            // Create a new workbook and sheet
            var workbook = new XSSFWorkbook();
            var sheet = workbook.CreateSheet("Subsidiary Data");
            sheet.IsRightToLeft = true; // Set the sheet to be right-to-left for Arabic text
            sheet.CreateFreezePane(0, 2); // Freeze the first 2 rows (headers)

            // Helper function to get cell style for headers
            ICellStyle GetHeaderStyle()
            {
                var style = workbook.CreateCellStyle();
                var font = workbook.CreateFont();
                font.IsBold = true;
                font.FontHeightInPoints = 12;
                style.SetFont(font);
                style.Alignment = HorizontalAlignment.Center;
                style.VerticalAlignment = VerticalAlignment.Center;
                style.BorderTop = BorderStyle.Thin;
                style.BorderBottom = BorderStyle.Thin;
                style.BorderLeft = BorderStyle.Thin;
                style.BorderRight = BorderStyle.Thin;
                style.FillForegroundColor = IndexedColors.LightYellow.Index;
                style.FillPattern = FillPattern.SolidForeground;
                return style;
            }

            // Get all unique subaccounts from the first entry (all entries should have the same set of subaccounts)
            var subAccounts = data.FirstOrDefault()?.SubsidaryAccountDtos ?? new List<SubsidaryToExcelDto.SubsidaryAccountDto>();

            // Define column indices
            int fixedColumnCount = 10; // ID, FormName, CollageName, FundName, Num55, Num224, AuditorName, Details, TotalDebit,TotalCredit
            int subAccountsStartIndex = fixedColumnCount;
            int totalColumnCount = subAccountsStartIndex + subAccounts.Count + 2; // +1 for TotalCredit at the end

            var startSubAccountLetter = GetExcelColumnName(subAccountsStartIndex);
            var endSubAccountLetter = GetExcelColumnName(subAccountsStartIndex + subAccounts.Count - 1);

            // Create the first header row with ID numbers or codes
            var headerCodeRow = sheet.CreateRow(0);
            headerCodeRow.CreateCell(0).SetCellValue(string.Empty); // ID
            headerCodeRow.CreateCell(1).SetCellValue("A-1"); // Num55
            headerCodeRow.CreateCell(2).SetCellValue("A-2"); // Num224
            headerCodeRow.CreateCell(3).SetCellValue("A-3"); // Form Name
            headerCodeRow.CreateCell(4).SetCellValue("A-4"); // Auditor Name
            headerCodeRow.CreateCell(5).SetCellValue("A-5"); // College Name
            headerCodeRow.CreateCell(6).SetCellValue("A-6"); // Fund Name
            headerCodeRow.CreateCell(7).SetCellValue("A-7"); // Details
            headerCodeRow.CreateCell(8).SetCellValue(string.Empty); // Total Debit
            headerCodeRow.CreateCell(9).SetCellValue(string.Empty); // Total Credit

            // Add subaccount IDs to the first header row
            for (int i = 0; i < subAccounts.Count; i++)
            {
                headerCodeRow.CreateCell(subAccountsStartIndex + i).SetCellValue(subAccounts[i].Id); // Use index+1 as ID
            }



            // Apply header style to all cells in the first header row
            var headerStyle = GetHeaderStyle();
            for (int i = 0; i < totalColumnCount; i++)
            {
                var cell = headerCodeRow.GetCell(i) ?? headerCodeRow.CreateCell(i);
                cell.CellStyle = headerStyle;
            }

            // Create the second header row with names
            var headerNameRow = sheet.CreateRow(1);
            headerNameRow.CreateCell(0).SetCellValue("الرقم");
            headerNameRow.CreateCell(1).SetCellValue("رقم 55");
            headerNameRow.CreateCell(2).SetCellValue("رقم 224");
            headerNameRow.CreateCell(3).SetCellValue("اسم الملف");
            headerNameRow.CreateCell(4).SetCellValue("المراجع");
            headerNameRow.CreateCell(5).SetCellValue("الكلية");
            headerNameRow.CreateCell(6).SetCellValue("الصندوق");
            headerNameRow.CreateCell(7).SetCellValue("تفاصيل");
            headerNameRow.CreateCell(8).SetCellValue("إجمالي مدين");
            headerNameRow.CreateCell(9).SetCellValue("إجمالي دائن");


            // Add subaccount names to the second header row
            for (int i = 0; i < subAccounts.Count; i++)
            {
                headerNameRow.CreateCell(subAccountsStartIndex + i).SetCellValue(subAccounts[i].AccountName);
            }

            // Add Total Credit to the second header row
            headerNameRow.CreateCell(subAccountsStartIndex + subAccounts.Count).SetCellValue(" الصافى");
            headerNameRow.CreateCell(subAccountsStartIndex + subAccounts.Count + 1).SetCellValue(" التوازن");


            // Apply header style to all cells in the second header row
            for (int i = 0; i < totalColumnCount; i++)
            {
                var cell = headerNameRow.GetCell(i) ?? headerNameRow.CreateCell(i);
                cell.CellStyle = headerStyle;
            }

            // Create a style for data cells
            ICellStyle dataStyle = workbook.CreateCellStyle();
            dataStyle.BorderTop = BorderStyle.Thin;
            dataStyle.BorderBottom = BorderStyle.Thin;
            dataStyle.BorderLeft = BorderStyle.Thin;
            dataStyle.BorderRight = BorderStyle.Thin;

            // Create a style for numeric cells
            ICellStyle numericStyle = workbook.CreateCellStyle();
            numericStyle.CloneStyleFrom(dataStyle);
            numericStyle.DataFormat = workbook.CreateDataFormat().GetFormat("0.00");

            // Fill the data rows
            for (int rowIndex = 0; rowIndex < data.Count; rowIndex++)
            {
                var item = data[rowIndex];
                var row = sheet.CreateRow(rowIndex + 2); // +2 because we have 2 header rows

                // Set the fixed column data
                // row.CreateCell(0).SetCellValue(rowIndex + 1); // ID number (1-based)
                SetCellValueWithStyle(row, 0, item.FormDetailsId.ToString(), dataStyle);
                SetCellValueWithStyle(row, 1, item.Num55, dataStyle);
                SetCellValueWithStyle(row, 2, item.Num224, dataStyle);
                SetCellValueWithStyle(row, 3, item.FormName, dataStyle);
                SetCellValueWithStyle(row, 4, item.AuditorName, dataStyle);
                SetCellValueWithStyle(row, 5, item.CollageName, dataStyle);
                SetCellValueWithStyle(row, 6, item.FundName, dataStyle);
                SetCellValueWithStyle(row, 7, item.Details, dataStyle);

                // Set the total debit value
                var totalDebitCell = row.CreateCell(8);
                totalDebitCell.SetCellValue((double)(item.TotalDebit ?? 0));
                totalDebitCell.CellStyle = numericStyle;
                var totalCreditCell = row.CreateCell(9);
                totalCreditCell.SetCellValue((double)(item.TotalCredit ?? 0));
                totalCreditCell.CellStyle = numericStyle;

                // Set the subaccount values (debits)
                for (int i = 0; i < subAccounts.Count; i++)
                {
                    var cell = row.CreateCell(subAccountsStartIndex + i);
                    var subAccount = item.SubsidaryAccountDtos.ElementAtOrDefault(i);
                    cell.SetCellValue((double)(subAccount?.Credit ?? 0) - (double)(subAccount?.Debit ?? 0));
                    cell.CellStyle = numericStyle;
                }

                //set formula cell that sum all subaccount debits


                var sumSubDebitFormula = $"SUM({startSubAccountLetter}{rowIndex + 3}:{endSubAccountLetter}{rowIndex + 3})";
                var debitCreditBalanc = $"($j{rowIndex + 3}-$i{rowIndex + 3})";
                var sumSubDebitCell = row.CreateCell(subAccountsStartIndex + subAccounts.Count);
                sumSubDebitCell.SetCellFormula(debitCreditBalanc + "-" + sumSubDebitFormula);
                sumSubDebitCell.CellStyle = numericStyle;

                //setBalance
                var BalanceFormula = $"({sumSubDebitFormula}=($j{rowIndex + 3}-$i{rowIndex + 3}))";
                var BalanceFormulaCell = row.CreateCell(subAccountsStartIndex + subAccounts.Count + 1);
                BalanceFormulaCell.SetCellFormula(BalanceFormula);
                BalanceFormulaCell.CellStyle = numericStyle;
            }

            // Add conditional formatting to set cell color to green if the balance is zero
            var sheetCF = sheet.SheetConditionalFormatting;
            var rule = sheetCF.CreateConditionalFormattingRule(ComparisonOperator.Equal, "TRUE");
            var patternFormatting = rule.CreatePatternFormatting();
            patternFormatting.FillBackgroundColor = IndexedColors.LightGreen.Index;
            patternFormatting.FillPattern = FillPattern.SolidForeground;

            var rule2 = sheetCF.CreateConditionalFormattingRule(ComparisonOperator.Equal, "FALSE");
            var patternFormatting2 = rule2.CreatePatternFormatting();
            patternFormatting2.FillBackgroundColor = IndexedColors.Rose.Index;
            patternFormatting2.FillPattern = FillPattern.SolidForeground;


            var regions = new CellRangeAddress[] {
            new CellRangeAddress(2, data.Count + 1, subAccountsStartIndex + subAccounts.Count + 1, subAccountsStartIndex + subAccounts.Count + 1)

        };
            sheetCF.AddConditionalFormatting(regions, rule);

            sheetCF.AddConditionalFormatting(regions, rule2);

            // Auto-size the columns for better readability
            for (int i = 0; i < totalColumnCount + 1; i++)
            {
                sheet.AutoSizeColumn(i);
            }

            // Convert the workbook to a byte array
            using (var ms = new MemoryStream())
            {
                workbook.Write(ms);
                return ms.ToArray();
            }
        }

        // Helper method to set cell value with style
        private void SetCellValueWithStyle(IRow row, int cellIndex, string value, ICellStyle style)
        {
            var cell = row.CreateCell(cellIndex);
            cell.SetCellValue(value);
            cell.CellStyle = style;
        }

    }
}