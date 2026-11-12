using ReservaSalas.Api.DTOs;

namespace ReservaSalas.Api.Services;

public interface IReservaService
{
    Task<List<ReservaResponseDto>> ListarAsync(int? salaId, DateOnly? data);
    Task<ReservaResponseDto> CriarAsync(ReservaRequestDto dto);
    Task<ReservaResponseDto> CancelarAsync(int id);
}