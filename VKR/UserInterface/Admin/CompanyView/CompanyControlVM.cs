using System.Collections.Generic;
using System.Linq;
using System.Windows;

using DataAccess.Data;

using VKR.UserInterface.Admin.Abstract;
using VKR.Utils;
using VKR.Utils.Dialog;

using MessageBox = HandyControl.Controls.MessageBox;


namespace VKR.UserInterface.Admin.CompanyView;

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

    private readonly DialogService _ds;

    private readonly ResultDBContext _context;
    public DataAccess.Models.Company SelectedCompany { get; set; }

    public List<DataAccess.Models.Company> Companies => _context.Companies.ToList();

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
                _ds.ShowDialog<CompanyEditControl>(new WindowParameters
                                                   {
                                                       Height = 180,
                                                       Width = 500,
                                                       Title = "Добавление предприятия",
                                                   },
                                                   data: new DataAccess.Models.Company());

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
                _ds.ShowDialog<CompanyEditControl>(new WindowParameters
                                                   {
                                                       Height = 180,
                                                       Width = 500,
                                                       Title = "Добавление предприятия",
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
                if (MessageBox.Show($"Вы действительно хотите удалить предприятие: \"{SelectedCompany.Name}\" и все записи связанные с ним?",
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

