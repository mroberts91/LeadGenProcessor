using Dapr.Client;
using LeadGen.Core.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeadGen.Core.Store
{
    public class StateStore : IStateStore
    {
        private readonly DaprClient _daprClient;
        private readonly StateStoreOptions _storeOptions;

        public StateStore(DaprClient daprClient, IOptionsSnapshot<StateStoreOptions> storeOptions)
        {
            _storeOptions = storeOptions.Value;
            _daprClient = daprClient;
        }

        public async Task SaveValidatedLeadTokenAsync(ValidatedLead validatedLead)
        {
            await _daprClient.SaveStateAsync(_storeOptions.TokenStore, validatedLead.ValidationTokenKey, validatedLead.ValidationToken);
        }

        public async Task<byte[]?> GetValidateLeadTokenAsync(ValidatedLead validatedLead)
        {
            return await _daprClient.GetStateAsync<byte[]>(_storeOptions.TokenStore, validatedLead.ValidationTokenKey);
        }
    }
}
