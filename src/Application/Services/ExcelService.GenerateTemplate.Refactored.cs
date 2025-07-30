using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Models;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;
using Shared.Contracts.FormDetailsRequest;
using Application.Helper;
using Core.Interfaces.Repository;

namespace Application.Services
{
    public partial class ExcelService
    {
        public async Task<byte[]> GenerateTemplateExcelSheet(GetAccountDownloadTemplateRequest request, CancellationToken cancellationToken)
        {
            var workbook = new XSSFWorkbook();
            var (sheet, listsSheet) = CreateSheets(workbook);

            var (debitAccounts, creditAccounts) = ProcessAccounts(request);

            // Define column indices
            int fixedColumnCount = 8; //iD, Num55, Num224,FileName, AuditorName, Collage, Fund, Details
            int debitAccountsStartIndex = fixedColumnCount;
            int totalDebitColumnIndex = debitAccountsStartIndex + debitAccounts.Count;
            int creditAccountsStartIndex = totalDebitColumnIndex + 1;
            int totalCreditColumnIndex = creditAccountsStartIndex + creditAccounts.Count;
            int netColumnIndex = totalCreditColumnIndex + 1;
            int totalColumnCount = netColumnIndex + 1;

            CreateHeaderRow(sheet, debitAccounts, creditAccounts, debitAccountsStartIndex, totalDebitColumnIndex, creditAccountsStartIndex, totalCreditColumnIndex, netColumnIndex);
            PopulateSampleData(sheet, debitAccounts, creditAccounts, debitAccountsStartIndex, totalDebitColumnIndex, creditAccountsStartIndex, totalCreditColumnIndex, netColumnIndex);

            await PopulateListsSheet(workbook, listsSheet, cancellationToken);
            ApplyDataValidation(sheet, listsSheet, workbook);

            ApplyStylesToCells(workbook, sheet, debitAccountsStartIndex, totalDebitColumnIndex, creditAccountsStartIndex, totalCreditColumnIndex, totalColumnCount);
            CreateSumRow(workbook, sheet, fixedColumnCount, debitAccountsStartIndex, totalDebitColumnIndex, creditAccountsStartIndex, totalCreditColumnIndex, netColumnIndex, totalColumnCount);

            using (var memoryStream = new MemoryStream())
            {
                workbook.Write(memoryStream);
                return memoryStream.ToArray();
            }
        }

        private (ISheet, ISheet) CreateSheets(IWorkbook workbook)
        {
            var sheet = workbook.CreateSheet("Sheet1");
            sheet.IsRightToLeft = true;
            sheet.CreateFreezePane(0, 2);

            var listsSheet = workbook.CreateSheet("Lists");
            listsSheet.IsRightToLeft = true;
            workbook.SetSheetVisibility(workbook.GetSheetIndex("Lists"), SheetVisibility.Hidden);

            return (sheet, listsSheet);
        }

        private (List<Account>, List<Account>) ProcessAccounts(GetAccountDownloadTemplateRequest request)
        {
            List<Account> debitAccounts = request.Accounts
                .Where(a => a.DebitAccountNumber.HasValue && !string.IsNullOrEmpty(a.DebitAccountName))
                .Select(a => new Account
                {
                    AccountName = a.DebitAccountName!,
                    Id = a.DebitAccountNumber.Value!
                })
                .ToList();

            List<Account> creditAccounts = request.Accounts
                .Where(a => a.CreditAccountNumber.HasValue && !string.IsNullOrEmpty(a.CreditAccountName))
                .Select(a => new Account
                {
                    AccountName = a.CreditAccountName!,
                    Id = a.CreditAccountNumber.Value
                })
                .ToList();

            return (debitAccounts, creditAccounts);
        }


