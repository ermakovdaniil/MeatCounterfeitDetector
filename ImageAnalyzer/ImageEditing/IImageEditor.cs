using DataAccess.Models;
using System.Collections.Generic;
using System.Windows.Media.Imaging;

namespace ImageWorker.ImageEditing;

public interface IImageEditor
{
    public BitmapSource AdjustBrightnessAndContrast(BitmapSource source, int brightness, int contrast);

    public BitmapSource AdjustNoise(BitmapSource source, int noise);

    public int GetBrightness(BitmapSource source);

    public int GetContrast(BitmapSource source);
}