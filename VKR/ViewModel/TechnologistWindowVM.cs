using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VKR.Utils;

namespace VKR.ViewModel
{
    public class TechnologistWindowVM : ViewModelBase
    {
        #region Functions
        public TechnologistWindowVM() { }

        #endregion

        #region Properties
        private string _displayedImagePath;
        public string DisplayedImagePath 
        { 
            get 
            { 
                return _displayedImagePath; 
            } 
            set 
            { 
                _displayedImagePath = value; 
                OnPropertyChanged();
            } 
        }
        #endregion

        #region Commands
        private RelayCommand _changePathImage;

        public RelayCommand ChangePathImageCommand
        { 
            get 
            {
                return _changePathImage ??= new RelayCommand(_ =>
                {
                    OpenFileDialog open = new OpenFileDialog();
                    open.DefaultExt = (".png");
                    open.Filter = "Pictures (*.jpg;*.gif;*.png)|*.jpg;*.gif;*.png";

                    if (open.ShowDialog() == true)
                        DisplayedImagePath = open.FileName;
                });
            } 
        }
        #endregion
    }
}
