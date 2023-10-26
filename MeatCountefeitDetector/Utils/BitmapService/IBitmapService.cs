using Emgu.CV;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace MeatCountefeitDetector.Utils.BitmapService
{
    public interface IBitmapService
    {
        public BitmapSource LoadBitmap(string filePath);
        public BitmapSource ConvertMatToBitmapSource(Mat mat);
    }
}
