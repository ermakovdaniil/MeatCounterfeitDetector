using System.Windows.Media.Imaging;

namespace MeatCountefeitDetector.Utils.EventAggregator
{
    public class EventImageData
    {
        public BitmapSource ImageBitmapSource { get; }

        public EventImageData(BitmapSource imageBitmapSource)
        {
            ImageBitmapSource = imageBitmapSource;
        }
    }
}
