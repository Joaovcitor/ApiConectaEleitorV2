using ConectaEleitor.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ConectaEleitor.Infrastructure.Data.Configuration;

public class AgendaConfiguration : IEntityTypeConfiguration<Agenda>
{
    public void Configure(EntityTypeBuilder<Agenda> builder)
    {
        builder.HasKey(x => x.AgendaId);

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(x => x.Description)
            .HasMaxLength(1000);

        builder.HasMany(x => x.Appointments)
            .WithOne(x => x.Agenda)
            .HasForeignKey(x => x.AgendaId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(x => x.OwnerId);
        builder.HasIndex(x => x.Name);
    }
}