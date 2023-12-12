using Emgu.CV;
using Emgu.CV.CvEnum;
using System;
using System.Drawing;

namespace ImageWorker.ImageEditing.ImageEditingAlgorithms
{
    public class NoiseAndSharpnessCorrector
    {
        public Mat AdjustNoise(Mat source, int noise)
        {
            CvInvoke.GaussianBlur(source, source, new Size(2 * ((int)(Math.Ceiling(noise / 10.0))) + 1, 2 * ((int)(Math.Ceiling(noise / 10.0))) + 1), 0);
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
    }
}