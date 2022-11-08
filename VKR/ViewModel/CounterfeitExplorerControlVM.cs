using System.Collections.ObjectModel;
using System.Windows;

using DataAccess.Data;
using DataAccess.Models;

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

            _db = new CounterfeitKBContext();
            Counterfeits = _db.Counterfeits.Local.ToObservableCollection();
        }

        #endregion

        #endregion


        #region Properties

        private readonly CounterfeitKBContext _db;
        public ObservableCollection<Counterfeit> Counterfeits { get; set; }
        public Counterfeit SelectedCounterfeit { get; set; }

        #endregion


        #region Commands

        private RelayCommand _addNewCounterfeit;

        /// <summary>
        ///     Команда, открывающая окно создания нового фальсификата
        /// </summary>
        public RelayCommand AddNewCounterfeit
        {
            get
            {
                return _addNewCounterfeit ??= new RelayCommand(o =>
                {
                    ShowChildWindow(new CounterfeitEditWindow(new Counterfeit()));
                });
            }
        }

        private RelayCommand _editMemObject;

        /// <summary>
        ///     Команда, открывающая окно редактирования нового фальсификата
        /// </summary>
        public RelayCommand EditMemObject
        {
            get
            {
                return _editMemObject ??= new RelayCommand(o =>
                {
                    ShowChildWindow(new CounterfeitEditWindow(SelectedCounterfeit));
                },
                                                           c => SelectedCounterfeit != null);
            }
        }

        private RelayCommand _deleteMemObject;

        /// <summary>
        ///     Команда, удаляющая фальсификат
        /// </summary>
        public RelayCommand DeleteMemObject
        {
            get
            {
                return _deleteMemObject ??= new RelayCommand(o =>
                {
                    if (MessageBox.Show($"Вы действительно хотите удалить фальсификат \"{SelectedCounterfeit.Name}\"?",
                                        "Удаление объекта", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                    {
                        foreach (var value in SelectedMemObject.Values)
                        {
                            _db.Values.Remove(value);
                        }

                        _db.Counterfeits.Remove(SelectedCounterfeit);
                        _db.SaveChanges();
                    }
                }, c => SelectedCounterfeit != null);
            }
        }

        #endregion
    }
}

