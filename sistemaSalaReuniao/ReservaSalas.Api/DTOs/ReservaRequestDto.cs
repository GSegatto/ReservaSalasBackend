namespace ReservaSalas.Api.DTOs;

public class ReservaRequestDto
{
    public int SalaId { get; set; }

    public string Responsavel { get; set; } = string.Empty;

    public string Titulo { get; set; } = string.Empty;

    public DateTime Inicio { get; set; }

    public DateTime Fim { get; set; }

    public int Participantes { get; set; }
}