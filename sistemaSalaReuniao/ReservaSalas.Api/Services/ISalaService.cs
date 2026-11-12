using ReservaSalas.Api.DTOs;

namespace ReservaSalas.Api.Services;

public interface ISalaService
{
    Task<List<SalaResponseDto>> ListarAsync();
    Task<SalaResponseDto> ObterPorIdAsync(int id);
    Task<SalaResponseDto> CadastrarAsync(SalaRequestDto dto);
    Task<SalaResponseDto> EditarAsync(int id, SalaRequestDto dto);
    Task<SalaResponseDto> AtivarAsync(int id);
    Task<SalaResponseDto> DesativarAsync(int id);
    Task ExcluirAsync(int id);
}