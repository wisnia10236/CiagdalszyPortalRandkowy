﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PortalRandkowy.API.Data;

namespace PortalRandkowy.API.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20200815123617_mysqlinitial")]
    partial class mysqlinitial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("PortalRandkowy.API.Models.Like", b =>
                {
                    b.Property<int>("UserLikesId")
                        .HasColumnType("int");

                    b.Property<int>("UserIsLikedId")
                        .HasColumnType("int");

                    b.HasKey("UserLikesId", "UserIsLikedId");

                    b.HasIndex("UserIsLikedId");

                    b.ToTable("Likes");
                });

            modelBuilder.Entity("PortalRandkowy.API.Models.Message", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Content")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<DateTime?>("DateRead")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime>("DateSend")
                        .HasColumnType("datetime(6)");

                    b.Property<bool>("IsRead")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("RecipientDeleted")
                        .HasColumnType("tinyint(1)");

                    b.Property<int>("RecipientId")
                        .HasColumnType("int");

                    b.Property<bool>("SenderDeleted")
                        .HasColumnType("tinyint(1)");

                    b.Property<int>("SenderId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("RecipientId");

                    b.HasIndex("SenderId");

                    b.ToTable("Messages");
                });

            modelBuilder.Entity("PortalRandkowy.API.Models.Photo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime>("DateAdded")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Description")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<bool>("IsMain")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Url")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<string>("public_id")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Photos");
                });

            modelBuilder.Entity("PortalRandkowy.API.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Children")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("City")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Country")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime>("DateOfBirth")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Description")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Education")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("EyeColor")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("FreeTime")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("FriendeWouldDescribeMe")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Gender")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Growth")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("HairColor")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("ILike")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("IdoNotLike")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Interests")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("ItFeelsBestIn")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Languages")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<DateTime>("LastActive")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("LookingFor")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("MakesMeLaugh")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("MartialStatus")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Motto")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Movies")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Music")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<byte[]>("PasswordHash")
                        .HasColumnType("longblob");

                    b.Property<byte[]>("PasswordSalt")
                        .HasColumnType("longblob");

                    b.Property<string>("Personality")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Profession")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Sport")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Username")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("ZodiacSign")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("PortalRandkowy.API.Models.Value", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.HasKey("Id");

                    b.ToTable("Values");
                });

            modelBuilder.Entity("PortalRandkowy.API.Models.Like", b =>
                {
                    b.HasOne("PortalRandkowy.API.Models.User", "UserIsLiked")
                        .WithMany("UserLikes")
                        .HasForeignKey("UserIsLikedId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("PortalRandkowy.API.Models.User", "UserLikes")
                        .WithMany("UserIsLiked")
                        .HasForeignKey("UserLikesId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                });

            modelBuilder.Entity("PortalRandkowy.API.Models.Message", b =>
                {
                    b.HasOne("PortalRandkowy.API.Models.User", "Recipient")
                        .WithMany("MessagesRecived")
                        .HasForeignKey("RecipientId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("PortalRandkowy.API.Models.User", "Sender")
                        .WithMany("MessagesSend")
                        .HasForeignKey("SenderId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                });

            modelBuilder.Entity("PortalRandkowy.API.Models.Photo", b =>
                {
                    b.HasOne("PortalRandkowy.API.Models.User", "User")
                        .WithMany("Photos")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}