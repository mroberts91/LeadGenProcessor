using LeadGen.Core.Models;
using System.Collections.Generic;

namespace LeadGen.Processor.Queues
{
    public class LeadProcessingQueue : ILeadQueue
    {
        private readonly Queue<ValidatedLead> _queue = new();

        public void AddLeadToQueue(ValidatedLead lead)
        {
            _queue.Enqueue(lead);
        }

        public ValidatedLead? GetNextLead()
        {
            return _queue.TryDequeue(out var nextLead) ? nextLead : null;
        }

    }
}
