using MeatCounterfeitDetector.UserInterface.Admin.Abstract;
using MeatCounterfeitDetector.UserInterface.Admin.User;
using MeatCounterfeitDetector.UserInterface.EntityVM;
using System;

namespace MeatCountefeitDetector.UserInterface.EntityVM;

public class ResultVM : ViewModelBase
{
    public Guid Id { get; set; }
    public string Date { get; set; }
    //public Guid UserId { get; set; }
    public string AnRes { get; set; }
    public double Time { get; set; }
    public double PercentOfSimilarity { get; set; }
    //public Guid OrigPathId { get; set; }
    //public Guid ResPathId { get; set; }

    public UserVM User { get; set; }
    public OriginalPathVM OriginalPath { get; set; }
    public ResultPathVM ResultPath { get; set; }
}
