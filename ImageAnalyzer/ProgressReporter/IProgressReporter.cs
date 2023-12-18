using System;

namespace ImageWorker.ProgressReporter
{
    public interface IProgressReporter
    {
        event ProgressEventHandler ProgressUpdated;
        void ReportProgress(int newProgress);
        int GetProgress();
    }
}