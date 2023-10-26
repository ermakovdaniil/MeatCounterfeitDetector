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
        //https://github.com/halanch599/EmguCV4.4/blob/master/EmgucvDemo/formBrightnessContrast.cs

        // lblCurrentContrast.Text = ((float) trackBar1.Value / 100).ToString();
        // imgOutput =   ImgInput.Mul(double.Parse(lblCurrentContrast.Text)) + trackBar2.Value;
        // pictureBox1.Image = imgOutput.AsBitmap();
        // public Image<Bgr, byte> ImgInput { get; set; }
        // Image<Bgr, byte> imgOutput;

        // public Image<Bgr, byte> ImgInput { get; set; }
        // Image<Bgr, byte> imgOutput;
        // contrast
        // imgOutput = ImgInput.Mul(contrast) + brightness;
        // pictureBox1.Image = imgOutput.AsBitmap();



        public BitmapSource AdjustBrightnessAndContrast(BitmapSource source, int brightness, int contrast)
        {
            var bitmapService = new BitmapService.BitmapService();
            Mat inputImageMat = bitmapService.BitmapSourceToMat(source);
            Image<Bgr, byte> inputImage = inputImageMat.ToImage<Bgr, byte>();

            Image<Bgr, byte> outputImage;

            outputImage = inputImage.Mul((double)contrast/20) + brightness;

            return outputImage.ToBitmapSource();
        }

        //public BitmapSource AdjustBrightnessAndContrast(BitmapSource source, int targetBrightness, int targetContrast)
        //{
        //    var bitmapService = new BitmapService.BitmapService();
        //    Mat inputImage = bitmapService.BitmapSourceToMat(source);



        //    // Calculate adjustment factors for brightness and contrast
        //    int currentBrightness = GetBrightness(source);
        //    int currentContrast = GetContrast(source);

        //    double brightnessFactor = targetBrightness / currentBrightness;
        //    double contrastFactor = targetContrast / currentContrast;

        //    // Apply brightness and contrast adjustments
        //    Mat adjustedImage = new Mat();
        //    CvInvoke.AddWeighted(inputImage, contrastFactor, inputImage, 0, 0, adjustedImage);
        //    CvInvoke.Add(adjustedImage, new UMat(inputImage.Size, DepthType.Cv8U, 3), adjustedImage, new UMat(inputImage.Size, DepthType.Cv8U, 3), (DepthType)brightnessFactor);


        //    BitmapSource outputImage = adjustedImage.ToBitmapSource();

        //    return outputImage;
        //}

        //// Calculate brightness of the image
        //private double CalculateBrightness(Mat image)
        //{
        //    Image<Gray, float> grayImage = image.ToImage<Gray, float>();
        //    MCvScalar mean = CvInvoke.Mean(grayImage);
        //    return mean.V0;
        //}

        //// Calculate contrast of the image
        //private double CalculateContrast(Mat image)
        //{
        //    Image<Gray, float> grayImage = image.ToImage<Gray, float>();
        //    grayImage = grayImage.Pow(2);
        //    MCvScalar meanSquare = CvInvoke.Mean(grayImage);
        //    MCvScalar mean = CvInvoke.Mean(grayImage);
        //    double contrast = Math.Sqrt(meanSquare.V0 - Math.Pow(mean.V0, 2));
        //    return contrast;
        //}

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
