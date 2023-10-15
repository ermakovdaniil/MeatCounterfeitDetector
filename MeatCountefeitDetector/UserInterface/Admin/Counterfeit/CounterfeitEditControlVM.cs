﻿using DataAccess.Data;
using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using MeatCounterfeitDetector.UserInterface.Admin.Abstract;
using MeatCounterfeitDetector.Utils;
using MeatCounterfeitDetector.Utils.Dialog.Abstract;


namespace MeatCounterfeitDetector.UserInterface.Admin.Counterfeit;

public class CounterfeitEditControlVM : ViewModelBase, IDataHolder, IResultHolder, IInteractionAware
{
    private object _data;


    #region Functions

    #region Constructors

    public CounterfeitEditControlVM(CounterfeitKBContext context)
    {
        _context = context;
    }

    #endregion

    #endregion


    public object Data
    {
        get => _data;
        set
        {
            _data = value;

            TempCounterfeit = new DataAccess.Models.Counterfeit
            {
                Id = EditingCounterfeit.Id,
                Name = EditingCounterfeit.Name,
            };

            OnPropertyChanged(nameof(TempCounterfeit));
        }
    }

    public Action FinishInteraction { get; set; }

    public object? Result { get; }


    #region Properties

    public DataAccess.Models.Counterfeit TempCounterfeit { get; set; }

    public DataAccess.Models.Counterfeit EditingCounterfeit => (DataAccess.Models.Counterfeit)Data;

    private readonly CounterfeitKBContext _context;

    public List<CounterfeitPath> CounterfeitPaths => _context.CounterfeitPaths.ToList();

    public List<DataAccess.Models.Counterfeit> Counterfeits => _context.Counterfeits.ToList();

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
}