        /*************  ✨ Windsurf Command ⭐  *************/
        /// <summary>
        /// Creates a header row for an Excel sheet with specified column titles and subaccount IDs.
        /// </summary>
        /// <param name="sheet">The Excel sheet where the header row will be created.</param>
        /// <param name="debitAccounts">List of debit accounts used to populate the header.</param>
        /*******  18066867-be99-49f5-a351-283229fdb41c  *******/
        private void CreateHeaderRow(ISheet sheet, List<Account> debitAccounts, List<Account> creditAccounts, int debitAccountsStartIndex, int totalDebitColumnIndex, int creditAccountsStartIndex, int totalCreditColumnIndex, int netColumnIndex)
        {

            // Create the first header row with ID numbers or codes
            var headerCodeRow = sheet.CreateRow(0);
            headerCodeRow.CreateCell(0).SetCellValue(string.Empty); // ID
            headerCodeRow.CreateCell(1).SetCellValue("A-1"); // Auditor Name
            headerCodeRow.CreateCell(2).SetCellValue("A-2"); // Num55
            headerCodeRow.CreateCell(3).SetCellValue("A-3"); // Num224
            headerCodeRow.CreateCell(4).SetCellValue("A-4"); // Form Name
            headerCodeRow.CreateCell(5).SetCellValue("A-5"); // College Name
            headerCodeRow.CreateCell(6).SetCellValue("A-6"); // Fund Name
            headerCodeRow.CreateCell(7).SetCellValue("A-7"); // Details
            headerCodeRow.CreateCell(8).SetCellValue(string.Empty); // Total Debit
            headerCodeRow.CreateCell(9).SetCellValue(string.Empty); // Total Credit

            // Add subaccount IDs to the first header row
            for (int i = 0; i < debitAccounts.Count; i++)
            {
                headerCodeRow.CreateCell(debitAccountsStartIndex + i).SetCellValue(debitAccounts[i].Id);
                //     headerCodeRow.CreateCell(debitAccountsStartIndex + i).SetCellValue(subAccounts[i].Id); // Use index+1 as ID
            }
            headerCodeRow.CreateCell(totalDebitColumnIndex).SetCellValue(string.Empty);

            for (int i = 0; i < creditAccounts.Count; i++)
            {
                headerCodeRow.CreateCell(creditAccountsStartIndex + i).SetCellValue(creditAccounts[i].Id);
            }
            headerCodeRow.CreateCell(totalCreditColumnIndex).SetCellValue(string.Empty);
            headerCodeRow.CreateCell(netColumnIndex).SetCellValue(string.Empty);


            var headerRow = sheet.CreateRow(1);
            headerRow.CreateCell(0).SetCellValue("م");
            headerRow.CreateCell(1).SetCellValue("المراجع");
            headerRow.CreateCell(2).SetCellValue("رقم 55");
            headerRow.CreateCell(3).SetCellValue("رقم 224");
            headerRow.CreateCell(4).SetCellValue("أسم الملف");
            headerRow.CreateCell(5).SetCellValue("الكليه");
            headerRow.CreateCell(6).SetCellValue("الصندوق");
            headerRow.CreateCell(7).SetCellValue("تفاصيل");

            for (int i = 0; i < debitAccounts.Count; i++)
            {
                headerRow.CreateCell(debitAccountsStartIndex + i).SetCellValue(debitAccounts[i].AccountName);
            }
            headerRow.CreateCell(totalDebitColumnIndex).SetCellValue("اجمالى مدين");

            for (int i = 0; i < creditAccounts.Count; i++)
            {
                headerRow.CreateCell(creditAccountsStartIndex + i).SetCellValue(creditAccounts[i].AccountName);
            }
            headerRow.CreateCell(totalCreditColumnIndex).SetCellValue("اجمالى دائن");
            headerRow.CreateCell(netColumnIndex).SetCellValue("الصافى");
        }

