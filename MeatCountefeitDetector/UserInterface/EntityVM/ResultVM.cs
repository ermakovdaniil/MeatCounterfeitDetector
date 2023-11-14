using MeatCounterfeitDetector.UserInterface.Admin.Abstract;
using MeatCounterfeitDetector.UserInterface.Admin.User;
using MeatCounterfeitDetector.UserInterface.EntityVM;
using System;

namespace MeatCounterfeitDetector.UserInterface.EntityVM;

public class ResultVM : ViewModelBase
{
    public Guid Id { get; set; }
    public string Date { get; set; }
    public Guid UserId { get; set; }
    public string AnalysisResult { get; set; }
    public double Time { get; set; }
    public double PercentOfSimilarity { get; set; }
    public Guid OriginalImageId { get; set; }
    public Guid ResultImageId { get; set; }

    public string UserName { get; set; }
    public string OriginalImagePath { get; set; }
    public string? ResultImagePath { get; set; }
}
