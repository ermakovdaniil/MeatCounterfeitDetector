using System.Collections.ObjectModel;

using DataAccess.Data;
using DataAccess.Models;
using VKR.Utils;

namespace VKR.ViewModel
{
    public class CounterfeitEditWindowVM : ViewModelBase
    {
        #region Functions

        #region Constructors

        public CounterfeitEditWindowVM(Counterfeit counterfeit)
        {
            Counterfeit = counterfeit;
            Values = Counterfeit.Values;
        }

        #endregion

        #endregion


        #region Properties

        public ObservableCollection<Value> Values { get; set; }
        public Counterfeit Counterfeit { get; set; }

        #endregion


        #region Commands

        private RelayCommand _saveChanges;

        /// <summary>
        ///     Команда, сохраняющая результаты редактирования в базу данных
        /// </summary>
        public RelayCommand SaveChanges
        {
            get
            {
                return _saveChanges ?? (_saveChanges = new RelayCommand(o =>
                {
                    //DbContextSingleton.GetInstance().SaveChanges();
                    Db.SaveChanges();
                    OnClosingRequest();
                }));
            }
        }

        #endregion
    }
}