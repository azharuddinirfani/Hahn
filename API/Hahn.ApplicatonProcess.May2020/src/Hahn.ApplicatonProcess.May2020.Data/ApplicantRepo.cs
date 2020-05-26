using Hahn.ApplicatonProcess.May2020.Domain.Models;
using Hahn.ApplicatonProcess.May2020.Domain.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Hahn.ApplicatonProcess.May2020.Data
{
    public class ApplicantRepo : IRepository<Applicant>
    {
        private readonly ApplicantContext applicantContext;

        public ApplicantRepo(ApplicantContext applicantContext)
        {
            this.applicantContext = applicantContext;
        }
        public async Task<Applicant> Create(Applicant item)
        {
            if (await GetById(item.Id) != null)
            {
                return default;
            }
            var applicantEntity = await applicantContext.Applicants.AddAsync(item);
            await applicantContext.SaveChangesAsync();
            return applicantEntity.Entity;
        }

        public async Task<Result> Delete(int id)
        {
            var dbEntity = await GetById(id);
            if (dbEntity != null)
            {
                applicantContext.Applicants.Remove(dbEntity);
                await applicantContext.SaveChangesAsync();
                return Result.Success;

            }
            return Result.Fail;
        }

        public Task<Applicant> GetById(int id)
        {
            return applicantContext.Applicants.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<(Result, int)> Update(int id, Applicant item)
        {
            var dbEntity = await GetById(id);
            if (dbEntity != null)
            {
                dbEntity.Name = item.Name;
                dbEntity.FamilyName = item.FamilyName;
                dbEntity.EmailAddress = item.EmailAddress;
                dbEntity.Address = item.Address;
                dbEntity.Age = item.Age;
                dbEntity.CountryOfOrigin = item.CountryOfOrigin;
                dbEntity.Hired = item.Hired;

                await applicantContext.SaveChangesAsync();

                return (Result.Success, dbEntity.Id);
            }
            return (Result.Fail, default);
        }
    }
}
