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
        public BitmapSource AdjustBrightnessAndContrast(BitmapSource source, int brightness, int contrast)
        {
            var bitmapService = new BitmapService.BitmapService();
            Mat inputImageMat = bitmapService.BitmapSourceToMat(source);

            //double scaledBrightness = (brightness - 50) * 2; // Scale from -100 to 100
            //double scaledContrast = contrast / 50.0; // Scale from 0 to 2

            double scaledBrightness = ((brightness - 50) / 50.0) * 255;
            double scaledContrast = contrast * 0.05;

            //Mat adjustedImage = new Mat();
            //inputImageMat.ConvertTo(adjustedImage, DepthType.Cv8U, scaledContrast, scaledBrightness);
            //BitmapSource bitmapSource = adjustedImage.ToImage<Bgr, byte>().ToBitmapSource();
            //return adjustedImage.ToBitmapSource();

            Mat newImage = new Mat(inputImageMat.Size, DepthType.Cv8U, inputImageMat.NumberOfChannels);
            CvInvoke.AddWeighted(inputImageMat, scaledContrast, new Mat(inputImageMat.Size, inputImageMat.Depth, inputImageMat.NumberOfChannels), 0, scaledBrightness, inputImageMat);
            return inputImageMat.ToBitmapSource();
        }

        public int GetBrightness(BitmapSource source)
        {
            var bitmapService = new BitmapService.BitmapService();
            var mat = bitmapService.BitmapSourceToMat(source);
           
            CvInvoke.CvtColor(mat, mat, ColorConversion.Bgr2Gray);

            MCvScalar mean = CvInvoke.Mean(mat);

            double brightness = mean.V0;

            int mappedBrightness = (int)((brightness / 255) * 100);

            return mappedBrightness;
        }

        public int GetContrast(BitmapSource source)
        {
            var bitmapService = new BitmapService.BitmapService();
            var mat = bitmapService.BitmapSourceToMat(source);

            MCvScalar stdDev = new MCvScalar();
            MCvScalar mean = CvInvoke.Mean(mat);
            CvInvoke.MeanStdDev(mat, ref mean, ref stdDev);

            double contrast = stdDev.V0;

            int mappedContrast = MapValue(contrast, 0, 255, 0, 100);

            return mappedContrast;
        }

        private int MapValue(double value, double fromSource, double toSource, double fromTarget, double toTarget)
        {
            return (int)((value - fromSource) / (toSource - fromSource) * (toTarget - fromTarget) + fromTarget);
        }
    }
}
