using Hahn.ApplicatonProcess.May2020.Web.Models;
using Swashbuckle.AspNetCore.Filters;

namespace Hahn.ApplicatonProcess.May2020.Web.Examples
{
    public class ApplicantDtoExample : IExamplesProvider<ApplicantDto>
    {
        public ApplicantDto GetExamples()
        {
            return new ApplicantDto
            {
                Address = "DefaultAddress",
                Age = 23,
                CountryOfOrigin = "UK",
                EmailAddress ="test@test.com",
                FamilyName = "DefaultFamilyName",
                Hired = false,
                Id = 1,
                Name = "DefaultName"
            };
        }
    }
}
