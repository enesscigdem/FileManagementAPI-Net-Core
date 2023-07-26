﻿// <auto-generated />
using System;
using FileOrbis.DataAccessLayer.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace FileOrbis.DataAccessLayer.Migrations
{
    [DbContext(typeof(FileOrbisContext))]
    [Migration("20230726051640_mig6")]
    partial class mig6
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.9")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("FileOrbis.EntityLayer.Concrete.FileInfos", b =>
                {
                    b.Property<int>("FileID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("FileID"));

                    b.Property<DateTime>("FileCreationDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("FileName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FilePath")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("FileSize")
                        .HasColumnType("float");

                    b.Property<int>("FolderID")
                        .HasColumnType("int");

                    b.HasKey("FileID");

                    b.HasIndex("FolderID");

                    b.ToTable("FileInfo");
                });

            modelBuilder.Entity("FileOrbis.EntityLayer.Concrete.FolderInfo", b =>
                {
                    b.Property<int>("FolderID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("FolderID"));

                    b.Property<DateTime>("FolderCreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("FolderName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FolderPath")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UserID")
                        .HasColumnType("int");

                    b.HasKey("FolderID");

                    b.HasIndex("UserID");

                    b.ToTable("FolderInfo");
                });

            modelBuilder.Entity("FileOrbis.EntityLayer.Concrete.UserInfo", b =>
                {
                    b.Property<int>("UserID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("UserID"));

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("UserID");

                    b.ToTable("UserInfo");
                });

            modelBuilder.Entity("FileOrbis.EntityLayer.Concrete.FileInfos", b =>
                {
                    b.HasOne("FileOrbis.EntityLayer.Concrete.FolderInfo", "Folder")
                        .WithMany("Files")
                        .HasForeignKey("FolderID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Folder");
                });

            modelBuilder.Entity("FileOrbis.EntityLayer.Concrete.FolderInfo", b =>
                {
                    b.HasOne("FileOrbis.EntityLayer.Concrete.UserInfo", "User")
                        .WithMany("Folders")
                        .HasForeignKey("UserID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("FileOrbis.EntityLayer.Concrete.FolderInfo", b =>
                {
                    b.Navigation("Files");
                });

            modelBuilder.Entity("FileOrbis.EntityLayer.Concrete.UserInfo", b =>
                {
                    b.Navigation("Folders");
                });
#pragma warning restore 612, 618
        }
    }
}
