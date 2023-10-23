using DataAccess.Data;
using DataAccess.Models;
using ImageAnalyzis;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using ClientAPI;
using ClientAPI.DTO.Counterfeit;
using ClientAPI.DTO.CounterfeitPath;
using ClientAPI.DTO.Result;
using Mapster;
using MeatCounterfeitDetector.UserInterface.Admin.Abstract;
using MeatCounterfeitDetector.Utils;
using MeatCounterfeitDetector.Utils.Dialog;
using MeatCounterfeitDetector.Utils.IOService;
using MeatCounterfeitDetector.Utils.MainWindowControlChanger;
using MeatCounterfeitDetector.Utils.MessageBoxService;
using MeatCounterfeitDetector.Utils.UserService;
using ClientAPI.DTO.ResultPath;
using MeatCounterfeitDetector.UserInterface;

namespace MeatCounterfeitDetector.UserInterface.Technologist.Analysis;

public class AnalysisControlVM : ViewModelBase
{
    private readonly IImageAnalyzer _analyzer;
    private readonly IFileDialogService _dialogService;
    private readonly IMessageBoxService _messageBoxService;
    private readonly CounterfeitClient _counterfeitClient;
    private readonly CounterfeitPathClient _counterfeitPathClient;
    private readonly ResultClient _resultClient;
    private readonly NavigationManager _navigationManager;
    private readonly IUserService _userService;


    #region Functions

    public AnalysisControlVM(CounterfeitClient counterfeitClient,
                                 CounterfeitPathClient counterfeitPathClient,
                                 ResultClient resultClient,
                                 NavigationManager navigationManager,
                                 IImageAnalyzer analyzer,
                                 IFileDialogService dialogService,
                                 IMessageBoxService messageBoxService,
                                 IUserService userService)
    {
        _counterfeitClient = counterfeitClient;
        _counterfeitPathClient = counterfeitPathClient;
        _resultClient = resultClient;
        _navigationManager = navigationManager;
        _analyzer = analyzer;
        _dialogService = dialogService;
        _messageBoxService = messageBoxService;
        _userService = userService;
        Task.Run(async () =>
        {
            Counterfeits = (await _counterfeitClient.CounterfeitGetAsync()).ToList();
        });
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

    public List<GetCounterfeitDTO> Counterfeits { get; set; }
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
            return _scanImage ??= new RelayCommand(async _ =>
            {
                if (DisplayedImagePath is null)
                {
                    _messageBoxService.ShowMessage("Недостаточно данных для произведения анализа", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    try
                    {
                        List<GetCounterfeitPathDTO> counterfeitPathsDTOs = new List<GetCounterfeitPathDTO>();
                        if (SelectedCounterfeit is null)
                        {
                            counterfeitPathsDTOs = (await _counterfeitPathClient.CounterfeitPathGetAsync()).ToList();
                        }
                        else
                        {
                            counterfeitPathsDTOs = (await _counterfeitPathClient.GetAllByCounterfeitIdAsync(SelectedCounterfeit.Id)).ToList();

                            //counterfeitPaths = _counterfeitsContext.CounterfeitPaths
                            //    .Include(c => c.Counterfeit)
                            //    .Where(c => c.CounterfeitId == SelectedCounterfeit.Id).ToList();
                        }

                        var counterfeitPaths = counterfeitPathsDTOs.Adapt<List<CounterfeitPath>>();

                        //AnalysisResult = _analyzer.RunAnalysis(DisplayedImagePath, counterfeitPaths, PercentOfSimilarity, _userService.User);                       

                        SearchResult = CreateSearchResult(AnalysisResult);

                        if (AnalysisResult.ResPath.Path is not null)
                        {
                            string pathToBase = Directory.GetCurrentDirectory();
                            string pathToResults = @"..\..\..\resources\resImages\";
                            string combinedPath = Path.Combine(pathToBase, AnalysisResult.ResPath.Path);
                            ResultImagePath = combinedPath;
                        }

                        var analysisResultDTO = AnalysisResult.Adapt<CreateResultDTO>();

                        var res = await _resultClient.ResultPostAsync(analysisResultDTO);

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
                        //FileSystem.ExportPdf(filePath, AnalysisResult, _userService.User);
                    }
                }
            });
        }
    }



    #endregion
}
