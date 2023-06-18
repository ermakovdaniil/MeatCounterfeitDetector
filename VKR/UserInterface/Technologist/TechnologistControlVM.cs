using DataAccess.Data;
using DataAccess.Models;
using ImageAnalyzis;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using VKR.UserInterface.Admin.Abstract;
using VKR.Utils;
using VKR.Utils.Dialog;
using VKR.Utils.IOService;
using VKR.Utils.MainWindowControlChanger;
using VKR.Utils.MessageBoxService;
using VKR.Utils.UserService;

namespace VKR.UserInterface.Technologist;

public class TechnologistControlVM : ViewModelBase
{
    private object _data;
    private readonly IImageAnalyzer _analyzer;
    private readonly IFileDialogService _dialogService;
    private readonly IMessageBoxService _messageBoxService;
    private readonly NavigationManager _navigationManager;
    private readonly IUserService _userService;

    #region Functions

    public TechnologistControlVM(ResultDBContext resultContext,
                                 CounterfeitKBContext counterfeitsContext,
                                 NavigationManager navigationManager,
                                 IImageAnalyzer analyzer,
                                 IFileDialogService dialogService,
                                 IMessageBoxService messageBoxService,
                                 IUserService userService)
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
        _userService = userService;
    }

    private string CreateSearchResult(Result AnalysisResult)
    {
        string searchResult;
        searchResult = AnalysisResult.AnRes + "\n" +
                       "Дата проведения анализа: " + AnalysisResult.Date + "\n" +
                       "Время проведения: " + AnalysisResult.Time + " с\n" +
                       "Процент сходства: " + AnalysisResult.PercentOfSimilarity + "%";
        return searchResult;
    }

    #endregion


    #region Properties

    private readonly ResultDBContext _resultContext;
    private readonly CounterfeitKBContext _counterfeitsContext;
    public List<Counterfeit> Counterfeits => _counterfeitsContext.Counterfeits.ToList();
    public List<Result> Results => _resultContext.Results.ToList();
    public Counterfeit SelectedCounterfeit { get; set; }
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
                var path = _dialogService.OpenFileDialog(filter: "Pictures (*.jpg;*.jpeg;*.gif;*.png)|*.jpg;*.gif;*.png", ext: ".jpg");

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
                if (DisplayedImagePath is null)
                {
                    _messageBoxService.ShowMessage("Недостаточно данных для произведения анализа", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    try
                    {
                        List<CounterfeitPath> counterfeitPaths = new List<CounterfeitPath>();
                        if (SelectedCounterfeit is null)
                        {
                            counterfeitPaths = _counterfeitsContext.CounterfeitPaths.ToList();
                        }
                        else
                        {
                            counterfeitPaths = _counterfeitsContext.CounterfeitPaths.Include(c => c.Counterfeit).Where(c => c.CounterfeitId == SelectedCounterfeit.Id).ToList();
                        }

                        AnalysisResult = _analyzer.RunAnalysis(DisplayedImagePath, counterfeitPaths, PercentOfSimilarity, _userService.User);                       
                        SearchResult = CreateSearchResult(AnalysisResult);

                        if (AnalysisResult.ResPath.Path is not null)
                        {
                            string pathToBase = Directory.GetCurrentDirectory();
                            string pathToResults = @"..\..\..\resources\resImages\";
                            string combinedPath = Path.Combine(pathToBase, AnalysisResult.ResPath.Path);
                            ResultImagePath = combinedPath;
                        }
                        _resultContext.Results.Add(AnalysisResult);
                        _resultContext.SaveChanges();
                        OnPropertyChanged(nameof(_resultContext.Results));
                    }
                    catch (ArgumentException)
                    {
                        _messageBoxService.ShowMessage("Данные в базе фальсификатов были удалены или повреждены.\nПеред запуском анализа устраните проблему.", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    catch (Emgu.CV.Util.CvException)
                    {
                        _messageBoxService.ShowMessage("Данное изображение не удаётся обработать. Попробуйте изменить разрешение изображения.", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
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
                if (AnalysisResult is null)
                {
                    _messageBoxService.ShowMessage("Недостаточно данных для сохранения", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    var filename = "АНАЛИЗ_" + DateTime.Now.ToString("dd.mm.yyyy_hh.mm.ss");
                    var filePath = _dialogService.SaveFileDialog(filename, ext: ".pdf");
                    if (!string.IsNullOrEmpty(filePath))
                    {
                        FileSystem.ExportPdf(filePath, AnalysisResult, _userService.User);
                    }
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
