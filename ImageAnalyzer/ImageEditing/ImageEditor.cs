﻿using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using System;
using System.Drawing;
using System.Windows.Media.Imaging;

namespace ImageWorker.ImageEditing
{
    public class ImageEditor : IImageEditor
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

        #region Adjustment 

        public BitmapSource AdjustFilter(BitmapSource source, int brightness, int contrast, int noise, int sharpness, int glare, int focalLengthX, int focalLengthY, double width, double height, int rotation)
        {
            var bitmapService = new BitmapService.BitmapService();
            Mat inputImageMat = bitmapService.BitmapSourceToMat(source);

            // Яркость и контраст
            if (brightness != _prevBrightness || brightness != _prevContrast)
            {
                inputImageMat = AdjustBrightnessAndContrast(inputImageMat, brightness, contrast);
                _prevBrightness = brightness;
                _prevContrast = contrast;
            }

            // Шум
            if (noise != _prevNoise)
            {
                inputImageMat = AdjustNoise(inputImageMat, noise);
                _prevNoise = noise;
            }

            // Резкость
            if (sharpness != _prevSharpness)
            {
                inputImageMat = AdjustSharpness(inputImageMat, sharpness);
                _prevSharpness = sharpness;
            }

            // Блики
            if (glare != _prevGlare)
            {
                inputImageMat = AdjustGlare(inputImageMat, glare);
                _prevGlare = glare;
            }

            // Искажение
            if (focalLengthX != _prevFocalLengthX || focalLengthY != _prevFocalLengthY)
            {
                inputImageMat = AdjustDistortion(inputImageMat, focalLengthX, focalLengthY);
                _prevFocalLengthX = focalLengthX;
                _prevFocalLengthY = focalLengthY;
            }

            // Высота и ширина
            if (width != _prevWidth || height != _prevHeight)
            {
                inputImageMat = AdjustWidthAndHeight(inputImageMat, width, height);
                _prevWidth = width;
                _prevHeight = height;
            }

            // Поврот
            if (rotation != _prevRotation)
            {
                inputImageMat = AdjustRoatation(inputImageMat, rotation);
                _prevRotation = rotation;
            }

            return inputImageMat.ToBitmapSource();
        }

        public Mat AdjustBrightnessAndContrast(Mat source, int brightness, int contrast)
        {
            //double scaledBrightness = (brightness - 50) * 2; // Scale from -100 to 100
            //double scaledContrast = contrast / 50.0; // Scale from 0 to 2

            double scaledBrightness = (brightness - 50) / 50.0 * 255;
            double scaledContrast = contrast / 50.0;

            //Mat adjustedImage = new Mat();
            //inputImageMat.ConvertTo(adjustedImage, DepthType.Cv8U, scaledContrast, scaledBrightness);
            //BitmapSource bitmapSource = adjustedImage.ToImage<Bgr, byte>().ToBitmapSource();
            //return adjustedImage.ToBitmapSource();

            CvInvoke.AddWeighted(source, scaledContrast, new Mat(source.Size, source.Depth, source.NumberOfChannels), 0, scaledBrightness, source);
            return source;
        }

        public Mat AdjustNoise(Mat source, int noise)
        {
            CvInvoke.GaussianBlur(source, source, new Size(2 * (int)Math.Ceiling(noise / 10.0) + 1, 2 * (int)Math.Ceiling(noise / 10.0) + 1), 0);
            return source;
        }

        public Mat AdjustSharpness(Mat source, int sharpness)
        {
            //float[,] kernelArray = {
            //    { -1, -1, -1 },
            //    { -1,  9, -1 },
            //    { -1, -1, -1 }
            //};

            float[,] kernelArray = {
                { 0, -1, 0 },
                { -1, 5, -1 },
                { 0, -1, 0 }
            };

            Matrix<float> kernel = new Matrix<float>(kernelArray) * (sharpness / 50.0f);
            CvInvoke.Filter2D(source, source, kernel, new Point(-1, -1));
            return source;
        }

