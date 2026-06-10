using ConectaEleitor.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ConectaEleitor.Infrastructure.Data.Configuration;

public class CitizenConfiguration : IEntityTypeConfiguration<Citizen>
{
    public void Configure(EntityTypeBuilder<Citizen> builder)
    {
        builder.HasKey(x => x.CitizenId);

        builder.Property(x => x.FullName)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(x => x.Nickname).HasMaxLength(100);
        builder.Property(x => x.Cpf).HasMaxLength(14);
        builder.Property(x => x.VoterRegistration).HasMaxLength(30);

        builder.Property(x => x.Phone).HasMaxLength(20);
        builder.Property(x => x.WhatsApp).HasMaxLength(20);

        builder.Property(x => x.Street).HasMaxLength(200);
        builder.Property(x => x.Number).HasMaxLength(20);
        builder.Property(x => x.Neighborhood).HasMaxLength(100);
        builder.Property(x => x.District).HasMaxLength(100);

        builder.Property(x => x.ElectoralZone).HasMaxLength(20);
        builder.Property(x => x.ElectoralSection).HasMaxLength(20);
        builder.Property(x => x.City).HasMaxLength(100);
        builder.Property(x => x.State).HasMaxLength(100);
        builder.Property(x => x.Notes).HasMaxLength(500);

        builder.Property(x => x.Type)
            .HasConversion<string>()
            .HasMaxLength(30)
            .IsRequired();

        builder.HasOne(x => x.Leader)
            .WithMany(x => x.LedCitizens)
            .HasForeignKey(x => x.LeaderId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasIndex(x => x.OwnerId);
        builder.HasIndex(x => x.LeaderId);
        builder.HasIndex(x => x.UserId);
        builder.HasIndex(x => x.Cpf);
        builder.HasIndex(x => x.FullName);
        
    }
}