using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace VKR.ViewModel
{
    public class MainWindowVM : ViewModelBase
    {
        
        internal void SetNewContent(UserControl content)
        {
            Debug.WriteLine($"#######################################################################33");
            Debug.WriteLine(ContentWindow);
            ContentWindow = content;
            Debug.WriteLine(ContentWindow);
            
        }
        
        
        private UserControl _content;
        public UserControl ContentWindow
        {
            get { return _content; }
            set
            {
                _content = value;
                OnPropertyChanged();
            }
        }
    }
}
