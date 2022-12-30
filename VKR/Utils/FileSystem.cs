using iText.IO.Font;
using iText.IO.Image;
using iText.Kernel.Font;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;


namespace VKR.Utils;

/// <summary>
///     Класс для работы с файлами
/// </summary>
internal static class FileSystem
{
    private static Image CreateAndFitImage(byte[] bitmap, Document document)
    {
        var image = new Image(ImageDataFactory.Create(bitmap)).SetTextAlignment(TextAlignment.CENTER);
        FitImageToDocument(image, document);

        return image;
    }

    private static void FitImageToDocument(Image image, Document document)
    {
        var widthscaler =
            (document.GetPageEffectiveArea(PageSize.A4).GetWidth() - document.GetLeftMargin() -
             document.GetRightMargin()) / image.GetImageWidth();

        var heighscaler =
            (document.GetPageEffectiveArea(PageSize.A4).GetHeight() - document.GetTopMargin() -
             document.GetBottomMargin()) / image.GetImageHeight();

        float scaler;

        if (widthscaler < heighscaler)
        {
            scaler = widthscaler;
        }
        else
        {
            scaler = heighscaler;
        }

        image.Scale(scaler, scaler);
    }

    public static void ExportPdf(string path, byte[] initialBitmap, byte[] resBitMap, string result, string dateAnalysis, string companyDate)
    {
        var writer = new PdfWriter(path);
        var pdf = new PdfDocument(writer);
        var document = new Document(pdf);

        var fontFilename = "../../../resources/fonts/Times_New_Roman.ttf";
        var font = PdfFontFactory.CreateFont(fontFilename, PdfEncodings.IDENTITY_H);

        var header = new Paragraph("Отчёт об анализе изображения мясной продукции.").SetFont(font)
                                                                                    .SetTextAlignment(TextAlignment.CENTER)
                                                                                    .SetFontSize(20);

        document.SetFont(font);

        document.Add(header);
        document.Add(new Paragraph("Исходное изображение:"));
        var initialImage = CreateAndFitImage(initialBitmap, document);
        document.Add(initialImage);
        document.Add(new AreaBreak());

        if (resBitMap != null)
        {
            document.Add(new Paragraph("Обработанное изображение:"));
            var resImage = CreateAndFitImage(resBitMap, document);
            document.Add(resImage);
        }

        document.Add(new Paragraph(""));

        //document.Add(new AreaBreak());

        document.Add(new Paragraph("Результат анализа:"));
        document.Add(new Paragraph(result));
        document.Add(new Paragraph("Дата проведения анализа:"));
        document.Add(new Paragraph(dateAnalysis));
        document.Add(new Paragraph("Предприятие, которое проводило анализ:"));
        document.Add(new Paragraph(companyDate));
        document.Close();
    }
}
