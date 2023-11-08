using MeatCounterfeitDetector.UserInterface.Admin.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeatCountefeitDetector.UserInterface.Admin.Result
{
    public class OriginalPathVM : ViewModelBase
    {
        public Guid Id { get; set; }
        public string Path { get; set; }
    }
}
