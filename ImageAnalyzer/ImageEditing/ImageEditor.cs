using ImageWorker.ImageEditing.ImageEditingAlgorithms;
using System.Windows.Media.Imaging;

namespace ImageWorker.ImageEditing
{
    public class ImageEditor : IImageEditor
    {
        public BitmapSource ChangeBrightness(BitmapSource imageBitmapSource, double brightness)
        {
            return new BrightnessAndSharpnessCorrector().AdjustBrightness(imageBitmapSource, brightness);
        }
    }
}
