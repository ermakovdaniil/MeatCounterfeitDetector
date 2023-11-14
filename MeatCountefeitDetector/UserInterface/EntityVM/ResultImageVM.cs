using MeatCounterfeitDetector.UserInterface.Admin.Abstract;
using System;

namespace MeatCounterfeitDetector.UserInterface.EntityVM;

public class ResultImageVM : ViewModelBase
{
    public Guid Id { get; set; }
    public Guid OriginalId { get; set; }
    public string ImagePath { get; set; }
    //public string OriginalImagePath { get; set; }
}

