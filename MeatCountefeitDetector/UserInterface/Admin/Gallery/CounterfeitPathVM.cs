using MeatCounterfeitDetector.UserInterface.Admin.Abstract;
using System;
using System.ComponentModel.DataAnnotations;

namespace MeatCounterfeitDetector.UserInterface.Admin.Gallery;

public class CounterfeitPathVM : ViewModelBase
{
    public Guid Id { get; set; }
    public Guid CounterfeitId { get; set; }
    public string ImagePath { get; set; }
}