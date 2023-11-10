﻿using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using MeatCounterfeitDetector.UserInterface.Admin.Abstract;
using MeatCounterfeitDetector.Utils;
using MeatCounterfeitDetector.Utils.MessageBoxService;
using MeatCounterfeitDetector.Utils.UserService;
using ClientAPI;
using Mapster;
using System.Collections.ObjectModel;
using MeatCountefeitDetector.UserInterface.EntityVM;
using MeatCounterfeitDetector.Utils.Dialog;
using MeatCountefeitDetector.Utils.EventAggregator;
using MeatCounterfeitDetector.UserInterface.EntityVM;

namespace MeatCounterfeitDetector.UserInterface.Admin.Result;

public class ResultControlVM : ViewModelBase
{
    #region Functions

    #region Constructors

    public ResultControlVM(ResultClient resultClient,
                           OriginalImageClient originalImageClient,
                           ResultImageClient resultImageClient,
                           DialogService dialogService,
                           IMessageBoxService messageBoxService)
    {
        _resultClient = resultClient;
        _originalImageClient = originalImageClient;
        _resultImageClient = resultImageClient;
        _dialogService = dialogService;
        _messageBoxService = messageBoxService;

        _resultClient.ResultGetAsync()
                     .ContinueWith(c => { ResultVMs = c.Result.ToList().Adapt<ObservableCollection<ResultVM>>(); });
    }

    #endregion

    #endregion


    #region Properties

    private readonly ResultClient _resultClient;
    private readonly OriginalImageClient _originalImageClient;
    private readonly ResultImageClient _resultImageClient;
    private readonly DialogService _dialogService;
    private readonly IMessageBoxService _messageBoxService;

    public ObservableCollection<ResultVM> ResultVMs { get; set; }
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

                        await _resultClient.ResultDeleteAsync(SelectedResult.Id);
                        ResultVMs.Remove(SelectedResult);

                        await _resultImageClient.ResultImageDeleteAsync(SelectedResult.ResultImageId);

                        if(!ResultVMs.Any(r => r.OriginalImageId == SelectedResult.OriginalImageId)) {
                            await _originalImageClient.OriginalImageDeleteAsync(SelectedResult.OriginalImageId);
                        }
          
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

