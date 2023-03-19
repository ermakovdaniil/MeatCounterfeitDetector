using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Features2D;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Numpy;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;


namespace ImageAnalyzis
{
    public static class SIFTAlgorithm
    {
        private static Mat previousModelImage;
        private static Mat modelDescriptors;
        private static VectorOfKeyPoint modelKeyPoints;

        public static Mat Draw(Mat modelImage, Mat observedImage, out double matchTime, out double score, double percentOfSimilarity)
        {
            Stopwatch watch;
            watch = Stopwatch.StartNew();

            Mat homography;
            VectorOfKeyPoint observedKeyPoints;
            using (VectorOfVectorOfDMatch matches = new VectorOfVectorOfDMatch())
            {
                Mat mask;
                FindMatch(modelImage, observedImage, out observedKeyPoints, matches, out mask, out homography);

                CalculateScore(matches, mask, out score, modelImage, observedImage);

                Mat result = new Mat();
                if (score > percentOfSimilarity)
                {
                    Features2DToolbox.DrawMatches(modelImage, modelKeyPoints, observedImage, observedKeyPoints,
                        matches, result, new MCvScalar(0, 0, 255), new MCvScalar(255, 255, 255), mask, Features2DToolbox.KeypointDrawType.NotDrawSinglePoints);

                    if (homography != null)
                    {
                        Rectangle rect = new Rectangle(Point.Empty, modelImage.Size);

                        PointF[] pts = new PointF[]
                        {
                        new PointF(rect.Left, rect.Bottom),
                        new PointF(rect.Right, rect.Bottom),
                        new PointF(rect.Right, rect.Top),
                        new PointF(rect.Left, rect.Top)
                        };
                        pts = CvInvoke.PerspectiveTransform(pts, homography);

                        Point[] points = Array.ConvertAll<PointF, Point>(pts, Point.Round);
                        using (VectorOfPoint vp = new VectorOfPoint(points))
                        {
                            CvInvoke.Polylines(result, vp, true, new MCvScalar(255, 0, 0, 255), 0, LineType.EightConnected, 0);
                        }
                    }
                }

                watch.Stop();
                matchTime = watch.ElapsedMilliseconds;
                return result;
            }
        }

        private static void CalculateScore(VectorOfVectorOfDMatch matches, Mat mask, out double score, Mat modelImage, Mat observedImage)
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

            var temp = (double)mask.GetData().Length - goodMatches;
            double fisrtScore = goodMatches / temp;

            scores.Add(fisrtScore);

            double secondScore = goodMatches / distMatches;
            if(goodMatches == distMatches)
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
            double minVal = Double.MaxValue;
            double fourthScore = Double.MinValue;
            var minLoc = new Point();
            var maxLoc = new Point();
            CvInvoke.MatchTemplate(modelImage, observedImage, scoreImg, TemplateMatchingType.CcoeffNormed);
            CvInvoke.MinMaxLoc(scoreImg, ref minVal, ref fourthScore, ref minLoc, ref maxLoc);

            scores.Add(fourthScore);
            score = scores.Max();

            score *= 100;
            score = Math.Round(score, 2);
        }

        private static void FindMatch(Mat modelImage, Mat observedImage, out VectorOfKeyPoint observedKeyPoints, VectorOfVectorOfDMatch matches, out Mat mask, out Mat homography)
        {
            int k = 2;
            double uniquenessThreshold = 0.9;
            homography = null;

            SIFT sift = new SIFT();

            if (modelKeyPoints is null || modelKeyPoints is null || previousModelImage != modelImage)
            {
                previousModelImage = modelImage;
                modelDescriptors = new Mat();
                modelKeyPoints = new VectorOfKeyPoint();
                sift.DetectAndCompute(modelImage, null, modelKeyPoints, modelDescriptors, false);
            }

            Mat observedDescriptors = new Mat();
            observedKeyPoints = new VectorOfKeyPoint();
            sift.DetectAndCompute(observedImage, null, observedKeyPoints, observedDescriptors, false);

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

        private static double CountHowManyPairsExist(Mat mask)
        {
            var matched = mask.GetData();
            var list = matched.OfType<byte>().ToList();
            var count = list.Count(a => a.Equals(1));
            return count;
        }
    }
}