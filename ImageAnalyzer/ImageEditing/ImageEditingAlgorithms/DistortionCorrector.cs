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
    }
}
