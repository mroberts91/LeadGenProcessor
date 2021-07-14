﻿using LeadGen.Core.Attributes;
using LeadGen.Core.Events.Core;
using LeadGen.Core.Models;

namespace LeadGen.Core.Events.Leads
{
    [PubSub("leads")]
    public class LeadValidated : EventBase
    {
        public ValidatedLead? Lead { get; init; }

    }
}