        private void PopulateSampleData(ISheet sheet, List<Account> debitAccounts, List<Account> creditAccounts, int debitAccountsStartIndex, int totalDebitColumnIndex, int creditAccountsStartIndex, int totalCreditColumnIndex, int netColumnIndex)
        {
            for (int i = 0; i < 200; i++)
            {
                var row = sheet.CreateRow(i + 2);
                row.CreateCell(0).SetCellValue(i + 1);

                for (int j = debitAccountsStartIndex; j < totalDebitColumnIndex; j++)
                {
                    row.CreateCell(j).SetCellValue(0);
                }

                if (debitAccounts.Count > 0)
                {
                    string debitStartColumnLetter = GetExcelColumnName(debitAccountsStartIndex);
                    string debitSumFormula;
                    int rowNum = i + 3;

                    if (debitAccounts.Count == 1)
                    {
                        debitSumFormula = "SUM(" + debitStartColumnLetter + rowNum + ")";
                    }
                    else
                    {
                        string debitEndColumnLetter = GetExcelColumnName(debitAccountsStartIndex + debitAccounts.Count - 1);
                        string debitRange = debitStartColumnLetter + rowNum + ":" + debitEndColumnLetter + rowNum;
                        debitSumFormula = "SUM(" + debitRange + ")";
                    }
                    row.CreateCell(totalDebitColumnIndex).SetCellFormula(debitSumFormula);
                }
                else
                {
                    row.CreateCell(totalDebitColumnIndex).SetCellValue(0);
                }

                for (int j = creditAccountsStartIndex; j < totalCreditColumnIndex; j++)
                {
                    row.CreateCell(j).SetCellValue(0);
                }

                if (creditAccounts.Count > 0)
                {
                    string creditStartColumnLetter = GetExcelColumnName(creditAccountsStartIndex);
                    string creditSumFormula;
                    int rowNum = i + 3;

                    if (creditAccounts.Count == 1)
                    {
                        creditSumFormula = "SUM(" + creditStartColumnLetter + rowNum + ")";
                    }
                    else
                    {
                        string creditEndColumnLetter = GetExcelColumnName(creditAccountsStartIndex + creditAccounts.Count - 1);
                        string creditRange = creditStartColumnLetter + rowNum + ":" + creditEndColumnLetter + rowNum;
                        creditSumFormula = "SUM(" + creditRange + ")";
                    }
                    row.CreateCell(totalCreditColumnIndex).SetCellFormula(creditSumFormula);
                }
                else
                {
                    row.CreateCell(totalCreditColumnIndex).SetCellValue(0);
                }

                if (debitAccounts.Count > 0 || creditAccounts.Count > 0)
                {
                    string totalDebitColumnLetter = GetExcelColumnName(totalDebitColumnIndex);
                    string totalCreditColumnLetter = GetExcelColumnName(totalCreditColumnIndex);
                    int rowNum = i + 3;
                    string netFormula = totalDebitColumnLetter + rowNum + "-" + totalCreditColumnLetter + rowNum;
                    row.CreateCell(netColumnIndex).SetCellFormula(netFormula);
                }
                else
                {
                    row.CreateCell(netColumnIndex).SetCellValue(0);
                }
            }
        }

        private async Task PopulateListsSheet(IWorkbook workbook, ISheet listsSheet, CancellationToken cancellationToken)
        {
            var collages = await _collageRepository.GetAll(null, cancellationToken);
            var funds = await _fundRepository.GetAll(null, cancellationToken);

            var collageListHeaderRow = listsSheet.CreateRow(0);
            collageListHeaderRow.CreateCell(0).SetCellValue("Collages");
            for (int i = 0; i < collages.Count; i++)
            {
                var row = listsSheet.GetRow(i + 1) ?? listsSheet.CreateRow(i + 1);
                row.CreateCell(0).SetCellValue(collages[i]?.CollageName ?? string.Empty);
            }

            var fundsByCollage = funds.Where(f => f != null).GroupBy(f => f!.CollageId).ToList();
            int fundColumnIndex = 1;

            foreach (var group in fundsByCollage)
            {
                var collage = collages.FirstOrDefault(c => c?.Id == group.Key);
                if (collage != null && collage.CollageName != null)
                {
                    string collageName = collage.CollageName;
                    string sanitizedCollageName = collageName.Replace(" ", "_");

                    var fundGroupHeaderRow = listsSheet.GetRow(0) ?? listsSheet.CreateRow(0);
                    fundGroupHeaderRow.CreateCell(fundColumnIndex).SetCellValue(collageName);

                    int fundRowIndex = 1;
                    foreach (var fund in group)
                    {
                        var row = listsSheet.GetRow(fundRowIndex) ?? listsSheet.CreateRow(fundRowIndex);
                        row.CreateCell(fundColumnIndex).SetCellValue(fund?.FundName ?? string.Empty);
                        fundRowIndex++;
                    }

                    string rangeAddress = $"Lists!${GetExcelColumnName(fundColumnIndex)}$2:${GetExcelColumnName(fundColumnIndex)}${fundRowIndex}";
                    var name = workbook.CreateName();
                    name.NameName = sanitizedCollageName;
                    name.RefersToFormula = rangeAddress;

                    fundColumnIndex++;
                }
            }
        }

