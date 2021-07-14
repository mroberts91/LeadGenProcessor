using Dapr;
using LeadGen.Core.Endpoints;
using LeadGen.Core.Events.Leads;
using LeadGen.Reporting.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace LeadGen.Reporting.SubscriptionHandlers
{
    public class LeadRejectedHandler : SubscriptionHandler<LeadRejected>
    {
        private readonly IEventCache _cache;

        public LeadRejectedHandler(IEventCache cache)
        {
            _cache = cache;
        }

        [Route($"/{nameof(LeadRejected)}")]
        [Topic("leads", nameof(LeadRejected))]
        public async override Task<IActionResult> HandleAsync(LeadRejected eventData)
        {
            await Task.CompletedTask;
            _cache.AddEventToCache(eventData);
            return Ok();
        }
    }
}
