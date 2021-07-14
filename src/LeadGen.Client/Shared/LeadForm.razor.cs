﻿using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LeadGen.Connectors;
using Microsoft.Extensions.Configuration;
using System.ComponentModel.DataAnnotations;

namespace LeadGen.Client.Shared
{
    public partial class LeadForm : ComponentBase
    {
        [Inject] IValidationConnector Validation { get; set; }
        [Inject] NavigationManager NavigationManager { get; set; }
        [Inject] IConfiguration Configuration { get; set; }

        private LeadFormViewModel FormModel { get; set; } = new();
        private bool Error { get; set; }
        private string? ErrorMessage { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
        }

        private async Task OnFormSubmit()
        {
            Error = false;

            var result = await Validation.ValidateLeadAsync(FormModel.FirstName, FormModel.LastName, FormModel.Email, FormModel.Phone, Configuration.GetValue<string>("AppName"));
            if (result.IsValidated)
                NavigationManager.NavigateTo("/thank-you");

            Error = true;
            ErrorMessage = result.ErrorMessage;
        }

        public class LeadFormViewModel
        {
            [Required]
            public string? FirstName { get; set; }
            public string? LastName { get; set; }
            public string? Email { get; set; }
            public string? Phone { get; set; }
        }
    }
}
