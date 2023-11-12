using MeatCounterfeitDetector.UserInterface.Admin.Abstract;
using System;

namespace MeatCounterfeitDetector.UserInterface.EntityVM;

public class CounterfeitVM : ViewModelBase, ICloneable
{
    public Guid Id { get; set; }
    public string Name { get; set; }

    public object Clone()
    {
        return MemberwiseClone();
    }
}