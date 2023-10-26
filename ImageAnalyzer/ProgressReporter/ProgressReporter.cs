namespace ImageWorker.ProgressReporter
{
    public class ProgressReporter : IProgressReporter
    {
        public int Progress;

        public void ReportProgress(int progress)
        {
            Progress = progress;
        }
    }
}
