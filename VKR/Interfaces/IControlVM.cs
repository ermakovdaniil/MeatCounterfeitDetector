using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Windows;

using DataAccess.Data;
using DataAccess.Models;

using VKR.Utils;
using VKR.View;

namespace VKR.Interfaces
{
    public interface IControlVM
    {
        // TODO: конструктор?
        RelayCommand Add();
        RelayCommand Edit();
        RelayCommand Delete();
    }
}
