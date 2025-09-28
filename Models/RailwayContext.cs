using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Scaffolding.Internal;

namespace dotnet_webapi_ef.Models;

public partial class RailwayContext : DbContext
{
    public RailwayContext()
    {
    }

    public RailwayContext(DbContextOptions<RailwayContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Lottery> Lotteries { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<Result> Results { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<WalletTxn> WalletTxns { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseMySql("server=shuttle.proxy.rlwy.net;port=58058;database=railway;user=root;password=QsAuLMzTPcKIQABatuHFyUggFThSdqRv;sslmode=Required", Microsoft.EntityFrameworkCore.ServerVersion.Parse("9.4.0-mysql"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Lottery>(entity =>
        {
            entity.HasKey(e => e.Lid).HasName("PRIMARY");

            entity
                .ToTable("Lottery")
                .UseCollation("utf8mb4_general_ci");

            entity.HasIndex(e => e.Uid, "fk_lottery_users");

            entity.Property(e => e.Lid).HasColumnName("lid");
            entity.Property(e => e.Date)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("date");
            entity.Property(e => e.EndDate).HasColumnName("End_Date");
            entity.Property(e => e.Number)
                .HasMaxLength(32)
                .HasColumnName("number");
            entity.Property(e => e.Price)
                .HasPrecision(14, 2)
                .HasColumnName("price");
            entity.Property(e => e.StartDate).HasColumnName("Start_Date");
            entity.Property(e => e.Status)
                .IsRequired()
                .HasDefaultValueSql("'1'")
                .HasColumnName("status");
            entity.Property(e => e.Total).HasColumnName("total");
            entity.Property(e => e.Uid).HasColumnName("uid");

            entity.HasOne(d => d.UidNavigation).WithMany(p => p.Lotteries)
                .HasForeignKey(d => d.Uid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_lottery_users");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.Oid).HasName("PRIMARY");

            entity
                .ToTable("Order")
                .UseCollation("utf8mb4_general_ci");

            entity.HasIndex(e => e.Lid, "fk_order_lottery");

            entity.HasIndex(e => e.Uid, "fk_order_users");

            entity.Property(e => e.Oid).HasColumnName("oid");
            entity.Property(e => e.Date)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("date");
            entity.Property(e => e.Lid).HasColumnName("lid");
            entity.Property(e => e.Status)
                .IsRequired()
                .HasDefaultValueSql("'1'")
                .HasColumnName("status");
            entity.Property(e => e.Uid).HasColumnName("uid");

            entity.HasOne(d => d.LidNavigation).WithMany(p => p.Orders)
                .HasForeignKey(d => d.Lid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_order_lottery");

            entity.HasOne(d => d.UidNavigation).WithMany(p => p.Orders)
                .HasForeignKey(d => d.Uid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_order_users");
        });

        modelBuilder.Entity<Result>(entity =>
        {
            entity.HasKey(e => e.Rid).HasName("PRIMARY");

            entity
                .ToTable("Result")
                .UseCollation("utf8mb4_general_ci");

            entity.Property(e => e.Rid).HasColumnName("rid");
            entity.Property(e => e.Amount)
                .HasMaxLength(50)
                .HasColumnName("amount");
            entity.Property(e => e.PayoutRate)
                .HasPrecision(12, 2)
                .HasColumnName("payout_rate");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Uid).HasName("PRIMARY");

            entity.UseCollation("utf8mb4_general_ci");

            entity.HasIndex(e => e.Email, "email").IsUnique();

            entity.HasIndex(e => e.Phone, "phone").IsUnique();

            entity.Property(e => e.Uid).HasColumnName("uid");
            entity.Property(e => e.Balance)
                .HasPrecision(14, 2)
                .HasColumnName("balance");
            entity.Property(e => e.BankName)
                .HasMaxLength(100)
                .HasColumnName("bank_name");
            entity.Property(e => e.BankNumber)
                .HasMaxLength(30)
                .HasColumnName("bank_number");
            entity.Property(e => e.Date)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("date");
            entity.Property(e => e.Email).HasColumnName("email");
            entity.Property(e => e.FullName)
                .HasMaxLength(100)
                .HasColumnName("full_name");
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .HasColumnName("password");
            entity.Property(e => e.Phone)
                .HasMaxLength(20)
                .HasColumnName("phone");
            entity.Property(e => e.Role)
                .HasMaxLength(20)
                .HasDefaultValueSql("'admin'")
                .HasColumnName("role");
        });

        modelBuilder.Entity<WalletTxn>(entity =>
        {
            entity.HasKey(e => e.Wid).HasName("PRIMARY");

            entity
                .ToTable("WalletTxn")
                .UseCollation("utf8mb4_general_ci");

            entity.HasIndex(e => e.Uid, "fk_wallettxn_users");

            entity.Property(e => e.Wid).HasColumnName("wid");
            entity.Property(e => e.Date)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("date");
            entity.Property(e => e.Status).HasColumnName("status");
            entity.Property(e => e.TopUp)
                .HasPrecision(14, 2)
                .HasDefaultValueSql("'0.00'")
                .HasColumnName("top_up");
            entity.Property(e => e.Uid).HasColumnName("uid");
            entity.Property(e => e.Withdraw)
                .HasPrecision(14, 2)
                .HasDefaultValueSql("'0.00'")
                .HasColumnName("withdraw");

            entity.HasOne(d => d.UidNavigation).WithMany(p => p.WalletTxns)
                .HasForeignKey(d => d.Uid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_wallettxn_users");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
