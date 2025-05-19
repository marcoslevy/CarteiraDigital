using CarteiraDigital.Core.Entities.Carteiras;
using CarteiraDigital.Core.Entities.Transacoes;
using CarteiraDigital.Core.Entities.Usuarios;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace CarteiraDigital.Infra.Data;

public class CarteiraDigitalDbContext : IdentityDbContext<Usuario>
{
    public CarteiraDigitalDbContext(DbContextOptions<CarteiraDigitalDbContext> options) : base(options)
    {

    }

    public DbSet<Usuario> Usuarios { get; set; }
    public DbSet<Carteira> Carteiras { get; set; }
    public DbSet<Transacao> Transacoes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasIndex(u => u.Email).IsUnique();
            entity.HasIndex(u => u.UserName).IsUnique();

            entity.HasOne(u => u.Carteira)
                  .WithOne(c => c.Usuario)
                  .HasForeignKey<Carteira>(c => c.UsuarioId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Carteira>(entity =>
        {
            entity.HasKey(w => w.Id);
            entity.Property(w => w.Saldo)
                  .HasColumnType("decimal(18,2)");
        });

        modelBuilder.Entity<Transacao>(entity =>
        {
            entity.HasKey(t => t.Id);
            entity.Property(t => t.Valor)
                  .HasColumnType("decimal(18,2)");

            entity.HasOne(t => t.Remetente)
                  .WithMany(u => u.TransacoesEnviadas)
                  .HasForeignKey(t => t.RemetenteId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(t => t.Destinatario)
                  .WithMany(u => u.TransacoesRecebidas)
                  .HasForeignKey(t => t.DestinatarioId)
                  .OnDelete(DeleteBehavior.Restrict);
        });
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder.Properties<DateTime>()
            .HaveConversion<DateTimeToUtcConverter>();
    }

    public class DateTimeToUtcConverter : ValueConverter<DateTime, DateTime>
    {
        public DateTimeToUtcConverter()
            : base(
                v => v.Kind == DateTimeKind.Utc ? v : v.ToUniversalTime(),
                v => DateTime.SpecifyKind(v, DateTimeKind.Utc))
        {
        }
    }
}
