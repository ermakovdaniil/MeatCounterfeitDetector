using System;
using System.IO;
using System.Linq;
using System.Windows;
using MeatCounterfeitDetector.UserInterface.Admin.Abstract;
using MeatCounterfeitDetector.Utils;
using MeatCounterfeitDetector.Utils.Dialog.Abstract;
using MeatCounterfeitDetector.Utils.IOService;
using MeatCounterfeitDetector.Utils.MessageBoxService;
using ClientAPI;
using MeatCounterfeitDetector.UserInterface.Admin.Counterfeit;
using System.Collections.Generic;
using Mapster;

namespace MeatCounterfeitDetector.UserInterface.Admin.Gallery;

public class GalleryEditControlVM : ViewModelBase, IDataHolder, IResultHolder, IInteractionAware
{
    #region Functions

    #region Constructors

    public GalleryEditControlVM(CounterfeitClient counterfeitClient,
                                CounterfeitPathClient counterfeitPathClient,
                                IFileDialogService dialogService,
                                IMessageBoxService messageBoxService)
    {
        _counterfeitClient = counterfeitClient;
        _counterfeitPathClient = counterfeitPathClient;
        _dialogService = dialogService;
        _messageBoxService = messageBoxService;

        _counterfeitClient.CounterfeitGetAsync()
                          .ContinueWith(c => { CounterfeitVMs = c.Result.ToList().Adapt<List<CounterfeitVM>>(); });  
    }

    #endregion

    #endregion

    #region Properties

    private readonly CounterfeitClient _counterfeitClient;
    private readonly CounterfeitPathClient _counterfeitPathClient;
    private readonly IFileDialogService _dialogService;
    private readonly IMessageBoxService _messageBoxService;

    private object _data;
    public object Data
    {
        get => _data;
        set
        {
            _data = value;

            TempCounterfeitPath = new CounterfeitPathVM
            {
                Id = EditingCounterfeitPath.Id,
                CounterfeitId = EditingCounterfeitPath.CounterfeitId, // TODO
                ImagePath = EditingCounterfeitPath.ImagePath,
            };
            OnPropertyChanged(nameof(TempCounterfeitPath));
        }
    }

    public Action FinishInteraction { get; set; }
    public object? Result { get; set; }

    public string CounterfeitImagePath;

    public List<CounterfeitVM> CounterfeitVMs { get; set; }
    public CounterfeitPathVM TempCounterfeitPath { get; set; }
    public CounterfeitPathVM EditingCounterfeitPath => (CounterfeitPathVM)Data;

    #endregion


    #region Commands

    private RelayCommand _changePathImage;
    public RelayCommand ChangePathImageCommand // TODO
    {
        get
        {
            return _changePathImage ??= new RelayCommand(_ =>
            {
                var path = _dialogService.OpenFileDialog(filter: "Pictures (*.png)|*.png", ext: ".png");
                if (path != "")
                {
                    CounterfeitImagePath = @"..\..\..\resources\counterfeits\" + Path.GetFileName(path);
                    TempCounterfeitPath.ImagePath = path;
                }
            });
        }
    }

    private RelayCommand _saveCounterfeitPath;
    public RelayCommand SaveCounterfeitPath
    {
        get
        {
            return _saveCounterfeitPath ??= new RelayCommand(o =>
            {
                try
                {
                    if (CounterfeitImagePath is not null)
                    {
                        EditingCounterfeitPath.Id = TempCounterfeitPath.Id;
                        EditingCounterfeitPath.CounterfeitId = TempCounterfeitPath.CounterfeitId; // TODO
                        EditingCounterfeitPath.ImagePath = CounterfeitImagePath; // TODO
                        Result = EditingCounterfeitPath;
                        FinishInteraction();
                    }
                    else
                    {
                        _messageBoxService.ShowMessage("Не указан путь к файлу!", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                catch (ArgumentNullException ex)
                {
                    _messageBoxService.ShowMessage("Файла по указанному пути не существует!", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                }

            });
        }
    }

    private RelayCommand _closeCommand;
    public RelayCommand CloseCommand
    {
        get
        {
            return _closeCommand ??= new RelayCommand(o =>
            {
                FinishInteraction();
            });
        }
    }

    #endregion
}

