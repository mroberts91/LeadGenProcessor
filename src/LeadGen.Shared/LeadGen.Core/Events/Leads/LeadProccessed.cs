using LeadGen.Core.Attributes;
using LeadGen.Core.Events.Core;
using LeadGen.Core.Models;
using System;

namespace LeadGen.Core.Events.Leads
{
    [PubSub("leads")]
    public class LeadProccessed : EventBase
    {
        public ValidatedLead? Lead { get; init; }
        public DateTime ProcessedUtc { get; init; }

    }
}
