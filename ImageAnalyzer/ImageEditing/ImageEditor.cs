using ImageWorker.ImageEditing.ImageEditingAlgorithms;
using System.Windows.Media.Imaging;

namespace ImageWorker.ImageEditing
{
    public class ImageEditor : IImageEditor
    {
        public BitmapSource AdjustBrightnessAndContrast(BitmapSource source, int targetBrightness, int targetContrast)
        {
            return new BrightnessAndContrastCorrector().AdjustBrightnessAndContrast(source, targetBrightness, targetContrast);
        }

        public int GetBrightness(BitmapSource source)
        {
            return new BrightnessAndContrastCorrector().GetBrightness(source);
        }
    }
}
