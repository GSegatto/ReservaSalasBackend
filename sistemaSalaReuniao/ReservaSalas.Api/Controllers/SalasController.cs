using Microsoft.AspNetCore.Mvc;
using ReservaSalas.Api.DTOs;
using ReservaSalas.Api.Services;


namespace ReservaSalas.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SalasController : ControllerBase
{
    private readonly ISalaService _service;

    public SalasController(ISalaService service) => _service = service;

    [HttpGet]
    [ProducesResponseType(typeof(List<SalaResponseDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Listar() =>
        Ok(await _service.ListarAsync());

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(SalaResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ObterPorId(int id) =>
        Ok(await _service.ObterPorIdAsync(id));

    [HttpPost]
    [Consumes("multipart/form-data")]
    [ProducesResponseType(typeof(SalaResponseDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [RequestSizeLimit(5 * 1024 * 1024)]
    public async Task<IActionResult> Cadastrar([FromForm] SalaRequestDto dto)
    {
        var sala = await _service.CadastrarAsync(dto);
        return CreatedAtAction(nameof(ObterPorId), new { id = sala.Id }, sala);
    }

    [HttpPut("{id:int}")]
    [Consumes("multipart/form-data")]
    [ProducesResponseType(typeof(SalaResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [RequestSizeLimit(5 * 1024 * 1024)]
    public async Task<IActionResult> Editar(int id, [FromForm] SalaRequestDto dto) =>
        Ok(await _service.EditarAsync(id, dto));

    [HttpPatch("{id:int}/ativar")]
    [ProducesResponseType(typeof(SalaResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Ativar(int id) =>
        Ok(await _service.AtivarAsync(id));

    [HttpPatch("{id:int}/desativar")]
    [ProducesResponseType(typeof(SalaResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    
    public async Task<IActionResult> Desativar(int id) =>
        Ok(await _service.DesativarAsync(id));

    [HttpDelete("{id:int}")] 
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Excluir(int id)
    {
        await _service.ExcluirAsync(id);
        return NoContent();
    }
}
