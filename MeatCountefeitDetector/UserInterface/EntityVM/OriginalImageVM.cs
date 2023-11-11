using MeatCounterfeitDetector.UserInterface.Admin.Abstract;
using System;

namespace MeatCounterfeitDetector.UserInterface.EntityVM;

public class OriginalImageVM : ViewModelBase
{
    public Guid Id { get; set; }
    public string EncodedImage { get; set; }
}