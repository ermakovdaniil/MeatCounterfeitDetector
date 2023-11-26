using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Util;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace ImageWorker.ImageAnalyzis.KeypointAlgorithms
{
    public class FeatureMatchingHelper
    {
        protected double CountGoodMatches(VectorOfVectorOfDMatch matches, double threshold)
        {
            double distMatches = 0;
            for (int i = 0; i < matches.Size; i++)
            {
                if (matches[i][0].Distance < threshold * matches[i][1].Distance)
                {
                    distMatches++;
                }
            }

            return distMatches;
        }

        protected void CalculateScore(Mat mask, out double score, Mat grayscaleImageMat, Mat grayscaleObservedImageMat, double distMatches)
        {
            List<double> scores = new List<double>();

            double goodMatches = CountHowManyPairsExist(mask);

            var temp = mask.GetData().Length - goodMatches;
            double fisrtScore = goodMatches / temp;

            scores.Add(fisrtScore);

            //double secondScore = goodMatches / distMatches;
            //if (goodMatches == distMatches)
            //{
            //    secondScore = goodMatches / (distMatches + 3);
            //}
            //scores.Add(secondScore);

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
            CvInvoke.MatchTemplate(grayscaleImageMat, grayscaleObservedImageMat, scoreImg, TemplateMatchingType.CcoeffNormed);
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

        protected double CountHowManyPairsExist(Mat mask)
        {
            var matched = mask.GetData();
            var list = matched.OfType<byte>().ToList();
            var count = list.Count(a => a != 0);
            return count;
        }
    }
}
