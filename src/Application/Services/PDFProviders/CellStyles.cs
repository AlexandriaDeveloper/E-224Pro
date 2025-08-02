
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

namespace Application.Services.PDFProviders;

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


