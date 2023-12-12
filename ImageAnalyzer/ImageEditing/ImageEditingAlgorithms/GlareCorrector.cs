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
            //CvInvoke.Threshold(source, thresholdImage, (glare * 255)/100, 255, ThresholdType.Binary);

            //// Inpainting to remove glare
            //CvInvoke.Inpaint(source, thresholdImage, source, 3, InpaintType.Telea);




            //// Apply morphological operations to reduce glare
            //CvInvoke.MorphologyEx(source, source, MorphOp.Close, CvInvoke.GetStructuringElement(ElementShape.Rectangle, new Size(glare, glare), new Point(-1, -1)), new Point(-1, -1), 1, BorderType.Default, new MCvScalar());

            //// Subtract the morphologically processed image from the original to reduce glare
            //Mat resultImage = new Mat();
            //CvInvoke.AbsDiff(source, source, resultImage);



            //// Create a CLAHE object
            //CudaClahe clahe = new CudaClahe(clipLimit: 2.0, tileGridSize: new Size(glare, glare));

            //// Apply CLAHE
            //Mat cl1 = new Mat();
            //clahe.Apply(source, cl1);

            return source;
        }
    }
}