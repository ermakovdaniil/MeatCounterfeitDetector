using System.Collections.ObjectModel;

namespace DataAccess.Models
{
    public partial class Result
    {
        public long Id { get; set; }
        public string Date { get; set; } = null!;
        public long CompanyId { get; set; }
        public string AnRes { get; set; } = null!;
        public long OrigPathId { get; set; }
        public long ResPathId { get; set; }

        public virtual Company Company { get; set; } = null!;
        public virtual OriginalPath OrigPath { get; set; } = null!;
        public virtual ResultPath ResPath { get; set; } = null!;
    }
}
