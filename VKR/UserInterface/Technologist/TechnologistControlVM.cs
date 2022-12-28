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

namespace VKR.ViewModel;

public class TechnologistControlVM : ViewModelBase
{
    private readonly NavigationManager _navigationManager;

    #region Functions

    public TechnologistControlVM(ResultDBContext context, NavigationManager navigationManager)
    {
        _context = context;
        _context.Companies.Load();
        _context.OriginalPaths.Load();
        _context.ResultPaths.Load();
        _navigationManager = navigationManager;
    }

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

    public Company SelectedCompany { get; set; }

    public List<Company> Companies
    {
        get => _context.Companies.ToList();
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


    private Result _tempResult;

    public Result TempResult
    {
        get
        {
            return _tempResult;
        }
        set
        {
            _tempResult = value;
            OnPropertyChanged();
        }
    }

    public Result CurrentResult => (Result)Data;

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
            return _scanImage ??= new RelayCommand(_ =>
            {
                // TODO: Загулшка
                DisplayedImagePath = @"C:\Users\Даня\source\repos\VKR_v2\VKR\resources\origImages\orig.jpg";
                ResultImagePath = @"C:\Users\Даня\source\repos\VKR_v2\VKR\resources\resImages\res.jpg";
                AnalysisDate = DateTime.Now.ToString();
                SearchResult = "Обнаружен фальсификат: Каррагинан.\n" + "Дата проведения анализа: " + AnalysisDate;

                //CurrentResult.Id = TempResult.Id;
                //CurrentResult.Date = TempResult.Date;
                //CurrentResult.Company = TempResult.Company;
                //CurrentResult.AnRes = TempResult.AnRes;
                //CurrentResult.OrigPath = TempResult.OrigPath;
                //CurrentResult.ResPath = TempResult.ResPath;

                //CurrentResult.Date = AnalysisDate;
                //CurrentResult.Company = SelectedCompany;  
                //// TODO: Загулшка
                //CurrentResult.AnRes = "Обнаружен фальсификат: Каррагинан.";
                //// CurrentResult.AnRes = SearchResult;
                //CurrentResult.OrigPath.Path = DisplayedImagePath;
                //CurrentResult.ResPath.Path = ResultImagePath;

                //if (!_context.Results.Contains(CurrentResult))
                //{
                //    _context.Results.Add(CurrentResult);
                //}

                //_context.SaveChanges();
                //FinishInteraction();
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
                        FileName = "АНАЛИЗ_" + DateTime.Now.ToString().Replace(':', '_').Replace('/', '.'),
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

    private object _data;
    public object Data
    {
        get => _data;
        set
        {
            _data = value;
            TempResult = new Result()
            {
                //Id = CurrentResult.Id,
                Date = AnalysisDate,
                Company = SelectedCompany,
                // TODO: заглушка
                AnRes = "Обнаружен фальсификат: Каррагинан.",
                //AnRes = SearchResult,
                //OrigPath = DisplayedImagePath,
                //ResPath = ResultImagePath,
            };
            OnPropertyChanged(nameof(TempResult));
        }
    }

    public object? Result { get; }
    public Action FinishInteraction { get; set; }
}