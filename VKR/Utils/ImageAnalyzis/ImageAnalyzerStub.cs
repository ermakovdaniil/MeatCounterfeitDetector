using System;
using DataAccess.Models;

namespace VKR.Utils.ImageAnalyzis;

public class ImageAnalyzerStub : IImageAnalyzer
{
    private void runPythonScript()
    {
        //int number = 5;
        //ScriptEngine engine = Python.CreateEngine();
        //ScriptScope scope = engine.CreateScope();
        //engine.ExecuteFile("Test.py", scope);

        //int y = 22;
        //dynamic x = scope.GetVariable("x");
        //dynamic z = scope.GetVariable("z");
        //Console.WriteLine($"{x} + {y} = {z}");

        //dynamic square = scope.GetVariable("square");
        //// Вызываем функцию и получаем результат
        //dynamic result = square(number);

    }

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

