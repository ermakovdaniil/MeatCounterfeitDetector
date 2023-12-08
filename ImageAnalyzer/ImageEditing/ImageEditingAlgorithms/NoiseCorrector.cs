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
        public BitmapSource AdjustNoise(BitmapSource source, int noise)
        {
            var bitmapService = new BitmapService.BitmapService();
            Mat inputImageMat = bitmapService.BitmapSourceToMat(source);

            CvInvoke.GaussianBlur(inputImageMat, inputImageMat, new Size(2 * ((int)(Math.Ceiling(noise / 2.0))) + 1, 2 * ((int)(Math.Ceiling(noise / 2.0))) + 1), 0);
            return inputImageMat.ToBitmapSource();
        }
    }
}
