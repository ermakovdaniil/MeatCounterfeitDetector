using System;

using DataAccess.Models;


namespace VKR.UserInterface.Technologist.ImageAnalyzis;

public class ImageAnalyzerStub : IImageAnalyzer
{
    //TODO зачем вообще компания?
    //наверное лучше стоит передавать битмап вместо пути, но тут хз
    public Result analyze(string pathToOrig, Company company)
    {
        var TempOrigPath = new OriginalPath()
        {
            Path = pathToOrig,
        };

        var TempResPath = new ResultPath()
        {
            Init = TempOrigPath,
            Path = @"pack://application:,,,/resources/resImages/res.jpg",
        };

        var result = new Result()
        {
            Date = DateTime.Now.ToString(),
            Company = company,
            AnRes = "Обнаружен фальсификат: Каррагинан.",
            OrigPath = TempOrigPath,
            ResPath = TempResPath
        };

        return result;
    }
}
