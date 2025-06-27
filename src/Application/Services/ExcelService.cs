using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.Collections.Generic;
using System.IO;
using Core.Interfaces.Repository;
using Core.Models;
using System.Linq;
using NPOI.SS.UserModel.Charts;
using NPOI.SS.Util;
using Shared.Contracts.FormDetailsRequest;
using Microsoft.IdentityModel.Tokens;
using QuestPDF.Helpers;

namespace Application.Services
{
    public class ExcelService
    {
        private readonly ICollageRepository _collageRepository;
        private readonly IFundRepository _fundRepository;
        private readonly IAccountRepository _accountRepository;

        public ExcelService(ICollageRepository collageRepository, IFundRepository fundRepository, IAccountRepository accountRepository)
        {
            _collageRepository = collageRepository;
            _fundRepository = fundRepository;
            _accountRepository = accountRepository;

        }

        private async Task<List<Account>> LoadAccounts()
        {
            var accounts = await _accountRepository.GetAll(null);
            return accounts;

        }

        // Helper function to convert 0-based column index to Excel column name (A, B, ..., AA, AB, ...)
        private string GetExcelColumnName(int columnIndex)
        {
            // columnIndex is 0-based
            int dividend = columnIndex + 1; // Convert to 1-based for calculation
            string columnName = String.Empty;
            int modulo;

            while (dividend > 0)
            {
                modulo = (dividend - 1) % 26;
                columnName = Convert.ToChar(65 + modulo).ToString() + columnName;
                dividend = (dividend - modulo) / 26;
            }
            return columnName;
        }

