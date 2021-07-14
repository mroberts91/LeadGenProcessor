using Dapr;
using LeadGen.Core.Endpoints;
using LeadGen.Core.Events.Leads;
using LeadGen.Reporting.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LeadGen.Reporting.SubscriptionHandlers
{
    public class LeadValidatedHandler : SubscriptionHandler<LeadValidated>
    {
        private readonly IEventCache _cache;

        public LeadValidatedHandler(IEventCache cache)
        {
            _cache = cache;
        }

        [Route($"/{nameof(LeadValidated)}")]
        [Topic("leads", nameof(LeadValidated))]
        public async override Task<IActionResult> HandleAsync(LeadValidated eventData)
        {
            await Task.CompletedTask;
            _cache.AddEventToCache(eventData);
            return Ok();
        }
    }
}
