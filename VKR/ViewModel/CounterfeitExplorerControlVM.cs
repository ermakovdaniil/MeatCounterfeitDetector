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
                OnPropertyChanged(nameof(Counterfeits));
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

        private RelayCommand _addCounterfeit;

        /// <summary>
        ///     Команда, открывающая окно создания нового фальсификата
        /// </summary>
        public RelayCommand AddCounterfeit
        {
            get
            {
                return _addCounterfeit ??= new RelayCommand(o =>
                {
                    ShowChildWindow(new CounterfeitEditWindow(new Counterfeit()));
                });
            }
        }

        private RelayCommand _editCounterfeitObject;

        /// <summary>
        ///     Команда, открывающая окно редактирования нового фальсификата
        /// </summary>
        public RelayCommand EditCounterfeit
        {
            get
            {
                return _editCounterfeitObject ??= new RelayCommand(o =>
                {
                    ShowChildWindow(new CounterfeitEditWindow(SelectedCounterfeit));
                },
                                                           c => SelectedCounterfeit != null);
            }
        }

        private RelayCommand _deleteCounterfeit;

        /// <summary>
        ///     Команда, удаляющая фальсификат
        /// </summary>
        public RelayCommand DeleteCounterfeit
        {
            get
            {
                return _deleteCounterfeit ??= new RelayCommand(o =>
                {
                    if (MessageBox.Show($"Вы действительно хотите удалить фальсификат: \"{SelectedCounterfeit.Name}\"?",
                                        "Удаление объекта", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                    {
                        _db.Counterfeits.Remove(SelectedCounterfeit);
                        _db.SaveChanges();
                    }
                }, c => SelectedCounterfeit != null);
            }
        }

        #endregion
    }
}

