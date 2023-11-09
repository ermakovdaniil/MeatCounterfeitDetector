using DataAccess.Models;
using System.Collections.Generic;

namespace ImageWorker.ImageAnalyzis;

public interface IImageAnalyzer
{
    public Result RunAnalysis(string originalImage, List<CounterfeitImage> counterfeitImages, double percentOfSimilarity);
}