using MeatCounterfeitDetector.UserInterface.Admin.Abstract;
using System;

namespace MeatCounterfeitDetector.UserInterface.EntityVM;

public class CounterfeitImageVM : ViewModelBase
{
    public Guid Id { get; set; }
    public Guid CounterfeitId { get; set; }
    public string EncodedImage { get; set; }
    public string CounterfeitName { get; set; }
}