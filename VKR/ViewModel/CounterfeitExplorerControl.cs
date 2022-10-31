using System.Collections.ObjectModel;
using System.Windows;

using VKR.Data;
using VKR.Models;

using VKR.Utils;
using VKR.View;

using MessageBox = HandyControl.Controls.MessageBox;


namespace VKR.ViewModel
{
    public class CounterfeitExplorerControlVM : ViewModelBase
    {
        #region Functions

        #region Constructors

        public CounterfeitExplorerControlVM()
        {
            _db.SavedChanges += (sender, args) =>
            {
                OnPropertyChanged(nameof(Materials));
            };


            //var db = DbContextSingleton.GetInstance();
            Materials = _db.MembraneObjects.Local.ToObservableCollection();
        }

        #endregion

        #endregion


        #region Properties

        private readonly MembraneContext _db = DbContextSingleton.GetInstance();
        public ObservableCollection<MembraneObject> Materials { get; set; }

        public MembraneObject SelectedMemObject { get; set; }

        #endregion


        #region Commands

        private RelayCommand _addNewMemObject;

        /// <summary>
        ///     Команда, открывающая окно создания нового объекта
        /// </summary>
        public RelayCommand AddNewMemObject
        {
            get
            {
                return _addNewMemObject ??= new RelayCommand(o =>
                {
                    ShowChildWindow(new CreateMaterialWindow()); // TODO: ТУТ ТОЖЕ CounterfeitEditWindow
                });
            }
        }

        private RelayCommand _editMemObject;

        /// <summary>
        ///     Команда, открывающая окно редактирования нового объекта
        /// </summary>
        public RelayCommand EditMemObject
        {
            get
            {
                return _editMemObject ??= new RelayCommand(o =>
                {
                    ShowChildWindow(new CounterfeitEditWindow(SelectedMemObject));
                },
                                                           c => SelectedMemObject != null);
            }
        }

        private RelayCommand _deleteMemObject;

        /// <summary>
        ///     Команда, удаляющая объект
        /// </summary>
        public RelayCommand DeleteMemObject
        {
            get
            {
                return _deleteMemObject ??= new RelayCommand(o =>
                {
                    if (MessageBox.Show($"Вы действительно хотите удалить объект {SelectedMemObject.ObName}?",
                                        "Удаление объекта", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                    {
                        foreach (var value in SelectedMemObject.Values)
                        {
                            _db.Values.Remove(value);
                        }

                        _db.MembraneObjects.Remove(SelectedMemObject);
                        _db.SaveChanges();
                    }
                }, c => SelectedMemObject != null);
            }
        }

        #endregion
    }
}

