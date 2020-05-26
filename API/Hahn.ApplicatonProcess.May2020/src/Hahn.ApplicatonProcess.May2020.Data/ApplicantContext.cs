using Hahn.ApplicatonProcess.May2020.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Hahn.ApplicatonProcess.May2020.Data
{
    public class ApplicantContext : DbContext
    {
        public DbSet<Applicant> Applicants { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseInMemoryDatabase("ApplicantsDb");
        }

    }
}
