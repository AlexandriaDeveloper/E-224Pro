using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

public class NpoiService
{
    private string _filePath;
    private IWorkbook _workbook;


    public NpoiService(string filePath)
    {
        // Initialize the service with the file path
        if (string.IsNullOrEmpty(filePath))
        {
            throw new ArgumentException("File path cannot be null or empty", nameof(filePath));
        }
        _filePath = filePath;
        if (filePath.EndsWith(".xlsx"))
        {
            // Load the Excel file
            using var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            _workbook = new XSSFWorkbook(stream);
            // Do something with the workbook if needed
        }
        else
        {
            _workbook = new HSSFWorkbook(new FileStream(filePath, FileMode.Open, FileAccess.Read));
        }

    }
    public string[] GetSheetNames()
    {
        // Get the sheet names from the Excel file
        if (_workbook == null)
        {
            throw new InvalidOperationException("Workbook is not initialized. Please ensure the file path is correct.");
        }
        var sheetNames = new string[_workbook.NumberOfSheets];
        for (int i = 0; i < _workbook.NumberOfSheets; i++)
        {
            sheetNames[i] = _workbook.GetSheetName(i);
        }
        return sheetNames;

    }
    public string[] GetSheetHeader(int rowIndex, string sheetName)
    {
        // Get the header row from the specified sheet
        if (_workbook == null)
        {
            throw new InvalidOperationException("Workbook is not initialized. Please ensure the file path is correct.");
        }
        var sheet = _workbook.GetSheet(sheetName);
        if (sheet == null)
        {
            throw new ArgumentException($"Sheet '{sheetName}' does not exist in the workbook.", nameof(sheetName));
        }
        var row = sheet.GetRow(rowIndex);
        if (row == null)
        {
            throw new ArgumentException($"Row {rowIndex} does not exist in the sheet '{sheetName}'.", nameof(rowIndex));
        }
        var headers = new string[row.LastCellNum];
        for (int i = 0; i < row.LastCellNum; i++)
        {
            headers[i] = row.GetCell(i)?.ToString() ?? string.Empty;
        }
        return headers;

    }

    public List<object[]> GetSheetData(int startRow, string sheetName)
    {
        // Get the data rows from the specified sheet
        if (_workbook == null)
        {
            throw new InvalidOperationException("Workbook is not initialized. Please ensure the file path is correct.");
        }
        var sheet = _workbook.GetSheet(sheetName);
        if (sheet == null)
        {
            throw new ArgumentException($"Sheet '{sheetName}' does not exist in the workbook.", nameof(sheetName));
        }
        var data = new List<object[]>();
        for (int i = startRow; i <= sheet.LastRowNum; i++)
        {
            var row = sheet.GetRow(i);
            if (row == null) continue; // Skip empty rows
            var rowData = new object[row.LastCellNum];
            for (int j = 0; j < row.LastCellNum; j++)
            {
                rowData[j] = row.GetCell(j)?.ToString() ?? string.Empty;
            }
            data.Add(rowData);
        }
        return data;
    }



}