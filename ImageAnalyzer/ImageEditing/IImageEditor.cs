using DataAccess.Models;
using System.Collections.Generic;
using System.Windows.Media.Imaging;

namespace ImageWorker.ImageEditing;

public interface IImageEditor
{
    public BitmapSource AdjustBrightnessAndContrast(BitmapSource source, int targetBrightness, int targetContrast);

    public int GetBrightness(BitmapSource source);
}