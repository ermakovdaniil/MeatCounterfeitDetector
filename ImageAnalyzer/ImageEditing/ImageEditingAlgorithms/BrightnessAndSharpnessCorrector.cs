using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Media.Imaging;

namespace ImageWorker.ImageEditing.ImageEditingAlgorithms
{
    public class BrightnessAndContrastCorrector
    {
        public Mat AdjustBrightnessAndContrast(Mat source, int brightness, int contrast)
        {
            //double scaledBrightness = (brightness - 50) * 2; // Scale from -100 to 100
            //double scaledContrast = contrast / 50.0; // Scale from 0 to 2

            double scaledBrightness = ((brightness - 50) / 50.0) * 255;
            double scaledContrast = contrast / 50.0;

            //Mat adjustedImage = new Mat();
            //inputImageMat.ConvertTo(adjustedImage, DepthType.Cv8U, scaledContrast, scaledBrightness);
            //BitmapSource bitmapSource = adjustedImage.ToImage<Bgr, byte>().ToBitmapSource();
            //return adjustedImage.ToBitmapSource();

            CvInvoke.AddWeighted(source, scaledContrast, new Mat(source.Size, source.Depth, source.NumberOfChannels), 0, scaledBrightness, inputImageMat);
            return source;
        }

        public int GetBrightness(BitmapSource source)
        {
            //var bitmapService = new BitmapService.BitmapService();
            //var originalMat = bitmapService.BitmapSourceToMat(source);

            //Mat grayMat = new Mat();
            //CvInvoke.CvtColor(originalMat, grayMat, ColorConversion.Bgr2Gray);

            //double averageIntensity = CvInvoke.Sum(grayMat).V0 / (grayMat.Width * grayMat.Height);

            //int brightness = (int)(((averageIntensity - 127.5) / 127.5) * 255);

            //int mappedBrightness = (int)(((brightness + 255) / 510.0) * 100);

            //return mappedBrightness;

            var bitmapService = new BitmapService.BitmapService();
            var originalMat = bitmapService.BitmapSourceToMat(source);

            Mat grayMat = new Mat();
            CvInvoke.CvtColor(originalMat, grayMat, ColorConversion.Bgr2Gray);

            MCvScalar mean = CvInvoke.Mean(originalMat);

            double brightness = mean.V0;

            int mappedBrightness = (int)((brightness / 255) * 100);

            return mappedBrightness;
        }

        public int GetContrast(BitmapSource source)
        {
            var bitmapService = new BitmapService.BitmapService();
            var originalMat = bitmapService.BitmapSourceToMat(source);

            Mat grayMat = new Mat();
            CvInvoke.CvtColor(originalMat, grayMat, ColorConversion.Bgr2Gray);

            MCvScalar stdDev = new MCvScalar();
            MCvScalar mean = CvInvoke.Mean(originalMat);
            CvInvoke.MeanStdDev(originalMat, ref mean, ref stdDev);

            double contrast = stdDev.V0;

            int mappedContrast = (int)((contrast / 255) * 100);

            return mappedContrast;
        }
    }
}
