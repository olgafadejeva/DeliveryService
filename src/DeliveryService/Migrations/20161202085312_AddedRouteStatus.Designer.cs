using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using DeliveryService.Data;

namespace DeliveryService.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20161202085312_AddedRouteStatus")]
    partial class AddedRouteStatus
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

                    b.Property<int>("CompanyID");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Discriminator")
                        .IsRequired();

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

                    b.HasDiscriminator<string>("Discriminator").HasValue("ApplicationUser");
                });

            modelBuilder.Entity("DeliveryService.Models.Entities.Address", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("City")
                        .IsRequired();

                    b.Property<string>("Discriminator")
                        .IsRequired();

                    b.Property<double>("Lat");

                    b.Property<string>("LineOne")
                        .IsRequired();

                    b.Property<string>("LineTwo");

                    b.Property<double>("Lng");

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

                    b.Property<int?>("CompanyID");

                    b.Property<string>("Email")
                        .IsRequired();

                    b.Property<string>("FirstName")
                        .IsRequired();

                    b.Property<string>("LastName");

                    b.HasKey("ID");

                    b.HasIndex("CompanyID");

                    b.ToTable("Clients");
                });

            modelBuilder.Entity("DeliveryService.Models.Entities.Company", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("CompanyName");

                    b.HasKey("ID");

                    b.ToTable("Companies");
                });

            modelBuilder.Entity("DeliveryService.Models.Entities.Delivery", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("ClientID");

                    b.Property<int?>("CompanyID");

                    b.Property<DateTime?>("DeliverBy");

                    b.Property<int>("DeliveryStatusID");

                    b.Property<int>("ItemSize");

                    b.Property<double>("ItemWeight");

                    b.Property<int?>("RouteID");

                    b.HasKey("ID");

                    b.HasIndex("ClientID");

                    b.HasIndex("CompanyID");

                    b.HasIndex("DeliveryStatusID");

                    b.HasIndex("RouteID");

                    b.ToTable("Deliveries");
                });

            modelBuilder.Entity("DeliveryService.Models.Entities.DeliveryStatus", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime?>("DeliveredDate");

                    b.Property<string>("ReasonFailed");

                    b.Property<int>("Status");

                    b.HasKey("ID");

                    b.ToTable("DeliveryStatus");
                });

            modelBuilder.Entity("DeliveryService.Models.Entities.Driver", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("AddressID");

                    b.Property<int?>("TeamID");

                    b.Property<string>("UserId");

                    b.HasKey("ID");

                    b.HasIndex("AddressID");

                    b.HasIndex("TeamID");

                    b.HasIndex("UserId");

                    b.ToTable("Drivers");
                });

            modelBuilder.Entity("DeliveryService.Models.Entities.DriverHoliday", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("DriverID");

                    b.Property<DateTime>("From");

                    b.Property<DateTime>("To");

                    b.HasKey("ID");

                    b.HasIndex("DriverID");

                    b.ToTable("DriverHolidays");
                });

            modelBuilder.Entity("DeliveryService.Models.Entities.Route", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("CompanyID");

                    b.Property<DateTime>("DeliverBy");

                    b.Property<DateTime?>("DeliveryDate");

                    b.Property<int?>("DriverID");

                    b.Property<double?>("OverallDistance");

                    b.Property<double?>("OverallTimeRequired");

                    b.Property<int?>("PickUpAddressID");

                    b.Property<int?>("Status");

                    b.Property<int?>("VehicleID");

                    b.HasKey("ID");

                    b.HasIndex("CompanyID");

                    b.HasIndex("DriverID");

                    b.HasIndex("PickUpAddressID");

                    b.ToTable("Routes");
                });

            modelBuilder.Entity("DeliveryService.Models.Entities.Team", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("CompanyID");

                    b.HasKey("ID");

                    b.HasIndex("CompanyID")
                        .IsUnique();

                    b.ToTable("Teams");
                });

            modelBuilder.Entity("DeliveryService.Models.Entities.Vehicle", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("DriverID");

                    b.Property<double>("Height");

                    b.Property<double>("Length");

                    b.Property<double>("MaxLoad");

                    b.Property<string>("RegistrationNumber");

                    b.Property<string>("VehicleName")
                        .IsRequired();

                    b.Property<double>("Width");

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

            modelBuilder.Entity("DeliveryService.Models.DriverUser", b =>
                {
                    b.HasBaseType("DeliveryService.Models.ApplicationUser");


                    b.ToTable("DriverUser");

                    b.HasDiscriminator().HasValue("DriverUser");
                });

            modelBuilder.Entity("DeliveryService.Models.EmployeeUser", b =>
                {
                    b.HasBaseType("DeliveryService.Models.ApplicationUser");

                    b.Property<bool>("PrimaryContact");

                    b.Property<int?>("TeamID");

                    b.HasIndex("TeamID");

                    b.ToTable("EmployeeUser");

                    b.HasDiscriminator().HasValue("EmployeeUser");
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

            modelBuilder.Entity("DeliveryService.Models.Entities.DriverAddress", b =>
                {
                    b.HasBaseType("DeliveryService.Models.Entities.Address");


                    b.ToTable("DriverAddress");

                    b.HasDiscriminator().HasValue("DriverAddress");
                });

            modelBuilder.Entity("DeliveryService.Models.Entities.PickUpAddress", b =>
                {
                    b.HasBaseType("DeliveryService.Models.Entities.Address");

                    b.Property<int?>("CompanyID");

                    b.HasIndex("CompanyID");

                    b.ToTable("PickUpAddress");

                    b.HasDiscriminator().HasValue("PickUpAddress");
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
                    b.HasOne("DeliveryService.Models.Entities.Company")
                        .WithMany("Clients")
                        .HasForeignKey("CompanyID");
                });

            modelBuilder.Entity("DeliveryService.Models.Entities.Delivery", b =>
                {
                    b.HasOne("DeliveryService.Models.Entities.Client", "Client")
                        .WithMany()
                        .HasForeignKey("ClientID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("DeliveryService.Models.Entities.Company")
                        .WithMany("Deliveries")
                        .HasForeignKey("CompanyID");

                    b.HasOne("DeliveryService.Models.Entities.DeliveryStatus", "DeliveryStatus")
                        .WithMany()
                        .HasForeignKey("DeliveryStatusID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("DeliveryService.Models.Entities.Route")
                        .WithMany("Deliveries")
                        .HasForeignKey("RouteID");
                });

            modelBuilder.Entity("DeliveryService.Models.Entities.Driver", b =>
                {
                    b.HasOne("DeliveryService.Models.Entities.DriverAddress", "Address")
                        .WithMany()
                        .HasForeignKey("AddressID");

                    b.HasOne("DeliveryService.Models.Entities.Team")
                        .WithMany("Drivers")
                        .HasForeignKey("TeamID");

                    b.HasOne("DeliveryService.Models.DriverUser", "User")
                        .WithMany()
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("DeliveryService.Models.Entities.DriverHoliday", b =>
                {
                    b.HasOne("DeliveryService.Models.Entities.Driver")
                        .WithMany("Holidays")
                        .HasForeignKey("DriverID");
                });

            modelBuilder.Entity("DeliveryService.Models.Entities.Route", b =>
                {
                    b.HasOne("DeliveryService.Models.Entities.Company")
                        .WithMany("Routes")
                        .HasForeignKey("CompanyID");

                    b.HasOne("DeliveryService.Models.Entities.Driver")
                        .WithMany("Routes")
                        .HasForeignKey("DriverID");

                    b.HasOne("DeliveryService.Models.Entities.PickUpAddress", "PickUpAddress")
                        .WithMany()
                        .HasForeignKey("PickUpAddressID");
                });

            modelBuilder.Entity("DeliveryService.Models.Entities.Team", b =>
                {
                    b.HasOne("DeliveryService.Models.Entities.Company", "Company")
                        .WithOne("Team")
                        .HasForeignKey("DeliveryService.Models.Entities.Team", "CompanyID")
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

            modelBuilder.Entity("DeliveryService.Models.EmployeeUser", b =>
                {
                    b.HasOne("DeliveryService.Models.Entities.Team")
                        .WithMany("Employees")
                        .HasForeignKey("TeamID");
                });

            modelBuilder.Entity("DeliveryService.Models.Entities.ClientAddress", b =>
                {
                    b.HasOne("DeliveryService.Models.Entities.Client")
                        .WithOne("Address")
                        .HasForeignKey("DeliveryService.Models.Entities.ClientAddress", "ClientId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("DeliveryService.Models.Entities.PickUpAddress", b =>
                {
                    b.HasOne("DeliveryService.Models.Entities.Company")
                        .WithMany("PickUpLocations")
                        .HasForeignKey("CompanyID");
                });
        }
    }
}
