using Emgu.CV;
using Emgu.CV.CvEnum;
using System;
using System.Drawing;

namespace ImageWorker.ImageEditing.ImageEditingAlgorithms
{
    public class NoiseCorrector
    {
        public Mat AdjustNoise(Mat source, int noise)
        {
            CvInvoke.GaussianBlur(source, source, new Size(2 * ((int)(Math.Ceiling(noise / 2.0))) + 1, 2 * ((int)(Math.Ceiling(noise / 2.0))) + 1), 0);
            //CvInvoke.MedianBlur(source, source, noise * 2 + 1);
            return source;
        }

        public Mat AdjustSharpness(Mat source, int sharpness)
        {
            // Define the sharpening kernel
            float[,] kernelArray = {
                { 0, -1, 0 },
                { -1, 5, -1 },
                { 0, -1, 0 }
            };

            // Create a kernel matrix
            Matrix<float> kernel = new Matrix<float>(kernelArray);

            // Apply the filter using filter2D
            CvInvoke.Filter2D(source, source, kernel, new Point(-1, -1));

            // Calculate the sharpening factor
            double alpha = 1 + (sharpness / 100.0);

            // Combine the original image with the sharpened edges
            CvInvoke.AddWeighted(source, alpha, source, -alpha, 0, source);

            return source;
        }
    }
}