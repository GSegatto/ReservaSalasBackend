namespace ReservaSalas.Api.DTOs;

public record ReservaResponseDto(
    int Id,
    int SalaId,
    string NomeSala,
    string Responsavel,
    string Titulo,
    DateTime Inicio,
    DateTime Fim,
    int Participantes,
    bool Canceladas
);