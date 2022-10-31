using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

using VKR.Data;
using VKR.Models;
using VKR.Utils;
using VKR.View;

using MessageBox = HandyControl.Controls.MessageBox;


namespace VKR.ViewModel
{
    internal class PropertiesControlVM : ViewModelBase

    {
        #region Functions

        #region Constructors

        public PropertiesControlVM()
        {
            MembraneObjectTypes = _db.ObjectTypes.Local.ToObservableCollection();
        }

        #endregion

        #endregion


        #region Properties

        private static readonly MembraneContext _db = DbContextSingleton.GetInstance();
        public ObjectType SelectedType { get; set; }

        public ObservableCollection<ObjectType> MembraneObjectTypes { get; set; }

        #endregion


        #region Commands

        private RelayCommand _createProperty;

        /// <summary>
        ///     Команда, открывающая окно создания свойства
        /// </summary>
        public RelayCommand CreateProperty
        {
            get
            {
                return _createProperty ??= new RelayCommand(o =>
                {
                    ShowChildWindow(new CreatePropertyWindow(new Property()));
                });
            }
        }

        private RelayCommand _editProperty;

        /// <summary>
        ///     Команда, открывающая окно редактирования свойства
        /// </summary>
        public RelayCommand EditProperty
        {
            get
            {
                return _editProperty ??= new RelayCommand(o =>
                {
                    ShowChildWindow(new CreatePropertyWindow(SelectedProperty));
                }, o => SelectedProperty != null);
            }
        }

        private RelayCommand _deleteProperty;

        /// <summary>
        ///     Команда, удаляющая свойство
        /// </summary>
        public RelayCommand DeleteProperty
        {
            get
            {
                return _deleteProperty ??= new RelayCommand(o =>
                {
                    //if (MessageBox.Show($"Вы действительно хотите удалить пользователя {SelectedUser.UserName}?", "Удаление пользователя", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                    //{
                    //    db.Properties.Remove(SelectedProperty);
                    //    db.SaveChanges();
                    //}
                });
            }
        }

        #endregion
    }
}