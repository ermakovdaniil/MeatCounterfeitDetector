using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VKR.UserInterface.Abstract
{
    public interface IParent
    {
        public bool IsChild();
        public void ChangeContent();
    }
}
