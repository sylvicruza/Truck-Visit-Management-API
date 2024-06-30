

using Microsoft.AspNetCore.Mvc;
using System.Data;
using Truck_Visit_Management.Dtos;
using Truck_Visit_Management.Entities;
using Truck_Visit_Management.Exceptions;
using Truck_Visit_Management.Services.ServiceImpl;


namespace Truck_Visit_Management.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize("Admin", "User")] // Specify roles that can access
    public class VisitsController : ControllerBase
    {

        private readonly IVisitService _visitService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public VisitsController(IVisitService visitService, IHttpContextAccessor httpContextAccessor)
        {
            _visitService = visitService;
            _httpContextAccessor = httpContextAccessor;
        }

        

        [HttpPost]
        public async Task<ActionResult<VisitRecordResponseDto>> CreateVisit([FromBody] VisitRecordRequestDto visitRecordRequest)
        {
            try
            {
                // Get the username of the logged-in user
                var loggedInUser = GetLoggedInUser();
                if (loggedInUser == null)
                {
                    return Unauthorized(new { message = "User is not logged in." });
                }

                // Set CreatedBy to the logged-in user
                visitRecordRequest.CreatedBy = loggedInUser.Username;

                var createdVisit = await _visitService.CreateVisitAsync(visitRecordRequest);
            
                return CreatedAtAction(nameof(GetVisitById), new { id = createdVisit.Id }, createdVisit);
            }
            catch (BadRequestException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An error occurred while creating the visit.", error = ex.Message });
            }
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<VisitRecordResponseDto>>> GetVisits()
        {
            try
            {
                var visits = await _visitService.GetVisitsAsync();
                
                return Ok(visits);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An error occurred while retrieving visits.", error = ex.Message });
            }
        }

        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateVisitStatus(int id, [FromBody] string status)
        {
            try
            {
                // Get the username of the logged-in user
               
                var loggedInUser = GetLoggedInUser();
                if (loggedInUser == null)
                {
                    return Unauthorized(new { message = "User is not logged in." });
                }

                await _visitService.UpdateVisitStatusAsync(id, status, loggedInUser.Username);
                return Ok(new { message = "Visit record updated successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An error occurred while updating the visit status.", error = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<VisitRecordResponseDto>> GetVisitById(int id)
        {
            try
            {
                var visit = await _visitService.GetVisitByIdAsync(id);
                if (visit == null)
                {
                    return NotFound();
                }
               
                return Ok(visit);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An error occurred while retrieving the visit.", error = ex.Message });
            }
        }

        private User GetLoggedInUser()
        {
            return _httpContextAccessor.HttpContext.Items["User"] as User;
        }
    }

}
