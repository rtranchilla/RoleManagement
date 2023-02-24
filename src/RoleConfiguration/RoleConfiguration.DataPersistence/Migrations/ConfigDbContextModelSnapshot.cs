﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using RoleConfiguration.DataPersistence;

#nullable disable

namespace RoleConfiguration.DataPersistence.Migrations
{
    [DbContext(typeof(ConfigDbContext))]
    partial class ConfigDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.14")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("RoleConfiguration.File", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Path")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<Guid>("SourceId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("SourceId");

                    b.HasIndex("Path", "SourceId")
                        .IsUnique();

                    b.ToTable("Files");
                });

            modelBuilder.Entity("RoleConfiguration.FileMember", b =>
                {
                    b.Property<Guid>("FileId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("MemberId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("FileId", "MemberId");

                    b.HasIndex("MemberId");

                    b.ToTable("FileMembers", (string)null);
                });

            modelBuilder.Entity("RoleConfiguration.FileRole", b =>
                {
                    b.Property<Guid>("FileId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("RoleId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("FileId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("FileRoles", (string)null);
                });

            modelBuilder.Entity("RoleConfiguration.FileTree", b =>
                {
                    b.Property<Guid>("FileId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("TreeId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("FileId", "TreeId");

                    b.HasIndex("TreeId");

                    b.ToTable("FileTrees", (string)null);
                });

            modelBuilder.Entity("RoleConfiguration.Member", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("UniqueName")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UniqueName")
                        .IsUnique();

                    b.ToTable("Members");
                });

            modelBuilder.Entity("RoleConfiguration.Role", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<Guid>("TreeId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("TreeId");

                    b.HasIndex("Name", "TreeId")
                        .IsUnique();

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("RoleConfiguration.Source", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Sources");
                });

            modelBuilder.Entity("RoleConfiguration.Tree", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Trees");
                });

            modelBuilder.Entity("RoleConfiguration.File", b =>
                {
                    b.HasOne("RoleConfiguration.Source", "Source")
                        .WithMany()
                        .HasForeignKey("SourceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Source");
                });

            modelBuilder.Entity("RoleConfiguration.FileMember", b =>
                {
                    b.HasOne("RoleConfiguration.File", null)
                        .WithMany("Members")
                        .HasForeignKey("FileId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("RoleConfiguration.Member", "Member")
                        .WithMany()
                        .HasForeignKey("MemberId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Member");
                });

            modelBuilder.Entity("RoleConfiguration.FileRole", b =>
                {
                    b.HasOne("RoleConfiguration.File", null)
                        .WithMany("Roles")
                        .HasForeignKey("FileId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("RoleConfiguration.Role", "Role")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Role");
                });

            modelBuilder.Entity("RoleConfiguration.FileTree", b =>
                {
                    b.HasOne("RoleConfiguration.File", null)
                        .WithMany("Trees")
                        .HasForeignKey("FileId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("RoleConfiguration.Tree", "Tree")
                        .WithMany()
                        .HasForeignKey("TreeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Tree");
                });

            modelBuilder.Entity("RoleConfiguration.Role", b =>
                {
                    b.HasOne("RoleConfiguration.Tree", "Tree")
                        .WithMany()
                        .HasForeignKey("TreeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Tree");
                });

            modelBuilder.Entity("RoleConfiguration.File", b =>
                {
                    b.Navigation("Members");

                    b.Navigation("Roles");

                    b.Navigation("Trees");
                });
#pragma warning restore 612, 618
        }
    }
}