using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ProyectoFinalMall.Models;

public partial class TiendaropaContext : DbContext
{
    public TiendaropaContext()
    {
    }

    public TiendaropaContext(DbContextOptions<TiendaropaContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Cliente> Cliente { get; set; }

    public virtual DbSet<Mercancia> Mercancia { get; set; }

    public virtual DbSet<Observacion> Observacion { get; set; }

    public virtual DbSet<Privilegios> Privilegios { get; set; }

    public virtual DbSet<Vistamercanciacliente> Vistamercanciacliente { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseMySql("server=localhost;user=root;password=root;database=tiendaropa", Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.30-mysql"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Cliente>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("cliente");

            entity.HasIndex(e => e.IdRol, "fkPrivilegios");

            entity.Property(e => e.Contrasena).HasMaxLength(256);
            entity.Property(e => e.Correo).HasMaxLength(90);
            entity.Property(e => e.Nombre).HasMaxLength(80);

            entity.HasOne(d => d.IdRolNavigation).WithMany(p => p.Cliente)
                .HasForeignKey(d => d.IdRol)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fkPrivilegios");
        });

        modelBuilder.Entity<Mercancia>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("mercancia");

            entity.HasIndex(e => e.IdCliente, "fkCliente");

            entity.Property(e => e.Descripcion).HasColumnType("text");
            entity.Property(e => e.Imagen).HasColumnType("text");
            entity.Property(e => e.Marca).HasMaxLength(50);
            entity.Property(e => e.Nacionalidad).HasMaxLength(60);
            entity.Property(e => e.Nombre).HasMaxLength(90);
            entity.Property(e => e.Precio).HasPrecision(6, 1);
            entity.Property(e => e.Tipo).HasMaxLength(60);

            entity.HasOne(d => d.IdClienteNavigation).WithMany(p => p.Mercancia)
                .HasForeignKey(d => d.IdCliente)
                .HasConstraintName("fkCliente");
        });

        modelBuilder.Entity<Observacion>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("observacion");

            entity.Property(e => e.Bitacora).HasColumnType("text");
            entity.Property(e => e.Correo).HasMaxLength(90);
            entity.Property(e => e.Fecha)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime");
        });

        modelBuilder.Entity<Privilegios>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("privilegios");

            entity.Property(e => e.Nombre).HasMaxLength(50);
        });

        modelBuilder.Entity<Vistamercanciacliente>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vistamercanciacliente");

            entity.Property(e => e.Correo).HasMaxLength(90);
            entity.Property(e => e.Descripcion).HasColumnType("text");
            entity.Property(e => e.Marca).HasMaxLength(50);
            entity.Property(e => e.Nacionalidad).HasMaxLength(60);
            entity.Property(e => e.NombreCliente).HasMaxLength(80);
            entity.Property(e => e.NombreMercancia).HasMaxLength(90);
            entity.Property(e => e.Precio).HasPrecision(6, 1);
            entity.Property(e => e.Tipo).HasMaxLength(60);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
