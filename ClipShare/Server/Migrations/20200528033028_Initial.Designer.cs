﻿// <auto-generated />
using System;
using ClipShare.Server.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace ClipShare.Server.Migrations;

[DbContext(typeof(AppDb))]
[Migration("20200528033028_Initial")]
partial class Initial
{
    protected override void BuildTargetModel(ModelBuilder modelBuilder)
    {
#pragma warning disable 612, 618
        modelBuilder
            .HasAnnotation("ProductVersion", "3.1.4");

        modelBuilder.Entity("ClipShare.Server.Models.LogEntry", b =>
            {
                b.Property<int>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("INTEGER");

                b.Property<string>("Category")
                    .HasColumnType("TEXT");

                b.Property<string>("EventId")
                    .HasColumnType("TEXT");

                b.Property<string>("ExceptionMessage")
                    .HasColumnType("TEXT");

                b.Property<string>("ExceptionStack")
                    .HasColumnType("TEXT");

                b.Property<int>("LogLevel")
                    .HasColumnType("INTEGER");

                b.Property<string>("ScopeStack")
                    .HasColumnType("TEXT");

                b.Property<string>("State")
                    .HasColumnType("TEXT");

                b.Property<string>("Timestamp")
                    .IsRequired()
                    .HasColumnType("TEXT");

                b.HasKey("Id");

                b.ToTable("Logs");
            });

        modelBuilder.Entity("ClipShare.Shared.Models.ArchiveFolder", b =>
            {
                b.Property<int>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("INTEGER");

                b.Property<string>("Name")
                    .HasColumnType("TEXT")
                    .HasMaxLength(300);

                b.Property<string>("UserId")
                    .HasColumnType("TEXT");

                b.HasKey("Id");

                b.HasIndex("UserId");

                b.ToTable("ArchiveFolders");
            });

        modelBuilder.Entity("ClipShare.Shared.Models.Clip", b =>
            {
                b.Property<int>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("INTEGER");

                b.Property<int?>("ArchiveFolderId")
                    .HasColumnType("INTEGER");

                b.Property<string>("Content")
                    .HasColumnType("TEXT")
                    .HasMaxLength(5000);

                b.Property<string>("Timestamp")
                    .IsRequired()
                    .HasColumnType("TEXT");

                b.Property<string>("UserId")
                    .HasColumnType("TEXT");

                b.HasKey("Id");

                b.HasIndex("ArchiveFolderId");

                b.HasIndex("UserId");

                b.ToTable("Clips");
            });

        modelBuilder.Entity("ClipShare.Shared.Models.ClipsUser", b =>
            {
                b.Property<string>("Id")
                    .HasColumnType("TEXT");

                b.Property<int>("AccessFailedCount")
                    .HasColumnType("INTEGER");

                b.Property<string>("ConcurrencyStamp")
                    .IsConcurrencyToken()
                    .HasColumnType("TEXT");

                b.Property<string>("Email")
                    .HasColumnType("TEXT")
                    .HasMaxLength(256);

                b.Property<bool>("EmailConfirmed")
                    .HasColumnType("INTEGER");

                b.Property<bool>("LockoutEnabled")
                    .HasColumnType("INTEGER");

                b.Property<string>("LockoutEnd")
                    .HasColumnType("TEXT");

                b.Property<string>("NormalizedEmail")
                    .HasColumnType("TEXT")
                    .HasMaxLength(256);

                b.Property<string>("NormalizedUserName")
                    .HasColumnType("TEXT")
                    .HasMaxLength(256);

                b.Property<string>("PasswordHash")
                    .HasColumnType("TEXT");

                b.Property<string>("PhoneNumber")
                    .HasColumnType("TEXT");

                b.Property<bool>("PhoneNumberConfirmed")
                    .HasColumnType("INTEGER");

                b.Property<string>("SecurityStamp")
                    .HasColumnType("TEXT");

                b.Property<bool>("TwoFactorEnabled")
                    .HasColumnType("INTEGER");

                b.Property<string>("UserName")
                    .HasColumnType("TEXT")
                    .HasMaxLength(256);

                b.HasKey("Id");

                b.HasIndex("NormalizedEmail")
                    .HasName("EmailIndex");

                b.HasIndex("NormalizedUserName")
                    .IsUnique()
                    .HasName("UserNameIndex");

                b.ToTable("ClipsUsers");
            });

        modelBuilder.Entity("IdentityServer4.EntityFramework.Entities.DeviceFlowCodes", b =>
            {
                b.Property<string>("UserCode")
                    .HasColumnType("TEXT")
                    .HasMaxLength(200);

                b.Property<string>("ClientId")
                    .IsRequired()
                    .HasColumnType("TEXT")
                    .HasMaxLength(200);

                b.Property<DateTime>("CreationTime")
                    .HasColumnType("TEXT");

                b.Property<string>("Data")
                    .IsRequired()
                    .HasColumnType("TEXT")
                    .HasMaxLength(50000);

                b.Property<string>("DeviceCode")
                    .IsRequired()
                    .HasColumnType("TEXT")
                    .HasMaxLength(200);

                b.Property<DateTime?>("Expiration")
                    .IsRequired()
                    .HasColumnType("TEXT");

                b.Property<string>("SubjectId")
                    .HasColumnType("TEXT")
                    .HasMaxLength(200);

                b.HasKey("UserCode");

                b.HasIndex("DeviceCode")
                    .IsUnique();

                b.HasIndex("Expiration");

                b.ToTable("DeviceCodes");
            });

        modelBuilder.Entity("IdentityServer4.EntityFramework.Entities.PersistedGrant", b =>
            {
                b.Property<string>("Key")
                    .HasColumnType("TEXT")
                    .HasMaxLength(200);

                b.Property<string>("ClientId")
                    .IsRequired()
                    .HasColumnType("TEXT")
                    .HasMaxLength(200);

                b.Property<DateTime>("CreationTime")
                    .HasColumnType("TEXT");

                b.Property<string>("Data")
                    .IsRequired()
                    .HasColumnType("TEXT")
                    .HasMaxLength(50000);

                b.Property<DateTime?>("Expiration")
                    .HasColumnType("TEXT");

                b.Property<string>("SubjectId")
                    .HasColumnType("TEXT")
                    .HasMaxLength(200);

                b.Property<string>("Type")
                    .IsRequired()
                    .HasColumnType("TEXT")
                    .HasMaxLength(50);

                b.HasKey("Key");

                b.HasIndex("Expiration");

                b.HasIndex("SubjectId", "ClientId", "Type");

                b.ToTable("PersistedGrants");
            });

        modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
            {
                b.Property<string>("Id")
                    .HasColumnType("TEXT");

                b.Property<string>("ConcurrencyStamp")
                    .IsConcurrencyToken()
                    .HasColumnType("TEXT");

                b.Property<string>("Name")
                    .HasColumnType("TEXT")
                    .HasMaxLength(256);

                b.Property<string>("NormalizedName")
                    .HasColumnType("TEXT")
                    .HasMaxLength(256);

                b.HasKey("Id");

                b.HasIndex("NormalizedName")
                    .IsUnique()
                    .HasName("RoleNameIndex");

                b.ToTable("AspNetRoles");
            });

        modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
            {
                b.Property<int>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("INTEGER");

                b.Property<string>("ClaimType")
                    .HasColumnType("TEXT");

                b.Property<string>("ClaimValue")
                    .HasColumnType("TEXT");

                b.Property<string>("RoleId")
                    .IsRequired()
                    .HasColumnType("TEXT");

                b.HasKey("Id");

                b.HasIndex("RoleId");

                b.ToTable("AspNetRoleClaims");
            });

        modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
            {
                b.Property<int>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("INTEGER");

                b.Property<string>("ClaimType")
                    .HasColumnType("TEXT");

                b.Property<string>("ClaimValue")
                    .HasColumnType("TEXT");

                b.Property<string>("UserId")
                    .IsRequired()
                    .HasColumnType("TEXT");

                b.HasKey("Id");

                b.HasIndex("UserId");

                b.ToTable("AspNetUserClaims");
            });

        modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
            {
                b.Property<string>("LoginProvider")
                    .HasColumnType("TEXT")
                    .HasMaxLength(128);

                b.Property<string>("ProviderKey")
                    .HasColumnType("TEXT")
                    .HasMaxLength(128);

                b.Property<string>("ProviderDisplayName")
                    .HasColumnType("TEXT");

                b.Property<string>("UserId")
                    .IsRequired()
                    .HasColumnType("TEXT");

                b.HasKey("LoginProvider", "ProviderKey");

                b.HasIndex("UserId");

                b.ToTable("AspNetUserLogins");
            });

        modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
            {
                b.Property<string>("UserId")
                    .HasColumnType("TEXT");

                b.Property<string>("RoleId")
                    .HasColumnType("TEXT");

                b.HasKey("UserId", "RoleId");

                b.HasIndex("RoleId");

                b.ToTable("AspNetUserRoles");
            });

        modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
            {
                b.Property<string>("UserId")
                    .HasColumnType("TEXT");

                b.Property<string>("LoginProvider")
                    .HasColumnType("TEXT")
                    .HasMaxLength(128);

                b.Property<string>("Name")
                    .HasColumnType("TEXT")
                    .HasMaxLength(128);

                b.Property<string>("Value")
                    .HasColumnType("TEXT");

                b.HasKey("UserId", "LoginProvider", "Name");

                b.ToTable("AspNetUserTokens");
            });

        modelBuilder.Entity("ClipShare.Shared.Models.ArchiveFolder", b =>
            {
                b.HasOne("ClipShare.Shared.Models.ClipsUser", "User")
                    .WithMany("ArchiveFolders")
                    .HasForeignKey("UserId");
            });

        modelBuilder.Entity("ClipShare.Shared.Models.Clip", b =>
            {
                b.HasOne("ClipShare.Shared.Models.ArchiveFolder", "ArchiveFolder")
                    .WithMany("Clips")
                    .HasForeignKey("ArchiveFolderId");

                b.HasOne("ClipShare.Shared.Models.ClipsUser", "User")
                    .WithMany("Clips")
                    .HasForeignKey("UserId");
            });

        modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
            {
                b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                    .WithMany()
                    .HasForeignKey("RoleId")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();
            });

        modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
            {
                b.HasOne("ClipShare.Shared.Models.ClipsUser", null)
                    .WithMany()
                    .HasForeignKey("UserId")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();
            });

        modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
            {
                b.HasOne("ClipShare.Shared.Models.ClipsUser", null)
                    .WithMany()
                    .HasForeignKey("UserId")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();
            });

        modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
            {
                b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                    .WithMany()
                    .HasForeignKey("RoleId")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();

                b.HasOne("ClipShare.Shared.Models.ClipsUser", null)
                    .WithMany()
                    .HasForeignKey("UserId")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();
            });

        modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
            {
                b.HasOne("ClipShare.Shared.Models.ClipsUser", null)
                    .WithMany()
                    .HasForeignKey("UserId")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();
            });
#pragma warning restore 612, 618
    }
}
