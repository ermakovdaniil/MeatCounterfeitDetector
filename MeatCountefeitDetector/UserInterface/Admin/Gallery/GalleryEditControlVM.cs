using DataAccess.Data;
using DataAccess.Models;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using MeatCounterfeitDetector.UserInterface.Admin.Abstract;
using MeatCounterfeitDetector.Utils;
using MeatCounterfeitDetector.Utils.Dialog.Abstract;
using MeatCounterfeitDetector.Utils.IOService;
using MeatCounterfeitDetector.Utils.MessageBoxService;

namespace MeatCounterfeitDetector.UserInterface.Admin.Gallery;

public class GalleryEditControlVM : ViewModelBase, IDataHolder, IResultHolder, IInteractionAware
{
    private object _data;
    private readonly IFileDialogService _dialogService;
    private readonly IMessageBoxService _messageBoxService;


    #region Functions

    #region Constructors

    public GalleryEditControlVM(CounterfeitKBContext context, IFileDialogService dialogService, IMessageBoxService messageBoxService)
    {
        _context = context;
        _dialogService = dialogService;
        _messageBoxService = messageBoxService;
        Counterfeits = new ObservableCollection<DataAccess.Models.Counterfeit>(_context.Counterfeits.ToList());
    }

    #endregion

    #endregion


    public object Data
    {
        get => _data;
        set
        {
            _data = value;

            TempCounterfeitPath = new CounterfeitPath
            {
                Id = EditingCounterfeitPath.Id,
                Counterfeit = EditingCounterfeitPath.Counterfeit,
                ImagePath = EditingCounterfeitPath.ImagePath,
            };
            OnPropertyChanged(nameof(TempCounterfeitPath));
        }
    }

    public Action FinishInteraction { get; set; }

    public object? Result { get; }


    #region Properties

    private CounterfeitPath _tempCounterfeitPath;

    public CounterfeitPath TempCounterfeitPath
    {
        get => _tempCounterfeitPath;
        set
        {
            _tempCounterfeitPath = value;
            OnPropertyChanged();
        }
    }

    public CounterfeitPath EditingCounterfeitPath => (CounterfeitPath)Data;
    public string CounterfeitImagePath;

    private readonly CounterfeitKBContext _context;

    public ObservableCollection<DataAccess.Models.Counterfeit> Counterfeits { get; set; }

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
                    CounterfeitImagePath = @"..\..\..\resources\counterfeits\" + Path.GetFileName(path);
                    TempCounterfeitPath.ImagePath = path;
                }
                OnPropertyChanged(nameof(TempCounterfeitPath));
            });
        }
    }

    private RelayCommand _saveCounterfeitPath;

    /// <summary>
    ///     Команда сохраняющая изменение данных о пути к изображению фальсификата в базе данных
    /// </summary>
    public RelayCommand SaveCounterfeitPath
    {
        get
        {
            return _saveCounterfeitPath ??= new RelayCommand(o =>
            {
                try
                {
                    if (CounterfeitImagePath is null)
                    {
                        CounterfeitImagePath = Directory.GetCurrentDirectory();
                    }
                    EditingCounterfeitPath.Id = TempCounterfeitPath.Id;
                    EditingCounterfeitPath.Counterfeit = TempCounterfeitPath.Counterfeit;
                    EditingCounterfeitPath.ImagePath = CounterfeitImagePath;
                    if (!_context.CounterfeitPaths.Contains(EditingCounterfeitPath))
                    {
                        _context.CounterfeitPaths.Add(EditingCounterfeitPath);
                    }
                    File.Copy(TempCounterfeitPath.ImagePath, EditingCounterfeitPath.ImagePath, true);
                    _context.SaveChanges();
                    FinishInteraction();
                }
                catch (ArgumentNullException ex)
                {
                    _messageBoxService.ShowMessage("Не указан путь к файлу", "Ошибка!",
                                                    MessageBoxButton.OK, MessageBoxImage.Error);
                }
                catch (Exception ex)
                {
                    _messageBoxService.ShowMessage("Файла по указанному пути не существует", "Ошибка!",
                                                    MessageBoxButton.OK, MessageBoxImage.Error);
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

