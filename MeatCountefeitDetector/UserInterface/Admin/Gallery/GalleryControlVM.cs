using DataAccess.Data;
using DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using MeatCountefeitDetector.UserInterface.Admin.Abstract;
using MeatCountefeitDetector.Utils;
using MeatCountefeitDetector.Utils.Dialog;
using MeatCountefeitDetector.Utils.MessageBoxService;


namespace MeatCountefeitDetector.UserInterface.Admin.Gallery;

public class GalleryControlVM : ViewModelBase
{
    #region Functions

    #region Constructors

    public GalleryControlVM(CounterfeitKBContext context, DialogService ds, IMessageBoxService messageBoxService)
    {
        _messageBoxService = messageBoxService;
        _context = context;
        _context.Counterfeits.Load();
        _ds = ds;
    }

    #endregion

    #endregion


    #region Properties

    private readonly DialogService _ds;
    private readonly CounterfeitKBContext _context;
    private readonly IMessageBoxService _messageBoxService;
    public CounterfeitPath SelectedCounterfeitPath { get; set; }
    public List<CounterfeitPath> CounterfeitPaths => _context.CounterfeitPaths.ToList();
    public List<DataAccess.Models.Counterfeit> Counterfeits => _context.Counterfeits.ToList();

    #endregion


    #region Commands

    private RelayCommand _addCounterfeitPath;

    /// <summary>
    ///     Команда, открывающая окно добавления нового изображения фальсификата
    /// </summary>
    public RelayCommand AddCounterfeitPath
    {
        get
        {
            return _addCounterfeitPath ??= new RelayCommand(o =>
            {
                _ds.ShowDialog<GalleryEditControl>(new WindowParameters
                {
                    Height = 550,
                    Width = 350,
                    Title = "Добавление изображения фальсификата",
                },
                data: new CounterfeitPath());
                OnPropertyChanged(nameof(CounterfeitPaths));
            });
        }
    }

    private RelayCommand _editCounterfeitPath;

    /// <summary>
    ///     Команда, открывающая окно редактирования нового изображения фальсификата
    /// </summary>
    public RelayCommand EditCounterfeitPath
    {
        get
        {
            return _editCounterfeitPath ??= new RelayCommand(o =>
            {
                _ds.ShowDialog<GalleryEditControl>(new WindowParameters
                {
                    Height = 550,
                    Width = 350,
                    Title = "Редактирование изображения фальсификата",
                },
                data: SelectedCounterfeitPath);
                OnPropertyChanged(nameof(CounterfeitPaths));
            }, _ => SelectedCounterfeitPath is not null);
        }
    }

    private RelayCommand _deleteCounterfeitPath;

    /// <summary>
    ///     Команда, удаляющая изображение фальсификата
    /// </summary>
    public RelayCommand DeleteCounterfeitPath
    {
        get
        {
            return _deleteCounterfeitPath ??= new RelayCommand(o =>
            {
                if (_messageBoxService.ShowMessage("Вы действительно хотите удалить изображение?", "Удаление изображения", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    string pathToBase = Directory.GetCurrentDirectory();
                    string combinedPath = Path.Combine(pathToBase, SelectedCounterfeitPath.ImagePath);
                    File.Delete(combinedPath);
                    _context.CounterfeitPaths.Remove(SelectedCounterfeitPath);
                    _context.SaveChanges();
                }
                OnPropertyChanged(nameof(CounterfeitPaths));
            }, c => SelectedCounterfeitPath is not null);
        }
    }

    #endregion
}

