using System.Collections.ObjectModel;
using System.Windows;

using DataAccess.Data;
using DataAccess.Models;

using VKR.Utils;
using VKR.View;

using MessageBox = HandyControl.Controls.MessageBox;


namespace VKR.ViewModel
{
    internal class ResultControlVM : ViewModelBase
    {
        #region Functions

        #region Constructors

        public ResultControlVM()
        {
            _db = new ResultDBContext();
            Results = _db.Results.Local.ToObservableCollection();
        }

        #endregion

        #endregion


        #region Properties

        private readonly ResultDBContext _db;
        public Result SelectedResult { get; set; }
        public ObservableCollection<Result> Results { get; set; }

        #endregion


        #region Commands

        private RelayCommand _deleteResult;

        /// <summary>
        ///     Команда, удаляющая запись с результатами
        /// </summary>
        public RelayCommand DeleteResult
        {
            get
            {
                return _deleteResult ??= new RelayCommand(o =>
                {
                    if (MessageBox.Show($"Вы действительно хотите удалить запись?",
                                        "Удаление записи", MessageBoxButton.YesNo, MessageBoxImage.Warning) ==
                        MessageBoxResult.Yes)
                    {
                        _db.Results.Remove(SelectedResult);
                        _db.SaveChanges();
                    }
                }, _ => SelectedResult != null);
            }
        }

        #endregion
    }
}
