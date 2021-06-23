using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapr;
using LeadGen.Core.Endpoints;
using LeadGen.Core.Events.Core;
using LeadGen.Core.Events.Leads;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace LeadGen.Processor.SubscriptionHandlers
{
    public class LeadValidatedHandler : SubscriptionHandler<LeadValidated>
    {
        private readonly ILogger<LeadValidatedHandler> _logger;

        public LeadValidatedHandler(ILogger<LeadValidatedHandler> logger)
        {
            _logger = logger;
        }

        [Route($"/{nameof(LeadValidated)}")]
        [Topic("leads", nameof(LeadValidated))]
        public async override Task<IActionResult> HandleAsync(LeadValidated eventData)
        {
            await Task.Delay(1);
            _logger.LogInformation("Hit handler {handler}", nameof(LeadValidatedHandler));
            return Ok();
        }
    }
}
