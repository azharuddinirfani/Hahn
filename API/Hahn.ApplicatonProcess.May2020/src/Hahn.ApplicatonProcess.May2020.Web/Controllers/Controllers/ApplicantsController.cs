using AutoMapper;
using Hahn.ApplicatonProcess.May2020.Domain.Models;
using Hahn.ApplicatonProcess.May2020.Domain.Services;
using Hahn.ApplicatonProcess.May2020.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Hahn.ApplicatonProcess.May2020.Web.Controllers.Controllers
{
    [ApiController]
    [Route("api/applicants")]
    public class ApplicantsController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IApplicantService applicantService;

        public ApplicantsController(IMapper mapper, IApplicantService applicantService)
        {
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            this.applicantService = applicantService ?? throw new ArgumentNullException(nameof(applicantService));
        }
        [HttpGet("{applicantId}", Name = "GetApplicant")]
        public async Task<ActionResult<ApplicantDto>> GetApplicant(int applicantId)
        {

            var applicant = await applicantService.GetApplicant(applicantId);

            if (applicant != null)
            {
                return Ok(mapper.Map<ApplicantDto>(applicant));
            }

            return NotFound();
        }

        [HttpPost]
        public async Task<ActionResult<ApplicantDto>> CreateApplicant([FromBody] ApplicantForCreationDto applicantForCreationDto)
        {
            var applicant = mapper.Map<Applicant>(applicantForCreationDto);

            var createdApplicant = await applicantService.CreateApplicant(applicant);

            var applicantToReturn = mapper.Map<ApplicantDto>(createdApplicant);

            return CreatedAtRoute("GetApplicant",  new { applicantId = applicantToReturn.Id } ,applicantToReturn);
        }

    }
}
