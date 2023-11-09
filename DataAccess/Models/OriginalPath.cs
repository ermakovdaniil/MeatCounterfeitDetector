using DataAccess.Interfaces;
using System.Collections.ObjectModel;

namespace DataAccess.Models
{
    public partial class OriginalImage : IBaseEntity
    {
        public Guid Id { get; set; }
        public string EncodedImage { get; set; } = null!;

        public virtual List<ResultImage> ResultImages { get; set; }
        public virtual List<Result> Results { get; set; }
    }
}