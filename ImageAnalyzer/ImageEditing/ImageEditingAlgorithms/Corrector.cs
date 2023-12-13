using Emgu.CV;
using System.Windows.Media.Imaging;

namespace ImageWorker.ImageEditing.ImageEditingAlgorithms
{
    public class Corrector
    {
        private int _prevBrightness = 50;
        private int _prevContrast = 50;
        private int _prevNoise = 0;
        private int _prevSharpness = 50;
        private int _prevGlare = 0;
        private int _prevFocalLengthX = 500;
        private int _prevFocalLengthY = 500;
        private double _prevWidth = 1;
        private double _prevHeight = 1;
        private int _prevRotation = 0;

        public BitmapSource AdjustFilter(BitmapSource source, int brightness, int contrast, int noise, int sharpness, int glare, int focalLengthX, int focalLengthY, double width, double height, int rotation)
        {
            var bitmapService = new BitmapService.BitmapService();
            Mat inputImageMat = bitmapService.BitmapSourceToMat(source);

            // Яркость и контраст
            if (brightness != _prevBrightness || brightness != _prevContrast)
            {
                new BrightnessAndContrastCorrector().AdjustBrightnessAndContrast(inputImageMat, brightness, contrast);
                _prevBrightness = brightness;
                _prevContrast = contrast;
            }

            // Шум
            if (noise != _prevNoise)
            {
                new NoiseAndSharpnessCorrector().AdjustNoise(inputImageMat, noise);
                _prevNoise = noise;
            }

            // Резкость
            if (sharpness != _prevSharpness)
            {
                new NoiseAndSharpnessCorrector().AdjustSharpness(inputImageMat, sharpness);
                _prevSharpness = sharpness;
            }

            // Блики
            if (glare != _prevGlare)
            {
                inputImageMat = new GlareCorrector().AdjustGlare(inputImageMat, glare);
                _prevGlare = glare;
            }

            // Искажение
            if (focalLengthX != _prevFocalLengthX || focalLengthY != _prevFocalLengthY)
            {
                inputImageMat = new DistortionCorrector().AdjustDistortion(inputImageMat, focalLengthX, focalLengthY);
                _prevFocalLengthX = focalLengthX;
                _prevFocalLengthY = focalLengthY;
            }

            // Высота и ширина
            if (width != _prevWidth || height != _prevHeight)
            {
                new SizeCorrector().AdjustWidthAndHeight(inputImageMat, width, height);
                _prevWidth = width;
                _prevHeight = height;
            }

            // Поврот
            if (rotation != _prevRotation)
            {
                inputImageMat = new SizeCorrector().AdjustRoatation(inputImageMat, rotation);
                _prevRotation = rotation;
            }

            return inputImageMat.ToBitmapSource();
        }
    }
}
