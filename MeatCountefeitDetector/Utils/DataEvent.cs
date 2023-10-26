using Emgu.CV;
using System.Windows.Media.Imaging;

namespace MeatCountefeitDetector.Utils
{
    public class DataEvent
    {
        public BitmapSource ImageBitmapSource { get; }

        public DataEvent(BitmapSource imageBitmapSource)
        {
            ImageBitmapSource = imageBitmapSource;
        }
    }
}
