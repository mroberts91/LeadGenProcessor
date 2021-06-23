using LeadGen.Core.Attributes;
using LeadGen.Core.Events.Core;
using LeadGen.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeadGen.Core.Events.Leads
{
    [PubSub("leads")]
    public class LeadValidated : EventBase
    {
        public ValidatedLead? Lead { get; init; }

    }
}
