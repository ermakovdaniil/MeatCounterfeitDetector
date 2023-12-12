using ImageWorker.ImageEditing.ImageEditingAlgorithms;
using System.Windows.Media.Imaging;

namespace ImageWorker.ImageEditing
{
    public class ImageEditor : IImageEditor
    {
        public BitmapSource AdjustFilter(BitmapSource source, int brightness, int contrast, int noise, int sharpness, int glare, int focalLengthX, int focalLengthY, double width, double height, int rotation)
        {
            return new Corrector().AdjustFilter(source, brightness, contrast, noise, sharpness, glare, focalLengthX, focalLengthY, width, height, rotation);
        }


        // TODO: получение значений

        public int GetBrightness(BitmapSource source)
        {
            return new BrightnessAndContrastCorrector().GetBrightness(source);
        }

        public int GetContrast(BitmapSource source)
        {
            return new BrightnessAndContrastCorrector().GetContrast(source);
        }
    }
}
