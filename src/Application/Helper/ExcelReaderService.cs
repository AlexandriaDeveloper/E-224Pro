using System.Data;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.HSSF.UserModel;
using System.Reflection;

namespace Application.Helper;

public interface IExcelReaderService
{
    /// <summary>
    /// Reads all sheets from an Excel file and returns a dictionary of sheet name to DataTable.
    /// </summary>
    /// <param name="filePath">The path to the Excel file.</param>
    /// <returns>Dictionary<string, DataTable> where key is sheet name and value is DataTable.</returns>
    Dictionary<string, DataTable> ReadAllSheetsAsTables(string filePath);

    /// <summary>
    /// Reads all sheets from an Excel file and returns a dictionary of sheet name to list of T.
    /// </summary>
    /// <typeparam name="T">The type to map each row to.</typeparam>
    /// <param name="filePath">The path to the Excel file.</param>
    /// <returns>Dictionary<string, List<T>> where key is sheet name and value is list of T.</returns>
    Dictionary<string, List<T>> ReadAllSheetsAsList<T>(string filePath) where T : new();
}

// public class ExcelReaderService : IExcelReaderService
// {
//     public Dictionary<string, DataTable> ReadAllSheetsAsTables(string filePath)
//     {
//         var result = new Dictionary<string, DataTable>();
//         using var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
//         IWorkbook workbook = filePath.EndsWith(".xlsx") ? new XSSFWorkbook(stream) : new HSSFWorkbook(stream);
//         for (int i = 0; i < workbook.NumberOfSheets; i++)
//         {
//             var sheet = workbook.GetSheetAt(i);
//             var table = new DataTable(sheet.SheetName);
//             var headerRow = sheet.GetRow(0);
//             if (headerRow == null) continue;
//             for (int j = 0; j < headerRow.LastCellNum; j++)
//             {
//                 table.Columns.Add(headerRow.GetCell(j)?.ToString() ?? $"Column{j}");
//             }
//             for (int rowIdx = 1; rowIdx <= sheet.LastRowNum; rowIdx++)
//             {
//                 var row = sheet.GetRow(rowIdx);
//                 if (row == null) continue;
//                 var dataRow = table.NewRow();
//                 for (int colIdx = 0; colIdx < table.Columns.Count; colIdx++)
//                 {
//                     dataRow[colIdx] = row.GetCell(colIdx)?.ToString() ?? string.Empty;
//                 }
//                 table.Rows.Add(dataRow);
//             }
//             result[sheet.SheetName] = table;
//         }
//         return result;
//     }

//     public DataTable ReadAllSheetsAsList<T>(string filePath) where T : new()
//     {
//         var result = new Dictionary<string, List<T>>();
//         using var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
//         IWorkbook workbook = filePath.EndsWith(".xlsx") ? new XSSFWorkbook(stream) : new HSSFWorkbook(stream);

//         for (int i = 0; i < workbook.NumberOfSheets; i++)
//         {
//             var sheet = workbook.GetSheetAt(i);
//             var list = new List<T>();

//             // Read header row (Excel row 2, index 1)
//             var headerRow = sheet.GetRow(1);
//             if (headerRow == null) continue;

//             // Create a mapping from header text to property info
//             var props = typeof(T).GetProperties().ToDictionary(p => p.Name, StringComparer.InvariantCultureIgnoreCase);
//             var headerMap = new Dictionary<int, PropertyInfo>();
//             for (int colIdx = 0; colIdx < headerRow.LastCellNum; colIdx++)
//             {
//                 var headerText = headerRow.GetCell(colIdx)?.ToString()?.Trim().Replace(" ", ""); // Sanitize header text
//                 if (!string.IsNullOrEmpty(headerText) && props.TryGetValue(headerText, out var prop))
//                 {
//                     headerMap[colIdx] = prop;
//                 }
//             }

//             // Find the sum row index (row with "الاجمالى" in the first column)
//             int sumRowIndex = sheet.LastRowNum + 1; // Default to end of sheet if not found
//             for (int rowIdx = 2; rowIdx <= sheet.LastRowNum; rowIdx++)
//             {
//                 var row = sheet.GetRow(rowIdx);
//                 if (row != null && row.GetCell(0)?.ToString()?.Trim() == "الاجمالى")
//                 {
//                     sumRowIndex = rowIdx;
//                     break;
//                 }
//             }

//             // Read data rows (starting from Excel row 3, index 2, up to the row before the sum row)
//             for (int rowIdx = 2; rowIdx < sumRowIndex; rowIdx++)
//             {
//                 var row = sheet.GetRow(rowIdx);
//                 if (row == null) continue;

//                 var obj = new T();
//                 foreach (var entry in headerMap)
//                 {
//                     int colIdx = entry.Key;
//                     PropertyInfo prop = entry.Value;

//                     var cell = row.GetCell(colIdx);
//                     var cellValue = cell?.ToString();

//                     if (!string.IsNullOrEmpty(cellValue) && prop.CanWrite)
//                     {
//                         try
//                         {
//                             var safeValue = Convert.ChangeType(cellValue, prop.PropertyType);
//                             prop.SetValue(obj, safeValue);
//                         }
//                         catch
//                         {
//                             // Ignore conversion errors for now
//                         }
//                     }
//                 }
//                 list.Add(obj);
//             }
//             result[sheet.SheetName] = list;
//         }
//         return result;
//     }
// }
