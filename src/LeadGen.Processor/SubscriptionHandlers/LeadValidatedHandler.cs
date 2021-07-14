using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapr;
using Dapr.Client;
using LeadGen.Core.Endpoints;
using LeadGen.Core.Events.Core;
using LeadGen.Core.Events.Leads;
using LeadGen.Core.Models;
using LeadGen.Core.Store;
using LeadGen.Processor.Queues;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace LeadGen.Processor.SubscriptionHandlers
{
    public class LeadValidatedHandler : SubscriptionHandler<LeadValidated>
    {
        private readonly ILogger<LeadValidatedHandler> _logger;
        private readonly ILeadQueue _leadQueue;
        private readonly DaprClient _dapr;

        public LeadValidatedHandler(ILogger<LeadValidatedHandler> logger, DaprClient dapr, ILeadQueue leadQueue)
        {
            _logger = logger;
            _dapr = dapr;
            _leadQueue = leadQueue;
        }

        [Route($"/{nameof(LeadValidated)}")]
        [Topic("leads", nameof(LeadValidated))]
        public async override Task<IActionResult> HandleAsync(LeadValidated eventData)
        {
            await Task.CompletedTask;

            ValidatedLead? lead = eventData.Lead;
            if (lead is null)
                return Ok();

            _leadQueue.AddLeadToQueue(lead);

            return Ok();
        }
    }
}
