using System;

namespace ImageWorker.ProgressReporter
{
    public class ProgressEventArgs : EventArgs
    {
        public int Progress { get; set; }
    }
}
