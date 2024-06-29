

using Microsoft.AspNetCore.Mvc;
using System.Data;
using Truck_Visit_Management.Dtos;
using Truck_Visit_Management.Services.ServiceImpl;


namespace Truck_Visit_Management.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize("Admin", "User")] // Specify roles that can access
    public class VisitsController : ControllerBase
    {
        private readonly IVisitService _visitService;

        public VisitsController(IVisitService visitService)
        {
            _visitService = visitService;
        }

        [HttpPost]
        public async Task<ActionResult<VisitRecordDto>> CreateVisit([FromBody] VisitRecordDto visitRecord)
        {
            var createdVisit = await _visitService.CreateVisitAsync(visitRecord);
            return CreatedAtAction(nameof(GetVisitById), new { id = createdVisit.Id }, createdVisit);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<VisitRecordDto>>> GetVisits()
        {
            var visits = await _visitService.GetVisitsAsync();
            return Ok(visits);
        }

        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateVisitStatus(int id, [FromBody] string status)
        {
            await _visitService.UpdateVisitStatusAsync(id, status);
            return NoContent();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<VisitRecordDto>> GetVisitById(int id)
        {
            var visit = await _visitService.GetVisitByIdAsync(id);
            if (visit == null)
            {
                return NotFound();
            }
            return Ok(visit);
        }
    }

}
