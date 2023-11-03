using MeatCounterfeitDetector.UserInterface.Admin.Abstract;
using System;

namespace MeatCountefeitDetector.UserInterface.Admin.Result
{
    public class ResultVM : ViewModelBase
    {
        public string Date { get; set; }
        public Guid ResultId { get; set; }
        public string AnRes { get; set; }
        public double Time { get; set; }
        public double PercentOfSimilarity { get; set; }
        public Guid OrigPathId { get; set; }
        public Guid ResPathId { get; set; }
    }
}