        private void ApplyDataValidation(ISheet sheet, ISheet listsSheet, IWorkbook workbook)
        {
            var dvHelper = new XSSFDataValidationHelper((XSSFSheet)sheet);

            var collages = listsSheet.GetRow(0).Cells
                // .Where(c => c.ColumnIndex == 0)
                .Select(c => c.StringCellValue)
                .ToList();

            if (collages != null && collages.Any())
            {
                string collageListRange = $"Lists!$A$2:$A${collages.Count}";
                var collageConstraint = dvHelper.CreateFormulaListConstraint(collageListRange);
                var collageAddressList = new NPOI.SS.Util.CellRangeAddressList(2, 1001, 5, 5);
                var collageValidation = dvHelper.CreateValidation(collageConstraint, collageAddressList);
                collageValidation.ShowErrorBox = true;
                sheet.AddValidationData(collageValidation);
            }

            var fundAddressList = new NPOI.SS.Util.CellRangeAddressList(2, 1001, 6, 6);
            string fundValidationFormula = "INDIRECT(SUBSTITUTE(F3,\" \",\"_\"))";
            var fundConstraint = dvHelper.CreateFormulaListConstraint(fundValidationFormula);
            var fundValidation = dvHelper.CreateValidation(fundConstraint, fundAddressList);
            fundValidation.ShowErrorBox = true;
            sheet.AddValidationData(fundValidation);
        }

        private void ApplyStylesToCells(IWorkbook workbook, ISheet sheet, int debitAccountsStartIndex, int totalDebitColumnIndex, int creditAccountsStartIndex, int totalCreditColumnIndex, int totalColumnCount)
        {
            ICellStyle borderStyle = workbook.CreateCellStyle();
            borderStyle.BorderTop = BorderStyle.Thin;
            borderStyle.BorderBottom = BorderStyle.Thin;
            borderStyle.BorderLeft = BorderStyle.Thin;
            borderStyle.BorderRight = BorderStyle.Thin;

            ICellStyle debitStyle = workbook.CreateCellStyle();
            debitStyle.CloneStyleFrom(borderStyle);
            debitStyle.FillForegroundColor = IndexedColors.LightGreen.Index;
            debitStyle.FillPattern = FillPattern.SolidForeground;

            ICellStyle creditStyle = workbook.CreateCellStyle();
            creditStyle.CloneStyleFrom(borderStyle);
            creditStyle.FillForegroundColor = IndexedColors.LightYellow.Index;
            creditStyle.FillPattern = FillPattern.SolidForeground;

            IFont headerFont = workbook.CreateFont();
            headerFont.IsBold = true;
            headerFont.FontHeightInPoints = 14;
            ICellStyle headerStyle = workbook.CreateCellStyle();
            headerStyle.CloneStyleFrom(borderStyle);
            headerStyle.SetFont(headerFont);
            headerStyle.FillForegroundColor = IndexedColors.Grey25Percent.Index;
            headerStyle.FillPattern = FillPattern.SolidForeground;

            for (int rowIdx = 0; rowIdx <= 1; rowIdx++)
            {
                var row = sheet.GetRow(rowIdx);
                if (row != null)
                {
                    for (int colIndex = 0; colIndex < totalColumnCount; colIndex++)
                    {
                        var cell = row.GetCell(colIndex) ?? row.CreateCell(colIndex);
                        cell.CellStyle = headerStyle;

                        var style = workbook.CreateCellStyle();
                        style.CloneStyleFrom(headerStyle);
                        style.BorderTop = (rowIdx == 0) ? BorderStyle.Thick : BorderStyle.Thin;
                        style.BorderBottom = (rowIdx == 1) ? BorderStyle.Thick : BorderStyle.Thin;
                        style.BorderLeft = (colIndex == 0) ? BorderStyle.Thick : BorderStyle.Thin;
                        style.BorderRight = (colIndex == totalColumnCount - 1) ? BorderStyle.Thick : BorderStyle.Thin;
                        cell.CellStyle = style;
                    }
                }
            }

            for (int rowIndex = 0; rowIndex <= 201; rowIndex++)
            {
                var row = sheet.GetRow(rowIndex);
                if (row != null)
                {
                    for (int colIndex = 0; colIndex < totalColumnCount; colIndex++)
                    {
                        var cell = row.GetCell(colIndex) ?? row.CreateCell(colIndex);

                        if (colIndex >= debitAccountsStartIndex && colIndex <= totalDebitColumnIndex)
                        {
                            cell.CellStyle = debitStyle;
                        }
                        else if (colIndex >= creditAccountsStartIndex && colIndex <= totalCreditColumnIndex)
                        {
                            cell.CellStyle = creditStyle;
                        }
                        else
                        {
                            cell.CellStyle = borderStyle;
                        }
                    }
                }
            }
        }

