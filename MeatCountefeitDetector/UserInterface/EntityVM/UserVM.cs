using MeatCounterfeitDetector.UserInterface.Admin.Abstract;
using System;
using System.Collections.Generic;

namespace MeatCounterfeitDetector.UserInterface.EntityVM;

public class UserVM : ViewModelBase, ICloneable
{
    public Guid Id { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
    public string Name { get; set; }
    public Guid RoleId { get; set; }
    public List<string> Roles { get; set; }

    public object Clone()
    {
        return MemberwiseClone();
    }
}