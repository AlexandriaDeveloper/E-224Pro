using QuestPDF.Infrastructure;
namespace Application.Services;

// public partial class PDFReportService
// {
// Constants class - add this inside your class or as a separate class
public static class ReportConstants
{
    public const int HeaderFontSize = 12;
    public const int DataFontSize = 8;
    public const int TitleFontSize = 16;
    public const int SubtitleFontSize = 12;
    public const string ArabicFont = "Cairo";
    public const string EnglishFont = "Arial";

    public static readonly Color HeaderColor = Color.FromHex("#e1e1e1");
    public static readonly Color DataCellColor = Color.FromARGB(200, 255, 255, 255);
    public static readonly Color FooterColor = Color.FromHex("#e1e1e1");


    public const float DefaultScale = 1.2f;
    public const float HeaderScale = 1.4f;
    public const float MarginSize = 0.5f;
    public const int DefaultFontSize = 8;

    public const int MainTitleFontSize = 14;
    public const int SubTitleFontSize = 12;

    public const string FontFamily = "Arial";
}



//}