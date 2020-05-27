using AutoFixture;
using AutoMapper;
using Hahn.ApplicatonProcess.May2020.Domain.Models;
using Hahn.ApplicatonProcess.May2020.Domain.Services;
using Hahn.ApplicatonProcess.May2020.Web.Controllers.Controllers;
using Hahn.ApplicatonProcess.May2020.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace Hahn.ApplicatonProcess.May2020.Web.Tests.Controllers
{
    public class ApplicantsControllerTests
    {
        ApplicantsController sut;
        readonly Mock<ILogger<ApplicantsController>> loggerMock = new Mock<ILogger<ApplicantsController>>();
        readonly Mock<IMapper> mapperMock = new Mock<IMapper>();
        readonly Mock<IApplicantService> applicantServiceMock = new Mock<IApplicantService>();
        readonly Fixture fixture = new Fixture();
        [Fact]
        public async Task GivenAnExistingApplicantId_GetApplicant_ReturnsAnApplicant()
        {

            var applicantToBeReturned = fixture.Create<Applicant>();
            var applicantDtoToBeReturned = fixture.Create<ApplicantDto>();

            applicantServiceMock.Setup(mock => mock.GetApplicant(It.Is<int>(id => id == 1)))
                .ReturnsAsync(applicantToBeReturned);

            mapperMock.Setup(mock => mock.Map<ApplicantDto>(It.IsAny<Applicant>()))
                 .Returns(applicantDtoToBeReturned);

            sut = new ApplicantsController(loggerMock.Object, mapperMock.Object, applicantServiceMock.Object);

            var result = await sut.GetApplicant(1);
            var actionResult = Assert.IsType<ActionResult<ApplicantDto>>(result);
            var applicantReturned = Assert.IsType<OkObjectResult>(actionResult.Result).Value;

            Assert.Equal(applicantDtoToBeReturned, applicantReturned);
        }

        [Fact]
        public async Task GivenANonExistingApplicantId_GetApplicant_ReturnsNotFound()
        {


            sut = new ApplicantsController(loggerMock.Object, mapperMock.Object, applicantServiceMock.Object);

            var result = await sut.GetApplicant(1);
            var actionResult = Assert.IsType<ActionResult<ApplicantDto>>(result);
            Assert.IsType<NotFoundResult>(actionResult.Result);
        }


        [Fact]
        public async Task GivenAnApplicant_CreateApplicant_AddsAnApplicant()
        {

            var applicantToBeReturned = fixture.Create<Applicant>();
            var applicantForCreationDtoToBeCreated = fixture.Create<ApplicantForCreationDto>();
            var applicantDtoToBeReturned = fixture.Create<ApplicantDto>();

            applicantServiceMock.Setup(mock => mock.CreateApplicant(It.IsAny<Applicant>()))
                .ReturnsAsync(applicantToBeReturned);

            mapperMock.Setup(mock => mock.Map<ApplicantForCreationDto>(It.IsAny<Applicant>()))
                 .Returns(applicantForCreationDtoToBeCreated);

            mapperMock.Setup(mock => mock.Map<ApplicantDto>(It.IsAny<Applicant>()))
                 .Returns(applicantDtoToBeReturned);


            sut = new ApplicantsController(loggerMock.Object, mapperMock.Object, applicantServiceMock.Object);

            var result = await sut.CreateApplicant(applicantForCreationDtoToBeCreated);
            var actionResult = Assert.IsType<ActionResult<ApplicantDto>>(result);
            var applicantReturned = Assert.IsType<CreatedAtRouteResult>(actionResult.Result).Value;

            Assert.Equal(applicantDtoToBeReturned, applicantReturned);
        }

        [Fact]
        public async Task GivenAnExistingApplicant_UpdateApplicant_UpdatesApplicant()
        {

            var applicantToBeReturned = fixture.Create<Applicant>();
            var applicantForUpdateDto = fixture.Create<ApplicantForUpdateDto>();
            var applicantDtoToBeReturned = fixture.Create<ApplicantDto>();


            applicantServiceMock.Setup(mock => mock.GetApplicant(It.Is<int>(id => id == 1)))
              .ReturnsAsync(applicantToBeReturned);

            applicantServiceMock.Setup(mock => mock.UpdateApplicant(It.IsAny<int>(),It.IsAny<Applicant>()))
                .ReturnsAsync(Result.Success);

            mapperMock.Setup(mock => mock.Map<ApplicantForUpdateDto>(It.IsAny<Applicant>()))
                 .Returns(applicantForUpdateDto);

            mapperMock.Setup(mock => mock.Map<Applicant>(It.IsAny<ApplicantForUpdateDto>()))
                 .Returns(applicantToBeReturned);


            mapperMock.Setup(mock => mock.Map<ApplicantDto>(It.IsAny<Applicant>()))
                 .Returns(applicantDtoToBeReturned);


            sut = new ApplicantsController(loggerMock.Object, mapperMock.Object, applicantServiceMock.Object);

            var result = await sut.UpdateApplicant(1, applicantForUpdateDto);
            var actionResult = Assert.IsAssignableFrom<ActionResult>(result);
            var applicantReturned = Assert.IsType<NoContentResult>(actionResult);

        }

        [Fact]
        public async Task GivenAnExistingApplicantId_DeleteApplicant_RemovesApplicant()
        {

            var applicantToBeReturned = fixture.Create<Applicant>();
            var applicantDtoToBeReturned = fixture.Create<ApplicantDto>();

            applicantServiceMock.Setup(mock => mock.GetApplicant(It.Is<int>(id => id == 1)))
                .ReturnsAsync(applicantToBeReturned);

            applicantServiceMock.Setup(mock => mock.DeleteApplicant(It.Is<int>(id => id == 1)))
                .ReturnsAsync(Result.Success);

            mapperMock.Setup(mock => mock.Map<ApplicantDto>(It.IsAny<Applicant>()))
                 .Returns(applicantDtoToBeReturned);

            sut = new ApplicantsController(loggerMock.Object, mapperMock.Object, applicantServiceMock.Object);


            var result = await sut.DeleteApplicant(1);
            var actionResult = Assert.IsAssignableFrom<ActionResult>(result);
            var applicantReturned = Assert.IsType<NoContentResult>(actionResult);

        }
    }
}
