using Microsoft.EntityFrameworkCore;
using ReservaSalas.Api.Models;

namespace ReservaSalas.Api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Sala> Salas => Set<Sala>();
    public DbSet<Reserva> Reservas => Set<Reserva>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Sala>(e =>
        {
            e.ToTable("sala", t => t.HasCheckConstraint("ck_sala_capacidade", "capacidade > 0"));
            e.HasKey(s => s.Id);
            e.Property(s => s.Id).HasColumnName("id_sala");
            e.Property(s => s.Nome).HasColumnName("nome_sala").HasMaxLength(100).IsRequired();
            e.Property(s => s.Capacidade).HasColumnName("capacidade");
            e.Property(s => s.Ativa).HasColumnName("ativa");
            e.Property(s => s.ImagemUrl).HasColumnName("imagem_url").HasMaxLength(500);
        });

        modelBuilder.Entity<Reserva>(e =>
        {
            e.ToTable("reserva", t =>
            {
                t.HasCheckConstraint("ck_reserva_periodo", "data_hora_fim > data_hora_inicio");
                t.HasCheckConstraint("ck_reserva_participantes", "participantes > 0");
            });
            e.HasKey(r => r.Id);
            e.Property(r => r.Id).HasColumnName("id_reserva");
            e.Property(r => r.SalaId).HasColumnName("id_sala");
            e.Property(r => r.Responsavel).HasColumnName("responsavel").HasMaxLength(150).IsRequired();
            e.Property(r => r.Titulo).HasColumnName("titulo").HasMaxLength(200).IsRequired();
            e.Property(r => r.Inicio).HasColumnName("data_hora_inicio");
            e.Property(r => r.Fim).HasColumnName("data_hora_fim");
            e.Property(r => r.Participantes).HasColumnName("participantes");
            e.Property(r => r.Cancelada).HasColumnName("cancelada");

            e.HasOne(r => r.Sala)
             .WithMany(s => s.Reservas)
             .HasForeignKey(r => r.SalaId)
             .OnDelete(DeleteBehavior.Restrict);

            e.HasIndex(r => new { r.SalaId, r.Inicio, r.Fim })
             .HasDatabaseName("IX_Reserva_Sala_Periodo");
        });
    }
}
