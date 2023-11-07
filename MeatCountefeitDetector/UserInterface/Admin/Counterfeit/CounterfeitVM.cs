using MeatCounterfeitDetector.UserInterface.Admin.Abstract;
using MeatCounterfeitDetector.UserInterface.Admin.Gallery;
using System;
using System.Collections.Generic;

namespace MeatCounterfeitDetector.UserInterface.Admin.Counterfeit;

public class CounterfeitVM : ViewModelBase
{
    public Guid Id { get; set; }
    public string Name { get; set; }
}