﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using dal;

#nullable disable

namespace dal.Migrations
{
    [DbContext(typeof(KipcornDbContext))]
    partial class KipcornDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("models.Artikel", b =>
                {
                    b.Property<int>("ArtikelId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ArtikelId"), 1L, 1);

                    b.Property<int>("CategorieId")
                        .HasColumnType("int");

                    b.Property<bool>("EcoCheques")
                        .HasColumnType("bit");

                    b.Property<string>("Naam")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("Prijs")
                        .HasColumnType("money");

                    b.HasKey("ArtikelId");

                    b.HasIndex("CategorieId");

                    b.ToTable("Artikels");
                });

            modelBuilder.Entity("models.Categorie", b =>
                {
                    b.Property<int>("CategorieId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CategorieId"), 1L, 1);

                    b.Property<string>("Naam")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("CategorieId");

                    b.ToTable("Categorieen");
                });

            modelBuilder.Entity("models.Klant", b =>
                {
                    b.Property<int>("KlantId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("KlantId"), 1L, 1);

                    b.Property<string>("BtwNummer")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("HuisNr")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Naam")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Plaats")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Postcode")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Straat")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Telefoon")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("KlantId");

                    b.ToTable("Klanten");
                });

            modelBuilder.Entity("models.Order", b =>
                {
                    b.Property<int>("OrderId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("OrderId"), 1L, 1);

                    b.Property<int>("BtwPercentage")
                        .HasColumnType("int");

                    b.Property<int>("KlantId")
                        .HasColumnType("int");

                    b.Property<DateTime>("OrderDatum")
                        .HasColumnType("datetime2");

                    b.HasKey("OrderId");

                    b.HasIndex("KlantId");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("models.Orderlijn", b =>
                {
                    b.Property<int>("OrderlijnId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("OrderlijnId"), 1L, 1);

                    b.Property<int>("Aantal")
                        .HasColumnType("int");

                    b.Property<int>("ArtikelId")
                        .HasColumnType("int");

                    b.Property<int>("OrderId")
                        .HasColumnType("int");

                    b.HasKey("OrderlijnId");

                    b.HasIndex("ArtikelId");

                    b.HasIndex("OrderId");

                    b.ToTable("Orderlijnen");
                });

            modelBuilder.Entity("models.Stock", b =>
                {
                    b.Property<int>("StockId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("StockId"), 1L, 1);

                    b.Property<int>("Aantal")
                        .HasColumnType("int");

                    b.Property<int>("ArtikelId")
                        .HasColumnType("int");

                    b.Property<int>("VestigingId")
                        .HasColumnType("int");

                    b.HasKey("StockId");

                    b.HasIndex("ArtikelId");

                    b.HasIndex("VestigingId");

                    b.ToTable("Stocks");
                });

            modelBuilder.Entity("models.Vestiging", b =>
                {
                    b.Property<int>("VestigingId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("VestigingId"), 1L, 1);

                    b.Property<string>("HuisNr")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Naam")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Plaats")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Postcode")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Straat")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Telefoon")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("VestigingId");

                    b.ToTable("Vestigingen");
                });

            modelBuilder.Entity("models.WinkelmandItem", b =>
                {
                    b.Property<int>("WinkelmandItemId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("WinkelmandItemId"), 1L, 1);

                    b.Property<int>("Aantal")
                        .HasColumnType("int");

                    b.Property<int>("ArtikelId")
                        .HasColumnType("int");

                    b.HasKey("WinkelmandItemId");

                    b.HasIndex("ArtikelId");

                    b.ToTable("WinkelmandItems");
                });

            modelBuilder.Entity("models.Artikel", b =>
                {
                    b.HasOne("models.Categorie", "Categorie")
                        .WithMany("Artikelen")
                        .HasForeignKey("CategorieId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Categorie");
                });

            modelBuilder.Entity("models.Order", b =>
                {
                    b.HasOne("models.Klant", "Klant")
                        .WithMany("Orders")
                        .HasForeignKey("KlantId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Klant");
                });

            modelBuilder.Entity("models.Orderlijn", b =>
                {
                    b.HasOne("models.Artikel", "Artikel")
                        .WithMany("Orderlijnen")
                        .HasForeignKey("ArtikelId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("models.Order", "Order")
                        .WithMany("Orderlijnen")
                        .HasForeignKey("OrderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Artikel");

                    b.Navigation("Order");
                });

            modelBuilder.Entity("models.Stock", b =>
                {
                    b.HasOne("models.Artikel", "Artikel")
                        .WithMany("StockVestigingen")
                        .HasForeignKey("ArtikelId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("models.Vestiging", "Vestiging")
                        .WithMany("StockArtikellen")
                        .HasForeignKey("VestigingId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Artikel");

                    b.Navigation("Vestiging");
                });

            modelBuilder.Entity("models.WinkelmandItem", b =>
                {
                    b.HasOne("models.Artikel", "Artikel")
                        .WithMany("WinkelmandItems")
                        .HasForeignKey("ArtikelId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Artikel");
                });

            modelBuilder.Entity("models.Artikel", b =>
                {
                    b.Navigation("Orderlijnen");

                    b.Navigation("StockVestigingen");

                    b.Navigation("WinkelmandItems");
                });

            modelBuilder.Entity("models.Categorie", b =>
                {
                    b.Navigation("Artikelen");
                });

            modelBuilder.Entity("models.Klant", b =>
                {
                    b.Navigation("Orders");
                });

            modelBuilder.Entity("models.Order", b =>
                {
                    b.Navigation("Orderlijnen");
                });

            modelBuilder.Entity("models.Vestiging", b =>
                {
                    b.Navigation("StockArtikellen");
                });
#pragma warning restore 612, 618
        }
    }
}
