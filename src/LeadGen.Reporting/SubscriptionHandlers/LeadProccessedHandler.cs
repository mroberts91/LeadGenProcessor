using Dapr;
using LeadGen.Core.Endpoints;
using LeadGen.Core.Events.Leads;
using LeadGen.Reporting.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace LeadGen.Reporting.SubscriptionHandlers
{
    public class LeadProccessedHandler : SubscriptionHandler<LeadProccessed>
    {
        private readonly IEventCache _cache;

        public LeadProccessedHandler(IEventCache cache)
        {
            _cache = cache;
        }

        [Route($"/{nameof(LeadProccessed)}")]
        [Topic("leads", nameof(LeadProccessed))]
        public async override Task<IActionResult> HandleAsync(LeadProccessed eventData)
        {
            await Task.CompletedTask;
            _cache.AddEventToCache(eventData);
            return Ok();
        }
    }
}
