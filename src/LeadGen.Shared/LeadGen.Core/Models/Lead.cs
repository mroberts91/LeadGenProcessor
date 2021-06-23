using System;

namespace LeadGen.Core.Models
{
    public record Lead(Guid Id, string? FirstName, string? LastName, string? Email, string? Phone)
    {
        public static Lead Create(string? firstname = null, string? lastname = null, string? email = null, string? phone = null)
        {
            return new(Guid.NewGuid(), firstname, lastname, email, phone);
        }
    }
}
