using Emgu.CV;
using Emgu.CV.Cuda;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using System;
using System.Drawing;

namespace ImageWorker.ImageEditing.ImageEditingAlgorithms
{
    public class GlareCorrector
    {
        public Mat AdjustGlare(Mat source, int glare)
        {
            //// Perform thresholding to identify glare regions
            //Mat thresholdImage = new Mat();
            //CvInvoke.Threshold(source, thresholdImage, (glare * 255) / 100, 255, ThresholdType.Binary);

            //// Inpainting to remove glare
            //CvInvoke.Inpaint(source, thresholdImage, source, 3, InpaintType.Telea);

            //// Create a CLAHE object
            //CudaClahe clahe = new CudaClahe(clipLimit: 2.0, tileGridSize: new Size(glare, glare));

            //// Apply CLAHE
            //Mat cl1 = new Mat();
            //Mat grayImage = new Mat();
            //CvInvoke.CvtColor(source, grayImage, ColorConversion.Bgr2Gray);
            //clahe.Apply(grayImage, cl1);

            //return cl1;

            return source;
        }
    }
}