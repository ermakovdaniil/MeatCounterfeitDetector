using System.Collections.Generic;
using System.Linq;
using System.Windows;

using DataAccess.Data;
using DataAccess.Models;

using Microsoft.EntityFrameworkCore;

using VKR.UserInterface.Admin.Abstract;
using VKR.Utils;
using VKR.Utils.Dialog;

using MessageBox = HandyControl.Controls.MessageBox;


namespace VKR.UserInterface.Admin.Gallery;

public class GalleryControlVM : ViewModelBase
{
    #region Functions

    #region Constructors

    public GalleryControlVM(CounterfeitKBContext context, DialogService ds)
    {
        _context = context;
        _context.Counterfeits.Load();
        _ds = ds;
    }

    #endregion

    #endregion


    #region Properties

    private readonly DialogService _ds;

    private readonly CounterfeitKBContext _context;
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
                    Height = 280,
                    Width = 500,
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
                    Height = 280,
                    Width = 500,
                    Title = "Редактирование изображения фальсификата",
                },
                data: SelectedCounterfeitPath);
                OnPropertyChanged(nameof(CounterfeitPaths));
            }, _ => SelectedCounterfeitPath != null);
        }
    }

    private RelayCommand _deleteCounterfeitPath;

    /// <summary>
    ///     Команда, удаляющая изображение фальсификата
    /// </summary>
    public RelayCommand DeleteounterfeitPath
    {
        get
        {
            return _deleteCounterfeitPath ??= new RelayCommand(o =>
            {
                if (MessageBox.Show("Вы действительно хотите удалить изображение?",
                                    "Удаление изображения", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    _context.CounterfeitPaths.Remove(SelectedCounterfeitPath);
                    _context.SaveChanges();
                }

                OnPropertyChanged(nameof(CounterfeitPaths));
            }, c => SelectedCounterfeitPath != null);
        }
    }

    #endregion
}

