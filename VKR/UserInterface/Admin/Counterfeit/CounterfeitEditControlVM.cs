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

public class CounterfeitEditControlVM : ViewModelBase, IDataHolder, IResultHolder, IInteractionAware
{
    #region Functions

    #region Constructors

    public CounterfeitEditControlVM(CounterfeitKBContext context)
    {
        _context = context;
    }

    #endregion

    #endregion


    #region Properties

    private Counterfeit _tempCounterfeit;

    public Counterfeit TempCounterfeit
    {
        get
        {
            return _tempCounterfeit;
        }
        set
        {
            _tempCounterfeit = value;
            OnPropertyChanged();
        }
    }

    public Counterfeit EditingCounterfeit => (Counterfeit)Data;

    private readonly CounterfeitKBContext _context;

    public List<CounterfeitPath> CounterfeitPaths
    {
        get => _context.CounterfeitPaths.ToList();
    }

    public List<Counterfeit> Counterfeits
    {
        get => _context.Counterfeits.ToList();
    }

    #endregion


    #region Commands

    private RelayCommand _saveCounterfeit;

    /// <summary>
    ///     Команда сохраняющая изменение данных о фальсификате в базе данных
    /// </summary>
    public RelayCommand SaveCounterfeit
    {
        get
        {
            return _saveCounterfeit ??= new RelayCommand(o =>
            {
                EditingCounterfeit.Id = TempCounterfeit.Id;
                EditingCounterfeit.Name = TempCounterfeit.Name;

                if (!_context.Counterfeits.Contains(EditingCounterfeit))
                {
                    _context.Counterfeits.Add(EditingCounterfeit);
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
            TempCounterfeit = new Counterfeit()
            {
                Id = EditingCounterfeit.Id,
                Name = EditingCounterfeit.Name,
            };
            OnPropertyChanged(nameof(TempCounterfeit));
        }
    }

    public object? Result { get; }
    public Action FinishInteraction { get; set; }
}
