﻿// <auto-generated />
using System;
using Eventix.Modules.Events.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Eventix.Modules.Events.Infrastructure.Database.Migrations
{
    [DbContext(typeof(EventsDbContext))]
    partial class EventsDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("events")
                .HasAnnotation("ProductVersion", "9.0.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Eventix.Modules.Events.Domain.Categories.Entities.Category", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("IsArchived")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("VARCHAR")
                        .HasColumnName("Name");

                    b.HasKey("Id");

                    b.ToTable("Categories", "events");
                });

            modelBuilder.Entity("Eventix.Modules.Events.Domain.Events.Entities.Event", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("CategoryId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.ToTable("Events", "events");
                });

            modelBuilder.Entity("Eventix.Modules.Events.Domain.TicketTypes.Entities.TicketType", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("EventId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.ToTable("TicketTypes", "events");
                });

            modelBuilder.Entity("Eventix.Shared.Infrastructure.Inbox.Models.InboxMessage", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("VARCHAR(3000)");

                    b.Property<string>("Error")
                        .HasColumnType("VARCHAR(256)");

                    b.Property<DateTime>("OccurredOnUtc")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("ProcessedOnUtc")
                        .HasColumnType("datetime2");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("VARCHAR(200)");

                    b.HasKey("Id");

                    b.ToTable("InboxMessages", "events");
                });

            modelBuilder.Entity("Eventix.Shared.Infrastructure.Inbox.Models.InboxMessageConsumer", b =>
                {
                    b.Property<Guid>("InboxMessageId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .HasColumnType("VARCHAR(256)");

                    b.HasKey("InboxMessageId", "Name");

                    b.ToTable("InboxMessageConsumers", "events");
                });

            modelBuilder.Entity("Eventix.Shared.Infrastructure.Outbox.Models.OutboxMessage", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("VARCHAR(3000)");

                    b.Property<string>("Error")
                        .HasColumnType("VARCHAR(256)");

                    b.Property<DateTime>("OccurredOnUtc")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("ProcessedOnUtc")
                        .HasColumnType("datetime2");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("VARCHAR(200)");

                    b.HasKey("Id");

                    b.ToTable("OutboxMessages", "events");
                });

            modelBuilder.Entity("Eventix.Shared.Infrastructure.Outbox.Models.OutboxMessageConsumer", b =>
                {
                    b.Property<Guid>("OutboxMessageId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .HasColumnType("VARCHAR(256)");

                    b.HasKey("OutboxMessageId", "Name");

                    b.ToTable("OutboxMessageConsumers", "events");
                });

            modelBuilder.Entity("Eventix.Modules.Events.Domain.Events.Entities.Event", b =>
                {
                    b.HasOne("Eventix.Modules.Events.Domain.Categories.Entities.Category", null)
                        .WithMany()
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.OwnsOne("Eventix.Modules.Events.Domain.Events.ValueObjects.EventSpecification", "Specification", b1 =>
                        {
                            b1.Property<Guid>("EventId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<string>("Description")
                                .IsRequired()
                                .HasColumnType("VARCHAR(256)")
                                .HasColumnName("Description");

                            b1.Property<string>("Title")
                                .IsRequired()
                                .HasColumnType("VARCHAR(80)")
                                .HasColumnName("Title");

                            b1.HasKey("EventId");

                            b1.ToTable("Events", "events");

                            b1.WithOwner()
                                .HasForeignKey("EventId");
                        });

                    b.OwnsOne("Eventix.Shared.Domain.ValueObjects.DateRange", "DateRange", b1 =>
                        {
                            b1.Property<Guid>("EventId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<DateTime?>("EndsAtUtc")
                                .HasColumnType("datetime2")
                                .HasColumnName("EndsAtUtc");

                            b1.Property<DateTime>("StartsAtUtc")
                                .HasColumnType("datetime2")
                                .HasColumnName("StartsAtUtc");

                            b1.HasKey("EventId");

                            b1.ToTable("Events", "events");

                            b1.WithOwner()
                                .HasForeignKey("EventId");
                        });

                    b.OwnsOne("Eventix.Shared.Domain.ValueObjects.Location", "Location", b1 =>
                        {
                            b1.Property<Guid>("EventId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<string>("AdditionalInfo")
                                .IsRequired()
                                .HasColumnType("VARCHAR(250)")
                                .HasColumnName("AdditionalInfo");

                            b1.Property<string>("City")
                                .IsRequired()
                                .HasColumnType("VARCHAR(100)")
                                .HasColumnName("City");

                            b1.Property<string>("Neighborhood")
                                .IsRequired()
                                .HasColumnType("VARCHAR(100)")
                                .HasColumnName("Neighborhood");

                            b1.Property<string>("Number")
                                .IsRequired()
                                .HasColumnType("VARCHAR(50)")
                                .HasColumnName("Number");

                            b1.Property<string>("State")
                                .IsRequired()
                                .HasColumnType("VARCHAR(50)")
                                .HasColumnName("State");

                            b1.Property<string>("Street")
                                .IsRequired()
                                .HasColumnType("VARCHAR(200)")
                                .HasColumnName("Street");

                            b1.Property<string>("ZipCode")
                                .IsRequired()
                                .HasColumnType("VARCHAR(20)")
                                .HasColumnName("ZipCode");

                            b1.HasKey("EventId");

                            b1.ToTable("Events", "events");

                            b1.WithOwner()
                                .HasForeignKey("EventId");
                        });

                    b.Navigation("DateRange")
                        .IsRequired();

                    b.Navigation("Location");

                    b.Navigation("Specification")
                        .IsRequired();
                });

            modelBuilder.Entity("Eventix.Modules.Events.Domain.TicketTypes.Entities.TicketType", b =>
                {
                    b.OwnsOne("Eventix.Modules.Events.Domain.TicketTypes.ValueObjects.TicketTypeSpecification", "Specification", b1 =>
                        {
                            b1.Property<Guid>("TicketTypeId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<string>("Name")
                                .IsRequired()
                                .HasColumnType("VARCHAR(80)")
                                .HasColumnName("Name");

                            b1.Property<decimal>("Quantity")
                                .HasColumnType("decimal(18,2)")
                                .HasColumnName("Quantity");

                            b1.HasKey("TicketTypeId");

                            b1.ToTable("TicketTypes", "events");

                            b1.WithOwner()
                                .HasForeignKey("TicketTypeId");
                        });

                    b.OwnsOne("Eventix.Shared.Domain.ValueObjects.Money", "Price", b1 =>
                        {
                            b1.Property<Guid>("TicketTypeId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<decimal>("Amount")
                                .HasColumnType("MONEY")
                                .HasColumnName("Amount");

                            b1.Property<string>("Currency")
                                .IsRequired()
                                .HasColumnType("VARCHAR(10)")
                                .HasColumnName("Currency");

                            b1.HasKey("TicketTypeId");

                            b1.ToTable("TicketTypes", "events");

                            b1.WithOwner()
                                .HasForeignKey("TicketTypeId");
                        });

                    b.Navigation("Price")
                        .IsRequired();

                    b.Navigation("Specification")
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
