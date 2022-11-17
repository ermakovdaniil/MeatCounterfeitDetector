using System.Collections.ObjectModel;
using System.Windows;

using Autofac;

using DataAccess.Data;
using DataAccess.Models;

using VKR.Utils;
using VKR.Utils.Dialog;
using VKR.View;
using MessageBox = HandyControl.Controls.MessageBox;

namespace VKR.ViewModel;

public class CounterfeitExplorerControlVM : ViewModelBase
{
    #region Functions

    #region Constructors

    public CounterfeitExplorerControlVM(CounterfeitKBContext context, DialogService ds)
    {
        _context = context;
        Counterfeits = _context.Counterfeits.Local.ToObservableCollection();
        _ds = ds;
    }

    #endregion

    #endregion


    #region Properties
    
    private DialogService _ds;

    private readonly CounterfeitKBContext _context;
    public Counterfeit SelectedCounterfeit { get; set; }
    public ObservableCollection<Counterfeit> Counterfeits { get; set; }

    #endregion


    #region Commands

    private RelayCommand _addCounterfeit;

    /// <summary>
    ///     Команда, открывающая окно создания нового фальсификата
    /// </summary>
    public RelayCommand AddCounterfeit
    {
        get
        {
            return _addCounterfeit ??= new RelayCommand(o =>
            {
                _ds.ShowDialog<CounterfeitExplorerControl>(
                windowParameters: new WindowParameters
                {
                    Height = 700,
                    Width = 300,
                    Title = "Добавление фальсификата"
                });
            });
        }
    }

    private RelayCommand _editCounterfeitObject;

    /// <summary>
    ///     Команда, открывающая окно редактирования нового фальсификата
    /// </summary>
    public RelayCommand EditCounterfeit
    {
        get
        {
            return _editCounterfeitObject ??= new RelayCommand(o =>
            {
                _ds.ShowDialog<CounterfeitExplorerControl>(
                windowParameters: new WindowParameters
                {
                    Height = 700,
                    Width = 300,
                    Title = "Добавление фальсификата"
                },
                data: new Counterfeit()
                {
                    Name = SelectedCounterfeit.Name,
                    ShapeId = SelectedCounterfeit.ShapeId,
                    BotLineSize = SelectedCounterfeit.BotLineSize,
                    UpLineSize = SelectedCounterfeit.UpLineSize,
                    ColorId = SelectedCounterfeit.ColorId,
                });
            }, _ => SelectedCounterfeit != null);
        }
    }

    private RelayCommand _deleteCounterfeit;

    /// <summary>
    ///     Команда, удаляющая фальсификат
    /// </summary>
    public RelayCommand DeleteCounterfeit
    {
        get
        {
            return _deleteCounterfeit ??= new RelayCommand(o =>
            {
                if (MessageBox.Show($"Вы действительно хотите удалить фальсификат: \"{SelectedCounterfeit.Name}\"?",
                                    "Удаление объекта", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    _context.Counterfeits.Remove(SelectedCounterfeit);
                    _context.SaveChanges();
                }
            }, c => SelectedCounterfeit != null);
        }
    }

    #endregion
}
