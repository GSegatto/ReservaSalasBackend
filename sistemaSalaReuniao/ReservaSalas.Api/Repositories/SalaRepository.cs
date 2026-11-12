using Microsoft.EntityFrameworkCore;
using ReservaSalas.Api.Data;
using ReservaSalas.Api.Models;

namespace ReservaSalas.Api.Repositories;

public class SalaRepository : ISalaRepository
{
    private readonly AppDbContext _context;

    public SalaRepository(AppDbContext context) => _context = context;

    public Task<List<Sala>> ListarAsync() =>
    _context.Salas
        .AsNoTracking()
        .Where(s => !s.Deleted)
        .OrderBy(s => s.Nome)
        .ToListAsync();

    public Task<Sala?> ObterPorIdAsync(int id) =>
        _context.Salas
            .FirstOrDefaultAsync(s => s.Id == id && !s.Deleted);

    public async Task AdicionarAsync(Sala sala) =>
        await _context.Salas.AddAsync(sala);

    public Task SalvarAsync() =>
        _context.SaveChangesAsync();
}