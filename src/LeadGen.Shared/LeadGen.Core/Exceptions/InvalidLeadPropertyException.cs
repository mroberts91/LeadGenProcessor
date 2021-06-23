using LeadGen.Core.Models;
using System;

namespace LeadGen.Core.Exceptions
{
    public class InvalidLeadPropertyException<TProperty> : ApplicationException
    {
        public string? PropertyName { get; set; }

        public InvalidLeadPropertyException(string? propertyName, TProperty? value)
            : base($"Unable to validate {nameof(Lead)} because property {propertyName} was invalid with value of: ({(value?.ToString() ?? "null")})")
        {
            PropertyName = propertyName;
        }
    }
}
