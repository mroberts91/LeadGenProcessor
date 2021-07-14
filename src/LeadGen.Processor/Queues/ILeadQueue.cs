using LeadGen.Core.Models;

namespace LeadGen.Processor.Queues
{
    public interface ILeadQueue
    {
        void AddLeadToQueue(ValidatedLead lead);
        ValidatedLead? GetNextLead();
    }
}