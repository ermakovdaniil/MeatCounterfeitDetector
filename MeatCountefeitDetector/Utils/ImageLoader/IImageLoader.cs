using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

namespace MeatCountefeitDetector.Utils.ImageLoader
{
    public interface IImageLoader
    {
        string GetFileName(string fileName);
    }
}
