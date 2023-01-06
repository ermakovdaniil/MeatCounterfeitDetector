using System;

using DataAccess.Models;


namespace VKR.Utils.ImageAnalyzis;

public class ImageAnalyzerStub : IImageAnalyzer
{
    // наверное лучше стоит передавать битмап вместо пути, но тут хз
    public Result analyze(string pathToOrig, User user, double precentOfSimilarity)
    {
        var tempOrigPath = new OriginalPath
        {
            Path = pathToOrig,
        };

        var tempResPath = new ResultPath
        {
            Init = tempOrigPath,
            Path = @"pack://application:,,,/resources/resImages/res.jpg",
        };

        var result = new Result
        {
            Date = DateTime.Now.ToString(),
            User = user,
            AnRes = "Обнаружен фальсификат: Каррагинан.",
            OrigPath = tempOrigPath,
            ResPath = tempResPath,
        };

        return result;
    }
}

