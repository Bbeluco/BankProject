﻿// <auto-generated />
using System;
using BankProject;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace BankProject.Migrations
{
    [DbContext(typeof(BankAppContext))]
    partial class BankAppContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.2");

            modelBuilder.Entity("BankProject.AssignorModel", b =>
                {
                    b.Property<int>("AssignorId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Document")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("AssignorId");

                    b.ToTable("Assignor");
                });

            modelBuilder.Entity("BankProject.ReceivableModel", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<int>("AssignorId")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("Date")
                        .HasColumnType("TEXT");

                    b.Property<double>("Value")
                        .HasColumnType("REAL");

                    b.HasKey("Id");

                    b.HasIndex("AssignorId");

                    b.ToTable("Receivable");
                });

            modelBuilder.Entity("BankProject.ReceivableModel", b =>
                {
                    b.HasOne("BankProject.AssignorModel", "Assignor")
                        .WithMany()
                        .HasForeignKey("AssignorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Assignor");
                });
#pragma warning restore 612, 618
        }
    }
}
