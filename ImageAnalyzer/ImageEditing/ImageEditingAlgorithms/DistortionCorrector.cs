using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using System;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Media.Imaging;

namespace ImageWorker.ImageEditing.ImageEditingAlgorithms
{
    public class DistortionCorrector
    {
        public Mat AdjustDistortion(Mat source, int focalLengthX, int focalLengthY)
        {
            Mat result = new Mat();

            // vers. 1

            //Mat mapX = new Mat(source.Size, DepthType.Cv32F, 1);
            //Mat mapY = new Mat(source.Size, DepthType.Cv32F, 1);

            //Size size = new Size(source.Size.Width, source.Size.Height);
            //CvInvoke.InitUndistortRectifyMap(Mat.Eye(3, 3, DepthType.Cv32F, 1), Mat.Eye(3, 3, DepthType.Cv32F, 1), size, focalLengthX, focalLengthY, mapX, mapY, DepthType.Cv32F, 1);

            //CvInvoke.Remap(source, result, mapX, mapY, Inter.Linear, BorderType.Constant, new MCvScalar(0, 0, 0));

            ///////////////

            // vers. 2

            Matrix<double> cameraMatrix = new Matrix<double>(new double[,]
            {
                { focalLengthX, 0, source.Width / 2 },
                { 0, focalLengthY, source.Height / 2 },
                { 0, 0, 1 }
            });

            Matrix<double> distortionCoefficients = new Matrix<double>(1, 5);

            CvInvoke.Undistort(source, source, cameraMatrix, distortionCoefficients);

            return source;
        }
    }
}
