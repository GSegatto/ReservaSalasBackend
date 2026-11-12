using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace ReservaSalas.Api.DTOs;

public class SalaRequestDto
{
    [Required(ErrorMessage = "O nome da sala é obrigatório.")]
    [StringLength(100, ErrorMessage = "O nome deve ter no máximo 100 caracteres.")]
    public string Nome { get; set; } = string.Empty;

    [Range(1, int.MaxValue, ErrorMessage = "A capacidade deve ser maior que zero.")]
    public int Capacidade { get; set; }

    public IFormFile? Imagem { get; set; }
}

public record SalaResponseDto(int Id, string Nome, int Capacidade, bool Ativa, string? ImagemUrl);
