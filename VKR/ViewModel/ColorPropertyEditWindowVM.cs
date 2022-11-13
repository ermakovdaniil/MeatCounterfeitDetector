using System.Collections.ObjectModel;
using System.Linq;

using DataAccess.Data;
using DataAccess.Models;

using VKR.Utils;


namespace VKR.ViewModel
{
    internal class ColorPropertyEditWindowVM : ViewModelBase

    {
        #region Functions

        #region Constructors

        public ColorPropertyEditWindowVM(Color tempColor)
        {
            TempColor = new Color
            {
                Id = tempColor.Id,
                Name = tempColor.Name,
                BotLine = tempColor.BotLine,
                UpLine = tempColor.UpLine
            };

            EditingColor = tempColor;
            Db = new CounterfeitKBContext();
            Colors = Db.Colors.Local.ToObservableCollection();
        }

        #endregion

        #endregion

        #region Properties

        public ObservableCollection<Color> Colors { get; set; }
        public Color TempColor { get; set; }
        public Color EditingColor { get; set; }

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
                    OnClosingRequest();
                });
            }
        }

        #endregion
    }
}
