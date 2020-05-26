using FluentValidation;
using FluentValidation.Validators;
using System.Net.Http;

namespace Hahn.ApplicatonProcess.May2020.Web.Models
{
    public class ApplicantBaseDto
    {
        public string Name { get; set; }
        public string FamilyName { get; set; }
        public string Address { get; set; }
        public string CountryOfOrigin { get; set; }
        public string EmailAddress { get; set; }
        public int Age { get; set; }
        public bool Hired { get; set; }
    }
    public class ApplicantForCreationDto : ApplicantBaseDto
    {

    }
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

    public class ApplicantBaseValidator<T> : AbstractValidator<T> where T: ApplicantBaseDto
    {
        public ApplicantBaseValidator(IHttpClientFactory httpClientFactory)
        {
            RuleFor(x => x.Name)
                //.NotEmpty()
                .MinimumLength(5);

            RuleFor(x => x.FamilyName)
                .MinimumLength(5);


            RuleFor(x => x.Age)
                .InclusiveBetween(20, 60);

            RuleFor(x => x.Address)
               .MinimumLength(10);

            RuleFor(x => x.EmailAddress)
                .EmailAddress(EmailValidationMode.AspNetCoreCompatible);

            RuleFor(x => x.CountryOfOrigin)
                .SetValidator(new CountryValidator(httpClientFactory));


        }

    }
}
