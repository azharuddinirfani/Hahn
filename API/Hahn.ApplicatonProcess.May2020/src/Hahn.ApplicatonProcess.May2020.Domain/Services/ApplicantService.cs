using Hahn.ApplicatonProcess.May2020.Domain.Models;
using Hahn.ApplicatonProcess.May2020.Domain.Persistence;
using System.Threading.Tasks;

namespace Hahn.ApplicatonProcess.May2020.Domain.Services

{
    public class ApplicantRepoService : IApplicantService
    {
        private readonly IRepository<Applicant> applicantRepository;

        public ApplicantRepoService(IRepository<Applicant> applicantRepository)
        {
            this.applicantRepository = applicantRepository;
        }

        public async Task<(Result, int)> AddAnApplicant(Applicant applicant)
        {
            return await applicantRepository.Create(applicant);
        }

        public async Task<Applicant> GetApplicant(int id)
        {
            return await applicantRepository.GetById(id);
        }

        public async Task<Result> RemoveAnApplicant(int id)
        {
            return await applicantRepository.Delete(id);
        }

        public async Task<(Result, int)> UpdateAnApplicant(int id, Applicant applicant)
        {
            return await applicantRepository.Update(id, applicant);
        }

    }
}