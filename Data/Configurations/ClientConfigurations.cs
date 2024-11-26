using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TestingProject.Models;

namespace TestingProject.Data.Configurations;

public class ClientConfigurations : IEntityTypeConfiguration<Client>
{

    public void Configure(EntityTypeBuilder<Client> builder)
    {
        builder.ToTable("clients");

        builder.HasKey(c => c.ClientId)
            .HasName("pk_clients");

        builder.Property(c => c.ClientId)
            .HasColumnName("id");

        builder.Property(c => c.SystemId)
            .IsRequired()
            .HasColumnName("system_id");

        builder.Property(c => c.Username)
            .IsRequired()
            .HasColumnName("username");
    }
}
