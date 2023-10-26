using Emgu.CV;
using System.Windows.Media.Imaging;

namespace ImageWorker.BitmapService
{
    public interface IBitmapService
    {
        public BitmapSource LoadBitmapSource(string filePath);
        public BitmapSource ConvertMatToBitmapSource(Mat mat);
        public Mat BitmapSourceToMat(BitmapSource bitmapSource);
    }
}
