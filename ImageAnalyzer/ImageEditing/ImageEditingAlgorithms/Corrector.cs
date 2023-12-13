using Emgu.CV;
using System.Windows.Media.Imaging;

namespace ImageWorker.ImageEditing.ImageEditingAlgorithms
{
    public class Corrector
    {
        public BitmapSource AdjustFilter(BitmapSource source, int brightness, int contrast, int noise, int sharpness, int glare, int focalLengthX, int focalLengthY, double width, double height, int rotation)
        {
            var bitmapService = new BitmapService.BitmapService();
            Mat inputImageMat = bitmapService.BitmapSourceToMat(source);

            // Яркость и контраст

            new BrightnessAndContrastCorrector().AdjustBrightnessAndContrast(inputImageMat, brightness, contrast);

            // Шум

            new NoiseAndSharpnessCorrector().AdjustNoise(inputImageMat, noise);

            // Резкость

            new NoiseAndSharpnessCorrector().AdjustSharpness(inputImageMat, sharpness);

            // Блики

            inputImageMat = new GlareCorrector().AdjustGlare(inputImageMat, glare);

            // Искажение

            inputImageMat = new DistortionCorrector().AdjustDistortion(inputImageMat, focalLengthX, focalLengthY);

            // Высота и ширина

            new SizeCorrector().AdjustWidthAndHeight(inputImageMat, width, height);

            // Поврот

            inputImageMat = new SizeCorrector().AdjustRoatation(inputImageMat, rotation);

            return inputImageMat.ToBitmapSource();
        }
    }
}
