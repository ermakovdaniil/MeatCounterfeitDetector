using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Windows;

using DataAccess.Data;
using DataAccess.Models;

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
        Companies = _context.Companies.Local.ToObservableCollection();
        _ds = ds;
    }

    #endregion

    #endregion


    #region Properties

    private DialogService _ds;

    private readonly ResultDBContext _context;
    public Company SelectedCompany { get; set; }
    public ObservableCollection<Company> Companies { get; set; }

    #endregion


    #region Commands

    private RelayCommand _addCompany;

    /// <summary>
    ///     Команда, открывающая окно создания предприятия
    /// </summary>
    public RelayCommand Add
    {
        get
        {
            return _addCompany ??= new RelayCommand(o =>
            {
                _ds.ShowDialog<CompanyControl>(
                windowParameters: new WindowParameters
                {
                    Height = 100,
                    Width = 300,
                    Title = "Добавление предприятия"
                });
            });
        }
    }

    private RelayCommand _editCompany;

    /// <summary>
    ///     Команда, открывающая окно редактирования предприятия
    /// </summary>
    public RelayCommand Edit
    {
        get
        {
            return _editCompany ??= new RelayCommand(o =>
            {
                _ds.ShowDialog<CompanyControl>(
                windowParameters: new WindowParameters
                {
                    Height = 100,
                    Width = 300,
                    Title = "Добавление предприятия"
                },
                data: new Company()
                {
                    Name = SelectedCompany.Name,
                });
            }, _ => SelectedCompany != null);
        }
    }

    private RelayCommand _deleteCompany;

    /// <summary>
    ///     Команда, удаляющая цвет
    /// </summary>
    public RelayCommand Delete
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
            }, _ => SelectedCompany != null);
        }
    }

    #endregion
}
