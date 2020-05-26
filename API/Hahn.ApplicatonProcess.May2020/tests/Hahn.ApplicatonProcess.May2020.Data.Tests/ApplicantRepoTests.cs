using AutoFixture;
using Hahn.ApplicatonProcess.May2020.Domain.Models;
using System.Threading.Tasks;
using Xunit;

namespace Hahn.ApplicatonProcess.May2020.Data.Tests
{

    public class ApplicantRepoTests
    {
        ApplicantRepo sut;
        readonly Fixture fixture = new Fixture();
        readonly ApplicantContext applicationContext = new ApplicantContext();
        [Fact]
        public async Task GivenAnApplicant_Create_CreatesAnApplicant()
        {
            sut = new ApplicantRepo(applicationContext);
            var applicant = fixture.Create<Applicant>();
            (var status, var id) = await sut.Create(applicant);
            Assert.Equal(Result.Success, status);
            Assert.NotEqual(default, id);
        }

        [Fact]
        public async Task GivenAnApplicantWithNoId_Create_CreatesAnApplicant()
        {
            sut = new ApplicantRepo(applicationContext);
            var applicant = fixture.Create<Applicant>();
            applicant.Id = 0;
            (var status, var id) = await sut.Create(applicant);
            Assert.Equal(Result.Success, status);
            Assert.NotEqual(default, id);
        }

        [Fact]
        public async Task GivenAnExistingApplicant_Update_UpdatesApplicant()
        {
            sut = new ApplicantRepo(applicationContext);
            var applicant = fixture.Create<Applicant>();
            (_, var id) = await sut.Create(applicant);
            var updatedApplicant = fixture.Create<Applicant>();
            (var updateStatus, var updatedId) = await sut.Update(id, updatedApplicant);
            Assert.Equal(Result.Success, updateStatus);
            var updatedEntity = await sut.GetById(id);
            Assert.Equal(id, updatedId);
            Assert.Equal(updatedEntity.Address, updatedApplicant.Address);
            Assert.Equal(updatedEntity.Age, updatedApplicant.Age);
            Assert.Equal(updatedEntity.FamilyName, updatedApplicant.FamilyName);
            Assert.Equal(updatedEntity.Name, updatedApplicant.Name);
            Assert.Equal(updatedEntity.Hired, updatedApplicant.Hired);
        }


        [Fact]
        public async Task GivenAnNonExistingApplicant_Update_ReturnsFailure()
        {
            sut = new ApplicantRepo(applicationContext);
            var updatedApplicant = fixture.Create<Applicant>();
            (var updateStatus, var updatedId) = await sut.Update(updatedApplicant.Id, updatedApplicant);
            Assert.Equal(Result.Fail, updateStatus);
            Assert.Equal(default, updatedId);
        }


        [Fact]
        public async Task GivenAnExistingApplicant_Delete_RemovesApplicantFromDB()
        {
            sut = new ApplicantRepo(applicationContext);
            var applicant = fixture.Create<Applicant>();
            (var status, var id) = await sut.Create(applicant);
            Assert.Equal(Result.Success, status);
            Assert.NotEqual(default, id);

            var deleteStatus = await sut.Delete(applicant.Id);
            Assert.Equal(Result.Success, deleteStatus);

            Assert.Null(await sut.GetById(id));
        }

        [Fact]
        public async Task GivenAnExistingApplicant_GetId_ReturnsApplicantFromDB()
        {
            sut = new ApplicantRepo(applicationContext);
            var applicant = fixture.Create<Applicant>();
            (var status, var id) = await sut.Create(applicant);
            Assert.Equal(Result.Success, status);
            Assert.NotEqual(default, id);

            var aplicantfromDb = await sut.GetById(id);
            Assert.NotNull(aplicantfromDb);
            Assert.Equal(id, aplicantfromDb.Id);
        }

    }
}
