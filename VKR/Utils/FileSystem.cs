using DataAccess.Models;
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
    private static Image CreateAndFitImage(string path, Document document)
    {
        var image = new Image(ImageDataFactory.Create(path)).SetTextAlignment(TextAlignment.CENTER);
        FitImageToDocument(image, document);

        return image;
    }

    private static void FitImageToDocument(Image image, Document document)
    {
        image.SetTextAlignment(TextAlignment.CENTER);
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

    public static void ExportPdf(string path, Result result)
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

        var initialImage = CreateAndFitImage(result.OrigPath.Path, document);
        document.Add(initialImage);
        document.Add(new AreaBreak());

        if (result.ResPath.Path is not null)
        {
            document.Add(new Paragraph("Обработанное изображение:"));
            var resImage = CreateAndFitImage(result.ResPath.Path, document);
            document.Add(resImage);
        }

        document.Add(new Paragraph(""));
        document.Add(new Paragraph("Результат анализа:"));
        string resultToString = result.AnRes + "\n" +
                                "Дата проведения анализа: " + result.Date + "\n" +
                                "Время проведения: " + result.Time + " мс\n" +
                                "Процент сходства: " + result.PercentOfSimilarity + "%\n" +
                                "Пользователь, который проводил анализ: " + result.User.Name;
        document.Add(new Paragraph(resultToString));
        document.Close();
    }
}
