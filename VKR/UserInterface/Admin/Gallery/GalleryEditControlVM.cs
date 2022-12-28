using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

using DataAccess.Data;
using DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using VKR.Utils;
using VKR.Utils.Dialog.Abstract;


namespace VKR.ViewModel;

public class GalleryEditControlVM : ViewModelBase, IDataHolder, IResultHolder, IInteractionAware
{
    #region Functions

    #region Constructors

    public GalleryEditControlVM(CounterfeitKBContext context)
    {
        _context = context;
        Counterfeits = new ObservableCollection<Counterfeit>(_context.Counterfeits.ToList());
    }

    #endregion

    #endregion


    #region Properties

    private CounterfeitPath _tempCounterfeitPath;

    public CounterfeitPath TempCounterfeitPath
    {
        get
        {
            return _tempCounterfeitPath;
        }
        set
        {
            _tempCounterfeitPath = value;
            OnPropertyChanged();
        }
    }

    public CounterfeitPath EditingCounterfeitPath => (CounterfeitPath)Data;

    private readonly CounterfeitKBContext _context;

    public ObservableCollection<Counterfeit> Counterfeits { get; set; }

    #endregion


    #region Commands

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
                EditingCounterfeitPath.Id = TempCounterfeitPath.Id;
                EditingCounterfeitPath.Counterfeit = TempCounterfeitPath.Counterfeit;
                EditingCounterfeitPath.ImagePath = TempCounterfeitPath.ImagePath;

                if (!_context.CounterfeitPaths.Contains(EditingCounterfeitPath))
                {
                    _context.CounterfeitPaths.Add(EditingCounterfeitPath);
                }

                _context.SaveChanges();
                FinishInteraction();
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

    private object _data;
    public object Data
    {
        get => _data;
        set
        {
            _data = value;
            TempCounterfeitPath = new CounterfeitPath()
            {
                Id = EditingCounterfeitPath.Id,
                Counterfeit = EditingCounterfeitPath.Counterfeit,
                ImagePath = EditingCounterfeitPath.ImagePath,
            };
            OnPropertyChanged(nameof(TempCounterfeitPath));
        }
    }

    public object? Result { get; }
    public Action FinishInteraction { get; set; }
}
