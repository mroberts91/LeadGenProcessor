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
            (byte[] currentState, string eTag) = await _daprClient.GetStateAndETagAsync<byte[]>(_storeOptions.TokenStore, validatedLead.ValidationTokenKey);
            currentState = validatedLead.ValidationToken;
            var success = await _daprClient.TrySaveStateAsync(_storeOptions.TokenStore, validatedLead.ValidationTokenKey, currentState, eTag);
            Console.WriteLine();
        }

        public async Task<byte[]> GetValidateLeadTokenAsync(ValidatedLead validatedLead)
        {
            var token = await _daprClient.GetStateAsync<byte[]>(_storeOptions.TokenStore, validatedLead.ValidationTokenKey, ConsistencyMode.Strong);
            return token;
        }
    }
}
