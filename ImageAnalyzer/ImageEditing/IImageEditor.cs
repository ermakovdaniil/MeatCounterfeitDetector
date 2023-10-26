using DataAccess.Models;
using System.Collections.Generic;
using System.Windows.Media.Imaging;

namespace ImageWorker.ImageEditing;

public interface IImageEditor
{
    public BitmapSource ChangeBrightness(BitmapSource imageBitmapSource, double brightness);
}