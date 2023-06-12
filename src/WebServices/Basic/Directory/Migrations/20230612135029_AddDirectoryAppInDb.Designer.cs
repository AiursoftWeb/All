﻿// <auto-generated />
using System;
using Aiursoft.Directory.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Aiursoft.Directory.Migrations
{
    [DbContext(typeof(DirectoryDbContext))]
    [Migration("20230612135029_AddDirectoryAppInDb")]
    partial class AddDirectoryAppInDb
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.16")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("Aiursoft.Directory.Models.AppGrant", b =>
                {
                    b.Property<int>("AppGrantId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("AppGrantId"), 1L, 1);

                    b.Property<string>("AppId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DirectoryUserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("GrantTime")
                        .HasColumnType("datetime2");

                    b.HasKey("AppGrantId");

                    b.HasIndex("DirectoryUserId");

                    b.ToTable("LocalAppGrant");
                });

            modelBuilder.Entity("Aiursoft.Directory.Models.AuditLogLocal", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("AppId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("HappenTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("IPAddress")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Success")
                        .HasColumnType("bit");

                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AuditLogs");
                });

            modelBuilder.Entity("Aiursoft.Directory.Models.DirectoryAppInDb", b =>
                {
                    b.Property<string>("AppId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("AppCreateTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("AppDescription")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("AppDomain")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("AppFailCallbackUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("AppName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("AppSecret")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("ChangeBasicInfo")
                        .HasColumnType("bit");

                    b.Property<bool>("ChangeGrantInfo")
                        .HasColumnType("bit");

                    b.Property<bool>("ChangePassword")
                        .HasColumnType("bit");

                    b.Property<bool>("ChangePhoneNumber")
                        .HasColumnType("bit");

                    b.Property<bool>("ConfirmEmail")
                        .HasColumnType("bit");

                    b.Property<string>("CreatorId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<bool>("DebugMode")
                        .HasColumnType("bit");

                    b.Property<bool>("EnableOAuth")
                        .HasColumnType("bit");

                    b.Property<bool>("ForceConfirmation")
                        .HasColumnType("bit");

                    b.Property<bool>("ForceInputPassword")
                        .HasColumnType("bit");

                    b.Property<string>("IconPath")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LicenseUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("ManageSocialAccount")
                        .HasColumnType("bit");

                    b.Property<string>("PrivacyStatementUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TrustedApp")
                        .HasColumnType("bit");

                    b.Property<bool>("ViewAuditLog")
                        .HasColumnType("bit");

                    b.Property<bool>("ViewOpenId")
                        .HasColumnType("bit");

                    b.Property<bool>("ViewPhoneNumber")
                        .HasColumnType("bit");

                    b.HasKey("AppId");

                    b.HasIndex("CreatorId");

                    b.ToTable("DirectoryAppInDb");
                });

            modelBuilder.Entity("Aiursoft.Directory.Models.DirectoryUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<DateTime>("AccountCreateTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("Bio")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<bool>("Has2FAKey")
                        .HasColumnType("bit");

                    b.Property<string>("IconFilePath")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("NickName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PreferedLanguage")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RegisterIPAddress")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SMSPasswordResetToken")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Sex")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers", (string)null);
                });

            modelBuilder.Entity("Aiursoft.Directory.Models.OAuthPack", b =>
                {
                    b.Property<int>("OAuthPackId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("OAuthPackId"), 1L, 1);

                    b.Property<TimeSpan>("AliveTime")
                        .HasColumnType("time");

                    b.Property<string>("ApplyAppId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Code")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreateTime")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("UseTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("OAuthPackId");

                    b.HasIndex("UserId");

                    b.ToTable("OAuthPack");
                });

            modelBuilder.Entity("Aiursoft.Directory.Models.ThirdPartyAccount", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<DateTime>("BindTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("OpenId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("OwnerId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("OwnerId");

                    b.ToTable("ThirdPartyAccounts");
                });

            modelBuilder.Entity("Aiursoft.Directory.Models.UserEmail", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("EmailAddress")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("LastSendTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("OwnerId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("Priority")
                        .HasColumnType("int");

                    b.Property<string>("ValidateToken")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Validated")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.HasIndex("OwnerId");

                    b.ToTable("UserEmails");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("RoleId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("Aiursoft.Directory.Models.AppGrant", b =>
                {
                    b.HasOne("Aiursoft.Directory.Models.DirectoryUser", "User")
                        .WithMany("GrantedApps")
                        .HasForeignKey("DirectoryUserId");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Aiursoft.Directory.Models.AuditLogLocal", b =>
                {
                    b.HasOne("Aiursoft.Directory.Models.DirectoryUser", "User")
                        .WithMany("AuditLogs")
                        .HasForeignKey("UserId");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Aiursoft.Directory.Models.DirectoryAppInDb", b =>
                {
                    b.HasOne("Aiursoft.Directory.Models.DirectoryUser", "Creator")
                        .WithMany("CreatedApps")
                        .HasForeignKey("CreatorId");

                    b.Navigation("Creator");
                });

            modelBuilder.Entity("Aiursoft.Directory.Models.OAuthPack", b =>
                {
                    b.HasOne("Aiursoft.Directory.Models.DirectoryUser", "User")
                        .WithMany("Packs")
                        .HasForeignKey("UserId");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Aiursoft.Directory.Models.ThirdPartyAccount", b =>
                {
                    b.HasOne("Aiursoft.Directory.Models.DirectoryUser", "Owner")
                        .WithMany("ThirdPartyAccounts")
                        .HasForeignKey("OwnerId");

                    b.Navigation("Owner");
                });

            modelBuilder.Entity("Aiursoft.Directory.Models.UserEmail", b =>
                {
                    b.HasOne("Aiursoft.Directory.Models.DirectoryUser", "Owner")
                        .WithMany("Emails")
                        .HasForeignKey("OwnerId");

                    b.Navigation("Owner");
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
                    b.HasOne("Aiursoft.Directory.Models.DirectoryUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("Aiursoft.Directory.Models.DirectoryUser", null)
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

                    b.HasOne("Aiursoft.Directory.Models.DirectoryUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("Aiursoft.Directory.Models.DirectoryUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Aiursoft.Directory.Models.DirectoryUser", b =>
                {
                    b.Navigation("AuditLogs");

                    b.Navigation("CreatedApps");

                    b.Navigation("Emails");

                    b.Navigation("GrantedApps");

                    b.Navigation("Packs");

                    b.Navigation("ThirdPartyAccounts");
                });
#pragma warning restore 612, 618
        }
    }
}
