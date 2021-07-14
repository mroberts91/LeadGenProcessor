using LeadGen.Core.Events;
using LeadGen.Core.Events.Leads;
using LeadGen.Core.Models;
using LeadGen.Processor.Queues;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LeadGen.Processor.Workers
{
    public class LeadProcessorWorker : BackgroundService
    {

        private readonly ILeadQueue _leadQueue;
        private readonly IEventBus _eventBus;
        private readonly ILogger<LeadProcessorWorker> _logger;

        public LeadProcessorWorker(ILeadQueue leadQueue, IEventBus eventBus, ILogger<LeadProcessorWorker> logger)
        {
            _leadQueue = leadQueue;
            _eventBus = eventBus;
            _logger = logger;
        }

        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(5, stoppingToken);
                await ProcessLeadAsync(stoppingToken);
            }
        }

        private async Task ProcessLeadAsync(CancellationToken stoppingToken)
        {
            if (_leadQueue.GetNextLead() is ValidatedLead lead)
            {
                LeadProccessed evt = new()
                {
                    Lead = lead,
                    ProcessedUtc = DateTime.UtcNow
                };

                _logger.LogWarning("Proccessed Lead - Id: {id}, LastName: {name}", evt.Lead.LeadData.Id, evt.Lead.LeadData.LastName);

                await _eventBus.PublishAsync(evt, stoppingToken);
            }
        }
    }
}
