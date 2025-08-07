using System;

namespace Shared.DTOs.Excel;

public class ExcelDto
{
    public List<ExcelHeaderDto> Headers { get; set; } = new List<ExcelHeaderDto>();
    public List<ExcelRowDto> Rows { get; set; } = new List<ExcelRowDto>();

}
public class ExcelHeaderDto
{
    public string CodeRow { get; set; }
    public string NameRow { get; set; }

}
public class ExcelRowDto
{
    public List<ExcelCellDto> Cells { get; set; } = new List<ExcelCellDto>();
}
public class ExcelCellDto
{
    public string CodeRow { get; set; }
    public string Value { get; set; }
}
