using Emgu.CV;
using System.Windows.Media.Imaging;

namespace ImageWorker.ImageEditing.ImageEditingAlgorithms
{
    public class Corrector
    {
        public BitmapSource AdjustFilter(BitmapSource source, int brightness, int contrast, int noise, int sharpness, int focalLengthX, int focalLengthY, double width, double height, int rotation)
        {
            var bitmapService = new BitmapService.BitmapService();
            Mat inputImageMat = bitmapService.BitmapSourceToMat(source);

            // Яркость и контраст

            new BrightnessAndContrastCorrector().AdjustBrightnessAndContrast(inputImageMat, brightness, contrast);

            // Шум

            new NoiseCorrector().AdjustNoise(inputImageMat, noise);

            // Резкость TODO

            new NoiseCorrector().AdjustSharpness(inputImageMat, sharpness);

            // Искажение TODO

            new DistortionCorrector().AdjustDistortion(inputImageMat, focalLengthX, focalLengthY);

            // Высота и ширина

            new SizeCorrector().AdjustWidthAndHeight(inputImageMat, width, height);

            // Поврот

            new SizeCorrector().AdjustRoatation(inputImageMat, rotation);

            return inputImageMat.ToBitmapSource();
        }
    }
}
