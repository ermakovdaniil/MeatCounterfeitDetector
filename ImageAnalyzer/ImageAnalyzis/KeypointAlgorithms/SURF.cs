using System;
using System.Runtime.InteropServices;
using Emgu.CV;
using Emgu.CV.Features2D;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Emgu.Util;

namespace ImageWorker.ImageAnalyzis.KeypointAlgorithms
{
    public class SURF : Feature2D
    {
        public SURF(double hessianThresh, int nOctaves = 4, int nOctaveLayers = 2, bool extended = true,
           bool upright = false)
        {
            _ptr = XFeatures2DInvoke.cveSURFCreate(hessianThresh, nOctaves, nOctaveLayers, extended, upright, ref _feature2D, ref _sharedPtr);
        }

        protected override void DisposeObject()
        {
            if (_sharedPtr != IntPtr.Zero)
                XFeatures2DInvoke.cveSURFRelease(ref _sharedPtr);
            base.DisposeObject();
        }

    }

    public static partial class XFeatures2DInvoke
    {

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal extern static IntPtr cveSURFCreate(
            double hessianThresh, int nOctaves, int nOctaveLayers,
            [MarshalAs(CvInvoke.BoolMarshalType)]
            bool extended,
            [MarshalAs(CvInvoke.BoolMarshalType)]
            bool upright,
            ref IntPtr feature2D,
            ref IntPtr sharedPtr);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal extern static void cveSURFRelease(ref IntPtr sharedPtr);
    }
}