using DataAccess.Models;
using Emgu.CV;
using ImageWorker.ProgressReporter;
using ImageWorker.ImageAnalyzis.KeypointAlgorithms;
using System;
using System.Collections.Generic;
using System.IO;

namespace ImageWorker.ImageAnalyzis;

public class ImageAnalyzer : IImageAnalyzer
{
    private int currentCounterfeitImage = 0;
    private string previousCounterfeitImage;
    private double previousPercent = 0;
    private Mat originalImageMat;

    private readonly IProgressReporter _progressReporter;

    public ImageAnalyzer(IProgressReporter progressReporter)
    {
        _progressReporter = progressReporter;

    }

    public Result RunAnalysis(string originalImage, List<CounterfeitImage> counterfeitImages, double percentOfSimilarity) // Возвращает DTO VM или Entity?
    {
        string analysisResult = "";
        string resultImage = "";
        double matchTime = 0;
        double score = 0;
        double tempTime = 0;

        if (previousCounterfeitImage is null || previousCounterfeitImage != originalImage)
        {
            currentCounterfeitImage = 0;
            previousCounterfeitImage = originalImage;
            originalImageMat = CvInvoke.Imread(originalImage, Emgu.CV.CvEnum.ImreadModes.AnyColor);
        }

        if (previousCounterfeitImage == originalImage && previousPercent > percentOfSimilarity)
        {
            previousPercent = percentOfSimilarity;
            currentCounterfeitImage = 0;
        }

        for (int i = 0; i < counterfeitImages.Count; i++)
        {
            AnalyzeImage(ref originalImage, counterfeitImages[i].ImagePath, out resultImage, out tempTime, out score, percentOfSimilarity, originalImageMat);
            matchTime += tempTime;
            if (score > percentOfSimilarity)
            {
                analysisResult = "Фальсификат обнаружен: " + counterfeitImages[i].Counterfeit.Name;
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

            _progressReporter.ReportProgress(i / counterfeitImages.Count);
        }
        matchTime = Math.Round(matchTime, 2);
        var result = new Result(); //CreateResult(originalImage, resultImage, analysisResult, matchTime, score);
        _progressReporter.ReportProgress(100);
        return result;
    }

    private void AnalyzeImage(ref string originalImage, string counterfeitImage, out string resultImage, out double matchTime, out double score, double percentOfSimilarity, Mat originalImageMat)
    {
        // УБРАТЬ
        string pathToBase = Directory.GetCurrentDirectory();
        string combinedPath = Path.Combine(pathToBase, counterfeitImage);

        // Получаем MAT и string base64
        Mat counterfeitMat = CvInvoke.Imread(combinedPath, Emgu.CV.CvEnum.ImreadModes.AnyColor);
        Mat resultImageMat = SIFTAlgorithm.Draw(originalImageMat, counterfeitMat, out matchTime, out score, percentOfSimilarity);

        resultImage = "";

        if (score > percentOfSimilarity)
        {
            //  originalImage = STRING BASE64 из originalImageMat
            //  resultImage = STRING BASE64 из resMat

            //var date = DateTime.Now.ToString("dd.mm.yyyy_hh.mm.ss");
            //var filename = "orig_" + date + ".png";
            //originalImage = @"..\..\..\resources\origImages\" + filename;
            //originalImageMat.Save(originalImage);

            //filename = "res_" + date + ".png";
            //resultImage = @"..\..\..\resources\resImages\" + filename;
            //resultImageMat.Save(resultImage);
        }
    }

    private Result CreateResult(string originalImage, string resultImage, string analysisResult, double time, double score, User user)
    {
        var tempOriginalImage = new OriginalImage
        {
            ImagePath = originalImage,
        };

        var tempResultImage = new ResultImage
        {
            OriginalImage = tempOriginalImage,
            ImagePath = resultImage,
        };

        var result = new Result
        {
            Date = DateTime.Now.ToString(),
            // User = user,
            AnalysisResult = analysisResult,
            Time = time,
            PercentOfSimilarity = score,
            OriginalImage = tempOriginalImage,
            ResultImage = tempResultImage,
        };
        return result;
    }
}