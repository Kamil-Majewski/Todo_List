﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Todo_List.Infrastructure;

#nullable disable

namespace Todo_List.Infrastructure.Migrations
{
    [DbContext(typeof(TodoListDbContext))]
    [Migration("20240408065418_added-reminderSet")]
    partial class addedreminderSet
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.17")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Todo_List.Infrastructure.Entities.Commitments.Abstract.Commitment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsCompleted")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Notes")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("Priority")
                        .HasColumnType("int");

                    b.Property<bool>("ReminderSet")
                        .HasColumnType("bit");

                    b.Property<string>("SubtasksSerialized")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Tasks");

                    b.HasDiscriminator<string>("Discriminator").HasValue("Commitment");

                    b.UseTphMappingStrategy();
                });

            modelBuilder.Entity("Todo_List.Infrastructure.Entities.Log", b =>
                {
                    b.Property<int>("LogId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("LogId"));

                    b.Property<DateTime>("LogCreationDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("LogDescription")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LogName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LogRelatedObjectsSerialized")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("LogId");

                    b.ToTable("Logs");
                });

            modelBuilder.Entity("Todo_List.Infrastructure.Entities.Reminder", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("CommitmentId")
                        .HasColumnType("int");

                    b.Property<DateTime>("ReminderTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Reminders");
                });

            modelBuilder.Entity("Todo_List.Infrastructure.Entities.Commitments.OneTimeCommitment", b =>
                {
                    b.HasBaseType("Todo_List.Infrastructure.Entities.Commitments.Abstract.Commitment");

                    b.Property<DateTime?>("DueDate")
                        .HasColumnType("datetime2");

                    b.HasDiscriminator().HasValue("OneTimeCommitment");
                });

            modelBuilder.Entity("Todo_List.Infrastructure.Entities.Commitments.RecurringCommitment", b =>
                {
                    b.HasBaseType("Todo_List.Infrastructure.Entities.Commitments.Abstract.Commitment");

                    b.Property<DateTime?>("DueDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("RecurInterval")
                        .HasColumnType("int");

                    b.Property<int>("RecurUnit")
                        .HasColumnType("int");

                    b.Property<DateTime>("RecurUntil")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("RecurrenceStart")
                        .HasColumnType("datetime2");

                    b.ToTable("Tasks", t =>
                        {
                            t.Property("DueDate")
                                .HasColumnName("RecurringCommitment_DueDate");
                        });

                    b.HasDiscriminator().HasValue("RecurringCommitment");
                });

            modelBuilder.Entity("Todo_List.Infrastructure.Entities.Commitments.UnscheduledCommitment", b =>
                {
                    b.HasBaseType("Todo_List.Infrastructure.Entities.Commitments.Abstract.Commitment");

                    b.HasDiscriminator().HasValue("UnscheduledCommitment");
                });
#pragma warning restore 612, 618
        }
    }
}