using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;

namespace MeatCountefeitDetector.Utils.ImageLoader
{
    public class ImageLoader : IImageLoader
    {
        public string GetFileName(string fileName)
        {
            string baseFileName = Path.GetFileNameWithoutExtension(fileName);
            string extension = Path.GetExtension(fileName);

            List<string> loadedFiles = Directory.GetFiles(Directory.GetCurrentDirectory(), baseFileName + "*" + extension).ToList();

            if (loadedFiles.Count > 0)
            {
                int count = 2;

                foreach (string loadedFile in loadedFiles)
                {
                    int number;
                    if (TryExtractNumberFromFileName(loadedFile, out number))
                    {
                        count = Math.Max(count, number + 1);
                    }
                }

                string newFileName = $"{baseFileName}({count}){extension}";
                Debug.WriteLine($"Loading: {newFileName}");
            }
            else
            {
                Debug.WriteLine($"Loading: {fileName}");
            }

            return fileName;
        }

        private bool TryExtractNumberFromFileName(string fileName, out int number)
        {
            int startIndex = fileName.LastIndexOf('(');
            int endIndex = fileName.LastIndexOf(')');

            if (startIndex != -1 && endIndex != -1 && endIndex > startIndex + 1)
            {
                string numberString = fileName.Substring(startIndex + 1, endIndex - startIndex - 1);

                if (int.TryParse(numberString, out number))
                {
                    return true;
                }
            }

            number = 0;
            return false;
        }
    }
}
