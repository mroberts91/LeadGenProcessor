using System.Threading.Tasks;

namespace LeadGen.Connectors
{
    public interface IValidationConnector
    {
        Task<ValidationConnector.LeadValidationResult> ValidateLeadAsync(string? firstName, string? lastName, string? email, string? phone);
    }
}