using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

namespace MeatCountefeitDetector.Utils.ImageLoader
{
    public class ImageLoader : IImageLoader
    {
        public string GetFileName(string fileName, string extraPartOfPath, string pathToInitialImage)
        {
            string baseFileName = Path.GetFileNameWithoutExtension(fileName);
            string extension = Path.GetExtension(fileName);

            List<string> loadedFiles = Directory.GetFiles(Directory.GetCurrentDirectory(), extraPartOfPath + baseFileName + "*" + extension).ToList();

            Image newImage = Image.FromFile(pathToInitialImage);

            string existingFileName = GetExistingFileName(newImage, loadedFiles);
            if (!string.IsNullOrEmpty(existingFileName))
            {
                return Path.GetFileName(existingFileName);
            }

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
                fileName = $"{baseFileName}({count}){extension}";
            }

            return fileName;
        }

        private string GetExistingFileName(Image newImage, List<string> loadedFiles)
        {
            foreach (string loadedFile in loadedFiles)
            {
                Image existingImage = Image.FromFile(loadedFile);

                if (ImagesAreEqual(newImage, existingImage))
                {
                    return loadedFile;
                }
            }

            return null;
        }

        private bool ImagesAreEqual(Image image1, Image image2)
        {
            using (MemoryStream ms1 = new MemoryStream())
            using (MemoryStream ms2 = new MemoryStream())
            {
                image1.Save(ms1, image1.RawFormat);
                image2.Save(ms2, image2.RawFormat);

                byte[] bytes1 = ms1.ToArray();
                byte[] bytes2 = ms2.ToArray();

                return StructuralComparisons.StructuralEqualityComparer.Equals(bytes1, bytes2);
            }
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
