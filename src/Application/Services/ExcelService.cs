using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.Collections.Generic;
using System.IO;

namespace Application.Services
{
    public class ExcelService
    {
        public void WriteExcel(string filePath, List<List<string>> data)
        {
            IWorkbook workbook = new XSSFWorkbook();
            ISheet sheet = workbook.CreateSheet("Sheet1");
            for (int i = 0; i < data.Count; i++)
            {
                IRow row = sheet.CreateRow(i);
                for (int j = 0; j < data[i].Count; j++)
                {
                    row.CreateCell(j).SetCellValue(data[i][j]);
                }
            }
            using (var fs = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            {
                workbook.Write(fs);
            }
        }

        public List<List<string>> ReadExcel(string filePath)
        {
            var result = new List<List<string>>();
            using (var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                IWorkbook workbook = new XSSFWorkbook(fs);
                ISheet sheet = workbook.GetSheetAt(0);
                for (int i = sheet.FirstRowNum; i <= sheet.LastRowNum; i++)
                {
                    IRow row = sheet.GetRow(i);
                    if (row == null) continue;
                    var rowData = new List<string>();
                    for (int j = row.FirstCellNum; j < row.LastCellNum; j++)
                    {
                        var cell = row.GetCell(j);
                        rowData.Add(cell?.ToString() ?? string.Empty);
                    }
                    result.Add(rowData);
                }
            }
            return result;
        }
    }
}