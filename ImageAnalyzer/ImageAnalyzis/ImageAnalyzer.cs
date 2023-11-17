﻿using DataAccess.Models;
using Emgu.CV;
using ImageWorker.ProgressReporter;
using ImageWorker.ImageAnalyzis.KeypointAlgorithms;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Media.Imaging;
using ImageWorker.BitmapService;
using ClientAPI.DTO.Counterfeit;
using ClientAPI.DTO.CounterfeitImage;
using ClientAPI.DTO.Result;

namespace ImageWorker.ImageAnalyzis;

public class ImageAnalyzer : IImageAnalyzer
{
    private int currentCounterfeitImage = 0;
    private BitmapSource previousCounterfeitImage;
    private double previousPercent = 0;
    private Mat originalImageMat;
    private readonly IBitmapService _bitmapService;
    private readonly IProgressReporter _progressReporter;

    public ImageAnalyzer(IBitmapService bitmapService, 
                         IProgressReporter progressReporter)
    {
        _bitmapService = bitmapService;
        _progressReporter = progressReporter;
    }

    public CreateResultDTO RunAnalysis(BitmapSource originalImage, List<GetCounterfeitImageDTO> counterfeitImagesDTOs, double percentOfSimilarity, Guid userId)
    {
        string analysisResult = "";
        string originalImagePath = "";
        string resultImagePath = "";
        double matchTime = 0;
        double score = 0;
        double tempTime = 0;

        if (previousCounterfeitImage is null || previousCounterfeitImage != originalImage)
        {
            currentCounterfeitImage = 0;
            previousCounterfeitImage = originalImage;
            //originalImageMat = CvInvoke.Imread(originalImage, Emgu.CV.CvEnum.ImreadModes.AnyColor);
            originalImageMat = _bitmapService.BitmapSourceToMat(originalImage);
        }

        if (previousCounterfeitImage == originalImage && previousPercent > percentOfSimilarity)
        {
            previousPercent = percentOfSimilarity;
            currentCounterfeitImage = 0;
        }

        for (int i = 0; i < counterfeitImagesDTOs.Count; i++)
        {
            AnalyzeImage(ref originalImagePath, counterfeitImagesDTOs[i].ImagePath, out resultImagePath, out tempTime, out score, percentOfSimilarity, originalImageMat);
            matchTime += tempTime;
            if (score > percentOfSimilarity)
            {
                analysisResult = "Фальсификат обнаружен: " + counterfeitImagesDTOs[i].CounterfeitName;
                matchTime += tempTime;
                currentCounterfeitImage = i;
                break;
            }
            else
            {
                score = 0;
                matchTime += tempTime;
                analysisResult = "Фальсификат не обнаружен";
            }

            _progressReporter.ReportProgress(i / counterfeitImagesDTOs.Count);
        }
        matchTime = Math.Round(matchTime, 2);
        var result = CreateResult(originalImagePath, resultImagePath, analysisResult, matchTime, score, userId);
        _progressReporter.ReportProgress(100);
        return result;
    }

    private void AnalyzeImage(ref string originalImagePath, string counterfeitImage, out string resultImagePath, out double matchTime, out double score, double percentOfSimilarity, Mat originalImageMat)
    {
        string pathToBase = Directory.GetCurrentDirectory();
        string combinedPath = Path.Combine(pathToBase, counterfeitImage);

        Mat counterfeitMat = CvInvoke.Imread(combinedPath, Emgu.CV.CvEnum.ImreadModes.AnyColor);
        Mat resultImageMat = SIFTAlgorithm.Draw(originalImageMat, counterfeitMat, out matchTime, out score, percentOfSimilarity);

        resultImagePath = "";

        if (score > percentOfSimilarity)
        {
            var date = DateTime.Now.ToString("dd.mm.yyyy_hh.mm.ss");
            var filename = "orig_" + date + ".png";
            originalImagePath = @"..\..\..\resources\origImages\" + filename;
            originalImageMat.Save(originalImagePath);

            filename = "res_" + date + ".png";
            resultImagePath = @"..\..\..\resources\resImages\" + filename;
            resultImageMat.Save(resultImagePath);
        }
    }

    private CreateResultDTO CreateResult(string originalImagePath, string resultImagePath, string analysisResult, double time, double score, Guid userId)
    {
        //var tempOriginalImage = new OriginalImage
        //{
        //    ImagePath = originalImagePath,
        //};

        //var tempResultImage = new ResultImage
        //{
        //    OriginalImage = tempOriginalImage,
        //    ImagePath = resultImagePath,
        //};

        var result = new CreateResultDTO
        {
            Date = DateTime.Now.ToString(),
            UserId = userId,
            AnalysisResult = analysisResult,
            Time = time,
            PercentOfSimilarity = score,
            OriginalImagePath = originalImagePath,
            ResultImagePath = resultImagePath,
        };
        return result;
    }
}