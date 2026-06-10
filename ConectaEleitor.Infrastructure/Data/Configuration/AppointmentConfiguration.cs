using ConectaEleitor.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ConectaEleitor.Infrastructure.Data.Configuration;

public class AppointmentConfiguration : IEntityTypeConfiguration<Appointment>
{
    public void Configure(EntityTypeBuilder<Appointment> builder)
    {
        builder.HasKey(x => x.AppointmentId);

        builder.Property(x => x.Title)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(x => x.Description)
            .HasMaxLength(2000);

        builder.Property(x => x.Location)
            .HasMaxLength(250);

        builder.Property(x => x.Status)
            .HasConversion<string>()
            .HasMaxLength(30)
            .IsRequired();

        builder.HasOne(x => x.Agenda)
            .WithMany(x => x.Appointments)
            .HasForeignKey(x => x.AgendaId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.Citizen)
            .WithMany()
            .HasForeignKey(x => x.CitizenId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasIndex(x => x.OwnerId);
        builder.HasIndex(x => x.AgendaId);
        builder.HasIndex(x => x.CitizenId);
        builder.HasIndex(x => x.StartAt);
        builder.HasIndex(x => x.Status);
    }
}