using DataAccess.Interfaces;
using System.Collections.ObjectModel;

namespace DataAccess.Models
{
    public partial class OriginalPath : IBaseEntity
    {
        public Guid Id { get; set; }
        public string Path { get; set; } = null!;

        public virtual List<ResultPath> ResultPaths { get; set; }
        public virtual List<Result> Results { get; set; }
    }
}