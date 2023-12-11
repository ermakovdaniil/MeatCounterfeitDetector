using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using System.Drawing;

namespace ImageWorker.ImageEditing.ImageEditingAlgorithms
{
    public class SizeCorrector
    {
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
    }
}