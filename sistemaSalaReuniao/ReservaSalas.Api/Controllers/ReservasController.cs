using Microsoft.AspNetCore.Mvc;
using ReservaSalas.Api.DTOs;
using ReservaSalas.Api.Services;

namespace ReservaSalas.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReservasController : ControllerBase
{
    private readonly IReservaService _service;
    public ReservasController(IReservaService service) => _service = service;

    [HttpGet]
    [ProducesResponseType(typeof(List<ReservaResponseDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Listar(
        [FromQuery] int? salaId, 
        [FromQuery] DateOnly? data) =>
            Ok(await _service.ListarAsync(salaId, data));

    [HttpPost]
    [ProducesResponseType(typeof(ReservaResponseDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Criar([FromBody] ReservaRequestDto dto)
    {
        var reserva = await _service.CriarAsync(dto);
        return Created($"/api/reservas/{reserva.Id}", reserva);
    }

    [HttpPatch("{id:int}/cancelar")]
    [ProducesResponseType(typeof(ReservaResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Cancelar(int id) => Ok(await _service.CancelarAsync(id));
}