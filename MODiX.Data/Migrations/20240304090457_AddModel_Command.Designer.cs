﻿// <auto-generated />
using System;
using MODiX.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace MODiX.Data.Migrations
{
    [DbContext(typeof(ModixDbContext))]
    [Migration("20240304090457_AddModel_Command")]
    partial class AddModel_Command
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("MODiX.Data.Models.Backpack", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("MemberId")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Backpack");
                });

            modelBuilder.Entity("MODiX.Data.Models.Command", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<int>("Cooldown")
                        .HasColumnType("integer");

                    b.Property<int>("Limit")
                        .HasColumnType("integer");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<string>("ServerId")
                        .HasColumnType("text");

                    b.Property<int>("Timeout")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("Commands");
                });

            modelBuilder.Entity("MODiX.Data.Models.Item", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("BackpackId")
                        .HasColumnType("text");

                    b.Property<Guid?>("BackpackId1")
                        .HasColumnType("uuid");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("BackpackId1");

                    b.ToTable("Item");
                });

            modelBuilder.Entity("MODiX.Data.Models.LocalChannelMessage", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("AuthorId")
                        .HasColumnType("text");

                    b.Property<Guid>("ChannelId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid?>("LocalServerMemberId")
                        .HasColumnType("uuid");

                    b.Property<string>("MessageContent")
                        .HasColumnType("text");

                    b.Property<string>("ServerId")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("LocalServerMemberId");

                    b.ToTable("Messages");
                });

            modelBuilder.Entity("MODiX.Data.Models.LocalServerMember", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid?>("BackpackId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("JoinedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Nickname")
                        .HasColumnType("text");

                    b.Property<long[]>("RoleIds")
                        .IsRequired()
                        .HasColumnType("bigint[]");

                    b.Property<string>("ServerId")
                        .HasColumnType("text");

                    b.Property<string>("UserId")
                        .HasColumnType("text");

                    b.Property<Guid?>("WalletId")
                        .HasColumnType("uuid");

                    b.Property<int>("Warnings")
                        .HasColumnType("integer");

                    b.Property<long>("Xp")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("BackpackId");

                    b.HasIndex("WalletId");

                    b.ToTable("ServerMembers");
                });

            modelBuilder.Entity("MODiX.Data.Models.Suggestion", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<bool>("Approved")
                        .HasColumnType("boolean");

                    b.Property<Guid?>("AuthorId")
                        .HasColumnType("uuid");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.HasIndex("AuthorId");

                    b.ToTable("Suggestions");
                });

            modelBuilder.Entity("MODiX.Data.Models.Ticket", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Content")
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("CreatorId")
                        .HasColumnType("uuid");

                    b.Property<int>("Status")
                        .HasColumnType("integer");

                    b.Property<int>("TicketType")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("CreatorId");

                    b.ToTable("Tickets");
                });

            modelBuilder.Entity("MODiX.Data.Models.Wallet", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("MemberId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("Points")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("Wallet");
                });

            modelBuilder.Entity("MODiX.Data.Models.Item", b =>
                {
                    b.HasOne("MODiX.Data.Models.Backpack", null)
                        .WithMany("Items")
                        .HasForeignKey("BackpackId1");
                });

            modelBuilder.Entity("MODiX.Data.Models.LocalChannelMessage", b =>
                {
                    b.HasOne("MODiX.Data.Models.LocalServerMember", null)
                        .WithMany("Messages")
                        .HasForeignKey("LocalServerMemberId");
                });

            modelBuilder.Entity("MODiX.Data.Models.LocalServerMember", b =>
                {
                    b.HasOne("MODiX.Data.Models.Backpack", "Backpack")
                        .WithMany()
                        .HasForeignKey("BackpackId");

                    b.HasOne("MODiX.Data.Models.Wallet", "Wallet")
                        .WithMany()
                        .HasForeignKey("WalletId");

                    b.Navigation("Backpack");

                    b.Navigation("Wallet");
                });

            modelBuilder.Entity("MODiX.Data.Models.Suggestion", b =>
                {
                    b.HasOne("MODiX.Data.Models.LocalServerMember", "Author")
                        .WithMany()
                        .HasForeignKey("AuthorId");

                    b.Navigation("Author");
                });

            modelBuilder.Entity("MODiX.Data.Models.Ticket", b =>
                {
                    b.HasOne("MODiX.Data.Models.LocalServerMember", "Creator")
                        .WithMany()
                        .HasForeignKey("CreatorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Creator");
                });

            modelBuilder.Entity("MODiX.Data.Models.Backpack", b =>
                {
                    b.Navigation("Items");
                });

            modelBuilder.Entity("MODiX.Data.Models.LocalServerMember", b =>
                {
                    b.Navigation("Messages");
                });
#pragma warning restore 612, 618
        }
    }
}
