using System;
using System.Windows.Media.Imaging;
using static iText.StyledXmlParser.Jsoup.Select.Evaluator;

namespace MeatCountefeitDetector.Utils.EventAggregator
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
