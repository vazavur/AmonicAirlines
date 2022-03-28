using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace AmonicAirlines
{
    public partial class AmonicdbContext : DbContext
    {
        public AmonicdbContext()
        {
        }

        public AmonicdbContext(DbContextOptions<AmonicdbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Aircraft> Aircrafts { get; set; }
        public virtual DbSet<Airport> Airports { get; set; }
        public virtual DbSet<Cabintype> Cabintypes { get; set; }
        public virtual DbSet<Country> Countries { get; set; }
        public virtual DbSet<Office> Offices { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<Route> Routes { get; set; }
        public virtual DbSet<Schedule> Schedules { get; set; }
        public virtual DbSet<Ticket> Tickets { get; set; }
        public virtual DbSet<Tracking> Trackings { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseMySql("server=194.32.248.98;user id=amonic-client;password=AMONIC2208client!;database=amonicdb;persistsecurityinfo=True", Microsoft.EntityFrameworkCore.ServerVersion.Parse("5.7.35-mysql"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasCharSet("latin1")
                .UseCollation("latin1_swedish_ci");

            modelBuilder.Entity<Aircraft>(entity =>
            {
                entity.ToTable("aircrafts");

                entity.HasCharSet("utf8")
                    .UseCollation("utf8_bin");

                entity.Property(e => e.Id)
                    .HasColumnType("int(11)")
                    .ValueGeneratedNever()
                    .HasColumnName("ID");

                entity.Property(e => e.BusinessSeats).HasColumnType("int(11)");

                entity.Property(e => e.EconomySeats).HasColumnType("int(11)");

                entity.Property(e => e.MakeModel).HasMaxLength(10);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.TotalSeats).HasColumnType("int(11)");
            });

            modelBuilder.Entity<Airport>(entity =>
            {
                entity.ToTable("airports");

                entity.HasCharSet("utf8")
                    .UseCollation("utf8_bin");

                entity.HasIndex(e => e.CountryId, "FK_AirPort_Country");

                entity.Property(e => e.Id)
                    .HasColumnType("int(11)")
                    .HasColumnName("ID");

                entity.Property(e => e.CountryId)
                    .HasColumnType("int(11)")
                    .HasColumnName("CountryID");

                entity.Property(e => e.Iatacode)
                    .IsRequired()
                    .HasMaxLength(3)
                    .HasColumnName("IATACode");

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.HasOne(d => d.Country)
                    .WithMany(p => p.Airports)
                    .HasForeignKey(d => d.CountryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_AirPort_Country");
            });

            modelBuilder.Entity<Cabintype>(entity =>
            {
                entity.ToTable("cabintypes");

                entity.HasCharSet("utf8")
                    .UseCollation("utf8_bin");

                entity.Property(e => e.Id)
                    .HasColumnType("int(11)")
                    .HasColumnName("ID");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Country>(entity =>
            {
                entity.ToTable("countries");

                entity.HasCharSet("utf8")
                    .UseCollation("utf8_bin");

                entity.Property(e => e.Id)
                    .HasColumnType("int(11)")
                    .HasColumnName("ID");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Office>(entity =>
            {
                entity.ToTable("offices");

                entity.HasCharSet("utf8")
                    .UseCollation("utf8_bin");

                entity.HasIndex(e => e.CountryId, "FK_Office_Country");

                entity.Property(e => e.Id)
                    .HasColumnType("int(11)")
                    .HasColumnName("ID");

                entity.Property(e => e.Contact)
                    .IsRequired()
                    .HasMaxLength(250);

                entity.Property(e => e.CountryId)
                    .HasColumnType("int(11)")
                    .HasColumnName("CountryID");

                entity.Property(e => e.Phone)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.Country)
                    .WithMany(p => p.Offices)
                    .HasForeignKey(d => d.CountryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Office_Country");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.ToTable("roles");

                entity.HasCharSet("utf8")
                    .UseCollation("utf8_bin");

                entity.Property(e => e.Id)
                    .HasColumnType("int(11)")
                    .ValueGeneratedNever()
                    .HasColumnName("ID");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Route>(entity =>
            {
                entity.ToTable("routes");

                entity.HasCharSet("utf8")
                    .UseCollation("utf8_bin");

                entity.HasIndex(e => e.DepartureAirportId, "FK_Routes_Airports2");

                entity.HasIndex(e => e.ArrivalAirportId, "FK_Routes_Airports3");

                entity.Property(e => e.Id)
                    .HasColumnType("int(11)")
                    .HasColumnName("ID");

                entity.Property(e => e.ArrivalAirportId)
                    .HasColumnType("int(11)")
                    .HasColumnName("ArrivalAirportID");

                entity.Property(e => e.DepartureAirportId)
                    .HasColumnType("int(11)")
                    .HasColumnName("DepartureAirportID");

                entity.Property(e => e.Distance).HasColumnType("int(11)");

                entity.Property(e => e.FlightTime).HasColumnType("int(11)");

                entity.HasOne(d => d.ArrivalAirport)
                    .WithMany(p => p.RouteArrivalAirports)
                    .HasForeignKey(d => d.ArrivalAirportId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Routes_Airports3");

                entity.HasOne(d => d.DepartureAirport)
                    .WithMany(p => p.RouteDepartureAirports)
                    .HasForeignKey(d => d.DepartureAirportId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Routes_Airports2");
            });

            modelBuilder.Entity<Schedule>(entity =>
            {
                entity.ToTable("schedules");

                entity.HasCharSet("utf8")
                    .UseCollation("utf8_bin");

                entity.HasIndex(e => e.AircraftId, "FK_Schedule_AirCraft");

                entity.HasIndex(e => e.RouteId, "FK_Schedule_Routes");

                entity.Property(e => e.Id)
                    .HasColumnType("int(11)")
                    .HasColumnName("ID");

                entity.Property(e => e.AircraftId)
                    .HasColumnType("int(11)")
                    .HasColumnName("AircraftID");

                entity.Property(e => e.Date).HasColumnType("date");

                entity.Property(e => e.FlightNumber).HasMaxLength(10);

                entity.Property(e => e.RouteId)
                    .HasColumnType("int(11)")
                    .HasColumnName("RouteID");

                entity.Property(e => e.Time).HasColumnType("time");

                entity.HasOne(d => d.Aircraft)
                    .WithMany(p => p.Schedules)
                    .HasForeignKey(d => d.AircraftId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Schedule_AirCraft");

                entity.HasOne(d => d.Route)
                    .WithMany(p => p.Schedules)
                    .HasForeignKey(d => d.RouteId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Schedule_Routes");
            });

            modelBuilder.Entity<Ticket>(entity =>
            {
                entity.ToTable("tickets");

                entity.HasCharSet("utf8")
                    .UseCollation("utf8_bin");

                entity.HasIndex(e => e.ScheduleId, "FK_Ticket_Schedule");

                entity.HasIndex(e => e.CabinTypeId, "FK_Ticket_TravelClass");

                entity.HasIndex(e => e.UserId, "FK_Ticket_User");

                entity.Property(e => e.Id)
                    .HasColumnType("int(11)")
                    .HasColumnName("ID");

                entity.Property(e => e.BookingReference)
                    .IsRequired()
                    .HasMaxLength(6);

                entity.Property(e => e.CabinTypeId)
                    .HasColumnType("int(11)")
                    .HasColumnName("CabinTypeID");

                entity.Property(e => e.Email).HasMaxLength(50);

                entity.Property(e => e.Firstname)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Lastname)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.PassportCountryId)
                    .HasColumnType("int(11)")
                    .HasColumnName("PassportCountryID");

                entity.Property(e => e.PassportNumber)
                    .IsRequired()
                    .HasMaxLength(9);

                entity.Property(e => e.Phone)
                    .IsRequired()
                    .HasMaxLength(14);

                entity.Property(e => e.ScheduleId)
                    .HasColumnType("int(11)")
                    .HasColumnName("ScheduleID");

                entity.Property(e => e.UserId)
                    .HasColumnType("int(11)")
                    .HasColumnName("UserID");

                entity.HasOne(d => d.CabinType)
                    .WithMany(p => p.Tickets)
                    .HasForeignKey(d => d.CabinTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Ticket_TravelClass");

                entity.HasOne(d => d.Schedule)
                    .WithMany(p => p.Tickets)
                    .HasForeignKey(d => d.ScheduleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Ticket_Schedule");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Tickets)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Ticket_User");
            });

            modelBuilder.Entity<Tracking>(entity =>
            {
                entity.ToTable("tracking");

                entity.HasIndex(e => e.UserId, "userId");

                entity.Property(e => e.Id)
                    .HasColumnType("int(11)")
                    .HasColumnName("id");

                entity.Property(e => e.LoginDateTime)
                    .HasColumnType("datetime")
                    .HasColumnName("loginDateTime");

                entity.Property(e => e.LogoutDateTime)
                    .HasColumnType("datetime")
                    .HasColumnName("logoutDateTime");

                entity.Property(e => e.LogoutReason)
                    .HasMaxLength(100)
                    .HasColumnName("logoutReason")
                    .UseCollation("utf8_general_ci")
                    .HasCharSet("utf8");

                entity.Property(e => e.TimeInSystem)
                    .HasColumnType("time")
                    .HasColumnName("timeInSystem");

                entity.Property(e => e.UserId)
                    .HasColumnType("int(11)")
                    .HasColumnName("userId");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Trackings)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("tracking_ibfk_1");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("users");

                entity.HasCharSet("utf8")
                    .UseCollation("utf8_bin");

                entity.HasIndex(e => e.OfficeId, "FK_Users_Offices");

                entity.HasIndex(e => e.RoleId, "FK_Users_Roles");

                entity.Property(e => e.Id)
                    .HasColumnType("int(11)")
                    .ValueGeneratedNever()
                    .HasColumnName("ID");

                entity.Property(e => e.Birthdate).HasColumnType("date");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.FirstName).HasMaxLength(50);

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.OfficeId)
                    .HasColumnType("int(11)")
                    .HasColumnName("OfficeID");

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.RoleId)
                    .HasColumnType("int(11)")
                    .HasColumnName("RoleID");

                entity.HasOne(d => d.Office)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.OfficeId)
                    .HasConstraintName("FK_Users_Offices");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Users_Roles");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
