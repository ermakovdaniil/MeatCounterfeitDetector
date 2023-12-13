using Emgu.CV;
using System.Windows.Media.Imaging;

namespace ImageWorker.ImageEditing
{
    public interface IImageEditor
    {
        public BitmapSource AdjustFilter(BitmapSource source, int brightness, int contrast, int noise, int sharpness, int glare, int focalLengthX, int focalLengthY, double width, double height, int rotation);

        public Mat AdjustBrightnessAndContrast(Mat imageMat, int brightness, int contrast);
        public Mat AdjustNoise(Mat imageMat, int noise);
        public Mat AdjustSharpness(Mat imageMat, int sharpness);
        public Mat AdjustGlare(Mat imageMat, int glare);
        public Mat AdjustDistortion(Mat imageMat, int focalLengthX, int focalLengthY);
        public Mat AdjustWidthAndHeight(Mat imageMat, double width, double height);
        public Mat AdjustRoatation(Mat imageMat, int rotation);

        public int GetBrightness(BitmapSource source);
        public int GetContrast(BitmapSource source);
        //public int GetNoise(BitmapSource source);
        //public int GetGlare(BitmapSource source);
        //public int GetDistortion(BitmapSource source);
    }
}
