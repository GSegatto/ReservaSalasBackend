namespace ReservaSalas.Api.Models;

public class Sala
{
    public int Id { get; set; }

    public string Nome { get; set; } = string.Empty;

    public int Capacidade { get; set; }

    public bool Ativa { get; set; } = true;

    public bool Deleted { get; set; } = false;

    public string? ImagemUrl { get; set; }

    public List<Reserva> Reservas { get; set; } = new();

    public DateTime DataCriacao { get; set; }
    public DateTime? DataModificacao { get; set; }
}
