using LeadGen.Core.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace LeadGen.Core.Store
{
    public interface IStateStore
    {
        Task<byte[]> GetValidateLeadTokenAsync(ValidatedLead validatedLead);
        Task SaveValidatedLeadTokenAsync(ValidatedLead validatedLead);
    }
}
