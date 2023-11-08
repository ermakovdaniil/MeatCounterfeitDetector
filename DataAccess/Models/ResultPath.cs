using DataAccess.Interfaces;
using System.Collections.ObjectModel;

namespace DataAccess.Models
{
    public partial class ResultPath : IBaseEntity
    {
        public Guid Id { get; set; }
        public Guid? InitId { get; set; }
        public string Path { get; set; } = null!;

        public virtual OriginalPath? Init { get; set; }
        public virtual List<Result> Results { get; set; }
    }
}