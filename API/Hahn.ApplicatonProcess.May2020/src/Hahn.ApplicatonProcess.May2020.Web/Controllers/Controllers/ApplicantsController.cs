using AutoMapper;
using Hahn.ApplicatonProcess.May2020.Domain.Models;
using Hahn.ApplicatonProcess.May2020.Domain.Services;
using Hahn.ApplicatonProcess.May2020.Web.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Hahn.ApplicatonProcess.May2020.Web.Controllers.Controllers
{
    [ApiController]
    [Route("api/applicants")]
    [Consumes("application/json")]
    [Produces("application/json")]

    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public class ApplicantsController : ControllerBase
    {
        private readonly ILogger<ApplicantsController> logger;
        private readonly IMapper mapper;
        private readonly IApplicantService applicantService;

        public ApplicantsController(ILogger<ApplicantsController> logger, IMapper mapper, IApplicantService applicantService)
        {
            this.logger = logger;
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            this.applicantService = applicantService ?? throw new ArgumentNullException(nameof(applicantService));
        }

        /// <summary>
        /// Get an applicant by his/her id
        /// </summary>
        /// <param name="applicantId">The id of author you want to get</param>
        /// <returns>an applicant</returns>
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
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
        [ProducesResponseType(StatusCodes.Status201Created)]

        public async Task<ActionResult<ApplicantDto>> CreateApplicant([FromBody] ApplicantForCreationDto applicantForCreationDto)
        {
            var applicant = mapper.Map<Applicant>(applicantForCreationDto);

            var createdApplicant = await applicantService.CreateApplicant(applicant);


            var applicantToReturn = mapper.Map<ApplicantDto>(createdApplicant);

            return CreatedAtRoute("GetApplicant", new { applicantId = applicantToReturn.Id }, applicantToReturn);

        }

        [HttpPut("{applicantId}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> UpdateApplicant(int applicantId,
            ApplicantForUpdateDto applicantForUpdateDto)
        {
            var existingApplicant = await applicantService.GetApplicant(applicantId);

            if (existingApplicant != null)
            {
                var applicant = mapper.Map<Applicant>(applicantForUpdateDto);

                await applicantService.UpdateApplicant(applicantId, applicant);

                return NoContent();
            }

            return NotFound();
        }

        [HttpDelete("{applicantId}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> DeleteApplicant(int applicantId)
        {
            var existingApplicant = await applicantService.GetApplicant(applicantId);

            if (existingApplicant != null)
            {

                await applicantService.DeleteApplicant(applicantId);

                return NoContent();
            }

            return NotFound();
        }

        [HttpOptions]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetApplicantOptions()
        {
            Response.Headers.Add("Allow", "OPTIONS,GET,POST,PUT,DELETE");
            return Ok();
        }

    }
}
