﻿// <auto-generated />
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Pgvector;
using backend;

#nullable disable

namespace backend.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20240630091950_Discord11")]
    partial class Discord11
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.HasPostgresExtension(modelBuilder, "vector");
            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex");

                    b.ToTable("AspNetRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("text");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("text");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("text");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("text");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("text");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("text");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("text");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("text");

                    b.Property<string>("RoleId")
                        .HasColumnType("text");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("text");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<string>("Value")
                        .HasColumnType("text");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("backend.Models.ApplicationUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("integer");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("text");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("boolean");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("boolean");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("text");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("text");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("boolean");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("text");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("boolean");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex");

                    b.ToTable("AspNetUsers", (string)null);
                });

            modelBuilder.Entity("backend.Models.Discord.Attachment", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<string>("FileName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("FileSizeBytes")
                        .HasColumnType("integer");

                    b.Property<string>("MessageId")
                        .HasColumnType("text");

                    b.Property<string>("Url")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("MessageId");

                    b.ToTable("DiscordAttachments");
                });

            modelBuilder.Entity("backend.Models.Discord.Channel", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<string>("Category")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("CategoryId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Topic")
                        .HasColumnType("text");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("DiscordChannels");
                });

            modelBuilder.Entity("backend.Models.Discord.DiscordServer", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("ChannelId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("ExportedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("GuildId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("ChannelId");

                    b.HasIndex("GuildId");

                    b.ToTable("DiscordServers");
                });

            modelBuilder.Entity("backend.Models.Discord.DiscordUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<string>("AvatarUrl")
                        .HasColumnType("text");

                    b.Property<string>("Color")
                        .HasColumnType("text");

                    b.Property<string>("Discriminator")
                        .HasColumnType("text");

                    b.Property<bool>("IsBot")
                        .HasColumnType("boolean");

                    b.Property<string>("MessageId")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("NickName")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("MessageId");

                    b.ToTable("DiscordUsers");
                });

            modelBuilder.Entity("backend.Models.Discord.Guild", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<string>("IconUrl")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("DiscordGuilds");
                });

            modelBuilder.Entity("backend.Models.Discord.Message", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<string>("AuthorId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime?>("CallEndedTimeStamp")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid?>("DiscordServerId")
                        .HasColumnType("uuid");

                    b.Property<bool>("IsPinned")
                        .HasColumnType("boolean");

                    b.Property<DateTime>("TimeStamp")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime?>("TimeStampEdited")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("AuthorId");

                    b.HasIndex("DiscordServerId");

                    b.ToTable("DiscordMessages");
                });

            modelBuilder.Entity("backend.Models.Entity.Audio", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<List<string>>("AuthorizedRoles")
                        .IsRequired()
                        .HasColumnType("text[]");

                    b.Property<string>("ContentId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("DateUploaded")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("Duration")
                        .HasColumnType("integer");

                    b.Property<string>("FileExtension")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("OwnerId")
                        .HasColumnType("text");

                    b.Property<string>("Tags")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("OwnerId");

                    b.ToTable("Audio");
                });

            modelBuilder.Entity("backend.Models.Entity.GenericFile", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<List<string>>("AuthorizedRoles")
                        .IsRequired()
                        .HasColumnType("text[]");

                    b.Property<string>("ContentId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("DateUploaded")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("FileExtension")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("OwnerId")
                        .HasColumnType("text");

                    b.Property<string>("Tags")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("OwnerId");

                    b.ToTable("Files");
                });

            modelBuilder.Entity("backend.Models.Entity.Image", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<List<string>>("AuthorizedRoles")
                        .IsRequired()
                        .HasColumnType("text[]");

                    b.Property<Vector>("ClipEmbedding")
                        .HasColumnType("vector(768)");

                    b.Property<string>("ContentId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("DateUploaded")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("FileExtension")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("GeneratedCaption")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("OwnerId")
                        .HasColumnType("text");

                    b.Property<string>("Tags")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("ClipEmbedding")
                        .HasAnnotation("Npgsql:StorageParameter:ef_construction", 64)
                        .HasAnnotation("Npgsql:StorageParameter:m", 16);

                    NpgsqlIndexBuilderExtensions.HasMethod(b.HasIndex("ClipEmbedding"), "hnsw");
                    NpgsqlIndexBuilderExtensions.HasOperators(b.HasIndex("ClipEmbedding"), new[] { "vector_l2_ops" });

                    b.HasIndex("OwnerId");

                    b.ToTable("Images");
                });

            modelBuilder.Entity("backend.Models.Entity.LongVideo", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("AudioTranscription")
                        .HasColumnType("text");

                    b.Property<List<string>>("AuthorizedRoles")
                        .IsRequired()
                        .HasColumnType("text[]");

                    b.Property<string>("ContentId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("DateUploaded")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("Duration")
                        .HasColumnType("integer");

                    b.Property<string>("FileExtension")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("OwnerId")
                        .HasColumnType("text");

                    b.Property<string>("Tags")
                        .HasColumnType("text");

                    b.Property<Guid?>("ThumbnailId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("OwnerId");

                    b.HasIndex("ThumbnailId");

                    b.ToTable("LongVideos");
                });

            modelBuilder.Entity("backend.Models.Entity.Reel", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("AudioTranscription")
                        .HasColumnType("text");

                    b.Property<List<string>>("AuthorizedRoles")
                        .IsRequired()
                        .HasColumnType("text[]");

                    b.Property<string>("ContentId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("DateUploaded")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("Duration")
                        .HasColumnType("integer");

                    b.Property<string>("FileExtension")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("OwnerId")
                        .HasColumnType("text");

                    b.Property<string>("Tags")
                        .HasColumnType("text");

                    b.Property<Guid?>("ThumbnailId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("OwnerId");

                    b.HasIndex("ThumbnailId");

                    b.ToTable("Reels");
                });

            modelBuilder.Entity("backend.Models.Entity.Thumbnail", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<List<string>>("AuthorizedRoles")
                        .IsRequired()
                        .HasColumnType("text[]");

                    b.Property<string>("ContentId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("DateUploaded")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("FileExtension")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("OwnerId")
                        .HasColumnType("text");

                    b.Property<string>("Tags")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("OwnerId");

                    b.ToTable("Thumbnails");
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
                    b.HasOne("backend.Models.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("backend.Models.ApplicationUser", null)
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

                    b.HasOne("backend.Models.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("backend.Models.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("backend.Models.Discord.Attachment", b =>
                {
                    b.HasOne("backend.Models.Discord.Message", null)
                        .WithMany("Attachments")
                        .HasForeignKey("MessageId");
                });

            modelBuilder.Entity("backend.Models.Discord.DiscordServer", b =>
                {
                    b.HasOne("backend.Models.Discord.Channel", "Channel")
                        .WithMany()
                        .HasForeignKey("ChannelId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("backend.Models.Discord.Guild", "Guild")
                        .WithMany()
                        .HasForeignKey("GuildId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Channel");

                    b.Navigation("Guild");
                });

            modelBuilder.Entity("backend.Models.Discord.DiscordUser", b =>
                {
                    b.HasOne("backend.Models.Discord.Message", null)
                        .WithMany("Mentions")
                        .HasForeignKey("MessageId");
                });

            modelBuilder.Entity("backend.Models.Discord.Message", b =>
                {
                    b.HasOne("backend.Models.Discord.DiscordUser", "Author")
                        .WithMany()
                        .HasForeignKey("AuthorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("backend.Models.Discord.DiscordServer", null)
                        .WithMany("Messages")
                        .HasForeignKey("DiscordServerId");

                    b.OwnsMany("backend.Models.Discord.Embed", "Embeds", b1 =>
                        {
                            b1.Property<string>("MessageId")
                                .HasColumnType("text");

                            b1.Property<Guid>("Id")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("uuid");

                            b1.Property<string>("Description")
                                .HasColumnType("text");

                            b1.Property<DateTime?>("TimeStamp")
                                .HasColumnType("timestamp with time zone");

                            b1.Property<string>("Title")
                                .IsRequired()
                                .HasColumnType("text");

                            b1.Property<string>("Url")
                                .IsRequired()
                                .HasColumnType("text");

                            b1.HasKey("MessageId", "Id");

                            b1.ToTable("Embed");

                            b1.WithOwner()
                                .HasForeignKey("MessageId");

                            b1.OwnsOne("backend.Models.Discord.EmbedAuthor", "Author", b2 =>
                                {
                                    b2.Property<string>("EmbedMessageId")
                                        .HasColumnType("text");

                                    b2.Property<Guid>("EmbedId")
                                        .HasColumnType("uuid");

                                    b2.HasKey("EmbedMessageId", "EmbedId");

                                    b2.ToTable("Embed");

                                    b2.WithOwner()
                                        .HasForeignKey("EmbedMessageId", "EmbedId");
                                });

                            b1.OwnsOne("backend.Models.Discord.EmbedThumbnail", "Thumbnail", b2 =>
                                {
                                    b2.Property<string>("EmbedMessageId")
                                        .HasColumnType("text");

                                    b2.Property<Guid>("EmbedId")
                                        .HasColumnType("uuid");

                                    b2.Property<int>("Height")
                                        .HasColumnType("integer");

                                    b2.Property<string>("Url")
                                        .IsRequired()
                                        .HasColumnType("text");

                                    b2.Property<int>("Width")
                                        .HasColumnType("integer");

                                    b2.HasKey("EmbedMessageId", "EmbedId");

                                    b2.ToTable("Embed");

                                    b2.WithOwner()
                                        .HasForeignKey("EmbedMessageId", "EmbedId");
                                });

                            b1.OwnsOne("backend.Models.Discord.EmbedVideo", "EmbedVideo", b2 =>
                                {
                                    b2.Property<string>("EmbedMessageId")
                                        .HasColumnType("text");

                                    b2.Property<Guid>("EmbedId")
                                        .HasColumnType("uuid");

                                    b2.Property<int>("Height")
                                        .HasColumnType("integer");

                                    b2.Property<string>("Url")
                                        .IsRequired()
                                        .HasColumnType("text");

                                    b2.Property<int>("Width")
                                        .HasColumnType("integer");

                                    b2.HasKey("EmbedMessageId", "EmbedId");

                                    b2.ToTable("Embed");

                                    b2.WithOwner()
                                        .HasForeignKey("EmbedMessageId", "EmbedId");
                                });

                            b1.Navigation("Author");

                            b1.Navigation("EmbedVideo");

                            b1.Navigation("Thumbnail");
                        });

                    b.OwnsMany("backend.Models.Discord.Reaction", "Reactions", b1 =>
                        {
                            b1.Property<string>("MessageId")
                                .HasColumnType("text");

                            b1.Property<int>("Id")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("integer");

                            NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b1.Property<int>("Id"));

                            b1.Property<int>("Count")
                                .HasColumnType("integer");

                            b1.HasKey("MessageId", "Id");

                            b1.ToTable("Reaction");

                            b1.WithOwner()
                                .HasForeignKey("MessageId");

                            b1.OwnsOne("backend.Models.Discord.Emoji", "Emoji", b2 =>
                                {
                                    b2.Property<string>("ReactionMessageId")
                                        .HasColumnType("text");

                                    b2.Property<int>("ReactionId")
                                        .HasColumnType("integer");

                                    b2.Property<string>("Code")
                                        .IsRequired()
                                        .HasColumnType("text");

                                    b2.Property<string>("Id")
                                        .IsRequired()
                                        .HasColumnType("text");

                                    b2.Property<string>("ImageUrl")
                                        .IsRequired()
                                        .HasColumnType("text");

                                    b2.Property<bool>("IsAnimated")
                                        .HasColumnType("boolean");

                                    b2.Property<string>("Name")
                                        .IsRequired()
                                        .HasColumnType("text");

                                    b2.HasKey("ReactionMessageId", "ReactionId");

                                    b2.ToTable("Reaction");

                                    b2.WithOwner()
                                        .HasForeignKey("ReactionMessageId", "ReactionId");
                                });

                            b1.Navigation("Emoji")
                                .IsRequired();
                        });

                    b.Navigation("Author");

                    b.Navigation("Embeds");

                    b.Navigation("Reactions");
                });

            modelBuilder.Entity("backend.Models.Entity.Audio", b =>
                {
                    b.HasOne("backend.Models.ApplicationUser", "Owner")
                        .WithMany()
                        .HasForeignKey("OwnerId");

                    b.Navigation("Owner");
                });

            modelBuilder.Entity("backend.Models.Entity.GenericFile", b =>
                {
                    b.HasOne("backend.Models.ApplicationUser", "Owner")
                        .WithMany()
                        .HasForeignKey("OwnerId");

                    b.Navigation("Owner");
                });

            modelBuilder.Entity("backend.Models.Entity.Image", b =>
                {
                    b.HasOne("backend.Models.ApplicationUser", "Owner")
                        .WithMany()
                        .HasForeignKey("OwnerId");

                    b.Navigation("Owner");
                });

            modelBuilder.Entity("backend.Models.Entity.LongVideo", b =>
                {
                    b.HasOne("backend.Models.ApplicationUser", "Owner")
                        .WithMany()
                        .HasForeignKey("OwnerId");

                    b.HasOne("backend.Models.Entity.Thumbnail", "Thumbnail")
                        .WithMany()
                        .HasForeignKey("ThumbnailId");

                    b.Navigation("Owner");

                    b.Navigation("Thumbnail");
                });

            modelBuilder.Entity("backend.Models.Entity.Reel", b =>
                {
                    b.HasOne("backend.Models.ApplicationUser", "Owner")
                        .WithMany()
                        .HasForeignKey("OwnerId");

                    b.HasOne("backend.Models.Entity.Thumbnail", "Thumbnail")
                        .WithMany()
                        .HasForeignKey("ThumbnailId");

                    b.Navigation("Owner");

                    b.Navigation("Thumbnail");
                });

            modelBuilder.Entity("backend.Models.Entity.Thumbnail", b =>
                {
                    b.HasOne("backend.Models.ApplicationUser", "Owner")
                        .WithMany()
                        .HasForeignKey("OwnerId");

                    b.Navigation("Owner");
                });

            modelBuilder.Entity("backend.Models.Discord.DiscordServer", b =>
                {
                    b.Navigation("Messages");
                });

            modelBuilder.Entity("backend.Models.Discord.Message", b =>
                {
                    b.Navigation("Attachments");

                    b.Navigation("Mentions");
                });
#pragma warning restore 612, 618
        }
    }
}