using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace Application.Services.PDFProviders
{
    public static class PdfProvider
    {
        public static void ConfigurePageSettings(PageDescriptor page, PageSize pageSizes)
        {
            page.ContentFromRightToLeft();
            page.Size(pageSizes);
            page.Margin(ReportConstants.MarginSize, Unit.Centimetre);
            page.PageColor(Colors.White);
            page.DefaultTextStyle(x => x.FontSize(ReportConstants.DefaultFontSize)
                                    .FontFamily(ReportConstants.FontFamily));
        }

    }
}