using LeadGen.Core.Events.Core;
using LeadGen.Core.Events.Leads;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeadGen.Core.Endpoints
{
    [ApiController]
    public abstract class SubscriptionHandler<TEvent> : ControllerBase  where TEvent : EventBase
    {
        public abstract Task<IActionResult> HandleAsync(TEvent eventData);
    }
}
