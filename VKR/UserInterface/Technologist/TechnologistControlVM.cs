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
using VKR.Utils.ImageAnalyzis;
using VKR.Utils.IOService;
using VKR.Utils.MainWindowControlChanger;
using VKR.Utils.MessageBoxService;


namespace VKR.UserInterface.Technologist;

public class TechnologistControlVM : ViewModelBase
{
    private readonly IImageAnalyzer _analyzer;
    private readonly IFileDialogService _dialogService;
    private readonly IMessageBoxService _messageBoxService;
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

    #endregion


    #region Properties

    private readonly ResultDBContext _resultContext;
    private readonly CounterfeitKBContext _counterfeitsContext;

    public List<Counterfeit> Counterfeits => _counterfeitsContext.Counterfeits.ToList();

    public Counterfeit SelectedCounterfeit { get; set; }
    public User SelectedUser { get; set; }
    public double PrecentOfSimilarity { get; set; }
    public string DisplayedImagePath { get; set; }
    public string ResultImagePath { get; set; }
    public string SearchResult { get; set; }
    public string AnalysisDate { get; set; }
    public Result CurrentResult { get; set; }

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
                if (SelectedCounterfeit != null && DisplayedImagePath != null && ResultImagePath != null)
                {
                    //название переменной со словом Current это очень плохой тон
                    CurrentResult = _analyzer.analyze(DisplayedImagePath, SelectedCounterfeit, PrecentOfSimilarity);

                    // вместо того чтобы обновлять все эти поля можно просто прибиндится к результату и его свойствам
                    ResultImagePath = CurrentResult.ResPath.Path;
                    SearchResult = CurrentResult.AnRes;
                    AnalysisDate = DateTime.Now.ToString();

                    // мне кажется эта проверка не нужна
                    //if (!_resultContext.Results.Contains(CurrentResult))
                    //{
                        _resultContext.Results.Add(CurrentResult);
                    //}

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
                if (SearchResult != "")
                {
                    var filename = "АНАЛИЗ_" + DateTime.Now.ToString().Replace(':', '.').Replace('/', '.');
                    var filePath = _dialogService.SaveFileDialog(filename, ext: ".pdf");

                    if (!string.IsNullOrEmpty(filePath))
                    {
                        var initialBitmap = ImagePathToByteArray(DisplayedImagePath);

                        //зачем эта проверка если все равно идет сохранение?????
                        //if (ResultImagePath != "")
                        //{
                        var resBitmap = ImagePathToByteArray(ResultImagePath);

                        //}
                        FileSystem.ExportPdf(filePath, initialBitmap, resBitmap, SearchResult, AnalysisDate, SelectedUser.Name);
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
