using ReservaSalas.Api.DTOs;
using ReservaSalas.Api.Exceptions;
using ReservaSalas.Api.Models;
using ReservaSalas.Api.Repositories;

namespace ReservaSalas.Api.Services;

public class SalaService : ISalaService
{
    private readonly ISalaRepository _repository;
    private readonly IWebHostEnvironment _environment; //Interface do ASP.NET Core que disponibiliza informações sobre o ambiente e os diretórios da aplicação.

    private const long TamanhoMaximoImagem = 5 * 1024 * 1024;

    private static readonly HashSet<string> TiposPermitidos = new(StringComparer.OrdinalIgnoreCase)
    {
        ".jpg", ".jpeg", ".png", ".webp"
    };

    public SalaService(
        ISalaRepository repository,
        IWebHostEnvironment environment)
    {
        _repository = repository;
        _environment = environment;
    }

    public async Task<List<SalaResponseDto>> ListarAsync() =>
        (await _repository.ListarAsync()).Select(ParaDto).ToList();

    public async Task<SalaResponseDto> ObterPorIdAsync(int id) =>
        ParaDto(await ObterOuFalharAsync(id));

    public async Task<SalaResponseDto> CadastrarAsync(SalaRequestDto dto)
    {
        var imagemUrl = await SalvarImagemAsync(dto.Imagem);

        try
        {
            var sala = new Sala
            {
                Nome = LimparNome(dto.Nome),
                Capacidade = dto.Capacidade,
                Ativa = true,
                ImagemUrl = imagemUrl
            };

            await _repository.AdicionarAsync(sala);
            await _repository.SalvarAsync();

            return ParaDto(sala);
        }
        catch
        {
            ExcluirArquivoImagem(imagemUrl);
            throw;
        }
    }

    public async Task<SalaResponseDto> EditarAsync(int id, SalaRequestDto dto)
    {
        var sala = await ObterOuFalharAsync(id);
        var imagemAnterior = sala.ImagemUrl;
        var novaImagemUrl = await SalvarImagemAsync(dto.Imagem);

        sala.Nome = LimparNome(dto.Nome);
        sala.Capacidade = dto.Capacidade;

        if (novaImagemUrl is not null)
            sala.ImagemUrl = novaImagemUrl;

        try
        {
            await _repository.SalvarAsync();

            if (novaImagemUrl is not null)
                ExcluirArquivoImagem(imagemAnterior);

            return ParaDto(sala);
        }
        catch
        {
            ExcluirArquivoImagem(novaImagemUrl);
            throw;
        }
    }

        public async Task<SalaResponseDto> AtivarAsync(int id)
    {
        var sala = await ObterOuFalharAsync(id);

        sala.Ativa = true;

        await _repository.SalvarAsync();

        return ParaDto(sala);
    }
    public async Task<SalaResponseDto> DesativarAsync(int id)
    {
        var sala = await ObterOuFalharAsync(id);
        sala.Ativa = false;
        await _repository.SalvarAsync();
        return ParaDto(sala);
    }

    public async Task ExcluirAsync(int id)
    {
        var sala = await ObterOuFalharAsync(id);
        sala.Deleted = true;
        await _repository.SalvarAsync();
    }

    private async Task<string?> SalvarImagemAsync(IFormFile? imagem)
    {
        if (imagem is null || imagem.Length == 0)
            return null;

        if (imagem.Length > TamanhoMaximoImagem)
            throw new NegocioException("A imagem deve ter no máximo 5 MB.");

        var extensao = Path.GetExtension(imagem.FileName);

        if (!TiposPermitidos.Contains(extensao))
            throw new NegocioException("Formato inválido. Envie uma imagem JPG, PNG ou WebP.");

        var pasta = Path.Combine(_environment.WebRootPath ?? Path.Combine(_environment.ContentRootPath, "wwwroot"), "uploads", "salas");
        Directory.CreateDirectory(pasta);

        var nomeArquivo = $"{Guid.NewGuid():N}{extensao.ToLowerInvariant()}";
        var caminho = Path.Combine(pasta, nomeArquivo);

        await using (var stream = new FileStream(caminho, FileMode.CreateNew))
        {
            await imagem.CopyToAsync(stream);
        }

        return $"/uploads/salas/{nomeArquivo}";
    }

    private void ExcluirArquivoImagem(string? imagemUrl)
    {
        if (string.IsNullOrWhiteSpace(imagemUrl) ||
            !imagemUrl.StartsWith("/uploads/salas/", StringComparison.OrdinalIgnoreCase))
            return;

        var nomeArquivo = Path.GetFileName(imagemUrl);
        var pasta = Path.Combine(_environment.WebRootPath ?? Path.Combine(_environment.ContentRootPath, "wwwroot"), "uploads", "salas");
        var caminho = Path.Combine(pasta, nomeArquivo);

        if (System.IO.File.Exists(caminho))
            System.IO.File.Delete(caminho);
    }

    private async Task<Sala> ObterOuFalharAsync(int id) =>
        await _repository.ObterPorIdAsync(id)
        ?? throw new NaoEncontradoException("Sala não encontrada.");

    private static string LimparNome(string nome)
    {
        var limpo = nome.Trim();
        if (limpo.Length == 0)
            throw new NegocioException("O nome da sala é obrigatório.");

        return limpo;
    }

    private static SalaResponseDto ParaDto(Sala s) =>
        new(s.Id, s.Nome, s.Capacidade, s.Ativa, s.ImagemUrl);
}
