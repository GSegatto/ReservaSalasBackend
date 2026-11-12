using ReservaSalas.Api.DTOs;
using ReservaSalas.Api.Exceptions;
using ReservaSalas.Api.Models;
using ReservaSalas.Api.Repositories;

namespace ReservaSalas.Api.Services;

public class ReservaService : IReservaService
{
    private readonly IReservaRepository _reservas;
    private readonly ISalaRepository _salas;

    public ReservaService(
        IReservaRepository reservas, 
        ISalaRepository salas)
    {
        _reservas = reservas;
        _salas = salas;
    }

    public async Task<List<ReservaResponseDto>> ListarAsync(int? salaId, DateOnly? data) =>
        (await _reservas.ListarAsync(salaId, data)).Select(ParaDto).ToList();

    public async Task<ReservaResponseDto> CriarAsync(ReservaRequestDto dto)
    {
        // RF07 / regra 3: fim deve ser maior que o início
        if (dto.Fim <= dto.Inicio)
            throw new NegocioException("O horário de fim deve ser maior que o horário de início.");

        // RF07 / regra 4: não reservar no passado
        if (dto.Inicio < DateTime.Now)
            throw new NegocioException("Não é permitido reservar uma sala em um horário no passado.");

        var sala = await _salas.ObterPorIdAsync(dto.SalaId)
            ?? throw new NaoEncontradoException("Sala não encontrada.");

        // RF07 / regra 4: sala inativa
        if (!sala.Ativa)
            throw new NegocioException("Não é possível reservar uma sala inativa.");

        // RF06 / regra 2: capacidade
        if (dto.Participantes > sala.Capacidade)
            throw new NegocioException(
                $"O número de participantes ({dto.Participantes}) excede a capacidade da sala ({sala.Capacidade}).");

        // RF05 / regra 1: sobreposição de horário
        if (await _reservas.ExisteConflitoAsync(sala.Id, dto.Inicio, dto.Fim))
            throw new ConflitoException("Já existe uma reserva para esta sala em um horário que se sobrepõe ao informado.");

        var reserva = new Reserva
        {
            SalaId = sala.Id,
            Sala = sala,
            Responsavel = dto.Responsavel.Trim(),
            Titulo = dto.Titulo.Trim(),
            Inicio = dto.Inicio,
            Fim = dto.Fim,
            Participantes = dto.Participantes,
            Cancelada = false
        };

        await _reservas.AdicionarAsync(reserva);
        await _reservas.SalvarAsync();
        return ParaDto(reserva);
    }

    public async Task<ReservaResponseDto> CancelarAsync(int id)
    {
        var reserva = await _reservas.ObterPorIdAsync(id)
            ?? throw new NaoEncontradoException("Reserva não encontrada.");

        if (reserva.Cancelada)
            throw new NegocioException("Esta reserva já está cancelada.");

        reserva.Cancelada = true;
        await _reservas.SalvarAsync();
        return ParaDto(reserva);
    }

    private static ReservaResponseDto ParaDto(Reserva r) => new(
        r.Id, 
        r.SalaId, 
        r.Sala.Nome, 
        r.Responsavel, 
        r.Titulo,
        r.Inicio, 
        r.Fim, 
        r.Participantes, 
        r.Cancelada);
}