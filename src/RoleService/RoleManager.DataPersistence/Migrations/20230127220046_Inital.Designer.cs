﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using RoleManager.DataPersistence;

#nullable disable

namespace RoleManager.DataPersistence.Migrations
{
    [DbContext(typeof(RoleDbContext))]
    [Migration("20230127220046_Inital")]
    partial class Inital
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.12")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("RoleManager.Member", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("DisplayName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UniqueName")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UniqueName")
                        .IsUnique();

                    b.ToTable("Members");
                });

            modelBuilder.Entity("RoleManager.MemberRole", b =>
                {
                    b.Property<Guid>("MemberId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("TreeId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("RoleId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("MemberId", "TreeId");

                    b.HasIndex("RoleId");

                    b.HasIndex("TreeId");

                    b.ToTable("MemberRoles", (string)null);
                });

            modelBuilder.Entity("RoleManager.Node", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("BaseNode")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<Guid>("TreeId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("TreeId");

                    b.HasIndex("Name", "TreeId")
                        .IsUnique();

                    b.ToTable("Nodes");
                });

            modelBuilder.Entity("RoleManager.Role", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("Reversible")
                        .HasColumnType("bit");

                    b.Property<Guid>("TreeId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("TreeId");

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("RoleManager.RoleNode", b =>
                {
                    b.Property<Guid>("RoleId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("NodeId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Order")
                        .HasColumnType("int");

                    b.HasKey("RoleId", "NodeId");

                    b.HasIndex("NodeId");

                    b.ToTable("RoleNodes", (string)null);
                });

            modelBuilder.Entity("RoleManager.RoleRequiredNode", b =>
                {
                    b.Property<Guid>("RoleId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("NodeId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("RoleId", "NodeId");

                    b.HasIndex("NodeId");

                    b.ToTable("RoleRequiredNodes", (string)null);
                });

            modelBuilder.Entity("RoleManager.Tree", b =>
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

            modelBuilder.Entity("RoleManager.TreeRequiredNode", b =>
                {
                    b.Property<Guid>("TreeId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("NodeId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("TreeId", "NodeId");

                    b.HasIndex("NodeId");

                    b.ToTable("TreeRequiredNodes", (string)null);
                });

            modelBuilder.Entity("RoleManager.MemberRole", b =>
                {
                    b.HasOne("RoleManager.Member", null)
                        .WithMany("Roles")
                        .HasForeignKey("MemberId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("RoleManager.Role", "Role")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("RoleManager.Tree", null)
                        .WithMany()
                        .HasForeignKey("TreeId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Role");
                });

            modelBuilder.Entity("RoleManager.Node", b =>
                {
                    b.HasOne("RoleManager.Tree", "Tree")
                        .WithMany()
                        .HasForeignKey("TreeId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Tree");
                });

            modelBuilder.Entity("RoleManager.Role", b =>
                {
                    b.HasOne("RoleManager.Tree", "Tree")
                        .WithMany()
                        .HasForeignKey("TreeId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Tree");
                });

            modelBuilder.Entity("RoleManager.RoleNode", b =>
                {
                    b.HasOne("RoleManager.Node", "Node")
                        .WithMany()
                        .HasForeignKey("NodeId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("RoleManager.Role", null)
                        .WithMany("Nodes")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Node");
                });

            modelBuilder.Entity("RoleManager.RoleRequiredNode", b =>
                {
                    b.HasOne("RoleManager.Node", "Node")
                        .WithMany()
                        .HasForeignKey("NodeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("RoleManager.Role", null)
                        .WithMany("RequiredNodes")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Node");
                });

            modelBuilder.Entity("RoleManager.TreeRequiredNode", b =>
                {
                    b.HasOne("RoleManager.Node", "Node")
                        .WithMany()
                        .HasForeignKey("NodeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("RoleManager.Tree", null)
                        .WithMany("RequiredNodes")
                        .HasForeignKey("TreeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Node");
                });

            modelBuilder.Entity("RoleManager.Member", b =>
                {
                    b.Navigation("Roles");
                });

            modelBuilder.Entity("RoleManager.Role", b =>
                {
                    b.Navigation("Nodes");

                    b.Navigation("RequiredNodes");
                });

            modelBuilder.Entity("RoleManager.Tree", b =>
                {
                    b.Navigation("RequiredNodes");
                });
#pragma warning restore 612, 618
        }
    }
}