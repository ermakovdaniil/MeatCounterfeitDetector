﻿using DataAccess.Models;
using Emgu.CV;
using ImageWorker.ProgressReporter;
using ImageWorker.ImageAnalyzis.KeypointAlgorithms;
using System;
using System.Collections.Generic;
using System.IO;

namespace ImageWorker.ImageAnalyzis;

public class ImageAnalyzer : IImageAnalyzer
{
    private int currentPath = 0;
    private string previousPath;
    private double previousPercent = 0;
    private Mat origMat;

    private readonly IProgressReporter _progressReporter;

    public ImageAnalyzer(IProgressReporter progressReporter)
    {
        _progressReporter = progressReporter;
    }

    public Result RunAnalysis(string pathToOrig, List<CounterfeitPath> counterfeitPaths, double percentOfSimilarity, User user)
    {
        string anRes = "";
        string resPath = "";
        double matchTime = 0;
        double score = 0;
        double tempTime = 0;

        if (previousPath is null || previousPath != pathToOrig)
        {
            currentPath = 0;
            previousPath = pathToOrig;
            origMat = CvInvoke.Imread(pathToOrig, Emgu.CV.CvEnum.ImreadModes.AnyColor);
        }

        if (previousPath == pathToOrig && previousPercent > percentOfSimilarity)
        {
            previousPercent = percentOfSimilarity;
            currentPath = 0;
        }

        for (int i = 0; i < counterfeitPaths.Count; i++)
        {
            AnalyzeImage(ref pathToOrig, counterfeitPaths[i].EncodedImage, out resPath, out tempTime, out score, percentOfSimilarity, origMat);
            matchTime += tempTime;
            if (score > percentOfSimilarity)
            {
                anRes = "Фальсификат обнаружен: " + counterfeitPaths[i].Counterfeit.Name;
                matchTime += tempTime;
                currentPath = i;
                break;
            }
            else
            {
                score = 0;
                matchTime += tempTime;
                anRes = "Фальсификат не обнаружен";
            }

            _progressReporter.ReportProgress(i / counterfeitPaths.Count);
        }
        matchTime = Math.Round(matchTime, 2);
        var res = CreateResult(pathToOrig, resPath, anRes, user, matchTime, score);
        _progressReporter.ReportProgress(100);
        return res;
    }

    private void AnalyzeImage(ref string pathToOrig, string pathToCounterfeit, out string pathToResult, out double matchTime, out double score, double percentOfSimilarity, Mat origMat)
    {
        string pathToBase = Directory.GetCurrentDirectory();
        string combinedPath = Path.Combine(pathToBase, pathToCounterfeit);
        Mat counterfeitMat = CvInvoke.Imread(combinedPath, Emgu.CV.CvEnum.ImreadModes.AnyColor);
        Mat resMat = SIFTAlgorithm.Draw(origMat, counterfeitMat, out matchTime, out score, percentOfSimilarity);
        pathToResult = "";
        if (score > percentOfSimilarity)
        {
            var date = DateTime.Now.ToString("dd.mm.yyyy_hh.mm.ss");
            var filename = "orig_" + date + ".png";
            pathToOrig = @"..\..\..\resources\origImages\" + filename;
            origMat.Save(pathToOrig);

            filename = "res_" + date + ".png";
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