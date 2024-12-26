using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Data.Models;

public partial class RwaContext : DbContext
{
    public RwaContext()
    {
    }

    public RwaContext(DbContextOptions<RwaContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Author> Authors { get; set; }

    public virtual DbSet<Book> Books { get; set; }

    public virtual DbSet<BookLocation> BookLocations { get; set; }

    public virtual DbSet<Genre> Genres { get; set; }

    public virtual DbSet<Location> Locations { get; set; }

    public virtual DbSet<Reservation> Reservations { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=ConnectionStrings:Default");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Author>(entity =>
        {
            entity.HasKey(e => e.IdAuthor).HasName("PK__Authors__7411B2548C985A40");

            entity.Property(e => e.IdAuthor).HasColumnName("id_author");
            entity.Property(e => e.AuthorName)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("author_name");
        });

        modelBuilder.Entity<Book>(entity =>
        {
            entity.HasKey(e => e.IdBook).HasName("PK__Books__DAE712E896C04930");

            entity.HasIndex(e => e.Isbn, "UQ__Books__99F9D0A4F585BDA6").IsUnique();

            entity.Property(e => e.IdBook).HasColumnName("id_book");
            entity.Property(e => e.AuthorId).HasColumnName("author_id");
            entity.Property(e => e.Description)
                .HasColumnType("text")
                .HasColumnName("description");
            entity.Property(e => e.GenreId).HasColumnName("genre_id");
            entity.Property(e => e.Isbn)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("isbn");
            entity.Property(e => e.PublicationYear).HasColumnName("publication_year");
            entity.Property(e => e.Title)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("title");

            entity.HasOne(d => d.Author).WithMany(p => p.Books)
                .HasForeignKey(d => d.AuthorId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Books__author_id__17F790F9");

            entity.HasOne(d => d.Genre).WithMany(p => p.Books)
                .HasForeignKey(d => d.GenreId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Books__genre_id__18EBB532");
        });

        modelBuilder.Entity<BookLocation>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Book_Loc__3213E83F481F3694");

            entity.ToTable("Book_Location");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.BookId).HasColumnName("book_id");
            entity.Property(e => e.LocationId).HasColumnName("location_id");

            entity.HasOne(d => d.Book).WithMany(p => p.BookLocations)
                .HasForeignKey(d => d.BookId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Book_Loca__book___1DB06A4F");

            entity.HasOne(d => d.Location).WithMany(p => p.BookLocations)
                .HasForeignKey(d => d.LocationId)
                .HasConstraintName("FK__Book_Loca__locat__1EA48E88");
        });

        modelBuilder.Entity<Genre>(entity =>
        {
            entity.HasKey(e => e.IdGenre).HasName("PK__Genres__CB767C69892A9672");

            entity.HasIndex(e => e.GenreName, "UQ__Genres__1E98D151F73F5DAA").IsUnique();

            entity.Property(e => e.IdGenre).HasColumnName("id_genre");
            entity.Property(e => e.GenreName)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("genre_name");
        });

        modelBuilder.Entity<Location>(entity =>
        {
            entity.HasKey(e => e.IdLocation).HasName("PK__Location__276C0C6939EF1A34");

            entity.Property(e => e.IdLocation).HasColumnName("id_location");
            entity.Property(e => e.Address)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("address");
            entity.Property(e => e.LocationName)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("location_name");
        });

        modelBuilder.Entity<Reservation>(entity =>
        {
            entity.HasKey(e => e.IdReservation).HasName("PK__Reservat__92EE588F4D027C1E");

            entity.Property(e => e.IdReservation).HasColumnName("id_reservation");
            entity.Property(e => e.BookId).HasColumnName("book_id");
            entity.Property(e => e.ReservationDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("reservation_date");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValueSql("('Pending')")
                .HasColumnName("status");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.Book).WithMany(p => p.Reservations)
                .HasForeignKey(d => d.BookId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Reservati__book___245D67DE");

            entity.HasOne(d => d.User).WithMany(p => p.Reservations)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Reservati__user___236943A5");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.IdUser).HasName("PK__Users__D2D14637FE8F925F");

            entity.HasIndex(e => e.Username, "UQ__Users__F3DBC57218810AE6").IsUnique();

            entity.Property(e => e.IdUser).HasColumnName("id_user");
            entity.Property(e => e.FullName)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("full_name");
            entity.Property(e => e.IsAdmin)
                .HasDefaultValueSql("((0))")
                .HasColumnName("is_admin");
            entity.Property(e => e.PasswordHash)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("password_hash");
            entity.Property(e => e.Username)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("username");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
