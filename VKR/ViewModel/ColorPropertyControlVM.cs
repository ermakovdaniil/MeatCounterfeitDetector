using System.Collections.ObjectModel;
using System.Windows;

using DataAccess.Data;
using DataAccess.Models;
using VKR.Utils;
using VKR.View;

using MessageBox = HandyControl.Controls.MessageBox;


namespace VKR.ViewModel
{
    internal class ColorPropertyControlVM : ViewModelBase
    {
        #region Functions

        #region Constructors

        public ColorPropertyControlVM()
        {
            _db = new CounterfeitKBContext();
            Colors = _db.Colors.Local.ToObservableCollection();
        }

        #endregion

        #endregion


        #region Properties

        private readonly CounterfeitKBContext _db;
        public Color SelectedColor { get; set; }
        public ObservableCollection<Color> Colors { get; set; }

        #endregion


        #region Commands

        private RelayCommand _addColor;

        /// <summary>
        ///     Команда, открывающая окно создания цвета
        /// </summary>
        public RelayCommand AddColor
        {
            get
            {
                return _addColor ??= new RelayCommand(o =>
                {
                    ShowChildWindow(new ColorPropertyEditWindow(new Color()));
                });
            }
        }

        private RelayCommand _editColor;

        /// <summary>
        ///     Команда, открывающая окно редактирования цвета
        /// </summary>
        public RelayCommand EditColor
        {
            get
            {
                return _editColor ??= new RelayCommand(o =>
                {
                    ShowChildWindow(new ColorPropertyEditWindow(SelectedColor));
                }, _ => SelectedColor != null);
            }
        }

        private RelayCommand _deleteColor;

        /// <summary>
        ///     Команда, удаляющая цвет
        /// </summary>
        public RelayCommand DeleteColor
        {
            get
            {
                return _deleteColor ??= new RelayCommand(o =>
                {
                    if (MessageBox.Show($"Вы действительно хотите удалить цвет: \"{SelectedColor.Name}\"?",
                                        "Удаление цвета", MessageBoxButton.YesNo, MessageBoxImage.Warning) ==
                        MessageBoxResult.Yes)
                    {
                        _db.Colors.Remove(SelectedColor);
                        _db.SaveChanges();
                    }
                }, _ => SelectedColor != null);
            }
        }

        #endregion
    }
}
