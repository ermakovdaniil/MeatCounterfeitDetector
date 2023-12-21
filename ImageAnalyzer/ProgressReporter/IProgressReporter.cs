using System;

namespace ImageWorker.ProgressReporter
{
    public interface IProgressReporter
    {
        public void Subscribe<TEvent>(Action<TEvent> action);
        public void Publish<TEvent>(TEvent eventToPublish);
    }
}