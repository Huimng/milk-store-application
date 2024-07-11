﻿// <auto-generated />
using System;
using DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace DAL.Migrations
{
    [DbContext(typeof(BSADBContext))]
    partial class BSADBContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.18")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("BusinessObjects.Account", b =>
                {
                    b.Property<int>("AccountId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("AccountId"));

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(32)
                        .HasColumnType("character varying(32)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("character varying(64)");

                    b.Property<int>("Role")
                        .HasColumnType("integer");

                    b.Property<bool>("Status")
                        .HasColumnType("boolean");

                    b.Property<DateTime>("UpdateDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasMaxLength(32)
                        .HasColumnType("character varying(32)");

                    b.HasKey("AccountId");

                    b.ToTable("Accounts");
                });

            modelBuilder.Entity("BusinessObjects.Comment", b =>
                {
                    b.Property<int>("CommentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("CommentId"));

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("CreateDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("PostId")
                        .HasColumnType("integer");

                    b.HasKey("CommentId");

                    b.HasIndex("PostId");

                    b.ToTable("Comments");
                });

            modelBuilder.Entity("BusinessObjects.Message", b =>
                {
                    b.Property<int>("MessageId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("MessageId"));

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)");

                    b.Property<int>("CustomerId")
                        .HasColumnType("integer");

                    b.Property<int>("GroupId")
                        .HasColumnType("integer");

                    b.Property<int>("ManagerId")
                        .HasColumnType("integer");

                    b.HasKey("MessageId");

                    b.HasIndex("CustomerId");

                    b.HasIndex("GroupId");

                    b.HasIndex("ManagerId");

                    b.ToTable("Messages");
                });

            modelBuilder.Entity("BusinessObjects.MessageGroup", b =>
                {
                    b.Property<int>("MessageGroupId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("MessageGroupId"));

                    b.Property<int>("CustomerId")
                        .HasColumnType("integer");

                    b.Property<string>("GroupName")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)");

                    b.Property<int>("ManagerId")
                        .HasColumnType("integer");

                    b.Property<int>("Status")
                        .HasColumnType("integer");

                    b.HasKey("MessageGroupId");

                    b.HasIndex("CustomerId");

                    b.HasIndex("ManagerId");

                    b.ToTable("MessageGroups");
                });

            modelBuilder.Entity("BusinessObjects.Order", b =>
                {
                    b.Property<int>("OrderId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("OrderId"));

                    b.Property<int>("AccountId")
                        .HasColumnType("integer");

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)");

                    b.Property<int>("CartId")
                        .HasColumnType("integer");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<double>("GrandTotal")
                        .HasColumnType("double precision");

                    b.Property<int>("PaymentMethod")
                        .HasColumnType("integer");

                    b.Property<int>("Status")
                        .HasColumnType("integer");

                    b.Property<double>("SubTotal")
                        .HasColumnType("double precision");

                    b.Property<double>("TotalDiscount")
                        .HasColumnType("double precision");

                    b.Property<int>("Type")
                        .HasColumnType("integer");

                    b.Property<DateTime>("UpdatedDate")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("OrderId");

                    b.HasIndex("AccountId");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("BusinessObjects.OrderContact", b =>
                {
                    b.Property<int>("OrdContacId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("OrdContacId"));

                    b.Property<string>("City")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("CustomerName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("District")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("HouseNumber")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("OrderId")
                        .HasColumnType("integer");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Province")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("UpdatedDate")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("OrdContacId");

                    b.HasIndex("OrderId")
                        .IsUnique();

                    b.ToTable("OrderContacts");
                });

            modelBuilder.Entity("BusinessObjects.OrderDetail", b =>
                {
                    b.Property<int>("OrderDetailId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("OrderDetailId"));

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("OrderId")
                        .HasColumnType("integer");

                    b.Property<int>("ProductId")
                        .HasColumnType("integer");

                    b.Property<int>("Quantity")
                        .HasColumnType("integer");

                    b.Property<double>("TotalPrice")
                        .HasColumnType("double precision");

                    b.Property<DateTime>("UpdatedDate")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("OrderDetailId");

                    b.HasIndex("OrderId");

                    b.HasIndex("ProductId");

                    b.ToTable("OrderDetails");
                });

            modelBuilder.Entity("BusinessObjects.Post", b =>
                {
                    b.Property<int>("PostId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("PostId"));

                    b.Property<string>("Contents")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("CreateBy")
                        .HasColumnType("integer");

                    b.Property<DateTime>("CreateDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("ProductId")
                        .HasColumnType("integer");

                    b.Property<int>("Status")
                        .HasColumnType("integer");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)");

                    b.HasKey("PostId");

                    b.HasIndex("CreateBy");

                    b.HasIndex("ProductId");

                    b.ToTable("Posts");
                });

            modelBuilder.Entity("BusinessObjects.Product", b =>
                {
                    b.Property<int>("ProductId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("ProductId"));

                    b.Property<string>("Brand")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<double>("Discount")
                        .HasColumnType("double precision");

                    b.Property<string>("ProductCode")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)");

                    b.Property<string>("ProductName")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)");

                    b.Property<int>("Quantity")
                        .HasColumnType("integer");

                    b.Property<int>("Status")
                        .HasColumnType("integer");

                    b.Property<string>("UrlImage")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)");

                    b.HasKey("ProductId");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("BusinessObjects.ProductFeedback", b =>
                {
                    b.Property<int>("AccountId")
                        .HasColumnType("integer");

                    b.Property<int>("ProductId")
                        .HasColumnType("integer");

                    b.Property<int>("OrderId")
                        .HasColumnType("integer");

                    b.Property<string>("Comment")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("CreateDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("Point")
                        .HasColumnType("integer");

                    b.HasKey("AccountId", "ProductId", "OrderId");

                    b.HasIndex("OrderId");

                    b.HasIndex("ProductId");

                    b.ToTable("ProductFeedbacks");
                });

            modelBuilder.Entity("BusinessObjects.ProductLine", b =>
                {
                    b.Property<int>("ProductLineId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("ProductLineId"));

                    b.Property<string>("AgeGroup")
                        .IsRequired()
                        .HasMaxLength(125)
                        .HasColumnType("character varying(125)");

                    b.Property<DateTime>("ExpireDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool?>("IsActived")
                        .HasColumnType("boolean");

                    b.Property<int>("ProductId")
                        .HasColumnType("integer");

                    b.HasKey("ProductLineId");

                    b.HasIndex("ProductId");

                    b.ToTable("ProductLines");
                });

            modelBuilder.Entity("BusinessObjects.Comment", b =>
                {
                    b.HasOne("BusinessObjects.Post", "Post")
                        .WithMany("Comments")
                        .HasForeignKey("PostId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Post");
                });

            modelBuilder.Entity("BusinessObjects.Message", b =>
                {
                    b.HasOne("BusinessObjects.Account", "Customer")
                        .WithMany("CustomerMessages")
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("BusinessObjects.MessageGroup", "MessageGroup")
                        .WithMany("Messages")
                        .HasForeignKey("GroupId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("BusinessObjects.Account", "Manager")
                        .WithMany("ManagerMessages")
                        .HasForeignKey("ManagerId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Customer");

                    b.Navigation("Manager");

                    b.Navigation("MessageGroup");
                });

            modelBuilder.Entity("BusinessObjects.MessageGroup", b =>
                {
                    b.HasOne("BusinessObjects.Account", "Customer")
                        .WithMany("CustomerGroups")
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("BusinessObjects.Account", "Manager")
                        .WithMany("ManagerGroups")
                        .HasForeignKey("ManagerId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Customer");

                    b.Navigation("Manager");
                });

            modelBuilder.Entity("BusinessObjects.Order", b =>
                {
                    b.HasOne("BusinessObjects.Account", "Account")
                        .WithMany("Orders")
                        .HasForeignKey("AccountId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Account");
                });

            modelBuilder.Entity("BusinessObjects.OrderContact", b =>
                {
                    b.HasOne("BusinessObjects.Order", "Order")
                        .WithOne("OrderContact")
                        .HasForeignKey("BusinessObjects.OrderContact", "OrderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Order");
                });

            modelBuilder.Entity("BusinessObjects.OrderDetail", b =>
                {
                    b.HasOne("BusinessObjects.Order", "Order")
                        .WithMany("OrderDetails")
                        .HasForeignKey("OrderId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("BusinessObjects.Product", "Product")
                        .WithMany("OrderDetail")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Order");

                    b.Navigation("Product");
                });

            modelBuilder.Entity("BusinessObjects.Post", b =>
                {
                    b.HasOne("BusinessObjects.Account", "Account")
                        .WithMany("Posts")
                        .HasForeignKey("CreateBy")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BusinessObjects.Product", "Product")
                        .WithMany("Posts")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Account");

                    b.Navigation("Product");
                });

            modelBuilder.Entity("BusinessObjects.ProductFeedback", b =>
                {
                    b.HasOne("BusinessObjects.Account", "Account")
                        .WithMany("ProductFeedbacks")
                        .HasForeignKey("AccountId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("BusinessObjects.Order", "Order")
                        .WithMany("ProductFeedbacks")
                        .HasForeignKey("OrderId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("BusinessObjects.Product", "Product")
                        .WithMany("ProductFeedbacks")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Account");

                    b.Navigation("Order");

                    b.Navigation("Product");
                });

            modelBuilder.Entity("BusinessObjects.ProductLine", b =>
                {
                    b.HasOne("BusinessObjects.Product", "Product")
                        .WithMany("ProductLines")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Product");
                });

            modelBuilder.Entity("BusinessObjects.Account", b =>
                {
                    b.Navigation("CustomerGroups");

                    b.Navigation("CustomerMessages");

                    b.Navigation("ManagerGroups");

                    b.Navigation("ManagerMessages");

                    b.Navigation("Orders");

                    b.Navigation("Posts");

                    b.Navigation("ProductFeedbacks");
                });

            modelBuilder.Entity("BusinessObjects.MessageGroup", b =>
                {
                    b.Navigation("Messages");
                });

            modelBuilder.Entity("BusinessObjects.Order", b =>
                {
                    b.Navigation("OrderContact");

                    b.Navigation("OrderDetails");

                    b.Navigation("ProductFeedbacks");
                });

            modelBuilder.Entity("BusinessObjects.Post", b =>
                {
                    b.Navigation("Comments");
                });

            modelBuilder.Entity("BusinessObjects.Product", b =>
                {
                    b.Navigation("OrderDetail");

                    b.Navigation("Posts");

                    b.Navigation("ProductFeedbacks");

                    b.Navigation("ProductLines");
                });
#pragma warning restore 612, 618
        }
    }
}
