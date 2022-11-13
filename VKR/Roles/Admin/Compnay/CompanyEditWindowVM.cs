using System.Collections.ObjectModel;
using System.Linq;

using DataAccess.Data;
using DataAccess.Models;

using VKR.Utils;


namespace VKR.ViewModel
{
    internal class CompanyEditWindowVM : ViewModelBase

    {
        #region Functions

        #region Constructors

        public CompanyEditWindowVM(Company tempCompany)
        {
            TempCompany = new Company
            {
                Id = tempCompany.Id,
                Name = tempCompany.Name,
            };

            EditingCompany = tempCompany;
            Db = new ResultDBContext();
            Companies = Db.Companies.Local.ToObservableCollection();
        }

        #endregion

        #endregion


        #region Properties

        public ObservableCollection<Company> Companies { get; set; }
        public Company TempCompany { get; set; }
        public Company EditingCompany { get; set; }

        private ResultDBContext Db { get; }

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
                    
                    if (!Db.Companies.Contains(EditingCompany))
                    {
                        Db.Companies.Add(EditingCompany);
                    }

                    Db.SaveChanges();
                    //OnClosingRequest();
                });
            }
        }

        #endregion
    }
}
