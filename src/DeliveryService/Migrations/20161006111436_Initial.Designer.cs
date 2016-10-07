using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using DeliveryService.Data;

namespace DeliveryService.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20161006111436_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.0.0-rtm-21431")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("DeliveryService.Entities.DriverRegistrationRequest", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("DriverEmail");

                    b.Property<int>("TeamID");

                    b.HasKey("ID");

                    b.HasIndex("TeamID");

                    b.ToTable("DriverRegistrationRequests");
                });

            modelBuilder.Entity("DeliveryService.Models.ApplicationUser", b =>
                {
                    b.Property<string>("Id");

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Email")
                        .HasAnnotation("MaxLength", 256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("NormalizedEmail")
                        .HasAnnotation("MaxLength", 256);

                    b.Property<string>("NormalizedUserName")
                        .HasAnnotation("MaxLength", 256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("SecurityStamp");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasAnnotation("MaxLength", 256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("DeliveryService.Models.Entities.Address", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("City")
                        .IsRequired();

                    b.Property<string>("Discriminator")
                        .IsRequired();

                    b.Property<string>("LineOne")
                        .IsRequired();

                    b.Property<string>("LineTwo");

                    b.Property<string>("PostCode")
                        .IsRequired();

                    b.HasKey("ID");

                    b.ToTable("Addresses");

                    b.HasDiscriminator<string>("Discriminator").HasValue("Address");
                });

            modelBuilder.Entity("DeliveryService.Models.Entities.Client", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Email")
                        .IsRequired();

                    b.Property<string>("FirstName")
                        .IsRequired();

                    b.Property<string>("LastName");

                    b.Property<int?>("ShipperID");

                    b.HasKey("ID");

                    b.HasIndex("ShipperID");

                    b.ToTable("Clients");
                });

            modelBuilder.Entity("DeliveryService.Models.Entities.Delivery", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("ClientID");

                    b.Property<int>("DeliveryStatusID");

                    b.Property<int?>("DriverID");

                    b.Property<int?>("PickUpAddressID");

                    b.Property<int?>("ShipperID");

                    b.HasKey("ID");

                    b.HasIndex("ClientID");

                    b.HasIndex("DeliveryStatusID");

                    b.HasIndex("DriverID");

                    b.HasIndex("PickUpAddressID");

                    b.HasIndex("ShipperID");

                    b.ToTable("Deliveries");
                });

            modelBuilder.Entity("DeliveryService.Models.Entities.DeliveryStatus", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("AssignedToId");

                    b.Property<int?>("PickedUpById");

                    b.Property<int>("Status");

                    b.HasKey("ID");

                    b.HasIndex("AssignedToId");

                    b.HasIndex("PickedUpById");

                    b.ToTable("DeliveryStatus");
                });

            modelBuilder.Entity("DeliveryService.Models.Entities.Driver", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("TeamID");

                    b.Property<string>("UserId");

                    b.HasKey("ID");

                    b.HasIndex("TeamID");

                    b.HasIndex("UserId");

                    b.ToTable("Drivers");
                });

            modelBuilder.Entity("DeliveryService.Models.Entities.Shipper", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("UserId");

                    b.HasKey("ID");

                    b.HasIndex("UserId");

                    b.ToTable("Shippers");
                });

            modelBuilder.Entity("DeliveryService.Models.Entities.Team", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("CompanyName");

                    b.Property<string>("Description");

                    b.Property<int>("ShipperId");

                    b.HasKey("ID");

                    b.HasIndex("ShipperId")
                        .IsUnique();

                    b.ToTable("Teams");
                });

            modelBuilder.Entity("DeliveryService.Models.Entities.Vehicle", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("DriverID");

                    b.Property<string>("RegistrationNumber");

                    b.HasKey("ID");

                    b.HasIndex("DriverID");

                    b.ToTable("Vehicles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRole", b =>
                {
                    b.Property<string>("Id");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasAnnotation("MaxLength", 256);

                    b.Property<string>("NormalizedName")
                        .HasAnnotation("MaxLength", 256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .HasName("RoleNameIndex");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("RoleId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("DeliveryService.Models.Entities.ClientAddress", b =>
                {
                    b.HasBaseType("DeliveryService.Models.Entities.Address");

                    b.Property<int>("ClientId");

                    b.HasIndex("ClientId")
                        .IsUnique();

                    b.ToTable("ClientAddress");

                    b.HasDiscriminator().HasValue("ClientAddress");
                });

            modelBuilder.Entity("DeliveryService.Models.Entities.PickUpAddress", b =>
                {
                    b.HasBaseType("DeliveryService.Models.Entities.Address");


                    b.ToTable("PickUpAddress");

                    b.HasDiscriminator().HasValue("PickUpAddress");
                });

            modelBuilder.Entity("DeliveryService.Models.Entities.ShippersDefaultPickUpAddress", b =>
                {
                    b.HasBaseType("DeliveryService.Models.Entities.Address");

                    b.Property<int>("ShipperId");

                    b.HasIndex("ShipperId")
                        .IsUnique();

                    b.ToTable("ShippersDefaultPickUpAddress");

                    b.HasDiscriminator().HasValue("ShippersDefaultPickUpAddress");
                });

            modelBuilder.Entity("DeliveryService.Entities.DriverRegistrationRequest", b =>
                {
                    b.HasOne("DeliveryService.Models.Entities.Team", "Team")
                        .WithMany()
                        .HasForeignKey("TeamID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("DeliveryService.Models.Entities.Client", b =>
                {
                    b.HasOne("DeliveryService.Models.Entities.Shipper")
                        .WithMany("Clients")
                        .HasForeignKey("ShipperID");
                });

            modelBuilder.Entity("DeliveryService.Models.Entities.Delivery", b =>
                {
                    b.HasOne("DeliveryService.Models.Entities.Client", "Client")
                        .WithMany()
                        .HasForeignKey("ClientID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("DeliveryService.Models.Entities.DeliveryStatus", "DeliveryStatus")
                        .WithMany()
                        .HasForeignKey("DeliveryStatusID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("DeliveryService.Models.Entities.Driver")
                        .WithMany("Deliveries")
                        .HasForeignKey("DriverID");

                    b.HasOne("DeliveryService.Models.Entities.PickUpAddress", "PickUpAddress")
                        .WithMany()
                        .HasForeignKey("PickUpAddressID");

                    b.HasOne("DeliveryService.Models.Entities.Shipper")
                        .WithMany("Deliveries")
                        .HasForeignKey("ShipperID");
                });

            modelBuilder.Entity("DeliveryService.Models.Entities.DeliveryStatus", b =>
                {
                    b.HasOne("DeliveryService.Models.Entities.Driver", "AssignedTo")
                        .WithMany()
                        .HasForeignKey("AssignedToId");

                    b.HasOne("DeliveryService.Models.Entities.Driver", "PickedUpBy")
                        .WithMany()
                        .HasForeignKey("PickedUpById");
                });

            modelBuilder.Entity("DeliveryService.Models.Entities.Driver", b =>
                {
                    b.HasOne("DeliveryService.Models.Entities.Team", "Team")
                        .WithMany("Drivers")
                        .HasForeignKey("TeamID");

                    b.HasOne("DeliveryService.Models.ApplicationUser", "User")
                        .WithMany()
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("DeliveryService.Models.Entities.Shipper", b =>
                {
                    b.HasOne("DeliveryService.Models.ApplicationUser", "User")
                        .WithMany()
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("DeliveryService.Models.Entities.Team", b =>
                {
                    b.HasOne("DeliveryService.Models.Entities.Shipper", "Shipper")
                        .WithOne("Team")
                        .HasForeignKey("DeliveryService.Models.Entities.Team", "ShipperId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("DeliveryService.Models.Entities.Vehicle", b =>
                {
                    b.HasOne("DeliveryService.Models.Entities.Driver")
                        .WithMany("Vehicles")
                        .HasForeignKey("DriverID");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRole")
                        .WithMany("Claims")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("DeliveryService.Models.ApplicationUser")
                        .WithMany("Claims")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("DeliveryService.Models.ApplicationUser")
                        .WithMany("Logins")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRole")
                        .WithMany("Users")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("DeliveryService.Models.ApplicationUser")
                        .WithMany("Roles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("DeliveryService.Models.Entities.ClientAddress", b =>
                {
                    b.HasOne("DeliveryService.Models.Entities.Client", "Client")
                        .WithOne("Address")
                        .HasForeignKey("DeliveryService.Models.Entities.ClientAddress", "ClientId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("DeliveryService.Models.Entities.ShippersDefaultPickUpAddress", b =>
                {
                    b.HasOne("DeliveryService.Models.Entities.Shipper", "Shipper")
                        .WithOne("DefaultPickUpAddress")
                        .HasForeignKey("DeliveryService.Models.Entities.ShippersDefaultPickUpAddress", "ShipperId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}
