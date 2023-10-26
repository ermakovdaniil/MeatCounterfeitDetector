using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using System.Windows.Media.Imaging;

namespace ImageWorker.ImageEditing.ImageEditingAlgorithms
{
    public class BrightnessAndSharpnessCorrector
    {
        public BitmapSource AdjustBrightness(BitmapSource imageBitmapSource, double brightnessFactor)
        {
            //var bitmapService = new BitmapService.BitmapService(); // TODO ИНЪЕКЦИЯ?
            //Mat inputImageMat = bitmapService.BitmapSourceToMat(imageBitmapSource);

            ////Mat outputImageMat = new Mat();

            //outputImageMat.ConvertTo(outputImageMat, (DepthType)(-1), brightnessFactor, 0);

            //BitmapSource outputImage = bitmapService.ConvertMatToBitmapSource(outputImageMat);


            
            double scaleFactor = 255.0 / 100.0 * brightnessFactor;

            var bitmapService = new BitmapService.BitmapService(); // TODO ИНЪЕКЦИЯ?
            Mat inputImageMat = bitmapService.BitmapSourceToMat(imageBitmapSource);
            Mat outputImageMat = new Mat();

            CvInvoke.ConvertScaleAbs(inputImageMat, outputImageMat, scaleFactor, 0);

            BitmapSource outputImage = bitmapService.ConvertMatToBitmapSource(outputImageMat);

            return outputImage;
        }
    }
}
