using ImageWorker.ImageEditing.ImageEditingAlgorithms;
using System.Windows.Media.Imaging;

namespace ImageWorker.ImageEditing
{
    public class ImageEditor : IImageEditor
    {
        public BitmapSource AdjustBrightnessAndContrast(BitmapSource source, int brightness, int contrast)
        {
            return new BrightnessAndContrastCorrector().AdjustBrightnessAndContrast(source, brightness, contrast);
        }

        public int GetBrightness(BitmapSource source)
        {
            return new BrightnessAndContrastCorrector().GetBrightness(source);
        }

        public int GetContrast(BitmapSource source)
        {
            return new BrightnessAndContrastCorrector().GetContrast(source);
        }

        public BitmapSource AdjustNoise(BitmapSource source, int noise)
        {
            return new NoiseCorrector().AdjustNoise(source, noise);
        }
    }
}
