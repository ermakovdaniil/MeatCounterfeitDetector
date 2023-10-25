using Emgu.CV;

namespace MeatCountefeitDetector.Utils
{
    public class DataEvent
    {
        public Mat Data { get; }

        public DataEvent(Mat data)
        {
            Data = data;
        }
    }
}
