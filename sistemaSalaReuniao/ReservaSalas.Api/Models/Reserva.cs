namespace ReservaSalas.Api.Models;

public class Reserva
{
    public int Id { get; set; }
    public int SalaId { get; set; }
    public Sala Sala { get; set; } = null!;
    public string Responsavel { get; set; } = string.Empty;
    public string Titulo { get; set; } = string.Empty;
    public DateTime Inicio { get; set; }
    public DateTime Fim { get; set; }
    public int Participantes { get; set; }
    public bool Cancelada { get; set; }
}