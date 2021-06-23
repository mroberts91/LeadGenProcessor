using System;
using Xunit;
using Shouldly;
using Bogus;
using Moq;
using LeadGen.Core.Models;
using LeadGen.Core.Exceptions;
using System.Text;

namespace LeadGen.Core.Tests
{
    public class ValidatedLeadTests
    {
        [Fact]
        public void Create_Success()
        {
            var data = GetLeadData();
            Lead lead = Lead.Create(data.FirstName, data.LastName, data.Email, data.Phone);
            var validatedLead = ValidatedLead.Create(lead);

            validatedLead.Value.ShouldBeOfType<ValidatedLead>();
        }

        [Fact]
        public void ValidationToken_Success()
        {
            var data = GetLeadData();
            Lead lead = Lead.Create(data.FirstName, data.LastName, data.Email, data.Phone);
            var validatedLead = ValidatedLead.Create(lead);

            validatedLead.Value.ShouldBeOfType<ValidatedLead>();

            var vLead = validatedLead.Value as ValidatedLead;
            vLead.ShouldNotBeNull();
            vLead.VerifyValidationToken(vLead.ValidationToken).ShouldBeTrue();
        }

        [Fact]
        public void ValidationToken_Failure()
        {
            var data = GetLeadData();
            Lead lead = Lead.Create(data.FirstName, data.LastName, data.Email, data.Phone);
            var validatedLead = ValidatedLead.Create(lead);

            validatedLead.Value.ShouldBeOfType<ValidatedLead>();

            var vLead = validatedLead.Value as ValidatedLead;
            vLead.ShouldNotBeNull();
            vLead.VerifyValidationToken(Encoding.UTF8.GetBytes("SomeToken")).ShouldBeFalse();
        }

        [Fact]
        public void Create_FirstName_Fail()
        {
            var data = GetLeadData();
            Lead lead = Lead.Create(null, data.LastName, data.Email, data.Phone);
            var validatedLead = ValidatedLead.Create(lead);

            validatedLead.Value.ShouldBeOfType<InvalidLeadPropertyException<string>>();

            var value = validatedLead.Value as InvalidLeadPropertyException<string>;
            value.ShouldNotBeNull();
            value.PropertyName.ShouldBe(nameof(Lead.FirstName));
        }

        [Fact]
        public void Create_LastName_Fail()
        {
            var data = GetLeadData();
            Lead lead = Lead.Create(data.FirstName, null, data.Email, data.Phone);
            var validatedLead = ValidatedLead.Create(lead);

            validatedLead.Value.ShouldBeOfType<InvalidLeadPropertyException<string>>();

            var value = validatedLead.Value as InvalidLeadPropertyException<string>;
            value.ShouldNotBeNull();
            value.PropertyName.ShouldBe(nameof(Lead.LastName));
        }

        [Fact]
        public void Create_Email_Fail()
        {
            var data = GetLeadData();
            Lead lead = Lead.Create(data.FirstName, data.LastName, null, data.Phone);
            var validatedLead = ValidatedLead.Create(lead);

            validatedLead.Value.ShouldBeOfType<InvalidLeadPropertyException<string>>();

            var value = validatedLead.Value as InvalidLeadPropertyException<string>;
            value.ShouldNotBeNull();
            value.PropertyName.ShouldBe(nameof(Lead.Email));
        }

        [Fact]
        public void Create_Phone_Fail()
        {
            var data = GetLeadData();
            Lead lead = Lead.Create(data.FirstName, data.LastName, data.Email, null);
            var validatedLead = ValidatedLead.Create(lead);

            validatedLead.Value.ShouldBeOfType<InvalidLeadPropertyException<string>>();

            var value = validatedLead.Value as InvalidLeadPropertyException<string>;
            value.ShouldNotBeNull();
            value.PropertyName.ShouldBe(nameof(Lead.Phone));
        }

        private PersonData GetLeadData()
        {
            Faker faker = new();

            return new()
            {
                Email = faker.Person.Email,
                Phone = faker.Person.Phone,
                FirstName = faker.Person.FirstName,
                LastName = faker.Person.LastName,
            };
        }

        record PersonData
        {
            public string Email;
            public string Phone;
            public string FirstName;
            public string LastName;
        }
    }
}
