using System.Collections.ObjectModel;
using System.Linq;

using DataAccess.Data;
using DataAccess.Models;

using VKR.Utils;


namespace VKR.ViewModel
{
    internal class ShapePropertyEditWindowVM : ViewModelBase

    {
        #region Functions

        #region Constructors

        public ShapePropertyEditWindowVM(Shape tempShape)
        {
            TempShape = new Shape
            {
                Id = tempShape.Id,
                Name = tempShape.Name,
                Formula = tempShape.Formula
            };

            EditingShape = tempShape;
            Db = new CounterfeitKBContext();
            Shapes = Db.Shapes.Local.ToObservableCollection();
        }

        #endregion

        #endregion


        #region Properties

        public ObservableCollection<Shape> Shapes { get; set; }
        public Shape TempShape { get; set; }
        public Shape EditingShape { get; set; }

        private CounterfeitKBContext Db { get; }

        #endregion


        #region Commands

        private RelayCommand _saveShape;

        /// <summary>
        ///     Команда сохраняющая изменение данных о форме в базе данных
        /// </summary>
        public RelayCommand SaveShape
        {
            get
            {
                return _saveShape ??= new RelayCommand(o =>
                {
                    EditingShape.Id = TempShape.Id;
                    EditingShape.Name = TempShape.Name;
                    EditingShape.Formula = TempShape.Formula;

                    if (!Db.Shapes.Contains(EditingShape))
                    {
                        Db.Shapes.Add(EditingShape);
                    }

                    Db.SaveChanges();
                    OnClosingRequest();
                });
            }
        }

        #endregion
    }
}
