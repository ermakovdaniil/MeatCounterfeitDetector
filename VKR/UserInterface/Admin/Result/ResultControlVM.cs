using System.Collections.ObjectModel;
using System.Windows;

using DataAccess.Data;
using DataAccess.Models;

using VKR.Utils;

using MessageBox = HandyControl.Controls.MessageBox;


namespace VKR.ViewModel;

public class ResultControlVM : ViewModelBase
{
    #region Functions

    #region Constructors

    public ResultControlVM(ResultDBContext context)
    {
        _context = context;
        Results = _context.Results.Local.ToObservableCollection();
    }

    #endregion

    #endregion


    #region Properties

    private readonly ResultDBContext _context;
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
                if (MessageBox.Show("Вы действительно хотите удалить запись?",
                                    "Удаление записи", MessageBoxButton.YesNo, MessageBoxImage.Warning) ==
                    MessageBoxResult.Yes)
                {
                    _context.Results.Remove(SelectedResult);
                    _context.SaveChanges();
                }
            }, _ => SelectedResult != null);
        }
    }

    #endregion
}
