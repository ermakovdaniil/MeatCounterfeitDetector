using MeatCounterfeitDetector.UserInterface.Admin.Abstract;
using System;
using static Emgu.CV.Dai.OpenVino;
using System.Windows.Controls;

namespace MeatCounterfeitDetector.UserInterface.EntityVM;

public class CounterfeitImageVM : ViewModelBase, ICloneable
{
    public Guid Id { get; set; }
    public Guid CounterfeitId { get; set; }
    public string EncodedImage { get; set; }
    public string CounterfeitName { get; set; }

    public object Clone()
    {
        return MemberwiseClone();
    }
}