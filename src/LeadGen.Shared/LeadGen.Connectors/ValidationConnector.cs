using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapr.Client;
using LeadGen.Core.Models;

namespace LeadGen.Connectors
{
    public class ValidationConnector : IValidationConnector
    {
        private readonly DaprClient _daprClient;
        private readonly ILogger<ValidationConnector> _logger;

        public ValidationConnector(DaprClient daprClient, ILogger<ValidationConnector> logger)
        {
            _daprClient = daprClient;
            _logger = logger;
        }

        public async Task<LeadValidationResult> ValidateLeadAsync(string? firstName, string? lastName, string? email, string? phone)
        {
            try
            {
                Lead lead = Lead.Create(firstName, lastName, email, phone);
                LeadValidationResult result = await _daprClient.InvokeMethodAsync<Lead, LeadValidationResult>("leadgen-validator", "Validate", lead);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to validate lead, {msg}", ex.Message);
                throw;
            }

        }
        public record LeadValidationResult(bool IsValidated, ValidatedLead? ValidatedLead = null, string? ErrorMessage = null);
    }

}
