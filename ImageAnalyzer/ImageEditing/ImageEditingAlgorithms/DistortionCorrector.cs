using DataAccess.Models;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Reg;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

namespace ImageWorker.ImageEditing.ImageEditingAlgorithms
{
    public class DistortionCorrector
    {
        public Mat AdjustDistortion(Mat source, int focalLengthX, int focalLengthY)
        {
            double correctionStrengthX = focalLengthX ;
            double correctionStrengthY = focalLengthY ;

            Mat result = new Mat();

            // vers. 4

            Mat cameraMatrix = new Mat(3, 3, DepthType.Cv64F, 1);
            Mat distortionCoefficients = new Mat(5, 1, DepthType.Cv64F, 1);

            // Set camera matrix with focal lengths
            cameraMatrix.SetValue(0, 0, correctionStrengthX);
            cameraMatrix.SetValue(0, 1, 0.0);
            cameraMatrix.SetValue(0, 2, source.Width / 2.0);

            cameraMatrix.SetValue(1, 0, 0.0);
            cameraMatrix.SetValue(1, 1, correctionStrengthY);
            cameraMatrix.SetValue(1, 2, source.Height / 2.0);

            cameraMatrix.SetValue(2, 0, 0.0);
            cameraMatrix.SetValue(2, 1, 0.0);
            cameraMatrix.SetValue(2, 2, 1.0);

            // Set distortion coefficients
            distortionCoefficients.SetValue(0, 0, 0.17352);
            distortionCoefficients.SetValue(0, 1, -0.484226);
            distortionCoefficients.SetValue(0, 2, 0.344761);
            distortionCoefficients.SetValue(0, 3, 0.00075256);
            distortionCoefficients.SetValue(0, 4, -0.000269617);

            // Apply distortion correction
            CvInvoke.Undistort(source, result, cameraMatrix, distortionCoefficients);

            // vers. 1

            //Mat mapX = new Mat(source.Size, DepthType.Cv32F, 1);
            //Mat mapY = new Mat(source.Size, DepthType.Cv32F, 1);

            //Size size = new Size(source.Size.Width, source.Size.Height);
            //CvInvoke.InitUndistortRectifyMap(Mat.Eye(3, 3, DepthType.Cv32F, 1), Mat.Eye(3, 3, DepthType.Cv32F, 1), size, focalLengthX, focalLengthY, mapX, mapY, DepthType.Cv32F, 1);

            //CvInvoke.Remap(source, result, mapX, mapY, Inter.Linear, BorderType.Constant, new MCvScalar(0, 0, 0));

            ///////////////

            // vers. 2

            //Mat cameraMatrix = new Mat(3, 3, DepthType.Cv64F, 1);
            //Mat distCoeffs = new Mat(5, 1, DepthType.Cv64F, 1);

            //Matrix<double> cameraMatrix = new Matrix<double>(new double[,]
            //{
            //    { focalLengthX, 0, source.Width / 2 },
            //    { 0, focalLengthY, source.Height / 2 },
            //    { 0, 0, 1 }
            //});

            //Matrix<double> distortionCoefficients = new Matrix<double>(1, 5);

            //CvInvoke.Undistort(source, result, cameraMatrix, distortionCoefficients);

            // vers. 3

            //// Fisheye distortion correction
            //Mat cameraMatrix = new Mat(3, 3, DepthType.Cv32F, 1);
            //Mat distortionCoefficients = new Mat(1, 5, DepthType.Cv32F, 1);

            //CvInvoke.Undistort(source, result, cameraMatrix, distortionCoefficients);

            //// Pillow distortion correction
            //Mat mapX = new Mat();
            //Mat mapY = new Mat();
            //CvInvoke.InitUndistortRectifyMap(cameraMatrix, distortionCoefficients, new Mat(), cameraMatrix, source.Size, DepthType.Cv32F, 1, mapX, mapY);

            //// Adjust mapX and mapY based on strengthX and strengthY
            //mapX = mapX * (float)correctionStrengthX;
            //mapY = mapY * (float)correctionStrengthY;

            //// Perform distortion correction
            //CvInvoke.Remap(source, result, mapX, mapY, Inter.Linear);



            return result;
        }
    }
}
