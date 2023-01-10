using Microsoft.AspNetCore.Mvc;
using Musbooking.TestTask.Enums;
using Musbooking.TestTask.ServiceAbstractions;

namespace Musbooking.TestTask.Controllers;

[ApiController]
[Route("services")]
public sealed class EquipmentController : ControllerBase
{
    private readonly IEquipmentService _equipmentService;

    public EquipmentController(IEquipmentService equipmentService)
    {
        _equipmentService = equipmentService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAsync([FromQuery] string id)
    {
        var guid = Guid.Parse(id);
        var response = await _equipmentService.GetAsync(guid);

        return response.Status == ServiceResponseStatus.Succeeded ? Ok(response.Result) : NotFound(response.Error);
    }

    [HttpGet("booked")]
    public async Task<IActionResult> GetBookedAsync([FromQuery] string id)
    {
        var guid = Guid.Parse(id);
        var response = await _equipmentService.GetBookedAsync(guid);
        
        return response.Status == ServiceResponseStatus.Succeeded ? Ok(response.Result) : NotFound(response.Error);
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreateAsync([FromQuery(Name = "n")] string name, [FromQuery(Name = "a")] int amount)
    {
        return Ok(await _equipmentService.CreateAsync(name, amount));
    }

    [HttpPatch("update/{id}")]
    public async Task<IActionResult> UpdateAsync([FromRoute] string id, [FromQuery(Name = "n")] string? name, [FromQuery(Name = "a")] int? amount)
    {
        var guid = Guid.Parse(id);
        var response = await _equipmentService.UpdateAsync(guid, name, amount);

        return response.Status == ServiceResponseStatus.Succeeded ? Ok(response.Result) : NotFound(response.Error);
    }

    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> DeleteAsync([FromRoute] string id)
    {
        var guid = Guid.Parse(id);
        var response = await _equipmentService.DeleteAsync(guid);
        
        return response.Status == ServiceResponseStatus.Succeeded ? NoContent() : NotFound(response.Error);
    }

    [HttpPost("booking")]
    public async Task<IActionResult> BookingAsync([FromQuery] string id, [FromQuery(Name = "a")] int amount)
    {
        var guid = Guid.Parse(id);
        var response = await _equipmentService.BookingAsync(guid, amount);

        return response.Status == ServiceResponseStatus.Succeeded ? Ok(response) : NotFound(response);
    }
}