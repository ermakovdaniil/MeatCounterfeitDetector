using Emgu.CV;
using System;
using System.Drawing;
using AForge.Imaging;
using AForge.Imaging.Filters;
using Emgu.CV.CvEnum;
using Emgu.CV.Reg;
using Emgu.CV.Structure;

namespace ImageWorker.ImageEditing.ImageEditingAlgorithms
{
    public class GlareCorrector
    {
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
            CvInvoke.Threshold(grayImage, thresholdImage, (glare * 255)/100, 255, ThresholdType.Binary);

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



            // vers. 4

            return result;
        }
    }
}