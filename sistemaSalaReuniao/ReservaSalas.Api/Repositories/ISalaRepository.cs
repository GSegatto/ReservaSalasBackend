using ReservaSalas.Api.Models;

namespace ReservaSalas.Api.Repositories;

public interface ISalaRepository
{
    Task<List<Sala>> ListarAsync();
    Task<Sala?> ObterPorIdAsync(int id);
    Task AdicionarAsync(Sala sala);
    Task SalvarAsync();
}