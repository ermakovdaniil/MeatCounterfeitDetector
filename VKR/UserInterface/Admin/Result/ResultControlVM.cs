using DataAccess.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using VKR.UserInterface.Admin.Abstract;
using VKR.Utils;
using VKR.Utils.MessageBoxService;


namespace VKR.UserInterface.Admin.Result;

public class ResultControlVM : ViewModelBase
{
    #region Functions

    #region Constructors

    public ResultControlVM(ResultDBContext context, IMessageBoxService messageBoxService)
    {
        _messageBoxService = messageBoxService;
        _context = context;
        _context.Users.Load();
        _context.OriginalPaths.Load();
        _context.ResultPaths.Load();
    }

    #endregion

    #endregion


    #region Properties

    private readonly ResultDBContext _context;
    private readonly IMessageBoxService _messageBoxService;
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
                if (_messageBoxService.ShowMessage("Вы действительно хотите удалить запись?", "Удаление записи", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    string pathToBase = Directory.GetCurrentDirectory();
                    string combinedPath = Path.Combine(pathToBase, SelectedResult.OrigPath.Path);
                    File.Delete(combinedPath);
                    if (SelectedResult.ResPath.Path is not null)
                    {
                        combinedPath = Path.Combine(pathToBase, SelectedResult.ResPath.Path);
                        File.Delete(combinedPath);
                    }
                    _context.Results.Remove(SelectedResult);
                    _context.SaveChanges();
                }
            }, _ => SelectedResult is not null);
        }
    }

    #endregion
}

