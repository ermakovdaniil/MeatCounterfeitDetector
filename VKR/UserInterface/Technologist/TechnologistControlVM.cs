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

using VKR.UserInterface.Technologist.ImageAnalyzis;


namespace VKR.ViewModel;

public class TechnologistControlVM : ViewModelBase
{
    private readonly NavigationManager _navigationManager;
    private readonly IImageAnalyzer _analyzer;


#region Functions

    public TechnologistControlVM(ResultDBContext context, NavigationManager navigationManager, IImageAnalyzer analyzer)
    {
        _context = context;
        _context.Companies.Load();
        _context.OriginalPaths.Load();
        _context.ResultPaths.Load();
        _navigationManager = navigationManager;
        _analyzer = analyzer;
    }

    //todo срочно убери передачу этого массива в функцию
    private byte[] ImagePathToByteArray(byte[] byteArray, string path)
    {
        JpegBitmapEncoder encoder = new JpegBitmapEncoder();
        encoder.Frames.Add(BitmapFrame.Create(new BitmapImage(new Uri(path, UriKind.Relative))));
        using (MemoryStream ms = new MemoryStream())
        {
            encoder.Save(ms);
            byteArray = ms.ToArray();
        }
        return byteArray;
    }

    #endregion

    #region Properties

    private readonly ResultDBContext _context;

    public List<Company> Companies
    {
        get => _context.Companies.ToList();
    }

    private Company _selectedCompany;

    public Company SelectedCompany
    {
        get
        {
            return _selectedCompany;
        }
        set
        {
            _selectedCompany = value;
            OnPropertyChanged();
        }
    }


    private string _displayedImagePath;
    public string DisplayedImagePath
    {
        get
        {
            return _displayedImagePath;
        }
        set
        {
            _displayedImagePath = value;
            OnPropertyChanged();
        }
    }

    private string _resultImagePath;
    public string ResultImagePath
    {
        get
        {
            return _resultImagePath;
        }
        set
        {
            _resultImagePath = value;
            OnPropertyChanged();
        }
    }

    private string _searchResult;
    public string SearchResult
    {
        get
        {
            return _searchResult;
        }
        set
        {
            _searchResult = value;
            OnPropertyChanged();
        }
    }

    private string _analysisDate;
    public string AnalysisDate
    {
        get
        {
            return _analysisDate;
        }
        set
        {
            _analysisDate = value;
            OnPropertyChanged();
        }
    }

    private Result _currentResult;

    public Result CurrentResult
    {
        get
        {
            return _currentResult;
        }
        set
        {
            _currentResult = value;
            OnPropertyChanged();
        }
    }


    #endregion

    #region Commands


    private RelayCommand _changePathImage;

    public RelayCommand ChangePathImageCommand
    {
        get
        {
            return _changePathImage ??= new RelayCommand(_ =>
            {
                OpenFileDialog open = new OpenFileDialog();
                open.DefaultExt = (".png");
                open.Filter = "Pictures (*.jpg;*.gif;*.png)|*.jpg;*.gif;*.png";

                if (open.ShowDialog() == true)
                {
                    DisplayedImagePath = open.FileName;
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
                    // мне кажется эта проверка не нужна
                    if (!_context.Results.Contains(CurrentResult))
                    {
                        _context.Results.Add(CurrentResult);
                    }
                    _context.SaveChanges();
                }
                else
                {
                    HandyControl.Controls.MessageBox.Show("Недостаточно данных для произведения анализа", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
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
                    var dlg = new SaveFileDialog
                    {
                        DefaultExt = ".pdf",
                        FileName = "АНАЛИЗ_" + DateTime.Now.ToString().Replace(':', '.').Replace('/', '.'),
                    };
                    var res = dlg.ShowDialog();
                    if (res == true)
                    {
                        byte[] initialBitmap = null;
                        byte[] resBitmap = null;
                        initialBitmap = ImagePathToByteArray(initialBitmap, DisplayedImagePath);
                        if (ResultImagePath != "")
                        {
                            resBitmap = ImagePathToByteArray(resBitmap, ResultImagePath);
                        }
                        FileSystem.ExportPdf(dlg.FileName, initialBitmap, resBitmap, SearchResult, AnalysisDate, SelectedCompany.Name);
                    }
                }
                else
                {
                    HandyControl.Controls.MessageBox.Show("Недостаточно данных для сохранения", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
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
            return _showInfo ??= new RelayCommand(_ =>
            {
                HandyControl.Controls.MessageBox.Show(
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
            return _exit ??= new RelayCommand(_ =>
            {
                Application.Current.Shutdown();
            });
        }
    }

    #endregion
}