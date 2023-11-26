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

namespace ImageWorker.ImageAnalyzis;

public class ImageAnalyzer : FeatureMatchingHelper, IImageAnalyzer
{
    private int currentCounterfeitImage = 0;
    private BitmapSource previousCounterfeitImage;
    private double previousPercent = 0;
    private Mat originalImageMat;
    private Mat grayscaleImageMat = new Mat();

    private VectorOfKeyPoint modelKeyPoints;

    private readonly Dictionary<Algorithms, IImageMatchingAlgorithm> algorithmDictionary;

    private readonly IBitmapService _bitmapService;
    private readonly IProgressReporter _progressReporter;
    //private readonly IImageMatchingAlgorithm _imageMatchingAlgorithm;

    public ImageAnalyzer(IBitmapService bitmapService,
                         IProgressReporter progressReporter
                         /*IImageMatchingAlgorithm imageMatchingAlgorithm*/)
    {
        _bitmapService = bitmapService;
        _progressReporter = progressReporter;
        //_imageMatchingAlgorithm = imageMatchingAlgorithm;

        algorithmDictionary = new Dictionary<Algorithms, IImageMatchingAlgorithm>
        {
            { Algorithms.SIFT, new SIFT_Algorithm() },
            { Algorithms.ORB, new ORB_Algorithm() },
            { Algorithms.AKAZE, new AKAZE_Algorithm() },
            { Algorithms.RANSAC, new RANSAC_Algorithm() },
            { Algorithms.SURF, new SURF_Algorithm() },
            { Algorithms.BRISK, new BRISK_Algorithm() },
            { Algorithms.MSER, new MSER_Algorithm() },
        };
    }

    public CreateResultDTO RunAnalysis(BitmapSource originalImage, List<GetCounterfeitImageDTO> counterfeitImagesDTOs, double percentOfSimilarity, Guid userId, Algorithms algorithm)
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
            CvInvoke.CvtColor(originalImageMat, grayscaleImageMat, ColorConversion.Bgr2Gray);
        }

        if (previousCounterfeitImage == originalImage && previousPercent > percentOfSimilarity)
        {
            previousPercent = percentOfSimilarity;
            currentCounterfeitImage = 0;
        }

        for (int i = 0; i < counterfeitImagesDTOs.Count; i++)
        {
            AnalyzeImage(ref originalImagePath, counterfeitImagesDTOs[i].ImagePath, out resultImagePath, out tempTime, out score, percentOfSimilarity, algorithm);
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

    private void AnalyzeImage(ref string originalImagePath, string counterfeitImage, out string resultImagePath, out double matchTime, out double score, double percentOfSimilarity, Algorithms algorithm)
    {
        string pathToBase = Directory.GetCurrentDirectory();
        string combinedPath = Path.Combine(pathToBase, counterfeitImage);

        Mat counterfeitMat = CvInvoke.Imread(combinedPath, Emgu.CV.CvEnum.ImreadModes.AnyColor);

        //Mat resultImageMat = algorithmDictionary[algorithm].Draw(originalImageMat, grayscaleImageMat, counterfeitMat, out matchTime, out score, percentOfSimilarity);
        Mat resultImageMat = Draw(originalImageMat, grayscaleImageMat, counterfeitMat, out matchTime, out score, percentOfSimilarity, algorithm);

        resultImagePath = "";

        if (score > percentOfSimilarity)
        {
            // TODO: вместо этого сделать сохранение оригинала с именем открытого изображения. И если уже такое есть то не делать копию.
            var date = DateTime.Now.ToString("dd.mm.yyyy_hh.mm.ss");
            var filename = "orig_" + date + ".png";
            originalImagePath = @"..\..\..\resources\origImages\" + filename;
            originalImageMat.Save(originalImagePath);

            filename = "res_" + date + ".png";
            resultImagePath = @"..\..\..\resources\resImages\" + filename;
            resultImageMat.Save(resultImagePath);
        }
    }

    public Mat Draw(Mat originalImageMat, Mat grayscaleImageMat, Mat observedImageMat, out double matchTime, out double score, double percentOfSimilarity, Algorithms algorithm)
    {
        double uniquenessThreshold = 0.75;

        Mat grayscaleObservedImageMat = new Mat();
        CvInvoke.CvtColor(observedImageMat, grayscaleObservedImageMat, ColorConversion.Bgr2Gray);

        Stopwatch watch;
        watch = Stopwatch.StartNew();

        Mat homography;
        VectorOfKeyPoint observedKeyPoints;
        using (VectorOfVectorOfDMatch matches = new VectorOfVectorOfDMatch())
        {
            Mat mask;
            modelKeyPoints = algorithmDictionary[algorithm].FindMatch(originalImageMat, observedImageMat, out observedKeyPoints, matches, out mask, out homography, uniquenessThreshold);
            //modelKeyPoints = algorithmDictionary[algorithm].FindMatch(grayscaleImageMat, grayscaleObservedImageMat, out observedKeyPoints, matches, out mask, out homography, uniquenessThreshold);

            //double goodMatchesCount = 0;
            //double distThreshold = 64;
            //for (int i = 0; i < matches.Size; i++)
            //{
            //    var arrayOfMatches = matches[i].ToArray();
            //    if (arrayOfMatches[0].Distance < distThreshold)
            //    {
            //        goodMatchesCount++;
            //    }
            //}

            //double dist_th = 64;
            //for (int i = 0; i < descriptors_object.rows; i++)
            //{
            //    if (matches[i].distance < dist_th)
            //    { good_matches.push_back(matches[i]); }
            //}

            double goodMatchesCount = CountGoodMatches(matches, uniquenessThreshold);

            CalculateScore(mask, out score, grayscaleImageMat, grayscaleObservedImageMat, goodMatchesCount);

            Mat result = new Mat();
            if (score > percentOfSimilarity)
            {
                Features2DToolbox.DrawMatches(originalImageMat, modelKeyPoints, observedImageMat, observedKeyPoints,
                    matches, result, new MCvScalar(0, 0, 255), new MCvScalar(255, 255, 255), mask, Features2DToolbox.KeypointDrawType.NotDrawSinglePoints);

                if (homography != null)
                {
                    Rectangle rect = new Rectangle(Point.Empty, originalImageMat.Size);

                    PointF[] pts = new PointF[]
                    {
                            new PointF(rect.Left, rect.Bottom),
                            new PointF(rect.Right, rect.Bottom),
                            new PointF(rect.Right, rect.Top),
                            new PointF(rect.Left, rect.Top)
                    };
                    pts = CvInvoke.PerspectiveTransform(pts, homography);

                    Point[] points = Array.ConvertAll(pts, Point.Round);
                    using (VectorOfPoint vp = new VectorOfPoint(points))
                    {
                        CvInvoke.Polylines(result, vp, true, new MCvScalar(255, 0, 0, 255), 0, LineType.EightConnected, 0);
                    }
                }
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