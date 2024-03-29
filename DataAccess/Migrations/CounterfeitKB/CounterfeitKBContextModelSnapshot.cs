﻿// <auto-generated />
using System;
using DataAccess.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace DataAccess.Migrations.CounterfeitKB
{
    [DbContext(typeof(CounterfeitKBContext))]
    partial class CounterfeitKBContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.21")
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

            modelBuilder.Entity("DataAccess.Models.CounterfeitImage", b =>
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

                    b.ToTable("CounterfeitImages");
                });

            modelBuilder.Entity("DataAccess.Models.CounterfeitImage", b =>
                {
                    b.HasOne("DataAccess.Models.Counterfeit", "Counterfeit")
                        .WithMany("CounterfeitImages")
                        .HasForeignKey("CounterfeitId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Counterfeit");
                });

            modelBuilder.Entity("DataAccess.Models.Counterfeit", b =>
                {
                    b.Navigation("CounterfeitImages");
                });
#pragma warning restore 612, 618
        }
    }
}
