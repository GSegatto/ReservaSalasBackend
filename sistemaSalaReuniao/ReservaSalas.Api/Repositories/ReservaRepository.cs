using Microsoft.EntityFrameworkCore;
using ReservaSalas.Api.Data;
using ReservaSalas.Api.Models;

namespace ReservaSalas.Api.Repositories;

public class ReservaRepository : IReservaRepository
{
    private readonly AppDbContext _context;
    public ReservaRepository(AppDbContext context) => _context = context;

    public Task<List<Reserva>> ListarAsync(int? salaId, DateOnly? data)
    {
        var query = _context.Reservas
            .Include(r => r.Sala)
            .AsNoTracking()
            .Where(r => !r.Cancelada)
            .AsQueryable();

        if (salaId.HasValue)
            query = query.Where(r => r.SalaId == salaId.Value);

        if (data.HasValue)
        {
            var inicioDia = data.Value.ToDateTime(TimeOnly.MinValue);
            var fimDia = inicioDia.AddDays(1);
            query = query.Where(r => r.Inicio < fimDia && r.Fim > inicioDia);
        }

        return query.OrderBy(r => r.Inicio).ToListAsync();
    }

    public Task<Reserva?> ObterPorIdAsync(int id) =>
        _context.Reservas.Include(r => r.Sala).FirstOrDefaultAsync(r => r.Id == id);

    // Dois intervalos se sobrepõem quando: inicioExistente < fimNovo E fimExistente > inicioNovo.
    // Reservas canceladas não contam. 10h-11h e 11h-12h NÃO conflitam.
    public Task<bool> ExisteConflitoAsync(int salaId, DateTime inicio, DateTime fim) =>
        _context.Reservas.AnyAsync(r =>
            r.SalaId == salaId &&
            !r.Cancelada &&
            r.Inicio < fim &&
            r.Fim > inicio);

    public async Task AdicionarAsync(Reserva reserva) => await _context.Reservas.AddAsync(reserva);

    public Task SalvarAsync() => _context.SaveChangesAsync();
}