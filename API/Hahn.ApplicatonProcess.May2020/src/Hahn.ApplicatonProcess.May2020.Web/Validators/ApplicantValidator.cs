using FluentValidation;
using FluentValidation.Validators;
using Hahn.ApplicatonProcess.May2020.Domain.Models;
using System.Net.Http;

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

public class ApplicantValidator : AbstractValidator<Applicant>
{
    public ApplicantValidator(IHttpClientFactory httpClientFactory)
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