        // https://stackoverflow.com/questions/43470569/remove-glare-from-photo-opencv
        public Mat AdjustGlare(Mat source, int glare)
        {
            // vers. 1

            //// Load the image
            //Image<Bgr, byte> img = source.ToImage<Bgr, byte>();
            //Bitmap originalImage = img.ToBitmap();

            //// Convert the image to grayscale
            //Bitmap grayImage = Grayscale.CommonAlgorithms.BT709.Apply(originalImage);

            //// Apply a threshold to create a binary image
            //Bitmap binaryImage = new Threshold(glare * 10).Apply(grayImage);

            //// Remove small objects (potential glare) from the binary image
            //BlobCounter blobCounter = new BlobCounter();
            //blobCounter.FilterBlobs = true;
            //blobCounter.MinWidth = 10;
            //blobCounter.MinHeight = 10;
            //blobCounter.ProcessImage(binaryImage);

            //// Invert the binary image to create a mask
            //Invert invertFilter = new Invert();
            //Bitmap maskImage = invertFilter.Apply(binaryImage);

            //// Apply the mask to the original image
            //ApplyMask applyMaskFilter = new ApplyMask(maskImage);
            //applyMaskFilter.ApplyInPlace(originalImage);

            //source = originalImage.ToMat();

            Mat result = new Mat();

            // vers. 2
            Mat grayImage = new Mat();
            CvInvoke.CvtColor(source, grayImage, ColorConversion.Bgr2Gray);

            // Perform thresholding to identify glare regions
            Mat thresholdImage = new Mat();

            CvInvoke.Threshold(grayImage, thresholdImage, 255 - glare / 100.0 * 255, 255, ThresholdType.Binary);

            // Inpainting to remove glare
            CvInvoke.Inpaint(source, thresholdImage, result, 3, InpaintType.Telea);

            // vers. 3

            //// Create a CLAHE object
            //CudaClahe clahe = new CudaClahe(clipLimit: 2.0, tileGridSize: new Size(glare, glare));

            //// Apply CLAHE
            //Mat cl1 = new Mat();
            //Mat grayImage = new Mat();
            //CvInvoke.CvtColor(source, grayImage, ColorConversion.Bgr2Gray);
            //clahe.Apply(grayImage, cl1);

            //return cl1;

            return result;
        }

        // https://learnopencv.com/understanding-lens-distortion/
        // https://github.com/kaustubh-sadekar/VirtualCam
        public Mat AdjustDistortion(Mat source, int focalLengthX, int focalLengthY)
        {
            Mat result = new Mat();

            Mat cameraMatrix = new Mat(3, 3, DepthType.Cv64F, 1);

            cameraMatrix.SetValue(0, 0, focalLengthX);
            cameraMatrix.SetValue(0, 1, 0.0);
            cameraMatrix.SetValue(0, 2, source.Width / 2.0);

            cameraMatrix.SetValue(1, 0, 0.0);
            cameraMatrix.SetValue(1, 1, focalLengthY);
            cameraMatrix.SetValue(1, 2, source.Height / 2.0);

            cameraMatrix.SetValue(2, 0, 0.0);
            cameraMatrix.SetValue(2, 1, 0.0);
            cameraMatrix.SetValue(2, 2, 1.0);

            // k1, k2, p1, p2, k3
            Mat distortionCoefficients = new Mat(1, 5, DepthType.Cv64F, 1);

            //double k1 = 9.1041365324307497e-002;
            //double k2 = -4.0485507081497402e-001;
            //double p1 = 3.4409596859645629e-004;
            //double p2 = -3.9472652058529605e-005;
            //double k3 = 3.3943759073230600e-001;

            //distortionCoefficients.SetValue(0, 0, k1);
            //distortionCoefficients.SetValue(0, 1, k2);
            //distortionCoefficients.SetValue(0, 2, p1);
            //distortionCoefficients.SetValue(0, 3, p2);
            //distortionCoefficients.SetValue(0, 4, k3);

            // Apply distortion correction
            CvInvoke.Undistort(source, result, cameraMatrix, distortionCoefficients);

            return result;
        }

        public Mat AdjustWidthAndHeight(Mat source, double width, double height)
        {
            CvInvoke.Resize(source, source, new Size((int)(source.Width * width), (int)(source.Height * height)), interpolation: Inter.Linear);
            return source;
        }

        public Mat AdjustRoatation(Mat source, int rotation)
        {
            PointF center = new PointF(source.Width / 2.0f, source.Height / 2.0f);

            var rotationMatrix = new Mat();
            CvInvoke.GetRotationMatrix2D(center, rotation, 1.0, rotationMatrix);

            CvInvoke.WarpAffine(source, source, rotationMatrix, source.Size, Inter.Linear, Warp.Default, BorderType.Constant, new MCvScalar(255, 255, 255));

            return source;
        }

        #endregion

        #region Estimation

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

            int mappedBrightness = (int)(brightness / 255 * 100);

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

            int mappedContrast = (int)(contrast / 255 * 100);

            return mappedContrast;
        }

        #endregion
    }
}