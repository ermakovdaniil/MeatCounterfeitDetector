using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using VKR.Utils;

namespace VKR.ViewModel
{
    public class TechnologistWindowVM : ViewModelBase
    {
        #region Functions
        public TechnologistWindowVM() { }

        #endregion

        #region Properties
        private string _originalImagePath;
        public string OriginalImagePath 
        { 
            get 
            { 
                return _originalImagePath; 
            } 
            set 
            {
                _originalImagePath = value; 
                OnPropertyChanged();
            } 
        }

        private string _resultImagePath;
        public string ResultImagePath
        {
            get
            {
                return _resultImagePath;
            }
            set
            {
                _resultImagePath = value;
                OnPropertyChanged();
            }
        }

        private string _resultMessage;
        public string ResultMessage
        {
            get 
            { 
                return _resultMessage; 
            }
            set 
            { 
                _resultMessage = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Commands
        private RelayCommand _changeImagePath;

        public RelayCommand ChangeImagePathCommand
        { 
            get 
            {
                return _changeImagePath ??= new RelayCommand(_ =>
                {
                    OpenFileDialog open = new OpenFileDialog();
                    open.DefaultExt = (".png");
                    open.Filter = "Pictures (*.jpg;*.jpeg;*.png)|*.jpg;*.jpeg;*.png";

                    if (open.ShowDialog() == true)
                        OriginalImagePath = open.FileName;
                });
            } 
        }

        private RelayCommand _showInfo;

        public RelayCommand ShowInfo
        {
            get
            {
                return _showInfo ??= new RelayCommand(_ =>
                {
                    HandyControl.Controls.MessageBox.Show(
                                    "Данный программный комплекс позволяет обработать входное изображение среза мясной продукции" +
                                    "для обнаружения фальсификата, по ряду его характеристик, такие как форма, цвет, размер.\n" +
                                    "В программе реализована функция формирования отчёта в файл формата PDF.\n" +
                                    "Автор:  Ермаков Даниил Игоревич\n" +
                                    "Группа: 494\n" +
                                    "Учебное заведение: СПбГТИ (ТУ)", "Справка о программе",
                                    MessageBoxButton.OK, MessageBoxImage.Information);
                });
            }
        }
        #endregion
    }
}
