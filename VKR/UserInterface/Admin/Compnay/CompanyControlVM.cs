using System.Linq;
using System.Windows;
using System.Collections.Generic;

using DataAccess.Data;
using DataAccess.Models;

using Microsoft.EntityFrameworkCore;

using VKR.Utils;
using VKR.Utils.Dialog;
using VKR.View;

using MessageBox = HandyControl.Controls.MessageBox;


namespace VKR.ViewModel;

public class CompanyControlVM : ViewModelBase
{
    #region Functions

    #region Constructors

    public CompanyControlVM(ResultDBContext context, DialogService ds)
    {
        _context = context;
        _ds = ds;
    }

    #endregion

    #endregion


    #region Properties

    private DialogService _ds;

    private readonly ResultDBContext _context;
    public Company SelectedCompany { get; set; }
    public List<Company> Companies
    {
        get => _context.Companies.ToList();
    }

    #endregion


    #region Commands

    private RelayCommand _addCompany;

    /// <summary>
    ///     Команда, открывающая окно создания предприятия
    /// </summary>
    public RelayCommand AddCompany
    {
        get
        {
            return _addCompany ??= new RelayCommand(o =>
            {
                _ds.ShowDialog<CompanyEditControl>(
                windowParameters: new WindowParameters
                {
                    Height = 180,
                    Width = 300,
                    Title = "Добавление предприятия"
                },
                data: new Company()
                {

                });
                OnPropertyChanged(nameof(Companies));
            });
        }
    }

    private RelayCommand _editCompany;

    /// <summary>
    ///     Команда, открывающая окно редактирования предприятия
    /// </summary>
    public RelayCommand EditCompany
    {
        get
        {
            return _editCompany ??= new RelayCommand(o =>
            {
                _ds.ShowDialog<CompanyEditControl>(
                windowParameters: new WindowParameters
                {
                    Height = 180,
                    Width = 300,
                    Title = "Добавление предприятия"
                },
                data: SelectedCompany);
                OnPropertyChanged(nameof(Companies));
            }, _ => SelectedCompany != null);
        }
    }

    private RelayCommand _deleteCompany;

    /// <summary>
    ///     Команда, удаляющая цвет
    /// </summary>
    public RelayCommand DeleteCompany
    {
        get
        {
            return _deleteCompany ??= new RelayCommand(o =>
            {
                if (HandyControl.Controls.MessageBox.Show($"Вы действительно хотите удалить предприятие: \"{SelectedCompany.Name}\" и все записи связанные с ним?",
                                    "Удаление предприятия", MessageBoxButton.YesNo, MessageBoxImage.Warning) ==
                    MessageBoxResult.Yes)
                {
                    _context.Companies.Remove(SelectedCompany);
                    _context.SaveChanges();
                }
                OnPropertyChanged(nameof(Companies));
            }, _ => SelectedCompany != null);
        }
    }

    #endregion
}
