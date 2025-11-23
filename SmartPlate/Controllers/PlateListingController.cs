using Microsoft.AspNetCore.Mvc;
using SmartPlate.DTOs.PlateListing;
using SmartPlate.Services.PlateListingService;
using System.Security.Claims;

namespace SmartPlate.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlateListingController : ControllerBase
    {
        private readonly IPlateListingService _service;

        public PlateListingController(IPlateListingService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<ActionResult<PlateListingResponseDto>> Create(Guid PlateId, PlateListingCreateDto dto)
        {
            var SellerId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            return Ok(await _service.CreateAsync(SellerId, PlateId, dto));
        }

        [HttpGet]
        public async Task<ActionResult<List<PlateListingResponseDto>>> GetAll()
        {
            return Ok(await _service.GetListingsAsync());
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            return await _service.DeleteAsync(id) ? NoContent() : NotFound();
        }

        [HttpGet("filter")]
        public async Task<IActionResult> Filter([FromQuery] PlateListingFilterDto filter)
        {
            var listings = await _service.GetFilteredAsync(filter);
            return Ok(listings);
        }
    }
}