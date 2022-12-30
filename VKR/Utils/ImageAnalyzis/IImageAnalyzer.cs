using DataAccess.Models;


namespace VKR.UserInterface.Technologist.ImageAnalyzis;

public interface IImageAnalyzer
{
    public Result analyze(string pathToOrig, Company company);
}
