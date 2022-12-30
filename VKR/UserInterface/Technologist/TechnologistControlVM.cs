using System;
using System.IO;
using System.Collections.Generic;
using System.Windows.Media.Imaging;
using Microsoft.Win32;
using System.Windows;
using System.Linq;

using VKR.Utils;
using VKR.Utils.MainWindowControlChanger;
using VKR.View;
using VKR.Utils.Dialog;

using DataAccess.Models;
using DataAccess.Data;
using Microsoft.EntityFrameworkCore;

using VKR.Services.IOService;
using VKR.UserInterface.Technologist.ImageAnalyzis;
using VKR.Utils.MessageBoxService;


namespace VKR.ViewModel;

public class TechnologistControlVM : ViewModelBase
{
    private readonly NavigationManager _navigationManager;
    private readonly IImageAnalyzer _analyzer;
    private readonly IFileDialogService _dialogService;
    private readonly IMessageBoxService _messageBoxService;


#region Functions

    public TechnologistControlVM(ResultDBContext context,
                                 NavigationManager navigationManager,
                                 IImageAnalyzer analyzer,
                                 IFileDialogService dialogService,
                                 IMessageBoxService messageBoxService)
    {
        _context = context;
        _context.Companies.Load();
        _context.OriginalPaths.Load();
        _context.ResultPaths.Load();
        _navigationManager = navigationManager;
        _analyzer = analyzer;
        _dialogService = dialogService;
        _messageBoxService = messageBoxService;
    }
    
    private byte[] ImagePathToByteArray(string path)
    {
        JpegBitmapEncoder encoder = new JpegBitmapEncoder();
        encoder.Frames.Add(BitmapFrame.Create(new BitmapImage(new Uri(path, UriKind.RelativeOrAbsolute))));
        using (MemoryStream ms = new MemoryStream())
        {
            encoder.Save(ms);
            return ms.ToArray();
        }
    }

    #endregion

    #region Properties

    private readonly ResultDBContext _context;
    public List<Company> Companies
    {
        get => _context.Companies.ToList();
    }
    public Company SelectedCompany { get; set; }
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
            // ReSharper disable once NullCoalescingConditionIsAlwaysNotNullAccordingToAPIContract
            return _changePathImage ??= new RelayCommand(_ =>
            {
                var path =_dialogService.OpenFileDialog(filter: "Pictures (*.jpg;*.gif;*.png)|*.jpg;*.gif;*.png", ext: ".png");
                if (path!= "")
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
            // ReSharper disable once NullCoalescingConditionIsAlwaysNotNullAccordingToAPIContract
            return _scanImage ??= new RelayCommand(_ =>
            {
                if (SelectedCompany != null && DisplayedImagePath != null && ResultImagePath != null)
                {
                    //название переменной со словом Current это очень плохой тон
                    CurrentResult = _analyzer.analyze(DisplayedImagePath, SelectedCompany);
                    // вместо того чтобы обновлять все эти поля можно просто прибиндится к результату и его свойствам
                    ResultImagePath = CurrentResult.ResPath.Path;
                    SearchResult = CurrentResult.AnRes;
                    AnalysisDate = DateTime.Now.ToString();
                    // мне кажется эта проверка не нужна
                    if (!_context.Results.Contains(CurrentResult))
                    {
                        _context.Results.Add(CurrentResult);
                    }
                    _context.SaveChanges();
                }
                else
                {
                    _messageBoxService.ShowMessage("Недостаточно данных для произведения анализа", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            });
        }
    }

    private RelayCommand _createFile;

    public RelayCommand CreateFile
    {
        get
        {
            // ReSharper disable once NullCoalescingConditionIsAlwaysNotNullAccordingToAPIContract
            return _createFile ??= new RelayCommand(_ =>
            {
                if (SearchResult != "")
                {
                    var filename = "АНАЛИЗ_" + DateTime.Now.ToString().Replace(':', '.').Replace('/', '.');
                    var filePath = _dialogService.SaveFileDialog(fileName: filename, ext: ".pdf");
                    
                    if (!String.IsNullOrEmpty(filePath))
                    {
                        var initialBitmap = ImagePathToByteArray(DisplayedImagePath);
                        
                        //зачем эта проверка если все равно идет сохранение?????
                        //if (ResultImagePath != "")
                        //{
                            var resBitmap = ImagePathToByteArray(ResultImagePath);
                        //}
                        FileSystem.ExportPdf(filePath, initialBitmap, resBitmap, SearchResult, AnalysisDate, SelectedCompany.Name);
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
            // ReSharper disable once NullCoalescingConditionIsAlwaysNotNullAccordingToAPIContract
            return _changeUser ??= new RelayCommand(_ =>
            {
                _navigationManager.Navigate<LoginControl>(new WindowParameters()
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
            // ReSharper disable once NullCoalescingConditionIsAlwaysNotNullAccordingToAPIContract
            return _showInfo ??= new RelayCommand(_ =>
            {
                _messageBoxService.ShowMessage(
                                               "Данный программный комплекс предназначен для обработки\n" +
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
            // ReSharper disable once NullCoalescingConditionIsAlwaysNotNullAccordingToAPIContract
            return _exit ??= new RelayCommand(_ =>
            {
                Application.Current.Shutdown();
            });
        }
    }

    #endregion
}