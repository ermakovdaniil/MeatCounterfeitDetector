using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Features2D;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;


namespace ImageWorker.ImageAnalyzis.KeypointAlgorithms
{
    public class ORB_Algorithm : IImageMatchingAlgorithm
    {
        private Mat previousModelImage;
        private Mat modelDescriptors;
        private VectorOfKeyPoint modelKeyPoints;

        public Mat Draw(Mat originalImageMat, Mat grayscaleImageMat, Mat observedImageMat, out double matchTime, out double score, double percentOfSimilarity)
        {
            Stopwatch watch;
            watch = Stopwatch.StartNew();

            Mat homography;
            VectorOfKeyPoint observedKeyPoints;
            using (VectorOfVectorOfDMatch matches = new VectorOfVectorOfDMatch())
            {
                Mat mask;
                FindMatch(grayscaleImageMat, observedImageMat, out observedKeyPoints, matches, out mask, out homography);

                CalculateScore(matches, mask, out score, grayscaleImageMat, observedImageMat);

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

        private void CalculateScore(VectorOfVectorOfDMatch matches, Mat mask, out double score, Mat grayscaleImageMat, Mat observedImageMat)
        {
            double distMatches = 0;
            for (int i = 0; i < matches.Size; i++)
            {
                var arrayOfMatches = matches[i].ToArray();
                if (arrayOfMatches[0].Distance < 0.9 * arrayOfMatches[1].Distance)
                {
                    distMatches++;
                }
            }

            List<double> scores = new List<double>();

            double goodMatches = CountHowManyPairsExist(mask);

            var temp = mask.GetData().Length - goodMatches;
            double fisrtScore = goodMatches / temp;

            scores.Add(fisrtScore);

            double secondScore = goodMatches / distMatches;
            if (goodMatches == distMatches)
            {
                secondScore = goodMatches / (distMatches + 3);
            }
            scores.Add(secondScore);

            double amountOfMatches = 21;
            double oneMatch = 0.9998 / amountOfMatches;
            double thirdScore;

            if (goodMatches > amountOfMatches)
            {
                thirdScore = oneMatch * amountOfMatches;
            }
            else
            {
                thirdScore = oneMatch * goodMatches;
            }
            scores.Add(thirdScore);

            Mat scoreImg = new Mat();
            double minVal = double.MaxValue;
            double fourthScore = double.MinValue;
            var minLoc = new Point();
            var maxLoc = new Point();
            CvInvoke.MatchTemplate(grayscaleImageMat, observedImageMat, scoreImg, TemplateMatchingType.CcoeffNormed);
            CvInvoke.MinMaxLoc(scoreImg, ref minVal, ref fourthScore, ref minLoc, ref maxLoc);

            scores.Add(fourthScore);
            score = scores.Max();

            if (double.IsInfinity(score) || score > 100.00)
            {
                score = 100.00;
            }
            else
            {
                score *= 100;
                score = Math.Round(score, 2);
            }
        }

        private void FindMatch(Mat grayscaleImageMat, Mat observedImageMat, out VectorOfKeyPoint observedKeyPoints, VectorOfVectorOfDMatch matches, out Mat mask, out Mat homography)
        {
            int k = 2;
            double uniquenessThreshold = 0.9;
            homography = null;

            SIFT sift = new SIFT();

            if (modelKeyPoints is null || modelKeyPoints is null || previousModelImage != grayscaleImageMat)
            {
                previousModelImage = grayscaleImageMat;
                modelDescriptors = new Mat();
                modelKeyPoints = new VectorOfKeyPoint();
                sift.DetectAndCompute(grayscaleImageMat, null, modelKeyPoints, modelDescriptors, false);
            }

            Mat observedDescriptors = new Mat();
            observedKeyPoints = new VectorOfKeyPoint();
            sift.DetectAndCompute(observedImageMat, null, observedKeyPoints, observedDescriptors, false);

            BFMatcher matcher = new BFMatcher(DistanceType.L2);
            matcher.Add(modelDescriptors);
            matcher.KnnMatch(observedDescriptors, matches, k, null);

            mask = new Mat(matches.Size, 1, DepthType.Cv8U, 1);
            mask.SetTo(new MCvScalar(255));

            Features2DToolbox.VoteForUniqueness(matches, uniquenessThreshold, mask);

            int nonZeroCount = CvInvoke.CountNonZero(mask);
            if (nonZeroCount >= 4)
            {
                nonZeroCount = Features2DToolbox.VoteForSizeAndOrientation(modelKeyPoints, observedKeyPoints, matches, mask, 1.5, 20);
                if (nonZeroCount >= 4)
                    homography = Features2DToolbox.GetHomographyMatrixFromMatchedFeatures(modelKeyPoints, observedKeyPoints, matches, mask, 2);
            }
        }

        private double CountHowManyPairsExist(Mat mask)
        {
            var matched = mask.GetData();
            var list = matched.OfType<byte>().ToList();
            var count = list.Count(a => a.Equals(1));
            return count;
        }
    }
}