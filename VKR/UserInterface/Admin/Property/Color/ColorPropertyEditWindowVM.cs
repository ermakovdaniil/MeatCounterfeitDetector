using System;
using System.Collections.ObjectModel;
using System.Linq;

using DataAccess.Data;
using DataAccess.Models;

using VKR.Utils;
using VKR.Utils.Dialog.Abstract;


namespace VKR.ViewModel;

internal class ColorPropertyEditWindowVM : ViewModelBase, IDataHolder,IResultHolder,IInteractionAware
{
    #region Functions

    #region Constructors

    public ColorPropertyEditWindowVM()
    {
        Db = new CounterfeitKBContext();
        //Colors = Db.Colors.Local.ToObservableCollection();
    }

    #endregion

    #endregion


    #region Properties

    //public ObservableCollection<Color> Colors { get; set; }
    private Color _tempColor;

    public Color TempColor
    {
        get
        {
            return _tempColor;
        }
        set
        {
            _tempColor = value;
            OnPropertyChanged();
        }
    }
    // public Color TempColor { get; set; }

    public Color EditingColor => (Color) Data;

    private readonly CounterfeitKBContext Db;

    #endregion


    #region Commands

    private RelayCommand _saveColor;

    /// <summary>
    ///     Команда сохраняющая изменение данных о цвете в базе данных
    /// </summary>
    public RelayCommand SaveColor
    {
        get
        {
            return _saveColor ??= new RelayCommand(o =>
            {
                EditingColor.Id = TempColor.Id;
                EditingColor.Name = TempColor.Name;
                EditingColor.BotLine = TempColor.BotLine;
                EditingColor.UpLine = TempColor.UpLine;

                if (!Db.Colors.Contains(EditingColor))
                {
                    Db.Colors.Add(EditingColor);
                }

                Db.SaveChanges();
                FinishInteraction();
                //OnClosingRequest();
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
                TempColor = new Color()
                {
                    BotLine = EditingColor.BotLine,
                    Counterfeits = EditingColor.Counterfeits,
                    Name = EditingColor.Name,
                    UpLine = EditingColor.UpLine,
                };
                OnPropertyChanged(nameof(TempColor));
            }
        }

        public object? Result { get; }
        public Action FinishInteraction { get; set; }

        
}
