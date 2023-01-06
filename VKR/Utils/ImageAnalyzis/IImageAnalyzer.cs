using DataAccess.Models;


namespace VKR.Utils.ImageAnalyzis;

public interface IImageAnalyzer
{
    public Result analyze(string pathToOrig, User user, double precentOfSimilarity);
}