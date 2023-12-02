using Emgu.CV;
using ImageWorker.ProgressReporter;
using ImageWorker.ImageAnalyzis.KeypointAlgorithms;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Media.Imaging;
using ImageWorker.BitmapService;
using ClientAPI.DTO.CounterfeitImage;
using ClientAPI.DTO.Result;
using ImageWorker.Enums;
using Emgu.CV.CvEnum;
using Emgu.CV.Features2D;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using System.Diagnostics;
using System.Drawing;
using Autofac.Features.Indexed;
using System.Windows;

namespace ImageWorker.ImageAnalyzis;

public class ImageAnalyzer : FeatureMatchingHelper, IImageAnalyzer
{
    private int currentCounterfeitImage = 0;
    private BitmapSource previousCounterfeitImage;
    private double previousPercent = 0;
    private Mat originalImageMat;
    private Mat grayscaleImageMat = new Mat();

    private VectorOfKeyPoint modelKeyPoints;

    private IImageMatchingAlgorithm _algorithm;
    private readonly IIndex<Algorithms, IImageMatchingAlgorithm> _algorithms;
    private readonly IBitmapService _bitmapService;
    private readonly IProgressReporter _progressReporter;
    //private readonly IImageMatchingAlgorithm _imageMatchingAlgorithm;

    public ImageAnalyzer(IBitmapService bitmapService,
                         IProgressReporter progressReporter,
                         IIndex<Algorithms, IImageMatchingAlgorithm> algorithms
                         /*IImageMatchingAlgorithm imageMatchingAlgorithm*/)
    {
        _bitmapService = bitmapService;
        _progressReporter = progressReporter;
        _algorithms = algorithms;
        //_imageMatchingAlgorithm = imageMatchingAlgorithm;

        //algorithmDictionary = new Dictionary<Algorithms, IImageMatchingAlgorithm>
        //{
        //    { Algorithms.SIFT, new SIFT_Algorithm() },
        //    { Algorithms.ORB, new ORB_Algorithm() },
        //    { Algorithms.AKAZE, new AKAZE_Algorithm() },
        //    { Algorithms.RANSAC, new RANSAC_Algorithm() },
        //    { Algorithms.BRISK, new BRISK_Algorithm() },
        //    { Algorithms.MSER, new MSER_Algorithm() },
        //};
    }

