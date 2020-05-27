using Hahn.ApplicatonProcess.May2020.Domain.Models;
using System.Threading.Tasks;

namespace Hahn.ApplicatonProcess.May2020.Domain.Services
{
    public interface IApplicantService
    {
        Task<Applicant> CreateApplicant(Applicant applicant);
        Task<Result> UpdateApplicant(int id, Applicant applicant);
        Task<Result> DeleteApplicant(int id);

        Task<Applicant> GetApplicant(int id);
    }
}