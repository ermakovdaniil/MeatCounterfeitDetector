using Emgu.CV;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageWorker.ImageAnalyzis.KeypointAlgorithms
{
    public interface IImageMatchingAlgorithm
    {
        Mat Draw(Mat originalImageMat, Mat grayscaleImageMat, Mat observedImageMat, out double matchTime, out double score, double percentOfSimilarity);
    }
}