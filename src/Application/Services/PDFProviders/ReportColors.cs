
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

namespace Application.Services.PDFProviders;


public static class ReportColors
{
    public static readonly Color Opening = Color.FromHex("#f0f0f0");
    public static readonly Color Monthly = Color.FromHex("#d1d1d1");
    public static readonly Color Closing = Color.FromHex("#fafafa");
    public static readonly Color Account = Color.FromHex("#fff");
    public static readonly Color Total = Color.FromHex("#a9a9a9");
    public static readonly Color GrandTotal = Color.FromHex("#708090");
    public static readonly Color Subtotal = Color.FromHex("#d3d3d3");

    public static readonly Color TransparentWhite = Color.FromARGB(200, 255, 255, 255); //Color.FromHex("#f5f5f5");
}


public static class CellStyles
{

    public static IContainer TitleStyle(IContainer container, Color color)
        => container.Border(0).Background(color).PaddingHorizontal(1).PaddingVertical(8).AlignCenter().AlignMiddle();

    public static IContainer HeaderCellStyle(IContainer container)
          => container.Border(1).Background(ReportConstants.HeaderColor).PaddingHorizontal(1).PaddingVertical(1).AlignCenter().AlignMiddle();
    public static IContainer DataCellStyle(IContainer container)
         => container.Border(1).Background(ReportConstants.DataCellColor).PaddingHorizontal(1).PaddingVertical(3).AlignCenter().AlignMiddle();

    public static IContainer FooterCellStyle(IContainer container)
         => container.Border(1).Background(ReportConstants.FooterColor).PaddingHorizontal(1).PaddingVertical(4).AlignCenter().AlignMiddle();


    public static IContainer MainHeaderStyle(IContainer container, Color color) =>
  container.Border(0)
           .Background(color)
           .Width(100)
           .PaddingHorizontal(1)
           .PaddingVertical(1)
           .AlignCenter()
           .AlignMiddle();

    public static IContainer SubtotalStyle(IContainer container, Color color) =>
        container.Height(20)
                 .Border(1.2f)
                 .Background(color)
                 .PaddingHorizontal(1)
                 .PaddingVertical(1)
                 .AlignCenter()
                 .AlignMiddle();

    public static IContainer TotalStyle(IContainer container, Color color) =>
        container.Height(25)
                 .Border(3)
                 .Background(color)
                 .PaddingHorizontal(1)
                 .PaddingVertical(1)
                 .AlignCenter()
                 .AlignMiddle();

    public static IContainer AccountCellStyle(IContainer container, Color color) =>
        container.Border(1)
                 .Background(color)
                 .PaddingHorizontal(1)
                 .PaddingVertical(1)
                 .AlignCenter()
                 .AlignMiddle();
}


