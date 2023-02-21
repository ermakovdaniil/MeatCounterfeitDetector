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
            //double numberOfKeypoints;
            //if (modelKeyPoints.Length <= observedKeyPoints.Length)
            //{
            //    numberOfKeypoints = (double)modelKeyPoints.Length;
            //}
            //else
            //{
            //    numberOfKeypoints = (double)observedKeyPoints.Length;
            //}
            //VectorOfVectorOfDMatch goodMatches = new VectorOfVectorOfDMatch();
            //List<MKeyPoint> modelMatched = new List<MKeyPoint>();
            //List<MKeyPoint> observedMatched = new List<MKeyPoint>();
            //List<MKeyPoint> modelInliners = new List<MKeyPoint>();
            //List<MKeyPoint> observedInliners = new List<MKeyPoint>();
            //double inlinerThreshold = 2.5;
            //for (int i = 0; i < modelMatched.Count; i++)
            //{
                //col = np.ones((3, 1), dtype = np.float64)
                //double[,,] col = new double[1, 1, 1];

                //col[0:2, 0] = m.pt
                //col[0, 0, 0] = modelMatched[i].Point.X;
                //col[0, 0, 1] = modelMatched[i].Point.Y;
                //*
                //two_d[0:2, 1:] → Select elements from row 0 to row2(excluded) and column 1 till the last column.
                //0:2 → Slice for x - axis → select row 0 till row 2(excluded)
                //1: → Slice for y - axis → select column 1 till the last column

                //# Project from image 1 to image 2
                //col = np.dot(homography, col)
                //col = np.dot();

                //col /= col[2, 0]

                //# Calculate euclidean distance
                //dist = sqrt(pow(col[0, 0] - matched2[i].pt[0], 2) + pow(col[1, 0] - matched2[i].pt[1], 2))

                //if dist < inlier_threshold: 
                //if(distMatches < inlinerThreshold)
                //{
                    //good_matches.append(cv.DMatch(len(inliers1), len(inliers2), 0))

                    //inliers1.append(matched1[i])

                    //inliers2.append(matched2[i])
                //}

            //    var arrayOfMatches = matches[i].ToArray();
            //    if (arrayOfMatches[0].Distance < 0.8 * arrayOfMatches[1].Distance)
            //    {
            //        modelMatched.Add(modelKeyPoints[arrayOfMatches[0].QueryIdx]);
            //        observedMatched.Add(observedKeyPoints[arrayOfMatches[1].TrainIdx]);
            //    }
            //}
            
            double distMatches = 0;
            for (int i = 0; i < matches.Size; i++)
            {
                var arrayOfMatches = matches[i].ToArray();
                if (arrayOfMatches[0].Distance < 0.8 * arrayOfMatches[1].Distance)
                {
                    //VectorOfDMatch tempVec = new VectorOfDMatch();
                    //tempVec.Push(arrayOfMatches);
                    //goodMatches.Push(tempVec);

                    //modelMatched.Add(modelKeyPoints[arrayOfMatches[0].QueryIdx]);
                    //observedMatched.Add(observedKeyPoints[arrayOfMatches[1].TrainIdx]);

                    distMatches++;
                }
            }

            double goodMatches = CountHowManyPairsExist(mask);

            //var temp = (double)mask.GetData().Length - goodMatches;
            //score = goodMatches / (temp / 3.0)  * 100.0;

            double fisrtScore = goodMatches / (goodMatches + 15);

            //score = ((double)matches.Length / numberOfKeypoints) * 100.0;

            //score = (goodMatches / (double)matches.Length) * 100.0;


            Mat scoreImg = new Mat();
            double minVal = Double.MaxValue;
            double secondScore = Double.MinValue;
            var minLoc = new Point();
            var maxLoc = new Point();
            CvInvoke.MatchTemplate(modelImage, observedImage, scoreImg, TemplateMatchingType.CcoeffNormed);
            CvInvoke.MinMaxLoc(scoreImg, ref minVal, ref secondScore, ref minLoc, ref maxLoc);


            if(fisrtScore > secondScore)
            {
                score = fisrtScore;
            }
            else
            {
                score = secondScore;
            }

            score *= 100;
            score = Math.Round(score, 2);
        }

        private static void FindMatch(Mat modelImage, Mat observedImage, out VectorOfKeyPoint observedKeyPoints, VectorOfVectorOfDMatch matches, out Mat mask, out Mat homography)
        {
            int k = 2;
            double uniquenessThreshold = 0.8;
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
                nonZeroCount = Features2DToolbox.VoteForSizeAndOrientation(modelKeyPoints, observedKeyPoints,
                   matches, mask, 1.5, 20);
                if (nonZeroCount >= 4)
                    homography = Features2DToolbox.GetHomographyMatrixFromMatchedFeatures(modelKeyPoints,
                       observedKeyPoints, matches, mask, 2);
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