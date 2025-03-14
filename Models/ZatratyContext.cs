﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ZatratyCore.Models;

public partial class ZatratyContext : DbContext
{
    public ZatratyContext()
    {
    }

    public ZatratyContext(DbContextOptions<ZatratyContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Autorize> Autorizes { get; set; }

    public virtual DbSet<Expense> Expenses { get; set; }

    public virtual DbSet<Fillial> Fillials { get; set; }

    public virtual DbSet<FillialsExpense> FillialsExpenses { get; set; }

    public virtual DbSet<Form4F> Form4Fs { get; set; }

    public virtual DbSet<GetExp> GetExps { get; set; }

    public virtual DbSet<Plan> Plans { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<TableFillialsExpense> TableFillialsExpenses { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=s25hv;Initial Catalog=Zatraty;User ID=Zatuser;Password=ZatuserZatuser;Encrypt=False");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Autorize>(entity =>
        {
            entity.ToTable("Autorize");

            entity.Property(e => e.DateMod).HasColumnType("datetime");
            entity.Property(e => e.Employee).HasMaxLength(250);
            entity.Property(e => e.FillialId).HasColumnName("FillialID");
            entity.Property(e => e.Login).HasMaxLength(50);
            entity.Property(e => e.Password).HasMaxLength(50);
            entity.Property(e => e.Primechanie).HasMaxLength(250);
            entity.Property(e => e.UserMod).HasMaxLength(50);

            entity.HasOne(d => d.Fillial).WithMany(p => p.Autorizes)
                .HasForeignKey(d => d.FillialId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Autorize_Fillials");
        });

        modelBuilder.Entity<Expense>(entity =>
        {
            entity.Property(e => e.ChildId).HasColumnName("ChildID");
            entity.Property(e => e.DateMod).HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(250);
            entity.Property(e => e.Primechanie).HasMaxLength(250);
            entity.Property(e => e.UserMod).HasMaxLength(50);
        });

        modelBuilder.Entity<Fillial>(entity =>
        {
            entity.Property(e => e.DateMod).HasColumnType("datetime");
            entity.Property(e => e.Filial).HasMaxLength(50);
            entity.Property(e => e.Primechanie).HasMaxLength(250);
            entity.Property(e => e.UserMod).HasMaxLength(50);
        });

        modelBuilder.Entity<FillialsExpense>(entity =>
        {
            entity.Property(e => e.Date).HasColumnType("datetime");
            entity.Property(e => e.DateMod).HasColumnType("datetime");
            entity.Property(e => e.FillialId).HasColumnName("FillialID");
            entity.Property(e => e.Primechanie).HasMaxLength(250);
            entity.Property(e => e.UserMod).HasMaxLength(50);

            entity.HasOne(d => d.Fillial).WithMany(p => p.FillialsExpenses)
                .HasForeignKey(d => d.FillialId)
                .HasConstraintName("FK_FillialsExpenses_Fillials");
        });

        modelBuilder.Entity<Form4F>(entity =>
        {
            entity.ToTable("Form4F");

            entity.Property(e => e.DateMod).HasColumnType("datetime");
            entity.Property(e => e.ExpensesId).HasColumnName("ExpensesID");
            entity.Property(e => e.FilialId).HasColumnName("FilialID");
            entity.Property(e => e.PlanId).HasColumnName("PlanID");
            entity.Property(e => e.Primechanie).HasMaxLength(150);
            entity.Property(e => e.UserMod).HasMaxLength(50);
            entity.Property(e => e.Value).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.Expenses).WithMany(p => p.Form4Fs)
                .HasForeignKey(d => d.ExpensesId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Form4F_Expenses");

            entity.HasOne(d => d.Filial).WithMany(p => p.Form4Fs)
                .HasForeignKey(d => d.FilialId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Form4F_Fillials");

            entity.HasOne(d => d.Plan).WithMany(p => p.Form4Fs)
                .HasForeignKey(d => d.PlanId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Form4F_Plans");
        });

        modelBuilder.Entity<GetExp>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("get_Exp");

            entity.Property(e => e.DateMod).HasColumnType("datetime");
            entity.Property(e => e.FullName).HasMaxLength(255);
            entity.Property(e => e.Primechanie).HasMaxLength(250);
            entity.Property(e => e.UserMod).HasMaxLength(50);
        });

        modelBuilder.Entity<Plan>(entity =>
        {
            entity.Property(e => e.DateMod).HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(250);
            entity.Property(e => e.Primechanie).HasMaxLength(250);
            entity.Property(e => e.UserMod).HasMaxLength(50);
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Rols");

            entity.Property(e => e.DateMod).HasColumnType("datetime");
            entity.Property(e => e.FillialId).HasColumnName("FillialID");
            entity.Property(e => e.PlansId).HasColumnName("PlansID");
            entity.Property(e => e.UserMod).HasMaxLength(50);

            entity.HasOne(d => d.Fillial).WithMany(p => p.Roles)
                .HasForeignKey(d => d.FillialId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Rols_Fillials");

            entity.HasOne(d => d.Plans).WithMany(p => p.Roles)
                .HasForeignKey(d => d.PlansId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Rols_Plans");
        });

        modelBuilder.Entity<TableFillialsExpense>(entity =>
        {
            entity.Property(e => e.DateMod).HasColumnType("datetime");
            entity.Property(e => e.ExpensesId).HasColumnName("ExpensesID");
            entity.Property(e => e.FillialExpensesId).HasColumnName("FillialExpensesID");
            entity.Property(e => e.PlanId).HasColumnName("PlanID");
            entity.Property(e => e.Primechanie).HasMaxLength(250);
            entity.Property(e => e.UserMod).HasMaxLength(50);
            entity.Property(e => e.Value).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.Expenses).WithMany(p => p.TableFillialsExpenses)
                .HasForeignKey(d => d.ExpensesId)
                .HasConstraintName("FK_TableFillialsExpenses_Expenses");

            entity.HasOne(d => d.FillialExpenses).WithMany(p => p.TableFillialsExpenses)
                .HasForeignKey(d => d.FillialExpensesId)
                .HasConstraintName("FK_TableFillialsExpenses_FillialsExpenses");

            entity.HasOne(d => d.Plan).WithMany(p => p.TableFillialsExpenses)
                .HasForeignKey(d => d.PlanId)
                .HasConstraintName("FK_TableFillialsExpenses_Plans");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}