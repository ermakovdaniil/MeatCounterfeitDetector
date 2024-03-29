﻿using System;

namespace MeatCounterfeitDetector.Utils.EventAggregator
{
    public interface IEventAggregator
    {
        public void Subscribe<TEvent>(Action<TEvent> action);
        public void Publish<TEvent>(TEvent eventToPublish);
    }
}
