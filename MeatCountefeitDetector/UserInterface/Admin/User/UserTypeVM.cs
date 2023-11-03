using MeatCounterfeitDetector.UserInterface.Admin.Abstract;
using System;

namespace MeatCounterfeitDetector.UserInterface.Admin.UserType;

public class UserTypeVM : ViewModelBase
{
    public Guid Id { get; set; }
    public string Name { get; set; }
}