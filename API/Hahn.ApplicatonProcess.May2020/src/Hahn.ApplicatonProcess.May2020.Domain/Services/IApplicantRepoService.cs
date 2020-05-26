using Hahn.ApplicatonProcess.May2020.Domain.Models;
using System.Threading.Tasks;

namespace Hahn.ApplicatonProcess.May2020.Domain.Services
{
    public interface IApplicantService
    {
        Task<(Result, int)> AddAnApplicant(Applicant applicant);
        Task<(Result, int)> UpdateAnApplicant(int id, Applicant applicant);
        Task<Result> RemoveAnApplicant(int id);

        Task<Applicant> GetApplicant(int id);
    }
}