        public async Task<byte[]> GenerateTemplateExcelSheet(GetAccountDownloadTemplateRequest request, CancellationToken cancellationToken)
        {
            var workbook = new XSSFWorkbook();
            var sheet = workbook.CreateSheet("Sheet1");
            sheet.IsRightToLeft = true; // Set the sheet to be right-to-left
            sheet.CreateFreezePane(0, 2); // Freeze the first 2 rows
            var listsSheet = workbook.CreateSheet("Lists");
            listsSheet.IsRightToLeft = true; // Set the Lists sheet to be right-to-left

            // Hide the Lists sheet
            workbook.SetSheetVisibility(workbook.GetSheetIndex("Lists"), SheetVisibility.Hidden);

            // Load accounts to determine debit/credit columns
            // var allAccounts = await LoadAccounts();
            List<Account> debitAccounts = request.Accounts
                .Where(a => !a.DebitAccountNumber.IsNullOrEmpty() && !string.IsNullOrEmpty(a.DebitAccountName))
                .Select(a => new Account
                {
                    AccountName = a.DebitAccountName!,
                    AccountNumber = a.DebitAccountNumber!
                })
                .ToList();
            List<Account> creditAccounts = request.Accounts
                .Where(a => !a.CreditAccountNumber.IsNullOrEmpty() && !string.IsNullOrEmpty(a.CreditAccountName))
                .Select(a => new Account
                {
                    AccountName = a.CreditAccountName!,
                    AccountNumber = a.CreditAccountNumber!
                })
                .ToList();

            // Define column indices
            int fixedColumnCount = 8; //iD, Num55, Num224,FileName, AuditorName, Collage, Fund, Details
            int debitAccountsStartIndex = fixedColumnCount;
            int totalDebitColumnIndex = debitAccountsStartIndex + debitAccounts.Count;
            int creditAccountsStartIndex = totalDebitColumnIndex + 1;
            int totalCreditColumnIndex = creditAccountsStartIndex + creditAccounts.Count;
            int netColumnIndex = totalCreditColumnIndex + 1;
            int totalColumnCount = netColumnIndex + 1;

            // Create header Code row (Row 1, index 0)
            var headerCodeRow = sheet.CreateRow(0);
            headerCodeRow.CreateCell(1).SetCellValue("A-1"); // Num55 Code?
            headerCodeRow.CreateCell(2).SetCellValue("A-2"); // Num224 Code?
            headerCodeRow.CreateCell(3).SetCellValue("A-3"); // AuditorName Code?
            headerCodeRow.CreateCell(4).SetCellValue("A-4"); // AuditorName Code?
            headerCodeRow.CreateCell(5).SetCellValue("A-5"); // Collage Code?
            headerCodeRow.CreateCell(6).SetCellValue("A-6"); // Fund Code?
            headerCodeRow.CreateCell(7).SetCellValue("A-7"); // Details?


            // Add Debit Account Numbers to code header row
            for (int i = 0; i < debitAccounts.Count; i++)
            {
                headerCodeRow.CreateCell(debitAccountsStartIndex + i).SetCellValue(debitAccounts[i].AccountNumber);
                //set header font 
                var cell = headerCodeRow.GetCell(debitAccountsStartIndex + i);
                if (cell != null)
                {
                    IFont font = workbook.CreateFont();
                    font.IsBold = true; // Set the font to bold
                    cell.CellStyle = workbook.CreateCellStyle();
                    cell.CellStyle.SetFont(font);
                }
            }
            // Add Total Debit Code header
            headerCodeRow.CreateCell(totalDebitColumnIndex).SetCellValue("Total Debit");

            // Add Credit Account Numbers to code header row
            for (int i = 0; i < creditAccounts.Count; i++)
            {
                headerCodeRow.CreateCell(creditAccountsStartIndex + i).SetCellValue(creditAccounts[i].AccountNumber);
                var cell = headerCodeRow.GetCell(creditAccountsStartIndex + i);
                if (cell != null)
                {
                    IFont font = workbook.CreateFont();
                    font.IsBold = true; // Set the font to bold
                    cell.CellStyle = workbook.CreateCellStyle();
                    cell.CellStyle.SetFont(font);
                }
            }
            // Add Total Credit Code header
            headerCodeRow.CreateCell(totalCreditColumnIndex).SetCellValue("Total Credit");
            // Add Net Code header
            headerCodeRow.CreateCell(netColumnIndex).SetCellValue("Net");


            // Create header row (Row 2, index 1)
            var headerRow = sheet.CreateRow(1);
            headerRow.CreateCell(0).SetCellValue("م");

            headerRow.CreateCell(1).SetCellValue("رقم 55");
            headerRow.CreateCell(2).SetCellValue("رقم 224");
            headerRow.CreateCell(3).SetCellValue("أسم الملف");
            headerRow.CreateCell(4).SetCellValue("المراجع");
            headerRow.CreateCell(5).SetCellValue("الكليه");
            headerRow.CreateCell(6).SetCellValue("الصندوق");

            headerRow.CreateCell(7).SetCellValue("تفاصيل");

            // Add Debit Account Names to name header row
            for (int i = 0; i < debitAccounts.Count; i++)
            {
                headerRow.CreateCell(debitAccountsStartIndex + i).SetCellValue(debitAccounts[i].AccountName);
            }
            // Add Total Debit Name header
            headerRow.CreateCell(totalDebitColumnIndex).SetCellValue("اجمالى مدين");

            // Add Credit Account Names to name header row
            for (int i = 0; i < creditAccounts.Count; i++)
            {
                headerRow.CreateCell(creditAccountsStartIndex + i).SetCellValue(creditAccounts[i].AccountName);
            }
            // Add Total Credit Name header
            headerRow.CreateCell(totalCreditColumnIndex).SetCellValue("اجمالى دائن");
            // Add Net Name header
            headerRow.CreateCell(netColumnIndex).SetCellValue("الصافى");

            // Add some sample data - need to adjust row index and column indices
            // Sample data should start from row 3 (index 2)
            // Columns 0-6 are fixed, columns 7 onwards are for Debit accounts, then Total Debit, Credit Accounts, Total Credit, Net
            for (int i = 0; i < 200; i++) // Loop 200 times for 200 rows of sample data
            {
                var row = sheet.CreateRow(i + 2); // Start from row index 2 (Excel row 3)
                row.CreateCell(0).SetCellValue(i + 1);


                // Add empty cells (or 0) for Debit account columns in sample data rows
                for (int j = debitAccountsStartIndex; j < totalDebitColumnIndex; j++)
                {
                    row.CreateCell(j).SetCellValue(0); // Use 0 for numeric columns
                }

                // Add formula for Total Debit
                // The formula sums the range of debit account columns for the current row
                if (debitAccounts.Count > 0)
                {
                    string debitStartColumnLetter = GetExcelColumnName(debitAccountsStartIndex);
                    string debitSumFormula;
                    int rowNum = i + 3; // Excel row number (3-based)

                    if (debitAccounts.Count == 1)
                    {
                        // Formula for a single cell, e.g., SUM(H3)
                        debitSumFormula = "SUM(" + debitStartColumnLetter + rowNum + ")";
                    }
                    else
                    {
                        string debitEndColumnLetter = GetExcelColumnName(debitAccountsStartIndex + debitAccounts.Count - 1);
                        // Construct the range string explicitly
                        string debitRange = debitStartColumnLetter + rowNum + ":" + debitEndColumnLetter + rowNum;
                        debitSumFormula = "SUM(" + debitRange + ")";
                    }
                    row.CreateCell(totalDebitColumnIndex).SetCellFormula(debitSumFormula);
                }
                else
                {
                    // If no debit accounts, total debit is 0
                    row.CreateCell(totalDebitColumnIndex).SetCellValue(0);
                }

                // Add empty cells (or 0) for Credit account columns in sample data rows
                for (int j = creditAccountsStartIndex; j < totalCreditColumnIndex; j++)
                {
                    row.CreateCell(j).SetCellValue(0); // Use 0 for numeric columns
                }

                // Add formula for Total Credit
                // The formula sums the range of credit account columns for the current row
                if (creditAccounts.Count > 0)
                {
                    string creditStartColumnLetter = GetExcelColumnName(creditAccountsStartIndex);
                    string creditSumFormula;
                    int rowNum = i + 3; // Excel row number (3-based)

                    if (creditAccounts.Count == 1)
                    {
                        creditSumFormula = "SUM(" + creditStartColumnLetter + rowNum + ")";
                    }
                    else
                    {
                        string creditEndColumnLetter = GetExcelColumnName(creditAccountsStartIndex + creditAccounts.Count - 1);
                        // Construct the range string explicitly
                        string creditRange = creditStartColumnLetter + rowNum + ":" + creditEndColumnLetter + rowNum;
                        creditSumFormula = "SUM(" + creditRange + ")";
                    }
                    row.CreateCell(totalCreditColumnIndex).SetCellFormula(creditSumFormula);
                }
                else
                {
                    // If no credit accounts, total credit is 0
                    row.CreateCell(totalCreditColumnIndex).SetCellValue(0);
                }

                // Add formula for Net (Total Debit - Total Credit)
                // Only add the net formula if there are either debit or credit accounts
                if (debitAccounts.Count > 0 || creditAccounts.Count > 0)
                {
                    string totalDebitColumnLetter = GetExcelColumnName(totalDebitColumnIndex);
                    string totalCreditColumnLetter = GetExcelColumnName(totalCreditColumnIndex);
                    int rowNum = i + 3; // Excel row number (3-based)
                    string netFormula = totalDebitColumnLetter + rowNum + "-" + totalCreditColumnLetter + rowNum;
                    row.CreateCell(netColumnIndex).SetCellFormula(netFormula);
                }
                else
                {
                    // If no debit or credit accounts, net is 0
                    row.CreateCell(netColumnIndex).SetCellValue(0);
                }
            }

            // Populate Lists sheet with Collages and Funds
            var collages = await _collageRepository.GetAll(null, cancellationToken);
            var funds = await _fundRepository.GetAll(null, cancellationToken);

            // Write Collages to Lists sheet (Column A)
            var collageListHeaderRow = listsSheet.CreateRow(0);
            collageListHeaderRow.CreateCell(0).SetCellValue("Collages");
            for (int i = 0; i < collages.Count; i++)
            {
                var row = listsSheet.GetRow(i + 1) ?? listsSheet.CreateRow(i + 1);
                row.CreateCell(0).SetCellValue(collages[i]?.CollageName ?? string.Empty);
            }

            // Write Funds to Lists sheet, grouped by Collage (Starting from Column B)
            // Assuming CollageId is int, not int?
            var fundsByCollage = funds.Where(f => f != null).GroupBy(f => f!.CollageId).ToList(); // Use null-forgiving operator
            int fundColumnIndex = 1;

            foreach (var group in fundsByCollage)
            {
                // Ensure group key (CollageId) is not null, although it should be int
                var collage = collages.FirstOrDefault(c => c?.Id == group.Key);
                if (collage != null && collage.CollageName != null)
                {
                    string collageName = collage.CollageName;
                    // Sanitize collage name for named range (replace spaces with underscores)
                    string sanitizedCollageName = collageName.Replace(" ", "_");

                    // Write Collage name as header for this fund list column
                    var fundGroupHeaderRow = listsSheet.GetRow(0) ?? listsSheet.CreateRow(0);
                    fundGroupHeaderRow.CreateCell(fundColumnIndex).SetCellValue(collageName);

                    // Write funds for this collage
                    int fundRowIndex = 1;
                    foreach (var fund in group)
                    {
                        var row = listsSheet.GetRow(fundRowIndex) ?? listsSheet.CreateRow(fundRowIndex);
                        row.CreateCell(fundColumnIndex).SetCellValue(fund?.FundName ?? string.Empty);
                        fundRowIndex++;
                    }

                    // Create a Named Range for this list of funds
                    // The range covers the cells containing fund names for this collage
                    // The range starts from row 2 (index 1) and goes down to the last fund row
                    string rangeAddress = $"Lists!${GetExcelColumnName(fundColumnIndex)}$2:${GetExcelColumnName(fundColumnIndex)}${fundRowIndex}";
                    var name = workbook.CreateName();
                    name.NameName = sanitizedCollageName;
                    name.RefersToFormula = rangeAddress;

                    fundColumnIndex++;
                }
            }

            // Apply Data Validation to main sheet
            var dvHelper = new XSSFDataValidationHelper((XSSFSheet)sheet);

            // Collage Data Validation (Dropdown list referencing Lists sheet Column A)
            if (collages != null && collages.Any())
            {
                string collageListRange = $"Lists!$A$2:$A${collages.Count + 1}"; // List is on Lists sheet, starts row 2
                var collageConstraint = dvHelper.CreateFormulaListConstraint(collageListRange);
                var collageAddressList = new NPOI.SS.Util.CellRangeAddressList(2, 1001, 5, 5); // Apply to column D (index 3), rows 3 to 1002
                var collageValidation = dvHelper.CreateValidation(collageConstraint, collageAddressList);
                collageValidation.ShowErrorBox = true;
                sheet.AddValidationData(collageValidation);
            }

            // Fund Data Validation (Dependent dropdown using INDIRECT and Named Ranges)
            // The formula INDIRECT(SUBSTITUTE(D3," ","_")) will look for a Named Range whose name is the value in cell D3 (the selected Collage), with spaces replaced by underscores.
            // This formula is applied relative to the top-left cell of the validation range (D3).
            var fundAddressList = new NPOI.SS.Util.CellRangeAddressList(2, 1001, 6, 6); // Apply to column E (index 4), rows 3 to 1002
            string fundValidationFormula = "INDIRECT(SUBSTITUTE(F3,\" \",\"_\"))"; // Reference D3 for the first row of validation
            var fundConstraint = dvHelper.CreateFormulaListConstraint(fundValidationFormula);
            var fundValidation = dvHelper.CreateValidation(fundConstraint, fundAddressList);
            fundValidation.ShowErrorBox = true;
            sheet.AddValidationData(fundValidation);



            // Create cell styles
            ICellStyle borderStyle = workbook.CreateCellStyle();
            borderStyle.BorderTop = BorderStyle.Thin;
            borderStyle.BorderBottom = BorderStyle.Thin;
            borderStyle.BorderLeft = BorderStyle.Thin;
            borderStyle.BorderRight = BorderStyle.Thin;

            // Create debit column style (light green)
            ICellStyle debitStyle = workbook.CreateCellStyle();
            debitStyle.CloneStyleFrom(borderStyle);
            debitStyle.FillForegroundColor = IndexedColors.LightGreen.Index;
            debitStyle.FillPattern = FillPattern.SolidForeground;

            // Create credit column style (light yellow)
            ICellStyle creditStyle = workbook.CreateCellStyle();
            creditStyle.CloneStyleFrom(borderStyle);
            creditStyle.FillForegroundColor = IndexedColors.LightYellow.Index;
            creditStyle.FillPattern = FillPattern.SolidForeground;

            // Create header style (large bold font, thick outside border, thin inside, darker background)
            IFont headerFont = workbook.CreateFont();
            headerFont.IsBold = true;
            headerFont.FontHeightInPoints = 14; // Larger font size
            ICellStyle headerStyle = workbook.CreateCellStyle();
            headerStyle.CloneStyleFrom(borderStyle);
            headerStyle.SetFont(headerFont);
            headerStyle.FillForegroundColor = IndexedColors.Grey25Percent.Index;
            headerStyle.FillPattern = FillPattern.SolidForeground;

            // Apply header style to all cells in row 0 and row 1, with thick outside border
            for (int rowIdx = 0; rowIdx <= 1; rowIdx++)
            {
                var row = sheet.GetRow(rowIdx);
                if (row != null)
                {
                    for (int colIndex = 0; colIndex < totalColumnCount; colIndex++)
                    {
                        var cell = row.GetCell(colIndex) ?? row.CreateCell(colIndex);
                        cell.CellStyle = headerStyle;

                        // Set thick border only on the outside of the header area
                        var style = workbook.CreateCellStyle();
                        style.CloneStyleFrom(headerStyle);
                        // Top border thick for first header row
                        style.BorderTop = (rowIdx == 0) ? BorderStyle.Thick : BorderStyle.Thin;
                        // Bottom border thick for last header row
                        style.BorderBottom = (rowIdx == 1) ? BorderStyle.Thick : BorderStyle.Thin;
                        // Left border thick for first column
                        style.BorderLeft = (colIndex == 0) ? BorderStyle.Thick : BorderStyle.Thin;
                        // Right border thick for last column
                        style.BorderRight = (colIndex == totalColumnCount - 1) ? BorderStyle.Thick : BorderStyle.Thin;
                        cell.CellStyle = style;
                    }
                }
            }

            // Apply styles to all cells in the table
            for (int rowIndex = 0; rowIndex <= 201; rowIndex++) // Include headers and 200 data rows
            {
                var row = sheet.GetRow(rowIndex);
                if (row != null)
                {
                    for (int colIndex = 0; colIndex < totalColumnCount; colIndex++)
                    {
                        var cell = row.GetCell(colIndex) ?? row.CreateCell(colIndex);

                        // Apply border style to all cells
                        if (colIndex >= debitAccountsStartIndex && colIndex <= totalDebitColumnIndex)
                        {
                            // Apply debit style (light green) to debit columns and total debit
                            cell.CellStyle = debitStyle;
                        }
                        else if (colIndex >= creditAccountsStartIndex && colIndex <= totalCreditColumnIndex)
                        {
                            // Apply credit style (light yellow) to credit columns and total credit
                            cell.CellStyle = creditStyle;
                        }
                        else
                        {
                            // Apply border style to other columns
                            cell.CellStyle = borderStyle;
                        }
                    }
                }
            }

            // Create a final row for sums
            var sumRowIndex = 202; // After 200 data rows (indices 2 to 201)
            var sumRow = sheet.CreateRow(sumRowIndex);

            // Add a label for the sum row
            sumRow.CreateCell(0).SetCellValue("الاجمالى");
            //Set cell0 to span 8 cells
            var cell0 = sumRow.GetCell(0) ?? sumRow.CreateCell(0);
            cell0.CellStyle = borderStyle; // Apply border style to the label cell
            // Set cell0 to span 8 cells
            var cellRangeAddress = new CellRangeAddress(sumRowIndex, sumRowIndex, 0, fixedColumnCount - 1);
            sheet.AddMergedRegion(cellRangeAddress);


            // Add formulas for sum of debit account columns
            for (int colIndex = debitAccountsStartIndex; colIndex < totalDebitColumnIndex; colIndex++)
            {
                string columnLetter = GetExcelColumnName(colIndex);
                // Hardcode the end row number (202) for debugging formula parsing
                string sumFormula = $"SUM({columnLetter}3:{columnLetter}202)";
                sumRow.CreateCell(colIndex).SetCellFormula(sumFormula);
            }

            // Add formula for sum of Total Debit column
            string sumTotalDebitColumnLetter = GetExcelColumnName(totalDebitColumnIndex);
            // Corrected formula: sum up to the row *before* the sum row (Excel row sumRowIndex)
            string totalDebitSumFormula = $"SUM({sumTotalDebitColumnLetter}3:{sumTotalDebitColumnLetter}{sumRowIndex})";
            sumRow.CreateCell(totalDebitColumnIndex).SetCellFormula(totalDebitSumFormula);

            // Add formulas for sum of credit account columns
            for (int colIndex = creditAccountsStartIndex; colIndex < totalCreditColumnIndex; colIndex++)
            {
                string columnLetter = GetExcelColumnName(colIndex);
                // Hardcode the end row number (202) for debugging formula parsing
                string sumFormula = $"SUM({columnLetter}3:{columnLetter}202)";
                sumRow.CreateCell(colIndex).SetCellFormula(sumFormula);
            }

            // Add formula for sum of Total Credit column
            string sumTotalCreditColumnLetter = GetExcelColumnName(totalCreditColumnIndex);
            // Corrected formula
            string totalCreditSumFormula = $"SUM({sumTotalCreditColumnLetter}3:{sumTotalCreditColumnLetter}{sumRowIndex})";
            sumRow.CreateCell(totalCreditColumnIndex).SetCellFormula(totalCreditSumFormula);

            // Add formula for sum of Net column
            string netColumnLetter = GetExcelColumnName(netColumnIndex);
            // Hardcode the end row number (202) for debugging formula parsing
            string netSumFormula = $"SUM({netColumnLetter}3:{netColumnLetter}202)";
            sumRow.CreateCell(netColumnIndex).SetCellFormula(netSumFormula);

            // Optional: Apply a style to the sum row (e.g., bold font)
            ICellStyle sumRowStyle = workbook.CreateCellStyle();
            IFont sumFont = workbook.CreateFont();
            sumFont.IsBold = true;
            sumRowStyle.SetFont(sumFont);
            // Apply border style to sum row cells
            sumRowStyle.CloneStyleFrom(borderStyle);

            for (int colIndex = 0; colIndex < totalColumnCount; colIndex++)
            {
                var cell = sumRow.GetCell(colIndex) ?? sumRow.CreateCell(colIndex);
                cell.CellStyle = sumRowStyle;
            }

            using (var memoryStream = new MemoryStream())
            {
                workbook.Write(memoryStream);
                return memoryStream.ToArray();
            }
        }
    }
}