using Emgu.CV;
using Emgu.CV.Structure;
using System;
using System.Drawing;
using System.IO;
using System.Windows.Media.Imaging;

namespace ImageWorker.BitmapService
{
    public class BitmapService : IBitmapService
    {
        public BitmapSource LoadBitmapSource(string filePath)
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

        public Mat BitmapSourceToMat(BitmapSource bitmapSource)
        {
            Bitmap bitmap = BitmapSourceToBitmap(bitmapSource);
            Image<Bgr, Byte> image = bitmap.ToImage<Bgr, byte>();
            Mat mat = image.Mat;
            return mat;
        }

        private Bitmap BitmapSourceToBitmap(BitmapSource bitmapSource)
        {
            using (MemoryStream outStream = new MemoryStream())
            {
                BitmapEncoder encoder = new BmpBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(bitmapSource));
                encoder.Save(outStream);
                Bitmap bitmap = new Bitmap(outStream);
                return new Bitmap(bitmap);
            }
        }
    }
}
