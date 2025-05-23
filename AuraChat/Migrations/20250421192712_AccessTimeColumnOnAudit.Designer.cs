﻿// <auto-generated />
using System;
using AuraChat.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace AuraChat.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20250421192712_AccessTimeColumnOnAudit")]
    partial class AccessTimeColumnOnAudit
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("AuraChat.Entities.Audit", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("AccessTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("EndPointPath")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("RequesterId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Audits");
                });

            modelBuilder.Entity("AuraChat.Entities.Chat", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int?>("UserAId")
                        .HasColumnType("int");

                    b.Property<int?>("UserBId")
                        .HasColumnType("int");

                    b.Property<int?>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserAId");

                    b.HasIndex("UserBId");

                    b.HasIndex("UserId");

                    b.ToTable("Chats");
                });

            modelBuilder.Entity("AuraChat.Entities.Group", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("CreatorId")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("GroupPictureGuid")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CreatorId")
                        .IsUnique();

                    b.HasIndex("UserId");

                    b.ToTable("Groups");
                });

            modelBuilder.Entity("AuraChat.Entities.Message", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int?>("ChatId")
                        .HasColumnType("int");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("GroupId")
                        .HasColumnType("int");

                    b.Property<string>("MediaGuid")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("SenderId")
                        .HasColumnType("int");

                    b.Property<DateTime>("SentTime")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("ChatId");

                    b.HasIndex("GroupId");

                    b.HasIndex("SenderId");

                    b.ToTable("Messages");
                });

            modelBuilder.Entity("AuraChat.Entities.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Bio")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("HashedPassword")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ProfilePictureGuid")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Salt")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.HasIndex("UserId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("AuraChat.Entities.UserGroup", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("GroupId")
                        .HasColumnType("int");

                    b.Property<int>("Role")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("GroupId");

                    b.HasIndex("UserId");

                    b.ToTable("UserGroup");
                });

            modelBuilder.Entity("AuraChat.Entities.Chat", b =>
                {
                    b.HasOne("AuraChat.Entities.User", "UserA")
                        .WithOne()
                        .HasForeignKey("AuraChat.Entities.Chat", "UserAId")
                        .OnDelete(DeleteBehavior.NoAction);

                    b.HasOne("AuraChat.Entities.User", "UserB")
                        .WithOne()
                        .HasForeignKey("AuraChat.Entities.Chat", "UserBId")
                        .OnDelete(DeleteBehavior.NoAction);

                    b.HasOne("AuraChat.Entities.User", null)
                        .WithMany("Chats")
                        .HasForeignKey("UserId");

                    b.Navigation("UserA");

                    b.Navigation("UserB");
                });

            modelBuilder.Entity("AuraChat.Entities.Group", b =>
                {
                    b.HasOne("AuraChat.Entities.User", "Creator")
                        .WithOne()
                        .HasForeignKey("AuraChat.Entities.Group", "CreatorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AuraChat.Entities.User", null)
                        .WithMany("Groups")
                        .HasForeignKey("UserId");

                    b.OwnsOne("AuraChat.Entities.GroupSettings", "Settings", b1 =>
                        {
                            b1.Property<int>("GroupId")
                                .HasColumnType("int");

                            b1.Property<bool>("AdminOnlyMessages")
                                .HasColumnType("bit");

                            b1.Property<bool>("AllowMembersToModifyInfo")
                                .HasColumnType("bit");

                            b1.Property<bool>("IsVisible")
                                .HasColumnType("bit");

                            b1.HasKey("GroupId");

                            b1.ToTable("Groups");

                            b1.WithOwner()
                                .HasForeignKey("GroupId");
                        });

                    b.Navigation("Creator");

                    b.Navigation("Settings")
                        .IsRequired();
                });

            modelBuilder.Entity("AuraChat.Entities.Message", b =>
                {
                    b.HasOne("AuraChat.Entities.Chat", null)
                        .WithMany("Messages")
                        .HasForeignKey("ChatId");

                    b.HasOne("AuraChat.Entities.Group", null)
                        .WithMany("Messages")
                        .HasForeignKey("GroupId");

                    b.HasOne("AuraChat.Entities.User", "Sender")
                        .WithMany()
                        .HasForeignKey("SenderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.OwnsMany("AuraChat.Entities.RecieverState", "RecieverState", b1 =>
                        {
                            b1.Property<int>("MessageId")
                                .HasColumnType("int");

                            b1.Property<int>("Id")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("int");

                            SqlServerPropertyBuilderExtensions.UseIdentityColumn(b1.Property<int>("Id"));

                            b1.Property<DateTime?>("IsRead")
                                .HasColumnType("datetime2");

                            b1.Property<DateTime?>("IsRecieved")
                                .HasColumnType("datetime2");

                            b1.Property<int>("RecieverId")
                                .HasColumnType("int");

                            b1.HasKey("MessageId", "Id");

                            b1.HasIndex("RecieverId");

                            b1.ToTable("RecieverState");

                            b1.WithOwner()
                                .HasForeignKey("MessageId");

                            b1.HasOne("AuraChat.Entities.User", "Reciever")
                                .WithMany()
                                .HasForeignKey("RecieverId")
                                .OnDelete(DeleteBehavior.NoAction)
                                .IsRequired();

                            b1.Navigation("Reciever");
                        });

                    b.Navigation("RecieverState");

                    b.Navigation("Sender");
                });

            modelBuilder.Entity("AuraChat.Entities.User", b =>
                {
                    b.HasOne("AuraChat.Entities.User", null)
                        .WithMany("Friends")
                        .HasForeignKey("UserId");

                    b.OwnsOne("AuraChat.Entities.UserSettings", "UserSettings", b1 =>
                        {
                            b1.Property<int>("UserId")
                                .HasColumnType("int");

                            b1.Property<bool>("FriendsOnlyGroupInvite")
                                .HasColumnType("bit");

                            b1.Property<bool>("FriendsOnlyMessages")
                                .HasColumnType("bit");

                            b1.Property<bool>("IsVisible")
                                .HasColumnType("bit");

                            b1.Property<int>("PasswordChangeCounter")
                                .HasColumnType("int");

                            b1.Property<bool>("TwoFactorAuthEnabled")
                                .HasColumnType("bit");

                            b1.HasKey("UserId");

                            b1.ToTable("Users");

                            b1.WithOwner()
                                .HasForeignKey("UserId");
                        });

                    b.Navigation("UserSettings")
                        .IsRequired();
                });

            modelBuilder.Entity("AuraChat.Entities.UserGroup", b =>
                {
                    b.HasOne("AuraChat.Entities.Group", "Group")
                        .WithMany("Members")
                        .HasForeignKey("GroupId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AuraChat.Entities.User", "User")
                        .WithMany("UsersGroups")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Group");

                    b.Navigation("User");
                });

            modelBuilder.Entity("AuraChat.Entities.Chat", b =>
                {
                    b.Navigation("Messages");
                });

            modelBuilder.Entity("AuraChat.Entities.Group", b =>
                {
                    b.Navigation("Members");

                    b.Navigation("Messages");
                });

            modelBuilder.Entity("AuraChat.Entities.User", b =>
                {
                    b.Navigation("Chats");

                    b.Navigation("Friends");

                    b.Navigation("Groups");

                    b.Navigation("UsersGroups");
                });
#pragma warning restore 612, 618
        }
    }
}
