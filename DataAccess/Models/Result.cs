using DataAccess.Interfaces;

namespace DataAccess.Models
{
    public partial class Result : IBaseEntity
    {
        public Guid Id { get; set; }
        public string Date { get; set; } = null!;
        public Guid UserId { get; set; }
        public string AnalysisResult { get; set; } = null!;
        public double Time { get; set; }
        public double PercentOfSimilarity { get; set; }
        public Guid OriginalImageId { get; set; }
        public Guid? ResultImageId { get; set; }

        public virtual OriginalImage OriginalImage { get; set; } = null!;
        public virtual ResultImage? ResultImage { get; set; }
        public virtual User User { get; set; } = null!;
    }
}
