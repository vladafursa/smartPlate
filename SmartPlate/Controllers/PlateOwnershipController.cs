using Microsoft.AspNetCore.Mvc;
using SmartPlate.DTOs.PlateOwnership;
using SmartPlate.Services.PlateOwnershipService;
using System.Security.Claims;

namespace SmartPlate.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlateOwnershipController : ControllerBase
    {
        private readonly IPlateOwnershipService _service;

        public PlateOwnershipController(IPlateOwnershipService service)
        {
            _service = service;
        }

        // All ownership records for a specific plate
        [HttpGet("plate/{plateId}")]
        public async Task<ActionResult<List<PlateOwnershipResponseDto>>> GetAllOwnershipHistoryForPlateAsync(Guid plateId)
        {
            var records = await _service.GetAllOwnershipHistoryForPlateAsync(plateId);
            return Ok(records);
        }

        // All ownership records for a specific user
        [HttpGet("user/{userId}")]
        public async Task<ActionResult<List<PlateOwnershipResponseDto>>> GetAllOwnershipHistoryForUserAsync(Guid userId)
        {
            var records = await _service.GetAllOwnershipHistoryForUserAsync(userId);
            return Ok(records);
        }

        // Creation of ownership record by currently logged in user
        [HttpPost("{plateId}")]
        public async Task<ActionResult<PlateOwnershipResponseDto>> Create(Guid plateId)
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var record = await _service.CreateAsync(userId, plateId);
            return Ok(record);
        }

        // Active ownership for specific user
        [HttpGet("user/{userId:guid}/current")]
        public async Task<ActionResult<List<PlateOwnershipResponseDto>>> GetCurrentForUser()
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var result = await _service.GetCurrentOwnershipRecordsForUserAsync(userId);
            return Ok(result);
        }

        // Active ownership for specific plate
        [HttpGet("plate/{plateId:guid}/current")]
        public async Task<ActionResult<PlateOwnershipResponseDto?>> GetCurrentForPlate(Guid plateId)
        {
            var result = await _service.GetCurrentOwnershipRecordForPlateAsync(plateId);

            if (result == null)
                return NotFound($"No active ownership found for plate: {plateId}");

            return Ok(result);
        }

        // Transfer owneship to a new owner
        [HttpPost("plate/{plateId:guid}/transfer/{newOwnerId:guid}")]
        public async Task<ActionResult<PlateOwnershipResponseDto>> TransferOwnership(Guid plateId, Guid newOwnerId)
        {
            var result = await _service.TransferOwnershipAsync(plateId, newOwnerId);
            return Ok(result);
        }

        // end of ownership
        [HttpPut("plate/{plateId:guid}/end")]
        public async Task<ActionResult<PlateOwnershipResponseDto?>> EndOwnership(Guid plateId)
        {
            var result = await _service.EndOwnershipAsync(plateId);

            if (result == null)
                return NotFound($"No active ownership record found for plate: {plateId}");

            return Ok(result);
        }

        // Deletion of the whole ownership histpry for a specific plate
        [HttpDelete("plate/{plateId:guid}")]
        public async Task<IActionResult> DeleteOwnershipRecords(Guid plateId)
        {
            var deleted = await _service.DeleteOwnershipRecordsAsync(plateId);

            if (!deleted)
                return NotFound($"No ownership records exist for plate: {plateId}");

            return NoContent();
        }
    }
}
