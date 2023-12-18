using System;
using System.Diagnostics;

namespace ImageWorker.ProgressReporter
{
    public delegate void ProgressEventHandler(object sender, ProgressEventArgs e);

    public class ProgressReporter : IProgressReporter
    {
        private int _progress;

        public event ProgressEventHandler ProgressUpdated;

        public void ReportProgress(int newProgress)
        {
            ProgressUpdated?.Invoke(this, new ProgressEventArgs { Progress = newProgress });
        }

        public int GetProgress()
        {
            return _progress;
        }
    }
}