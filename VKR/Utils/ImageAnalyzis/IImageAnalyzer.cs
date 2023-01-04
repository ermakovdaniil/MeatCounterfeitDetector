using DataAccess.Models;


namespace VKR.Utils.ImageAnalyzis;

public interface IImageAnalyzer
{
    public Result analyze(string pathToOrig, Counterfeit counterfeit, double precentOfSimilarity);
}

