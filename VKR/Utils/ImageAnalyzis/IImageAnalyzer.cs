using DataAccess.Models;
using System.Collections.Generic;

namespace VKR.Utils.ImageAnalyzis;

public interface IImageAnalyzer
{
    public Result RunAnalysis(string pathToOrig, List<CounterfeitPath> counterfeitPaths, double percentOfSimilarity, User user);
}