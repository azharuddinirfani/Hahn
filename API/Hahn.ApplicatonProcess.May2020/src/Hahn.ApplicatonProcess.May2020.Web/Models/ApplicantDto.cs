namespace Hahn.ApplicatonProcess.May2020.Web.Models
{
    /// <summary>
    /// An applicant
    /// </summary>
    public class ApplicantDto
    {
        /// <summary>
        /// The id of applicant 
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// The name of applicant
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// The family name of applicant
        /// </summary>
        public string FamilyName { get; set; }
        /// <summary>
        /// The address of applicant
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// The countryoforigin of applicant
        /// </summary>
        public string CountryOfOrigin { get; set; }
        /// <summary>
        /// The emailaddress of applicant
        /// </summary>
        public string EmailAddress { get; set; }
        /// <summary>
        /// The age of applicant
        /// </summary>
        public int Age { get; set; }
        /// <summary>
        /// The property specifying if the applicant is hired
        /// </summary>
        public bool Hired { get; set; }
    }
}
