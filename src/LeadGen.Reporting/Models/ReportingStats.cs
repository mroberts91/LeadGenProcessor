using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LeadGen.Reporting.Models
{
    public record ReportingStats
    {
        public DateTime? UpdatedUtc { get; init; }
        public double? AverageTimeToProccessed { get; init; }
        public IEnumerable<(string source, int requests)>? RequestsPerSource { get; init; }
        public double? RejectionPercentage { get; init; }

    }
}
