using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Movie.User.Service.Domain.Entities;
using Movie.User.Service.Domain.ValueObjects;

namespace Movie.User.Service.Infra.Data.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<Movie.User.Service.Domain.Entities.User>
{
    public void Configure(EntityTypeBuilder<Movie.User.Service.Domain.Entities.User> builder)
    {
        builder.ToTable("users");

        builder.HasKey(u => u.Id);

        builder.Property(u => u.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd();

        builder.Property(u => u.Name)
            .HasColumnName("name")
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(u => u.Username)
            .HasColumnName("username")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(u => u.Email)
            .HasColumnName("email")
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(u => u.Phone)
            .HasColumnName("phone")
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(u => u.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired();

        builder.Property(u => u.UpdatedAt)
            .HasColumnName("updated_at");

        builder.Property(u => u.LastLoginAt)
            .HasColumnName("last_login_at");

        builder.Property(u => u.IsActive)
            .HasColumnName("is_active")
            .IsRequired();

        // Configuração do Value Object Address
        builder.OwnsOne(u => u.Address, address =>
        {
            address.Property(a => a.Street)
                .HasColumnName("street")
                .HasMaxLength(255)
                .IsRequired();

            address.Property(a => a.City)
                .HasColumnName("city")
                .HasMaxLength(100)
                .IsRequired();

            address.Property(a => a.State)
                .HasColumnName("state")
                .HasMaxLength(100)
                .IsRequired();

            address.Property(a => a.ZipCode)
                .HasColumnName("zip_code")
                .HasMaxLength(20)
                .IsRequired();

            address.Property(a => a.Country)
                .HasColumnName("country")
                .HasMaxLength(100)
                .IsRequired();
        });

        // Índices
        builder.HasIndex(u => u.Email)
            .IsUnique()
            .HasDatabaseName("ix_users_email");

        builder.HasIndex(u => u.Username)
            .IsUnique()
            .HasDatabaseName("ix_users_username");

        builder.HasIndex(u => u.Phone)
            .HasDatabaseName("ix_users_phone");

        builder.HasIndex(u => u.IsActive)
            .HasDatabaseName("ix_users_is_active");
    }
}