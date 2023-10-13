using DataAccess.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using MeatCounterfeitDetector.UserInterface.Admin.Abstract;
using MeatCounterfeitDetector.Utils;
using MeatCounterfeitDetector.Utils.MessageBoxService;
using MeatCounterfeitDetector.Utils.UserService;

namespace MeatCounterfeitDetector.UserInterface.Admin.Result;

public class ResultControlVM : ViewModelBase
{
    #region Functions

    #region Constructors

    public ResultControlVM(ResultDBContext context, IMessageBoxService messageBoxService, IUserService userService)
    {
        _userService = userService;
        _messageBoxService = messageBoxService;
        _context = context;
        _context.Users.Load();
        _context.OriginalPaths.Load();
        _context.ResultPaths.Load();
        Results = new ObservableCollection<DataAccess.Models.Result>(_context.Results.ToList());
        _context.SavedChanges += (s, e) => Results = new ObservableCollection<DataAccess.Models.Result>(_context.Results.ToList());
    }

    #endregion

    #endregion


    #region Properties

    private readonly ResultDBContext _context;
    private readonly IMessageBoxService _messageBoxService;
    private readonly IUserService _userService;
    public DataAccess.Models.Result SelectedResult { get; set; }

    public ObservableCollection<DataAccess.Models.Result> Results { get; set; }

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
                try
                {
                    if (_messageBoxService.ShowMessage("Вы действительно хотите удалить запись?", "Удаление записи",
                            MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
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

                    OnPropertyChanged(nameof(Results));
                }
                catch (IOException)
                {
                    _messageBoxService.ShowMessage(
                        "Данную запись нельзя удалить в данный момент,\nтак как она используется в программе.",
                        "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                }


            });

        }
    }

    #endregion
}

