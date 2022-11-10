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
    public interface IEditWindowVM
    {
        RelayCommand Save();
    }
}
