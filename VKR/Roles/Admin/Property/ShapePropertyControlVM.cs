using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Data.Entity;

using DataAccess.Data;
using DataAccess.Models;

using VKR.Utils;
using VKR.View;

using MessageBox = HandyControl.Controls.MessageBox;


namespace VKR.ViewModel
{
    internal class ShapePropertyControlVM : ViewModelBase
    {
        #region Functions

        #region Constructors

        public ShapePropertyControlVM()
        {
            _db = new CounterfeitKBContext();
            Shapes = _db.Shapes.Local.ToObservableCollection();
        }

        #endregion

        #endregion


        #region Properties

        private readonly CounterfeitKBContext _db;
        public Shape SelectedShape { get; set; }
        public ObservableCollection<Shape> Shapes { get; set; }

        #endregion


        #region Commands

        private RelayCommand _addShape;

        /// <summary>
        ///     Команда, открывающая окно создания формы
        /// </summary>
        public RelayCommand AddShape
        {
            get
            {
                return _addShape ??= new RelayCommand(o =>
                {
                    //ShowChildWindow(new ShapePropertyEditWindow(new Shape()));
                });
            }
        }

        private RelayCommand _editShape;

        /// <summary>
        ///     Команда, открывающая окно редактирования формы
        /// </summary>
        public RelayCommand EditShape
        {
            get
            {
                return _editShape ??= new RelayCommand(o =>
                {
                    //ShowChildWindow(new ShapePropertyEditWindow(SelectedShape));
                }, _ => SelectedShape != null);
            }
        }

        private RelayCommand _deleteShape;

        /// <summary>
        ///     Команда, удаляющая форму
        /// </summary>
        public RelayCommand DeleteShape
        {
            get
            {
                return _deleteShape ??= new RelayCommand(o =>
                {
                    if (MessageBox.Show($"Вы действительно хотите удалить форму: \"{SelectedShape.Name}\" и все фальсификаты связанные с ней?" +
                                        $"\nСвязанные фальсфикаты:\n{string.Join("\n", _db.Counterfeits.Where(c => c.ShapeId == SelectedShape.Id).Include(c => c.Shape).Select(c => c.Name))}",
                                        "Удаление формы", MessageBoxButton.YesNo, MessageBoxImage.Warning) ==
                        MessageBoxResult.Yes)
                    {
                        _db.Shapes.Remove(SelectedShape);
                        _db.SaveChanges();
                    }
                }, _ => SelectedShape != null);
            }
        }

        #endregion
    }
}