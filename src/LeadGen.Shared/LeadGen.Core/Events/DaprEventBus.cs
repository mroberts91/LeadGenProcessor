using Dapr.Client;
using LeadGen.Core.Events.Core;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LeadGen.Core.Events
{
    internal class DaprEventBus : IEventBus
    {
        private readonly DaprClient _daprClient;
        private readonly ILogger<DaprEventBus> _logger;

        public DaprEventBus(DaprClient daprClient, ILogger<DaprEventBus> logger)
        {
            _daprClient = daprClient ?? throw new ArgumentNullException(nameof(daprClient));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task PublishAsync<T>(T eventData, CancellationToken cancellationToken = default) where T : EventBase
        {
            if (eventData.Subscription is EventSubscription subscription)
            {
                await _daprClient.PublishEventAsync<object>(subscription.Name, subscription.Topic, eventData, cancellationToken);
                return;
            }

            _logger.LogError("Unable to publish to topic for event {evt} due to missing or invalid subscription information", typeof(T).Name);
        }
    }
}
