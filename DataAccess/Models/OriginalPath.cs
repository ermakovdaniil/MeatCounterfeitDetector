using DataAccess.Interfaces;
using System.Collections.ObjectModel;

namespace DataAccess.Models
{
    public partial class OriginalPath : IBaseEntity
    {
        public OriginalPath()
        {
            ResultPaths = new ObservableCollection<ResultPath>();
            Results = new ObservableCollection<Result>();
        }

        public Guid Id { get; set; }
        public string Path { get; set; } = null!;

        public virtual ObservableCollection<ResultPath> ResultPaths { get; set; }
        public virtual ObservableCollection<Result> Results { get; set; }
    }
}
