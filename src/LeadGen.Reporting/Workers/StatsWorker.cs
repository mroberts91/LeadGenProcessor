using LeadGen.Core.Events.Leads;
using LeadGen.Reporting.Models;
using LeadGen.Reporting.Services;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LeadGen.Reporting.Workers
{
    public class StatsWorker : BackgroundService
    {
        private readonly IEventCache _cache;
        private readonly ILogger<StatsWorker> _logger;

        public StatsWorker(IEventCache cache, ILogger<StatsWorker> logger)
        {
            _cache = cache;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(1000 * 120, stoppingToken);
                await CalculateStatsAsync();
            }
        }

        private async Task CalculateStatsAsync() 
        {
            await Task.CompletedTask;

            IEnumerable<LeadValidated>? validateds = _cache.GetCachedEvents<LeadValidated>();
            IEnumerable<LeadProccessed>? proccesseds = _cache.GetCachedEvents<LeadProccessed>();

            if (validateds is IEnumerable<LeadValidated> v && proccesseds is IEnumerable<LeadProccessed> p)
            {
                IEnumerable<(LeadValidated validated, LeadProccessed? proccessed)> sets = 
                    v.Select(s => (s, p.Where(l => l?.Lead?.LeadData?.Id == s?.Lead?.LeadData?.Id).FirstOrDefault()))
                    .Where(pair => pair.s is LeadValidated && pair.Item2 is LeadProccessed);

                IEnumerable<TimeSpan> timeSpans = sets.Select(s => s.proccessed?.ProcessedUtc - s.validated?.Lead?.ValidatedUtc).Where(t => t != null && t.HasValue).Select(t => t.Value);

                var avgProcessingMs = timeSpans.Select(t => t.TotalMilliseconds).Average();

                IEnumerable<(string, int)> sourceCounts = p.GroupBy(l => l.Lead?.LeadData?.source).Select(group =>  (group.Key,group.Count()));

                double? rejectionPercentage = default;
                IEnumerable<LeadRejected>? rejecteds = _cache.GetCachedEvents<LeadRejected>();
                if (rejecteds is IEnumerable<LeadRejected> r && r.Any() && p.Any())
                    rejectionPercentage = (r.Count() / (double)p.Count()) * 100.0;

                ReportingStats stats = new()
                {
                    UpdatedUtc = DateTime.UtcNow,
                    AverageTimeToProccessed = avgProcessingMs,
                    RequestsPerSource = sourceCounts ?? new List<(string source, int requests)>(),
                    RejectionPercentage = rejectionPercentage ?? 0
                };

                _cache.UpdateReportingStats(stats);
            }
        }
    }
}
