using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using reto_Guardians.Models;

namespace reto_Guardians.DBContext;

public partial class AppDbContext : DbContext
{
    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Agendum> Agenda { get; set; }

    public virtual DbSet<Combate> Combates { get; set; }

    public virtual DbSet<Heroe> Heroes { get; set; }

    public virtual DbSet<Patrocinador> Patrocinadores { get; set; }

    public virtual DbSet<Persona> Personas { get; set; }

    public virtual DbSet<RelacionPersonal> RelacionesPersonales { get; set; }

    public virtual DbSet<Villano> Villanos { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=KAISER\\SQLEXPRESS;Initial Catalog=reto_Guardians;Integrated Security=True; Encrypt=False");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Agendum>(entity =>
        {
            entity.HasOne(d => d.IdHeroeNavigation).WithMany(p => p.Agenda)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Agenda_Heroe");
        });

        modelBuilder.Entity<Combate>(entity =>
        {
            entity.HasOne(d => d.IdHeroeNavigation).WithMany(p => p.Combates)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Heroe_Luchador");

            entity.HasOne(d => d.IdVillanoNavigation).WithMany(p => p.Combates)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Villano_Luchador");
        });

        modelBuilder.Entity<Heroe>(entity =>
        {
            entity.HasOne(d => d.IdPersonaNavigation).WithMany(p => p.Heroes)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Identidad_Heroe");
        });

        modelBuilder.Entity<Patrocinador>(entity =>
        {
            entity.HasOne(d => d.IdHeroeNavigation).WithMany(p => p.Patrocinadors)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Patrocinador_Heroe");
        });

        modelBuilder.Entity<RelacionPersonal>(entity =>
        {
            entity.HasOne(d => d.IdPersona1Navigation).WithMany(p => p.RelacionPersonalIdPersona1Navigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Primera_Persona");

            entity.HasOne(d => d.IdPersona2Navigation).WithMany(p => p.RelacionPersonalIdPersona2Navigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Segunda_Persona");
        });

        modelBuilder.Entity<Villano>(entity =>
        {
            entity.HasOne(d => d.IdPersonaNavigation).WithMany(p => p.Villanos)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Identidad_Villano");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
