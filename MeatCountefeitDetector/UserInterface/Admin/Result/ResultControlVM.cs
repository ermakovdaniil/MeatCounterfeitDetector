using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using MeatCounterfeitDetector.UserInterface.Admin.Abstract;
using MeatCounterfeitDetector.Utils;
using MeatCounterfeitDetector.Utils.MessageBoxService;
using MeatCounterfeitDetector.Utils.UserService;
using ClientAPI;
using MeatCounterfeitDetector.Utils.Dialog;
using MeatCountefeitDetector.UserInterface.Admin.Result;
using Mapster;

namespace MeatCounterfeitDetector.UserInterface.Admin.Result;

public class ResultControlVM : ViewModelBase
{
    #region Functions

    #region Constructors

    public ResultControlVM(ResultClient resultClient,
                           DialogService dialogService,
                           IUserService userService,
                           IMessageBoxService messageBoxService)
    {


        _resultClient = resultClient;
        _dialogService = dialogService;
        _userService = userService;
        _messageBoxService = messageBoxService;

        _resultClient.ResultGetAsync()
                     .ContinueWith(c => { ResultVMs = c.Result.ToList().Adapt<List<ResultVM>>(); });
    }

    #endregion

    #endregion


    #region Properties

    private readonly ResultClient _resultClient;
    private readonly DialogService _dialogService;
    private readonly IUserService _userService;
    private readonly IMessageBoxService _messageBoxService;


    public List<ResultVM> ResultVMs { get; set; }
    public ResultVM SelectedResult { get; set; }

    #endregion


    #region Commands

    private RelayCommand _deleteResult;
    public RelayCommand DeleteResult
    {
        get
        {
            return _deleteResult ??= new RelayCommand(async o =>
            {
                try
                {
                    if (_messageBoxService.ShowMessage("Вы действительно хотите удалить запись?", "Удаление записи", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                    {

                        //Application.Current.Dispatcher.Invoke(async () =>
                        //{

                        //string pathToBase = Directory.GetCurrentDirectory();
                        //string combinedPath = Path.Combine(pathToBase, SelectedResult.OrigPath.Path);
                        //File.Delete(combinedPath);
                        //if (SelectedResult.ResPath.Path is not null)
                        //{
                        //    combinedPath = Path.Combine(pathToBase, SelectedResult.ResPath.Path);
                        //    File.Delete(combinedPath);
                        //}

                        await _resultClient.ResultDeleteAsync(SelectedResult.ResultId)
                                           .ContinueWith(c => { ResultVMs.Remove(SelectedResult); });
                        //});
                    }
                }
                catch (IOException)
                {
                    _messageBoxService.ShowMessage("Данную запись нельзя удалить в данный момент,\nтак как она используется в программе.", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }, c => SelectedResult is not null);
        }
    }

    #endregion
}

