using DataAccess.Models;
using Emgu.CV;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;

namespace ImageAnalyzis;

public class ImageAnalyzer : IImageAnalyzer
{
    public Result RunAnalysis(string pathToOrig, List<CounterfeitPath> counterfeitPaths, double percentOfSimilarity, User user)
    {
        string anRes = "";
        string resPath = "";
        double matchTime = 0;
        double score = 0;
        double tempTime = 0;
        for (int i = 0; i < counterfeitPaths.Count; i++)
        {
            AnalyzeImage(ref pathToOrig, counterfeitPaths[i].ImagePath, out resPath, out tempTime, out score, percentOfSimilarity);
            matchTime += tempTime;
            if (score > percentOfSimilarity)
            {
                anRes = "Фальсификат обнаружен: " + counterfeitPaths[i].Counterfeit.Name;
                matchTime += tempTime;
                break;
            }
            else
            {
                score = 0;
                matchTime += tempTime;
                anRes = "Фальсификат не обнаружен";
            }
        }
        var res = CreateResult(pathToOrig, resPath, anRes, user, matchTime, score);
        return res;
    }

    private void AnalyzeImage(ref string pathToOrig, string pathToCounterfeit, out string pathToResult, out double matchTime, out double score, double percentOfSimilarity)
    {
        string pathToBase = Directory.GetCurrentDirectory();
        //string pathToCounterfeits = @"..\..\..\resources\counterfeits\";
        string combinedPath = Path.Combine(pathToBase, pathToCounterfeit);
        Mat origMat = CvInvoke.Imread(pathToOrig, Emgu.CV.CvEnum.ImreadModes.AnyColor);
        Mat counterfeitMat = CvInvoke.Imread(combinedPath, Emgu.CV.CvEnum.ImreadModes.AnyColor);
        Mat resMat = SIFTAlgorithm.Draw(origMat, counterfeitMat, out matchTime, out score);

        pathToResult = "";
        if (score > percentOfSimilarity)
        {
            var date = DateTime.Now.ToString("dd.mm.yyyy_hh.mm.ss");
            var filename = "orig_" + date + ".jpg";
            pathToOrig = @"..\..\..\resources\origImages\" + filename;
            origMat.Save(pathToOrig);

            filename = "res_" + date + ".jpg";
            pathToResult = @"..\..\..\resources\resImages\" + filename;
            resMat.Save(pathToResult);
        }
    }

    private Result CreateResult(string pathToOrig, string pathToRes, string anRes, User user, double time, double score)
    {
        var tempOrigPath = new OriginalPath
        {
            Path = pathToOrig,
        };

        var tempResPath = new ResultPath
        {
            Init = tempOrigPath,
            Path = pathToRes,
        };

        var result = new Result
        {
            Date = DateTime.Now.ToString(),
            UserId = user.Id,
            AnRes = anRes,
            Time = time,
            PercentOfSimilarity = score,
            OrigPath = tempOrigPath,
            ResPath = tempResPath,
        };
        return result;
    }
}