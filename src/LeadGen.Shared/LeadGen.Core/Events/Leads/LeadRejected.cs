using LeadGen.Core.Attributes;
using LeadGen.Core.Events.Core;
using LeadGen.Core.Models;
using System;

namespace LeadGen.Core.Events.Leads
{
    [PubSub("leads")]
    public class LeadRejected : EventBase
    {
        public LeadValidationResult? ValidationResult { get; set; }
        public DateTime? RejectedUtc { get; set; }
    }
}
