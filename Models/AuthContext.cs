using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace TestApi.Models;

public partial class AuthContext : DbContext
{
   

    public AuthContext(DbContextOptions<AuthContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AspNetRole> AspNetRoles { get; set; }

    public virtual DbSet<AspNetRoleClaim> AspNetRoleClaims { get; set; }

    public virtual DbSet<AspNetUser> AspNetUsers { get; set; }

    public virtual DbSet<AspNetUserClaim> AspNetUserClaims { get; set; }

    public virtual DbSet<AspNetUserLogin> AspNetUserLogins { get; set; }

    public virtual DbSet<AspNetUserToken> AspNetUserTokens { get; set; }

    public virtual DbSet<Boat> Boats { get; set; }

    public virtual DbSet<CardDetail> CardDetails { get; set; }

    public virtual DbSet<Chat> Chats { get; set; }

    public virtual DbSet<ChatMessage> ChatMessages { get; set; }

    public virtual DbSet<Conversation> Conversations { get; set; }

    public virtual DbSet<Email> Emails { get; set; }

    public virtual DbSet<EmailVerificationModel> EmailVerificationModels { get; set; }

    public virtual DbSet<Equipment> Equipments { get; set; }

    public virtual DbSet<FeedBack> FeedBacks { get; set; }

    public virtual DbSet<Reservation> Reservations { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserConversation> UserConversations { get; set; }

   
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AspNetRole>(entity =>
        {
            entity.HasIndex(e => e.NormalizedName, "RoleNameIndex")
                .IsUnique()
                .HasFilter("([NormalizedName] IS NOT NULL)");

            entity.Property(e => e.Name).HasMaxLength(256);
            entity.Property(e => e.NormalizedName).HasMaxLength(256);
        });

        modelBuilder.Entity<AspNetRoleClaim>(entity =>
        {
            entity.HasIndex(e => e.RoleId, "IX_AspNetRoleClaims_RoleId");

            entity.HasOne(d => d.Role).WithMany(p => p.AspNetRoleClaims).HasForeignKey(d => d.RoleId);
        });

        modelBuilder.Entity<AspNetUser>(entity =>
        {
            entity.HasIndex(e => e.NormalizedEmail, "EmailIndex");

            entity.HasIndex(e => e.NormalizedUserName, "UserNameIndex")
                .IsUnique()
                .HasFilter("([NormalizedUserName] IS NOT NULL)");

            entity.Property(e => e.Email).HasMaxLength(256);
            entity.Property(e => e.FirstName).HasMaxLength(256);
            entity.Property(e => e.LastName).HasMaxLength(256);
            entity.Property(e => e.NormalizedEmail).HasMaxLength(256);
            entity.Property(e => e.NormalizedUserName).HasMaxLength(256);
            entity.Property(e => e.UserName).HasMaxLength(256);

            entity.HasMany(d => d.Roles).WithMany(p => p.Users)
                .UsingEntity<Dictionary<string, object>>(
                    "AspNetUserRole",
                    r => r.HasOne<AspNetRole>().WithMany().HasForeignKey("RoleId"),
                    l => l.HasOne<AspNetUser>().WithMany().HasForeignKey("UserId"),
                    j =>
                    {
                        j.HasKey("UserId", "RoleId");
                        j.ToTable("AspNetUserRoles");
                        j.HasIndex(new[] { "RoleId" }, "IX_AspNetUserRoles_RoleId");
                    });
        });

        modelBuilder.Entity<AspNetUserClaim>(entity =>
        {
            entity.HasIndex(e => e.UserId, "IX_AspNetUserClaims_UserId");

            entity.HasOne(d => d.User).WithMany(p => p.AspNetUserClaims).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<AspNetUserLogin>(entity =>
        {
            entity.HasKey(e => new { e.LoginProvider, e.ProviderKey });

            entity.HasIndex(e => e.UserId, "IX_AspNetUserLogins_UserId");

            entity.HasOne(d => d.User).WithMany(p => p.AspNetUserLogins).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<AspNetUserToken>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.LoginProvider, e.Name });

            entity.HasOne(d => d.User).WithMany(p => p.AspNetUserTokens).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<Boat>(entity =>
        {
            entity.HasIndex(e => e.IdFeedBack, "IX_Boats_idFeedBack");

            entity.Property(e => e.AverageRating).HasColumnName("averageRating");
            entity.Property(e => e.BoatType)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.City)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("city");
            entity.Property(e => e.Country)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("country");
            entity.Property(e => e.Description)
                .HasMaxLength(2000)
                .IsUnicode(false);
            entity.Property(e => e.IdFeedBack).HasColumnName("idFeedBack");
            entity.Property(e => e.ImageUrl)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Type)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<CardDetail>(entity =>
        {
            entity.HasNoKey();

            entity.Property(e => e.CardNumber)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Cvv)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("CVV");
        });

        modelBuilder.Entity<Chat>(entity =>
        {
            entity.ToTable("Chat");

            entity.Property(e => e.Date).HasColumnType("date");
            entity.Property(e => e.Message)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<ChatMessage>(entity =>
        {
            entity.ToTable("ChatMessage");

            entity.Property(e => e.CreatedAt)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("createdAt");
            entity.Property(e => e.Message)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.IdReciverNavigation).WithMany(p => p.ChatMessageIdReciverNavigations)
                .HasForeignKey(d => d.IdReciver)
                .HasConstraintName("FK_ChatMessage_User1");

            entity.HasOne(d => d.IdSenderNavigation).WithMany(p => p.ChatMessageIdSenderNavigations)
                .HasForeignKey(d => d.IdSender)
                .HasConstraintName("FK_ChatMessage_User");
        });

        modelBuilder.Entity<Conversation>(entity =>
        {
            entity.Property(e => e.GroupName)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Email>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("Email");

            entity.Property(e => e.Body)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Subject)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.To)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<EmailVerificationModel>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("EmailVerificationModel");

            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.VerificationCode)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Equipment>(entity =>
        {
            entity.Property(e => e.Gps).HasColumnName("GPS");

            entity.HasOne(d => d.IdBoatNavigation).WithMany(p => p.Equipment)
                .HasForeignKey(d => d.IdBoat)
                .HasConstraintName("FK_Equipments_Boats");
        });

        modelBuilder.Entity<FeedBack>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Table_1");

            entity.ToTable("FeedBack");

            entity.Property(e => e.Avg).HasColumnName("avg");
            entity.Property(e => e.Comment)
                .HasMaxLength(1000)
                .IsUnicode(false);

            entity.HasOne(d => d.IdUserNavigation).WithMany(p => p.FeedBacks)
                .HasForeignKey(d => d.IdUser)
                .HasConstraintName("FK_FeedBack_Boats");
        });

        modelBuilder.Entity<Reservation>(entity =>
        {
            entity.ToTable("Reservation");

            entity.Property(e => e.DateDebut)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.DateFin)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.EndDate)
                .HasColumnType("date")
                .HasColumnName("endDate");
            entity.Property(e => e.RéservantName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("réservantName");
            entity.Property(e => e.StartDate)
                .HasColumnType("date")
                .HasColumnName("startDate");

            entity.HasOne(d => d.IdBoatNavigation).WithMany(p => p.Reservations)
                .HasForeignKey(d => d.IdBoat)
                .HasConstraintName("FK_Reservation_Boats");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.ToTable("Role");

            entity.Property(e => e.RoleName)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_User_1");

            entity.ToTable("User");

            entity.HasIndex(e => e.IdBoat, "IX_User_IdBoat");

            entity.HasIndex(e => e.IdChat, "IX_User_IdChat");

            entity.Property(e => e.Avatar)
                .HasMaxLength(1000)
                .IsUnicode(false);
            entity.Property(e => e.City)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ConnectionId)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Country)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.DateInscription).HasColumnType("date");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.Langue)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.Password)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("password");
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.StreetAddress)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.VerificationCode)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.VerificationToken)
                .HasMaxLength(200)
                .IsUnicode(false);

            entity.HasOne(d => d.IdRoleNavigation).WithMany(p => p.Users)
                .HasForeignKey(d => d.IdRole)
                .HasConstraintName("FK_User_Role");
        });

        modelBuilder.Entity<UserConversation>(entity =>
        {
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.IdConversationNavigation).WithMany(p => p.UserConversations)
                .HasForeignKey(d => d.IdConversation)
                .HasConstraintName("FK_UserConversations_UserConversations");

            entity.HasOne(d => d.User).WithMany(p => p.UserConversations)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_UserConversations_User");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
