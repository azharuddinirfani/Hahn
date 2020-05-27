using Hahn.ApplicatonProcess.May2020.Web.Models;
using Swashbuckle.AspNetCore.Filters;

namespace Hahn.ApplicatonProcess.May2020.Web.Examples
{
    public class ApplicantForCreationDtoExamples : IExamplesProvider<ApplicantForCreationDto>
    {
        public ApplicantForCreationDto GetExamples()
        {
            return new ApplicantForCreationDto
            {
                Address = "DefaultAddress",
                Age = 23,
                CountryOfOrigin = "UK",
                EmailAddress = "test@test.com",
                FamilyName = "DefaultFamilyName",
                Hired = false,
                Name = "DefaultName"
            };
        }
    }

    public class ApplicantForUpdateDtoExamples : IExamplesProvider<ApplicantForUpdateDto>
    {
        public ApplicantForUpdateDto GetExamples()
        {
            return new ApplicantForUpdateDto
            {
                Address = "DefaultAddress",
                Age = 23,
                CountryOfOrigin = "UK",
                EmailAddress = "test@test.com",
                FamilyName = "DefaultFamilyName",
                Hired = false,
                Name = "DefaultName"
            };
        }
    }
}
