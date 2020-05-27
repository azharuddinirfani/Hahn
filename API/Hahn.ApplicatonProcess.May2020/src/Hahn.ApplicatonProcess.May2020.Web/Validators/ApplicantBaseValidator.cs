using FluentValidation;
using FluentValidation.Validators;
using Hahn.ApplicatonProcess.May2020.Web.Models;
using System.Net.Http;

namespace Hahn.ApplicatonProcess.May2020.Web.Validators
{
    public class CountryValidator : PropertyValidator
    {
        //private readonly IHttpClientFactory httpClientFactory;
        private readonly HttpClient httpClient;

        public CountryValidator(IHttpClientFactory httpClientFactory)
            : base("{PropertyName} is not a valid country")
        {
            httpClient = httpClientFactory.CreateClient(nameof(CountryValidator)); ;

        }
        protected override bool IsValid(PropertyValidatorContext context)
        {
            var countryName = context.PropertyValue as string;

            if (string.IsNullOrEmpty(countryName))
            {
                return false;
            }

            using var response = httpClient.GetAsync($"{countryName}?fullText=true").GetAwaiter().GetResult();
            return response.IsSuccessStatusCode ? true : false;
        }


    }

    public class ApplicantBaseValidator<T> : AbstractValidator<T> where T : ApplicantBaseDto
    {
        public ApplicantBaseValidator(IHttpClientFactory httpClientFactory)
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .MinimumLength(5);

            RuleFor(x => x.FamilyName)
                .NotEmpty()
                .MinimumLength(5);


            RuleFor(x => x.Age)
                .InclusiveBetween(20, 60);

            RuleFor(x => x.Address)
                .NotEmpty()
               .MinimumLength(10);

            RuleFor(x => x.EmailAddress)
                .NotEmpty()
                .EmailAddress(EmailValidationMode.AspNetCoreCompatible);

            RuleFor(x => x.CountryOfOrigin)
                .NotEmpty()
                .SetValidator(new CountryValidator(httpClientFactory));


        }

    }
}