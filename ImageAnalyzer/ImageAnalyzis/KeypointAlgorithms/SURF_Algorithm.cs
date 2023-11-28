using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Features2D;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Emgu.CV.XFeatures2D;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;


namespace ImageWorker.ImageAnalyzis.KeypointAlgorithms
{
    public class SURF_Algorithm : FeatureMatchingHelper, IImageMatchingAlgorithm
    {
        private Mat previousModelImage;
        private Mat modelDescriptors;
        private VectorOfKeyPoint modelKeyPoints;

        public VectorOfKeyPoint FindMatch(Mat grayscaleImageMat, Mat grayscaleObservedImageMat, out VectorOfKeyPoint observedKeyPoints, VectorOfVectorOfDMatch matches, out Mat mask, out Mat homography, double uniquenessThreshold)
        {
            int k = 2;
            homography = null;

            SURF sift = new SURF(500);

            if (modelDescriptors is null || modelKeyPoints is null || previousModelImage != grayscaleImageMat)
            {
                previousModelImage = grayscaleImageMat;
                modelDescriptors = new Mat();
                modelKeyPoints = new VectorOfKeyPoint();
                sift.DetectAndCompute(grayscaleImageMat, null, modelKeyPoints, modelDescriptors, false);
            }

            Mat observedDescriptors = new Mat();
            observedKeyPoints = new VectorOfKeyPoint();
            sift.DetectAndCompute(grayscaleObservedImageMat, null, observedKeyPoints, observedDescriptors, false);

            BFMatcher matcher = new BFMatcher(DistanceType.L2);
            matcher.KnnMatch(modelDescriptors, observedDescriptors, matches, k, null);

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

            return modelKeyPoints;
        }

    }
}