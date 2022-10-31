using System.Collections.ObjectModel;

using VKR.Data;
using VKR.Models;
using VKR.Utils;

namespace VKR.ViewModel
{
    public class CounterfeitEditWindowVM : ViewModelBase
    {
        #region Functions

        #region Constructors

        public CounterfeitEditWindowVM(MembraneObject material)
        {
            Material = material;
            Values = Material.Values;
        }

        #endregion

        #endregion


        #region Properties

        public ObservableCollection<Value> Values { get; set; }
        public MembraneObject Material { get; set; }

        #endregion


        #region Commands

        private RelayCommand _saveChanges;

        /// <summary>
        ///     Команда, сохраняющая резульаьы редактирования в базу данных
        /// </summary>
        public RelayCommand SaveChanges
        {
            get
            {
                return _saveChanges ?? (_saveChanges = new RelayCommand(o =>
                {
                    DbContextSingleton.GetInstance().SaveChanges();
                    OnClosingRequest();
                }));
            }
        }

        #endregion
    }
}