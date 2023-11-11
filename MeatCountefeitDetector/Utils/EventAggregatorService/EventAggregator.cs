using System;
using System.Collections.Generic;

namespace MeatCounterfeitDetector.Utils.EventAggregator
{
    public class EventAggregator : IEventAggregator
    {
        private Dictionary<Type, List<Action<object>>> _subscribers = new Dictionary<Type, List<Action<object>>>();

        public void Subscribe<TEvent>(Action<TEvent> action)
        {
            var eventType = typeof(TEvent);
            if (!_subscribers.ContainsKey(eventType))
            {
                _subscribers[eventType] = new List<Action<object>>();
            }
            _subscribers[eventType].Add(obj => action((TEvent)obj));
        }

        public void Publish<TEvent>(TEvent eventToPublish)
        {
            var eventType = typeof(TEvent);
            if (_subscribers.ContainsKey(eventType))
            {
                foreach (var subscriber in _subscribers[eventType])
                {
                    subscriber(eventToPublish);
                }
            }
        }
    }

}
