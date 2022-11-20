using System.Linq;
using System.Windows;
using System.Collections.Generic;

using DataAccess.Data;
using DataAccess.Models;

using Microsoft.EntityFrameworkCore;

using VKR.Utils;
using VKR.Utils.Dialog;
using VKR.View;

using MessageBox = HandyControl.Controls.MessageBox;


namespace VKR.ViewModel;

public class ResultControlVM : ViewModelBase
{
    #region Functions

    #region Constructors

    public ResultControlVM(ResultDBContext context)
    {
        _context = context;
        _context.Companies.Load();
        _context.OriginalPaths.Load();
        _context.ResultPaths.Load();
    }

    #endregion

    #endregion


    #region Properties

    private readonly ResultDBContext _context;
    public Result SelectedResult { get; set; }
    public List<Result> Results
    {
        get => _context.Results.ToList();
    }

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
                OnPropertyChanged(nameof(Results));
            }, _ => SelectedResult != null);
        }
    }

    #endregion
}
