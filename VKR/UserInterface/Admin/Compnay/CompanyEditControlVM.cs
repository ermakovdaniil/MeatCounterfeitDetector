using System;
using System.Linq;
using System.Collections.ObjectModel;

using DataAccess.Data;
using DataAccess.Models;

using VKR.Utils;
using VKR.Utils.Dialog.Abstract;

namespace VKR.ViewModel;

public class CompanyEditControlVM : ViewModelBase, IDataHolder, IResultHolder, IInteractionAware
{
    #region Functions

    #region Constructors

    public CompanyEditControlVM(ResultDBContext context)
    {
        _context = context;
        Companies = new ObservableCollection<Company>(_context.Companies.ToList());
    }

    #endregion

    #endregion


    #region Properties

    private Company _tempCompany;

    public Company TempCompany
    {
        get
        {
            return _tempCompany;
        }
        set
        {
            _tempCompany = value;
            OnPropertyChanged();
        }
    }

    public Company EditingCompany => (Company)Data;

    private readonly ResultDBContext _context;

    public ObservableCollection<Company> Companies { get; set; }

    #endregion


    #region Commands

    private RelayCommand _saveCompany;

    /// <summary>
    ///     Команда сохраняющая изменение данных о предприятии в базе данных
    /// </summary>
    public RelayCommand SaveCompany
    {
        get
        {
            return _saveCompany ??= new RelayCommand(o =>
            {
                EditingCompany.Id = TempCompany.Id;
                EditingCompany.Name = TempCompany.Name;

                if (!_context.Companies.Contains(EditingCompany))
                {
                    _context.Companies.Add(EditingCompany);
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
            TempCompany = new Company()
            {
                Id = EditingCompany.Id,
                Name = EditingCompany.Name,
            };
            OnPropertyChanged(nameof(TempCompany));
        }
    }

    public object? Result { get; }
    public Action FinishInteraction { get; set; }
}