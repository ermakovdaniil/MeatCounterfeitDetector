using System;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Features2D;
using Emgu.CV.Structure;
using Emgu.CV.Util;


namespace VKR.Utils.ImageAnalyzis
{
    public static class SIFTAlgorithm
    {
        public static void FindMatch(Mat modelImage, Mat observedImage, out VectorOfKeyPoint modelKeyPoints, out VectorOfKeyPoint observedKeyPoints, VectorOfVectorOfDMatch matches, out Mat mask, out Mat homography)
        {
            int k = 2;
            double uniquenessThreshold = 0.8;


            homography = null;

            modelKeyPoints = new VectorOfKeyPoint();
            observedKeyPoints = new VectorOfKeyPoint();

            //using (UMat uModelImage = modelImage.ToUMat(AccessType.Read))
            //using (UMat uObservedImage = observedImage.ToUMat(AccessType.Read))
            {
                SIFT sift = new SIFT();
                //UMat modelDescriptors = new UMat();
                Mat modelDescriptors = new Mat();
                modelKeyPoints = new VectorOfKeyPoint();
                modelDescriptors = new Mat();
                // БЫЛО uModelImage
                sift.DetectAndCompute(modelImage, null, modelKeyPoints, modelDescriptors, false);
                //UMat observedDescriptors = new UMat();
                Mat observedDescriptors = new Mat();
                // БЫЛО uObservedImage
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
        }

        public static Mat Draw(Mat modelImage, Mat observedImage, out double matchTime, out double score)
        {
            Stopwatch watch;
            watch = Stopwatch.StartNew();

            Mat scoreImg = new Mat();
            double minVal = Double.MaxValue;
            score = Double.MinValue;
            var minLoc = new Point();
            var maxLoc = new Point();
            CvInvoke.MatchTemplate(modelImage, observedImage, scoreImg, TemplateMatchingType.CcoeffNormed);
            CvInvoke.MinMaxLoc(scoreImg, ref minVal, ref score, ref minLoc, ref maxLoc);
            score *= 100;
            score = Math.Round(score, 2);

            Mat homography;
            VectorOfKeyPoint modelKeyPoints;
            VectorOfKeyPoint observedKeyPoints;
            using (VectorOfVectorOfDMatch matches = new VectorOfVectorOfDMatch())
            {
                Mat mask;
                FindMatch(modelImage, observedImage, out modelKeyPoints, out observedKeyPoints, matches,
                    out mask, out homography);

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
                Features2DToolbox.DrawMatches(modelImage, modelKeyPoints, observedImage, observedKeyPoints,
                    matches, result, new MCvScalar(0, 0, 255), new MCvScalar(255, 255, 255), mask, Features2DToolbox.KeypointDrawType.NotDrawSinglePoints);
                int goodMatches = CountHowManyPairsExist(mask);

                //if (modelKeyPoints.Length <= observedKeyPoints.Length)
                //{
                //    numberOfKeypoints = (double)modelKeyPoints.Length;
                //}
                //else
                //{
                //    numberOfKeypoints = (double)observedKeyPoints.Length;
                //}
                //score = ((double)goodMatches / (double)matches.Length) * 100.0;
                //score = Math.Round(score, 2);

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
                        CvInvoke.Polylines(result, vp, true, new MCvScalar(255, 0, 0, 255), 0);
                    }
                }

                watch.Stop();
                matchTime = watch.ElapsedMilliseconds;
                return result;
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