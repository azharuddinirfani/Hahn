using Hahn.ApplicatonProcess.May2020.Domain.Models;
using Hahn.ApplicatonProcess.May2020.Domain.Persistence;
using System;
using System.Threading.Tasks;

namespace Hahn.ApplicatonProcess.May2020.Domain.Services

{
    public class ApplicantService : IApplicantService
    {
        private readonly IRepository<Applicant> applicantRepository;

        public ApplicantService(IRepository<Applicant> applicantRepository)
        {
            this.applicantRepository = applicantRepository;
        }

        public async Task<Applicant> CreateApplicant(Applicant applicant)
        {
            if (applicant is null)
            {
                throw new ArgumentNullException(nameof(applicant));
            }

            return await applicantRepository.Create(applicant);
        }

        public async Task<Applicant> GetApplicant(int id)
        {
            return await applicantRepository.GetById(id);
        }

        public async Task<Result> DeleteApplicant(int id)
        {
            return await applicantRepository.Delete(id);
        }

        public async Task<Result> UpdateApplicant(int id, Applicant applicant)
        {

            if (applicant is null)
            {
                throw new ArgumentNullException(nameof(applicant));
            }
            (var result, _) = await applicantRepository.Update(id, applicant);

            return result;

        }

    }
}