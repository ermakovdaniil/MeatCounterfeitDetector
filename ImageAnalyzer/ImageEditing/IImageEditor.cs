using DataAccess.Models;
using System.Collections.Generic;
using System.Windows.Media.Imaging;

namespace ImageWorker.ImageEditing;

public interface IImageEditor
{
    public BitmapSource AdjustFilter(BitmapSource source, int brightness, int contrast, int noise, int sharpness, int focalLengthX, int focalLengthY, double width, double height, int rotation);
    public int GetBrightness(BitmapSource source);
    public int GetContrast(BitmapSource source);
}