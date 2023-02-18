using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Features2D;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using System;
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
            //VectorOfKeyPoint modelKeyPoints;
            VectorOfKeyPoint observedKeyPoints;
            using (VectorOfVectorOfDMatch matches = new VectorOfVectorOfDMatch())
            {
                Mat mask;
                FindMatch(modelImage, observedImage, out observedKeyPoints, matches,
                    out mask, out homography);

                //Mat scoreImg = new Mat();
                //double minVal = Double.MaxValue;
                //score = Double.MinValue;
                //var minLoc = new Point();
                //var maxLoc = new Point();
                //CvInvoke.MatchTemplate(modelImage, observedImage, scoreImg, TemplateMatchingType.CcoeffNormed);
                //CvInvoke.MinMaxLoc(scoreImg, ref minVal, ref score, ref minLoc, ref maxLoc);
                //score *= 100;
                //score = Math.Round(score, 2);

                int goodMatches = CountHowManyPairsExist(mask);
                double numberOfKeypoints;
                if (modelKeyPoints.Length <= observedKeyPoints.Length)
                {
                    numberOfKeypoints = (double)modelKeyPoints.Length;
                }
                else
                {
                    numberOfKeypoints = (double)observedKeyPoints.Length;
                }
                //score = ((double)goodMatches / (double)mask.GetData().Length) * 100.0;
                score = ((double)matches.Length / numberOfKeypoints) * 100.0;
                //score = ((double)goodMatches / (double)matches.Length) * 100.0;
                score = Math.Round(score, 2);

                //VectorOfVectorOfDMatch goodMatches = new VectorOfVectorOfDMatch();
                //for (int i = 0; i < matches.Size; i++)
                //{
                //    var arrayOfMatches = matches[i].ToArray();
                //    if (arrayOfMatches[0].Distance < 0.8 * arrayOfMatches[1].Distance)
                //    {
                //        VectorOfDMatch tempVec = new VectorOfDMatch();
                //        tempVec.Push(arrayOfMatches);
                //        goodMatches.Push(tempVec);
                //    }
                //}

                Mat result = new Mat();
                if (score > percentOfSimilarity)
                {
                    //Mat result = new Mat();

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

        public static void FindMatch(Mat modelImage, Mat observedImage, out VectorOfKeyPoint observedKeyPoints, VectorOfVectorOfDMatch matches, out Mat mask, out Mat homography)
        {
            int k = 2;
            double uniquenessThreshold = 0.8;
            homography = null;

            SIFT sift = new SIFT();

            if(modelKeyPoints is null || modelKeyPoints is null || previousModelImage != modelImage)
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
                nonZeroCount = Features2DToolbox.VoteForSizeAndOrientation(modelKeyPoints, observedKeyPoints,
                   matches, mask, 1.5, 20);
                if (nonZeroCount >= 4)
                    homography = Features2DToolbox.GetHomographyMatrixFromMatchedFeatures(modelKeyPoints,
                       observedKeyPoints, matches, mask, 2);
            }
        }

        public static int CountHowManyPairsExist(Mat mask)
        {
            var matched = mask.GetData();
            var list = matched.OfType<byte>().ToList();
            var count = list.Count(a => a.Equals(1));
            return count;
        }
    }
}