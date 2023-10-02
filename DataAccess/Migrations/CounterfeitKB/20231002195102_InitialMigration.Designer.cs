﻿// <auto-generated />

#nullable disable

using DataAccess.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccess.Migrations.CounterfeitKB
{
    [DbContext(typeof(CounterfeitKBContext))]
    [Migration("20231002195102_InitialMigration")]
    partial class InitialMigration
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("DataAccess.Models.Counterfeit", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Counterfeits");
                });

            modelBuilder.Entity("DataAccess.Models.CounterfeitPath", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("CounterfeitId")
                        .HasColumnType("uuid");

                    b.Property<string>("ImagePath")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("CounterfeitId");

                    b.ToTable("CounterfeitPaths");
                });

            modelBuilder.Entity("DataAccess.Models.CounterfeitPath", b =>
                {
                    b.HasOne("DataAccess.Models.Counterfeit", "Counterfeit")
                        .WithMany("CounterfeitPaths")
                        .HasForeignKey("CounterfeitId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Counterfeit");
                });

            modelBuilder.Entity("DataAccess.Models.Counterfeit", b =>
                {
                    b.Navigation("CounterfeitPaths");
                });
#pragma warning restore 612, 618
        }
    }
}
