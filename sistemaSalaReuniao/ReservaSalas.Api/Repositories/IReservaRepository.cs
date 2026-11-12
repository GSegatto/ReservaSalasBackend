using ReservaSalas.Api.Models;

namespace ReservaSalas.Api.Repositories;

public interface IReservaRepository
{
    Task<List<Reserva>> ListarAsync(int? salaId, DateOnly? data);
    Task<Reserva?> ObterPorIdAsync(int id);
    Task<bool> ExisteConflitoAsync(int salaId, DateTime inicio, DateTime fim);
    Task AdicionarAsync(Reserva reserva);
    Task SalvarAsync();
}