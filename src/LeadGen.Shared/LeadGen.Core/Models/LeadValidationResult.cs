using LeadGen.Core.Exceptions;

namespace LeadGen.Core.Models
{
    public record LeadValidationResult(bool IsValidated, ValidatedLead? ValidatedLead = null, string? ErrorMessage = null)
    {
        public static LeadValidationResult CreateFailedResult(string errorMessage) => new(false, null, errorMessage);
        public static LeadValidationResult CreateFailedResult(InvalidLeadPropertyException<string> exception) => new(false, null, exception.Message);
        public static LeadValidationResult CreateSuccessResult(ValidatedLead validatedLead) => new(true, validatedLead, null);
    }
}