    public CreateResultDTO RunAnalysis(BitmapSource originalImage, List<GetCounterfeitImageDTO> counterfeitImagesDTOs,
                                       double percentOfSimilarity, Guid userId, Algorithms algorithm, string fileName)
    {
        string analysisResult = "";
        string originalImagePath = "";
        string resultImagePath = "";
        double matchTime = 0;
        double score = 0;
        double tempTime = 0;


        _algorithm = null;

        switch (algorithm)
        {
            case Algorithms.SIFT:
                _algorithm = _algorithms[Algorithms.SIFT];
                break;

            case Algorithms.ORB:
                _algorithm = _algorithms[Algorithms.ORB];
                break;

            case Algorithms.AKAZE:
                _algorithm = _algorithms[Algorithms.AKAZE];
                break;

            case Algorithms.RANSAC:
                _algorithm = _algorithms[Algorithms.RANSAC];
                break;

            case Algorithms.BRISK:
                _algorithm = _algorithms[Algorithms.BRISK];
                break;

            case Algorithms.MSER:
                _algorithm = _algorithms[Algorithms.MSER];
                break;

            default:
                break;
        }

        if (previousCounterfeitImage is null || previousCounterfeitImage != originalImage)
        {
            currentCounterfeitImage = 0;
            previousCounterfeitImage = originalImage;
            //originalImageMat = CvInvoke.Imread(originalImage, Emgu.CV.CvEnum.ImreadModes.AnyColor);
            originalImageMat = _bitmapService.BitmapSourceToMat(originalImage);
            CvInvoke.CvtColor(originalImageMat, grayscaleImageMat, ColorConversion.Bgr2Gray);
        }

        if (previousCounterfeitImage == originalImage && previousPercent > percentOfSimilarity)
        {
            previousPercent = percentOfSimilarity;
            currentCounterfeitImage = 0;
        }

        for (int i = 0; i < counterfeitImagesDTOs.Count; i++)
        {
            AnalyzeImage(ref originalImagePath, counterfeitImagesDTOs[i].ImagePath, out resultImagePath, out tempTime, out score, percentOfSimilarity, algorithm, fileName);
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

    private void AnalyzeImage(ref string originalImagePath, string counterfeitImage, out string resultImagePath, out double matchTime,
                              out double score, double percentOfSimilarity, Algorithms algorithm, string fileName)
    {
        string pathToBase = Directory.GetCurrentDirectory();
        string combinedPath = Path.Combine(pathToBase, counterfeitImage);

        Mat counterfeitMat = CvInvoke.Imread(combinedPath, Emgu.CV.CvEnum.ImreadModes.AnyColor);

        Mat resultImageMat = Draw(originalImageMat, grayscaleImageMat, counterfeitMat, out matchTime, out score, percentOfSimilarity, algorithm);

        resultImagePath = "";


            var date = DateTime.Now.ToString("dd.mm.yyyy_hh.mm.ss");
            //var filename = "orig_" + date + ".png";
            originalImagePath = @"..\..\..\resources\origImages\" + fileName;
            originalImageMat.Save(originalImagePath);

        if (score > percentOfSimilarity)
        {
            var resFilename = "res_" + date + ".png";
            resultImagePath = @"..\..\..\resources\resImages\" + resFilename;
            resultImageMat.Save(resultImagePath);
        }
    }

    public Mat Draw(Mat originalImageMat, Mat grayscaleImageMat, Mat observedImageMat, out double matchTime, out double score, double percentOfSimilarity, Algorithms algorithm)
    {
        double uniquenessThreshold = 0.85;

        Mat grayscaleObservedImageMat = new Mat();
        CvInvoke.CvtColor(observedImageMat, grayscaleObservedImageMat, ColorConversion.Bgr2Gray);

        Stopwatch watch;
        watch = Stopwatch.StartNew();

        Mat homography;
        VectorOfKeyPoint observedKeyPoints;
        using (VectorOfVectorOfDMatch matches = new VectorOfVectorOfDMatch())
        {
            Mat mask;
            modelKeyPoints = _algorithm.FindMatch(originalImageMat, observedImageMat, out observedKeyPoints, matches, out mask, out homography, uniquenessThreshold);

            double goodMatchesCount = CountGoodMatches(matches, uniquenessThreshold);

            CalculateScore(mask, out score, grayscaleImageMat, grayscaleObservedImageMat, goodMatchesCount);

            Mat result = new Mat();
            if (score > percentOfSimilarity)
            {
                Features2DToolbox.DrawMatches(originalImageMat, modelKeyPoints, observedImageMat, observedKeyPoints, matches, result, 
                    new MCvScalar(0, 0, 255), new MCvScalar(255, 255, 255), mask, Features2DToolbox.KeypointDrawType.DrawRichKeypoints);

                //Features2DToolbox.DrawMatches(originalImageMat, modelKeyPoints, observedImageMat, observedKeyPoints,
                //    matches, result, new MCvScalar(0, 0, 255), new MCvScalar(255, 255, 255), mask, Features2DToolbox.KeypointDrawType.NotDrawSinglePoints);
                //CvInvoke.Imshow("Matches", result);
                //if (homography != null)
                //{
                //    Rectangle rect = new Rectangle(Point.Empty, originalImageMat.Size);
                //    PointF[] pts = new PointF[]
                //    {
                //            new PointF(rect.Left, rect.Bottom),
                //            new PointF(rect.Right, rect.Bottom),
                //            new PointF(rect.Right, rect.Top),
                //            new PointF(rect.Left, rect.Top)
                //    };
                //    pts = CvInvoke.PerspectiveTransform(pts, homography);
                //    Point[] points = Array.ConvertAll(pts, Point.Round);
                //    using (VectorOfPoint vp = new VectorOfPoint(points))
                //    {
                //        CvInvoke.Polylines(result, vp, true, new MCvScalar(255, 0, 0, 255), 5, LineType.EightConnected, 0);
                //    }
                //}
            }

            watch.Stop();
            matchTime = watch.Elapsed.TotalSeconds;
            return result;
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