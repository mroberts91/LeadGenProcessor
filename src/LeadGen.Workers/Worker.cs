using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Bogus;
using System.Net.Http;
using System.Text.Json;
using System.Net.Http.Headers;

namespace LeadGen.Worker
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IHttpClientFactory _clientFactory;
        private readonly Random _random = new();
        private readonly string[] _fauxSources = new string[6]
        {
            "HMA",
            "MA",
            "SMA",
            "MS",
            "Anthem",
            "Humana"
        };

        public Worker(ILogger<Worker> logger, IHttpClientFactory client)
        {
            _logger = logger;
            _clientFactory = client;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            int initalDelay = 1000 * 60;
            //_logger.LogWarning("Waiting {seconds} seconds for other applications to warmup...", initalDelay / 1000);
            //await Task.Delay(initalDelay, stoppingToken);
            _logger.LogWarning("Starting to send requests...");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await SendLeadAsync();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Unhandled exception occued in worker {msg} : Inner Exception {imsg}", ex.Message, ex.InnerException?.Message);
                }

                await Task.Delay(1500, stoppingToken);
            }
        }

        private async Task SendLeadAsync()
        {
            int random = _random.Next(0, 50);
            string source = _fauxSources[_random.Next(0, _fauxSources.Length)];
            
            using var client = _clientFactory.CreateClient("validator");
            
            var person = new Faker().Person;
            Lead request = 
                random % 5 == 0 ?
                Lead.Create(string.Empty, string.Empty, string.Empty, string.Empty, source) :
                Lead.Create(person.FirstName, person.LastName, person.Email, person.Phone, source);

            var content = new StringContent(JsonSerializer.Serialize(request));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpRequestMessage requestMessage = new(HttpMethod.Post, "Validate")
            {
                Content = content,
            };
    
            await client.SendAsync(requestMessage);
        }
    }

    public record Lead(Guid Id, string? FirstName, string? LastName, string? Email, string? Phone, string? source)
    {
        public static Lead Create(string? firstname = null, string? lastname = null, string? email = null, string? phone = null, string? source = null)
        {
            return new(Guid.NewGuid(), firstname, lastname, email, phone, source);
        }
    }
}
