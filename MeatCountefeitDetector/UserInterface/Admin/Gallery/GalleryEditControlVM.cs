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
using Mapster;
using System.Collections.ObjectModel;
using MeatCounterfeitDetector.UserInterface.EntityVM;

namespace MeatCounterfeitDetector.UserInterface.Admin.Gallery;

public class GalleryEditControlVM : ViewModelBase, IDataHolder, IResultHolder, IInteractionAware
{
    #region Functions

    #region Constructors

    public GalleryEditControlVM(CounterfeitClient counterfeitClient,
                                IFileDialogService dialogService,
                                IMessageBoxService messageBoxService)
    {
        _counterfeitClient = counterfeitClient;
        _dialogService = dialogService;
        _messageBoxService = messageBoxService;

        _counterfeitClient.CounterfeitGetAsync()
                          .ContinueWith(c => { CounterfeitVMs = c.Result.ToList().Adapt<ObservableCollection<CounterfeitVM>>(); });  
    }

    #endregion

    #endregion


    #region Properties

    private readonly CounterfeitClient _counterfeitClient;
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
                //CounterfeitId = EditingCounterfeitPath.CounterfeitId, // TODO
                EncodedImage = EditingCounterfeitPath.EncodedImage,
                Counterfeit = EditingCounterfeitPath.Counterfeit
            };
            OnPropertyChanged(nameof(TempCounterfeitPath));
        }
    }

    public Action FinishInteraction { get; set; }
    public object? Result { get; set; }

    public ObservableCollection<CounterfeitVM> CounterfeitVMs { get; set; }
    public CounterfeitPathVM TempCounterfeitPath { get; set; }
    public CounterfeitPathVM EditingCounterfeitPath => (CounterfeitPathVM)Data;

    #endregion


    #region Commands

    private RelayCommand _changePathImage;
    public RelayCommand ChangePathImageCommand
    {
        get
        {
            return _changePathImage ??= new RelayCommand(_ =>
            {
                var path = _dialogService.OpenFileDialog(filter: "Pictures (*.png)|*.png", ext: ".png");
                if (path != "")
                {
                    TempCounterfeitPath.EncodedImage = path;
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
                    if (TempCounterfeitPath.EncodedImage is not null)
                    {
                        EditingCounterfeitPath.Id = TempCounterfeitPath.Id;
                        //EditingCounterfeitPath.CounterfeitId = TempCounterfeitPath.CounterfeitId; // TODO

                        // TODO: СДЕЛАТЬ ПРЕВРАЩЕНИЕ ИЗОБРАЖЕНИЯ В СТРОКУ BASE-64
                        //CounterfeitImage = @"..\..\..\resources\counterfeits\" + Path.GetFileName(path);
                        EditingCounterfeitPath.EncodedImage = TempCounterfeitPath.EncodedImage; // TODO

                        EditingCounterfeitPath.Counterfeit = TempCounterfeitPath.Counterfeit;
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

