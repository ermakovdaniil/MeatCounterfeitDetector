using DataAccess.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using VKR.UserInterface.Admin.Abstract;
using VKR.Utils;
using MessageBox = HandyControl.Controls.MessageBox;


namespace VKR.UserInterface.Admin.Result;

public class ResultControlVM : ViewModelBase
{
    #region Functions

    #region Constructors

    public ResultControlVM(ResultDBContext context)
    {
        _context = context;
        _context.Users.Load();
        _context.OriginalPaths.Load();
        _context.ResultPaths.Load();
    }

    #endregion

    #endregion


    #region Properties

    private readonly ResultDBContext _context;
    public DataAccess.Models.Result SelectedResult { get; set; }

    public List<DataAccess.Models.Result> Results => _context.Results.ToList();

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

