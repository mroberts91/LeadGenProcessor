using Either;
using LeadGen.Core.Exceptions;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace LeadGen.Core.Models
{
    public record ValidatedLead(Lead LeadData, byte[] ValidationToken, DateTime ValidatedUtc)
    {
        public bool VerifyValidationToken(byte[] hash) => hash.SequenceEqual(ValidationToken);

        public string ValidationTokenKey => LeadData.Id.ToString();


        public static Either<ValidatedLead, ArgumentNullException, InvalidLeadPropertyException<string>> Create(Lead leadData)
        {
            var createdUtc = DateTime.UtcNow;

            byte[] CreateHash() => SHA512.HashData(Encoding.UTF8.GetBytes($"{leadData.GetHashCode()}{createdUtc.GetHashCode()}"));

            return leadData switch
            {
                null                                    => new ArgumentNullException(nameof(leadData), "Unable to validate lead because it is null."),
                Lead l when l.FirstName.HasNoValue()    => new InvalidLeadPropertyException<string>(nameof(leadData.FirstName), leadData.FirstName),
                Lead l when l.LastName.HasNoValue()     => new InvalidLeadPropertyException<string>(nameof(leadData.LastName), leadData.LastName),
                Lead l when l.Email.HasNoValue()        => new InvalidLeadPropertyException<string>(nameof(leadData.Email), leadData.Email),
                Lead l when l.Phone.HasNoValue()        => new InvalidLeadPropertyException<string>(nameof(leadData.Phone), leadData.Phone),
                _                                       => new ValidatedLead(leadData, CreateHash(), createdUtc)
            };
        }
    }

    public static class Extensions
    {
        public static bool HasValue(this string? s) => !string.IsNullOrWhiteSpace(s);
        public static bool HasNoValue(this string? s) => string.IsNullOrWhiteSpace(s);
    }
}
