using ClientAPI.DTO.Counterfeit;
using MeatCounterfeitDetector.UserInterface.Admin.Abstract;
using MeatCounterfeitDetector.UserInterface.Admin.Counterfeit;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MeatCounterfeitDetector.UserInterface.Admin.Gallery;

public class CounterfeitPathVM : ViewModelBase
{
    public Guid Id { get; set; }
    public Guid CounterfeitId { get; set; }
    public string ImagePath { get; set; }
    public string CounterfeitName { get; set; }
}