using AutoMapper;
using Hahn.ApplicatonProcess.May2020.Domain.Models;
using Hahn.ApplicatonProcess.May2020.Web.Models;

namespace Hahn.ApplicatonProcess.May2020.Web.Profiles
{
    public class ApplicantProfile : Profile
    {

        public ApplicantProfile()
        {
            CreateMap<Applicant, ApplicantDto>();
            CreateMap<ApplicantForCreationDto, Applicant>();
            CreateMap<ApplicantForUpdateDto, Applicant>();
        }
    }
}
