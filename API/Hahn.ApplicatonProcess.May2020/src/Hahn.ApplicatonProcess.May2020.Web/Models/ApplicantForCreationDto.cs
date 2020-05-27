
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

    public class ApplicantForUpdateDto : ApplicantBaseDto
    {

    }
}
