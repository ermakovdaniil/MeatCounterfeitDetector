using MeatCounterfeitDetector.UserInterface.Admin.Abstract;
using System;

namespace MeatCountefeitDetector.UserInterface.EntityVM;

public class OriginalImageVM : ViewModelBase
{
    public Guid Id { get; set; }
    public string EncodedImage { get; set; }
}