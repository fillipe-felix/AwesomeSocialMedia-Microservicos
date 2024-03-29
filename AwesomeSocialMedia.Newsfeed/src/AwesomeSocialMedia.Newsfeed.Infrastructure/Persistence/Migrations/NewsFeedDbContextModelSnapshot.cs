﻿// <auto-generated />
using System;
using AwesomeSocialMedia.Newsfeed.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace AwesomeSocialMedia.Newsfeed.Infrastructure.Persistence.Migrations
{
    [DbContext(typeof(NewsFeedDbContext))]
    partial class NewsFeedDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("AwesomeSocialMedia.Newsfeed.Core.Core.Entities.Post", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("PublishedAt")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("UserNewsfeedId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("UserNewsfeedId");

                    b.ToTable("Posts");
                });

            modelBuilder.Entity("AwesomeSocialMedia.Newsfeed.Core.Core.Entities.UserNewsfeed", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.ToTable("UserNewsfeeds");
                });

            modelBuilder.Entity("AwesomeSocialMedia.Newsfeed.Core.Core.Entities.Post", b =>
                {
                    b.HasOne("AwesomeSocialMedia.Newsfeed.Core.Core.Entities.UserNewsfeed", null)
                        .WithMany("Posts")
                        .HasForeignKey("UserNewsfeedId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("AwesomeSocialMedia.Newsfeed.Core.Core.Entities.UserNewsfeed", b =>
                {
                    b.OwnsOne("AwesomeSocialMedia.Newsfeed.Core.Core.Entities.User", "User", b1 =>
                        {
                            b1.Property<Guid>("UserNewsfeedId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<string>("DisplayName")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)")
                                .HasColumnName("DisplayName");

                            b1.Property<Guid>("Id")
                                .HasColumnType("uniqueidentifier")
                                .HasColumnName("UserId");

                            b1.HasKey("UserNewsfeedId");

                            b1.ToTable("UserNewsfeeds");

                            b1.WithOwner()
                                .HasForeignKey("UserNewsfeedId");
                        });

                    b.Navigation("User")
                        .IsRequired();
                });

            modelBuilder.Entity("AwesomeSocialMedia.Newsfeed.Core.Core.Entities.UserNewsfeed", b =>
                {
                    b.Navigation("Posts");
                });
#pragma warning restore 612, 618
        }
    }
}