        private void CreateSumRow(IWorkbook workbook, ISheet sheet, int fixedColumnCount, int debitAccountsStartIndex, int totalDebitColumnIndex, int creditAccountsStartIndex, int totalCreditColumnIndex, int netColumnIndex, int totalColumnCount)
        {
            var sumRowIndex = 202;
            var sumRow = sheet.CreateRow(sumRowIndex);

            sumRow.CreateCell(0).SetCellValue("الاجمالى");
            var cell0 = sumRow.GetCell(0) ?? sumRow.CreateCell(0);
            ICellStyle borderStyle = workbook.CreateCellStyle();
            borderStyle.BorderTop = BorderStyle.Thin;
            borderStyle.BorderBottom = BorderStyle.Thin;
            borderStyle.BorderLeft = BorderStyle.Thin;
            borderStyle.BorderRight = BorderStyle.Thin;
            cell0.CellStyle = borderStyle;
            var cellRangeAddress = new CellRangeAddress(sumRowIndex, sumRowIndex, 0, fixedColumnCount - 1);
            sheet.AddMergedRegion(cellRangeAddress);

            for (int colIndex = debitAccountsStartIndex; colIndex < totalDebitColumnIndex; colIndex++)
            {
                string columnLetter = GetExcelColumnName(colIndex);
                string sumFormula = $"SUM({columnLetter}3:{columnLetter}202)";
                sumRow.CreateCell(colIndex).SetCellFormula(sumFormula);
            }

            string sumTotalDebitColumnLetter = GetExcelColumnName(totalDebitColumnIndex);
            string totalDebitSumFormula = $"SUM({sumTotalDebitColumnLetter}3:{sumTotalDebitColumnLetter}{sumRowIndex})";
            sumRow.CreateCell(totalDebitColumnIndex).SetCellFormula(totalDebitSumFormula);

            for (int colIndex = creditAccountsStartIndex; colIndex < totalCreditColumnIndex; colIndex++)
            {
                string columnLetter = GetExcelColumnName(colIndex);
                string sumFormula = $"SUM({columnLetter}3:{columnLetter}202)";
                sumRow.CreateCell(colIndex).SetCellFormula(sumFormula);
            }

            string sumTotalCreditColumnLetter = GetExcelColumnName(totalCreditColumnIndex);
            string totalCreditSumFormula = $"SUM({sumTotalCreditColumnLetter}3:{sumTotalCreditColumnLetter}{sumRowIndex})";
            sumRow.CreateCell(totalCreditColumnIndex).SetCellFormula(totalCreditSumFormula);

            string netColumnLetter = GetExcelColumnName(netColumnIndex);
            string netSumFormula = $"SUM({netColumnLetter}3:{netColumnLetter}202)";
            sumRow.CreateCell(netColumnIndex).SetCellFormula(netSumFormula);

            ICellStyle sumRowStyle = workbook.CreateCellStyle();
            IFont sumFont = workbook.CreateFont();
            sumFont.IsBold = true;
            sumRowStyle.SetFont(sumFont);
            sumRowStyle.CloneStyleFrom(borderStyle);

            for (int colIndex = 0; colIndex < totalColumnCount; colIndex++)
            {
                var cell = sumRow.GetCell(colIndex) ?? sumRow.CreateCell(colIndex);
                cell.CellStyle = sumRowStyle;
            }
        }

        // private string GetExcelColumnName(int columnNumber)
        // {
        //     return ExcelHelper.GetExcelColumnName(columnNumber);
        // }
    }

}

