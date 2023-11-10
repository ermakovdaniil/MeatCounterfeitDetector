using MeatCounterfeitDetector.UserInterface.Admin.Abstract;
using System;

namespace MeatCounterfeitDetector.UserInterface.EntityVM;

public class UserVM : ViewModelBase
{
    public Guid Id { get; set; }
    public string Login { get; set; }
    public string Password { get; set; }
    public string Name { get; set; }
    //public Guid TypeId { get; set; }
    public string UserTypeName { get; set; }
}