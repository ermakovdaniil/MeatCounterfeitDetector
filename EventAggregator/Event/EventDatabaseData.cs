using System;

namespace MeatCounterfeitDetector.Utils.EventAggregator
{
    public class EventDatabaseData
    {
        public Guid? Id { get; }

        public EventDatabaseData(Guid id)
        {
            Id = id;
        }
    }
}
