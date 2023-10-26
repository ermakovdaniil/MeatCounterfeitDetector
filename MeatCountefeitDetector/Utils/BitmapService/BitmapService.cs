using Emgu.CV;
using System;
using System.Windows.Media.Imaging;

namespace MeatCountefeitDetector.Utils.BitmapService
{
    public class BitmapService : IBitmapService
    {
        public BitmapSource LoadBitmap(string filePath)
        {
            BitmapImage bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.UriSource = new Uri(filePath, UriKind.Absolute);
            bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
            bitmapImage.EndInit();
            return bitmapImage as BitmapSource;
        }

        public BitmapSource ConvertMatToBitmapSource(Mat mat)
        {
            return mat.ToBitmapSource();
        }
    }
}
