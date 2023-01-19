using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Media.Imaging;

using DataAccess.Data;
using DataAccess.Models;

using Microsoft.EntityFrameworkCore;

using VKR.UserInterface.Admin.Abstract;
using VKR.Utils;
using VKR.Utils.Dialog;
using VKR.Utils.Dialog.Abstract;
using VKR.Utils.ImageAnalyzis;
using VKR.Utils.IOService;
using VKR.Utils.MainWindowControlChanger;
using VKR.Utils.MessageBoxService;


namespace VKR.UserInterface.Technologist;

public class TechnologistControlVM : ViewModelBase, IDataHolder
{
    private object _data;
    private readonly IImageAnalyzer _analyzer;
    private readonly IFileDialogService _dialogService;
    private readonly IMessageBoxService _messageBoxService;
    private readonly Counterfeit _counterfeit;
    private readonly NavigationManager _navigationManager;


    #region Functions

    public TechnologistControlVM(ResultDBContext resultContext,
                                 CounterfeitKBContext counterfeitsContext,
                                 NavigationManager navigationManager,
                                 IImageAnalyzer analyzer,
                                 IFileDialogService dialogService,
                                 IMessageBoxService messageBoxService)
    {
        _resultContext = resultContext;
        _counterfeitsContext = counterfeitsContext;
        _counterfeitsContext.Counterfeits.Load();
        _resultContext.OriginalPaths.Load();
        _resultContext.ResultPaths.Load();
        _navigationManager = navigationManager;
        _analyzer = analyzer;
        _dialogService = dialogService;
        _messageBoxService = messageBoxService;
    }

    private byte[] ImagePathToByteArray(string path)
    {
        var encoder = new JpegBitmapEncoder();
        encoder.Frames.Add(BitmapFrame.Create(new BitmapImage(new Uri(path, UriKind.RelativeOrAbsolute))));

        using (var ms = new MemoryStream())
        {
            encoder.Save(ms);

            return ms.ToArray();
        }
    }

    private string CreateSearchResult(Result AnalysisResult)
    {
        string searchResult;
        searchResult = AnalysisResult.AnRes + "\n" +
                       "Дата проведения анализа: " + AnalysisResult.Date + "\n" +
                       "Время проведения: " + AnalysisResult.PercentOfSimilarity + " мс\n" +
                       "Процент сходства: " + AnalysisResult.PercentOfSimilarity + "%";
        return searchResult;
    }

    #endregion

    public object Data
    {
        get => _data;
        set
        {
            _data = value;

            WorkingUser = new User
            {
                Id = TempUser.Id,
                Login = TempUser.Login,
                Password = TempUser.Password,
                Name = TempUser.Name,
                TypeId = TempUser.TypeId,
            };

            OnPropertyChanged(nameof(WorkingUser));
        }
    }

    #region Properties

    private readonly ResultDBContext _resultContext;
    private readonly CounterfeitKBContext _counterfeitsContext;

    public List<Counterfeit> Counterfeits => _counterfeitsContext.Counterfeits.ToList();
    public Counterfeit SelectedCounterfeit { get; set; }

    public User WorkingUser { get; set; }

    public User TempUser => (User)Data;

    public double PercentOfSimilarity { get; set; }
    public string DisplayedImagePath { get; set; }
    public string ResultImagePath { get; set; }
    public string SearchResult { get; set; }
    public Result AnalysisResult { get; set; }

    #endregion


    #region Commands

    private RelayCommand _changePathImage;

    public RelayCommand ChangePathImageCommand
    {
        get
        {
            return _changePathImage ??= new RelayCommand(_ =>
            {
                var path = _dialogService.OpenFileDialog(filter: "Pictures (*.jpg;*.gif;*.png)|*.jpg;*.gif;*.png", ext: ".png");

                if (path != "")
                {
                    DisplayedImagePath = path;
                    ResultImagePath = "";
                }
            });
        }
    }

    private RelayCommand _scanImage;

    public RelayCommand ScanImage
    {
        get
        {
            return _scanImage ??= new RelayCommand(_ =>
            {
                if (DisplayedImagePath != null)
                {
                    List<CounterfeitPath> counterfeitPaths = new List<CounterfeitPath>();
                    if (SelectedCounterfeit == null)
                    {
                        counterfeitPaths = _counterfeitsContext.CounterfeitPaths.ToList();
                    }
                    else
                    {

                        counterfeitPaths = _counterfeitsContext.CounterfeitPaths.Include(c => c.Counterfeit).Where(c => c.CounterfeitId == SelectedCounterfeit.Id).ToList();
                    }

                    AnalysisResult = _analyzer.RunAnalysis(DisplayedImagePath, counterfeitPaths, PercentOfSimilarity, WorkingUser);
                    ResultImagePath = AnalysisResult.ResPath.Path;
                    SearchResult = CreateSearchResult(AnalysisResult);
                    _resultContext.Results.Add(AnalysisResult);
                    _resultContext.SaveChanges();
                }
                else
                {
                    _messageBoxService.ShowMessage("Недостаточно данных для произведения анализа", "Ошибка!", MessageBoxButton.OK,
                                                   MessageBoxImage.Error);
                }
            });
        }
    }

    private RelayCommand _createFile;

    public RelayCommand CreateFile
    {
        get
        {
            return _createFile ??= new RelayCommand(_ =>
            {
                if (AnalysisResult != null)
                {
                    var filename = "АНАЛИЗ_" + DateTime.Now.ToString().Replace(':', '.').Replace('/', '.');
                    var filePath = _dialogService.SaveFileDialog(filename, ext: ".pdf");

                    if (!string.IsNullOrEmpty(filePath))
                    {
                        var initialBitmap = ImagePathToByteArray(DisplayedImagePath);
                        var resBitmap = ImagePathToByteArray(ResultImagePath);
                        FileSystem.ExportPdf(filePath, initialBitmap, resBitmap, AnalysisResult);
                    }
                }
                else
                {
                    _messageBoxService.ShowMessage("Недостаточно данных для сохранения", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            });
        }
    }

    private RelayCommand _changeUser;

    public RelayCommand ChangeUser
    {
        get
        {
            return _changeUser ??= new RelayCommand(_ =>
            {
                _navigationManager.Navigate<LoginControl>(new WindowParameters
                {
                    Height = 300,
                    Width = 350,
                    Title = "Вход в систему",
                    StartupLocation = WindowStartupLocation.CenterScreen,
                });
            });
        }
    }

    private RelayCommand _showInfo;

    public RelayCommand ShowInfo
    {
        get
        {
            return _showInfo ??= new RelayCommand(_ =>
            {
                _messageBoxService.ShowMessage("Данный программный комплекс предназначен для обработки\n" +
                                               "входного изображения среза мясной продукции в задаче\n" +
                                               "обнаружения фальсификата.\n" +
                                               "\n" +
                                               "Вам доступны следующие возможности:\n" +
                                               "   * Анализ изображения.\n" +
                                               "   * Сохранение результата анализа в виде отчёта.\n" +
                                               "\n" +
                                               "Автор:  Ермаков Даниил Игоревич\n" +
                                               "Группа: 494\n" +
                                               "Учебное заведение: СПбГТИ (ТУ)", "Справка о программе",
                                               MessageBoxButton.OK, MessageBoxImage.Information);
            });
        }
    }

    private RelayCommand _exit;

    public RelayCommand Exit
    {
        get
        {
            return _exit ??= new RelayCommand(_ =>
            {
                Application.Current.Shutdown();
            });
        }
    }

    #endregion
}
