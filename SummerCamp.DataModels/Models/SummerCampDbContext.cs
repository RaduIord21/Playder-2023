using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace SummerCamp.DataModels.Models;

public partial class SummerCampDbContext : DbContext
{
    public SummerCampDbContext()
    {
    }

    public SummerCampDbContext(DbContextOptions<SummerCampDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Coach> Coaches { get; set; }

    public virtual DbSet<Competition> Competitions { get; set; }

    public virtual DbSet<CompetitionMatch> CompetitionMatches { get; set; }

    public virtual DbSet<CompetitonTeam> CompetitonTeams { get; set; }

    public virtual DbSet<Player> Players { get; set; }

    public virtual DbSet<Sponsor> Sponsors { get; set; }

    public virtual DbSet<Team> Teams { get; set; }

    public virtual DbSet<TeamSponsor> TeamSponsors { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=ConnectionStrings:SummerCamp");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Coach>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Coach__3214EC079C992343");

            entity.ToTable("Coach");

            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Competition>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Competit__3213E83FCABBDF78");

            entity.ToTable("Competition");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Adress)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.EndDate).HasColumnType("date");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.StartDate).HasColumnType("date");

            entity.HasOne(d => d.Sponsor).WithMany(p => p.Competitions)
                .HasForeignKey(d => d.SponsorId)
                .HasConstraintName("FK__Competiti__Spons__46E78A0C");
        });

        modelBuilder.Entity<CompetitionMatch>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Competit__3214EC078ED1AAE7");

            entity.ToTable("CompetitionMatch");

            entity.HasOne(d => d.AwayTeam).WithMany(p => p.CompetitionMatchAwayTeams)
                .HasForeignKey(d => d.AwayTeamId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Competiti__AwayT__5EBF139D");

            entity.HasOne(d => d.Competition).WithMany(p => p.CompetitionMatches)
                .HasForeignKey(d => d.CompetitionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Competiti__Compe__5FB337D6");

            entity.HasOne(d => d.HomeTeam).WithMany(p => p.CompetitionMatchHomeTeams)
                .HasForeignKey(d => d.HomeTeamId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Competiti__HomeT__60A75C0F");
        });

        modelBuilder.Entity<CompetitonTeam>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Competit__3213E83FAE053D48");

            entity.ToTable("CompetitonTeam");

            entity.Property(e => e.Id).HasColumnName("id");

            entity.HasOne(d => d.Competition).WithMany(p => p.CompetitonTeams)
                .HasForeignKey(d => d.CompetitionId)
                .HasConstraintName("FK__Competito__Compe__4AB81AF0");

            entity.HasOne(d => d.Team).WithMany(p => p.CompetitonTeams)
                .HasForeignKey(d => d.TeamId)
                .HasConstraintName("FK__Competito__TeamI__4BAC3F29");
        });

        modelBuilder.Entity<Player>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Player__3213E83F1D715B35");

            entity.ToTable("Player");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Adress)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.BirthDate).HasColumnType("date");
            entity.Property(e => e.FullName)
                .HasMaxLength(255)
                .IsUnicode(false);

            entity.HasOne(d => d.Team).WithMany(p => p.Players)
                .HasForeignKey(d => d.TeamId)
                .HasConstraintName("FK__Player__TeamId__3C69FB99");
        });

        modelBuilder.Entity<Sponsor>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Sponsor__3213E83F66C8CDC5");

            entity.ToTable("Sponsor");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Team>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Team__3214EC0791ECDEC3");

            entity.ToTable("Team");

            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.NickName)
                .HasMaxLength(255)
                .IsUnicode(false);

            entity.HasOne(d => d.Coach).WithMany(p => p.Teams)
                .HasForeignKey(d => d.CoachId)
                .HasConstraintName("FK__Team__CoachId__398D8EEE");
        });

        modelBuilder.Entity<TeamSponsor>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TeamSpon__3213E83FC75DC58E");

            entity.ToTable("TeamSponsor");

            entity.Property(e => e.Id).HasColumnName("id");

            entity.HasOne(d => d.Sponsor).WithMany(p => p.TeamSponsors)
                .HasForeignKey(d => d.SponsorId)
                .HasConstraintName("FK__TeamSpons__Spons__4316F928");

            entity.HasOne(d => d.Team).WithMany(p => p.TeamSponsors)
                .HasForeignKey(d => d.TeamId)
                .HasConstraintName("FK__TeamSpons__TeamI__4222D4EF");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
