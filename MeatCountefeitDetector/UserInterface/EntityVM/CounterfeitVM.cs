﻿using MeatCounterfeitDetector.UserInterface.Admin.Abstract;
using System;

namespace MeatCounterfeitDetector.UserInterface.EntityVM;

public class CounterfeitVM : ViewModelBase
{
    public Guid Id { get; set; }
    public string Name { get; set; }
}