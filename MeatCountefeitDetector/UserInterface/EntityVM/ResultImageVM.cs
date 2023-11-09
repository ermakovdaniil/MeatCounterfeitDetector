using MeatCounterfeitDetector.UserInterface.Admin.Abstract;
using System;

namespace MeatCountefeitDetector.UserInterface.EntityVM;

public class ResultImageVM : ViewModelBase
{
    public Guid Id { get; set; }
    //public Guid OriginalId { get; set; }
    public string EncodedImage { get; set; }
    public OriginalImageVM OriginalImage { get; set; }
}

