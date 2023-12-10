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
    public class NoiseCorrector
    {
        public Mat AdjustNoise(Mat source, int noise)
        {
            CvInvoke.GaussianBlur(source, source, new Size(2 * ((int)(Math.Ceiling(noise / 2.0))) + 1, 2 * ((int)(Math.Ceiling(noise / 2.0))) + 1), 0);

            return source;
        }

        public Mat AdjustSharpness(Mat source, int sharpness)
        {

            return source;
        }
    }
}
