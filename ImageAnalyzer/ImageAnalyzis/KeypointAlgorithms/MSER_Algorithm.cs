using DataAccess.Models;
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
    public class MSER_Algorithm : FeatureMatchingHelper, IImageMatchingAlgorithm
    {
        private Mat previousModelImage;
        private Mat modelDescriptors;
        private VectorOfKeyPoint modelKeyPoints;

        public Mat Draw(Mat originalImageMat, Mat grayscaleImageMat, Mat observedImageMat, out double matchTime, out double score, double percentOfSimilarity)
        {
            Mat grayscaleObservedImageMat = new Mat();
            CvInvoke.CvtColor(observedImageMat, grayscaleObservedImageMat, ColorConversion.Bgr2Gray);

            Stopwatch watch;
            watch = Stopwatch.StartNew();

            Mat homography;
            VectorOfKeyPoint observedKeyPoints;
            using (VectorOfVectorOfDMatch matches = new VectorOfVectorOfDMatch())
            {
                Mat mask;
                FindMatch(grayscaleImageMat, grayscaleObservedImageMat, out observedKeyPoints, matches, out mask, out homography);

                double goodMatchesCount = CountGoodMatches(matches, 0.7);

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


        private void FindMatch(Mat grayscaleImageMat, Mat grayscaleObservedImageMat, out VectorOfKeyPoint observedKeyPoints, VectorOfVectorOfDMatch matches, out Mat mask, out Mat homography)
        {
            int k = 2;
            double uniquenessThreshold = 0.7;
            homography = null;


            MSER mser = new MSER();

            if (modelKeyPoints is null || modelKeyPoints is null || previousModelImage != grayscaleImageMat)
            {
                previousModelImage = grayscaleImageMat;
                modelDescriptors = new Mat();
                modelKeyPoints = new VectorOfKeyPoint();
                mser.DetectAndCompute(grayscaleImageMat, null, modelKeyPoints, modelDescriptors, false);
            }

            Mat observedDescriptors = new Mat();
            observedKeyPoints = new VectorOfKeyPoint();
            mser.DetectAndCompute(grayscaleObservedImageMat, null, observedKeyPoints, observedDescriptors, false);

            BFMatcher matcher = new BFMatcher(DistanceType.Hamming);
            //matcher.Add(modelDescriptors);
            //matcher.KnnMatch(observedDescriptors, matches, k, null);

            VectorOfDMatch matchess = new VectorOfDMatch();
            matcher.Match(modelDescriptors, observedDescriptors, matchess);

            matcher.KnnMatch(modelDescriptors, observedDescriptors, matches, k);

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

    }
}