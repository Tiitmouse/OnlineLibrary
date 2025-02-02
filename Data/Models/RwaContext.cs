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

    public virtual DbSet<Log> Logs { get; set; }

    public virtual DbSet<Rating> Ratings { get; set; }

    public virtual DbSet<Reservation> Reservations { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=ConnectionStrings:Default");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Author>(entity =>
        {
            entity.HasKey(e => e.IdAuthor).HasName("PK__Authors__7411B25428631CF0");

            entity.Property(e => e.IdAuthor).HasColumnName("id_author");
            entity.Property(e => e.AuthorName)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("author_name");
        });

        modelBuilder.Entity<Book>(entity =>
        {
            entity.HasKey(e => e.IdBook).HasName("PK__Books__DAE712E83E789D20");

            entity.HasIndex(e => e.Isbn, "UQ__Books__99F9D0A4A759FC6C").IsUnique();

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
                .HasConstraintName("FK__Books__author_id__3D5E1FD2");

            entity.HasOne(d => d.Genre).WithMany(p => p.Books)
                .HasForeignKey(d => d.GenreId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Books__genre_id__3E52440B");
        });

        modelBuilder.Entity<BookLocation>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Book_Loc__3213E83FA7AAF59E");

            entity.ToTable("Book_Location");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.BookId).HasColumnName("book_id");
            entity.Property(e => e.LocationId).HasColumnName("location_id");

            entity.HasOne(d => d.Book).WithMany(p => p.BookLocations)
                .HasForeignKey(d => d.BookId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Book_Loca__book___4316F928");

            entity.HasOne(d => d.Location).WithMany(p => p.BookLocations)
                .HasForeignKey(d => d.LocationId)
                .HasConstraintName("FK__Book_Loca__locat__440B1D61");
        });

        modelBuilder.Entity<Genre>(entity =>
        {
            entity.HasKey(e => e.IdGenre).HasName("PK__Genres__CB767C693E7C70BD");

            entity.HasIndex(e => e.GenreName, "UQ__Genres__1E98D15177A58CA9").IsUnique();

            entity.Property(e => e.IdGenre).HasColumnName("id_genre");
            entity.Property(e => e.GenreName)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("genre_name");
        });

        modelBuilder.Entity<Location>(entity =>
        {
            entity.HasKey(e => e.IdLocation).HasName("PK__Location__276C0C69F7E27A54");

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

        modelBuilder.Entity<Log>(entity =>
        {
            entity.HasKey(e => e.IdLog).HasName("PK__Logs__6CC851FE06FD988B");

            entity.Property(e => e.IdLog).HasColumnName("id_log");
            entity.Property(e => e.Date)
                .HasColumnType("datetime")
                .HasColumnName("date");
            entity.Property(e => e.Importance).HasColumnName("importance");
            entity.Property(e => e.Message)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("message");
        });

        modelBuilder.Entity<Rating>(entity =>
        {
            entity.HasKey(e => e.IdRating).HasName("PK__Ratings__12074E47F787B4B6");

            entity.Property(e => e.IdRating).HasColumnName("id_rating");
            entity.Property(e => e.BookId).HasColumnName("book_id");
            entity.Property(e => e.Comment)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("comment");
            entity.Property(e => e.Rating1).HasColumnName("rating");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.Book).WithMany(p => p.Ratings)
                .HasForeignKey(d => d.BookId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Ratings__book_id__534D60F1");

            entity.HasOne(d => d.User).WithMany(p => p.Ratings)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Ratings__user_id__52593CB8");
        });

        modelBuilder.Entity<Reservation>(entity =>
        {
            entity.HasKey(e => e.IdReservation).HasName("PK__Reservat__92EE588F98274C09");

            entity.Property(e => e.IdReservation).HasColumnName("id_reservation");
            entity.Property(e => e.BookLocationId).HasColumnName("bookLocation_id");
            entity.Property(e => e.ReservationDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("reservation_date");
            entity.Property(e => e.Status)
                .HasDefaultValueSql("((0))")
                .HasColumnName("status");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.BookLocation).WithMany(p => p.Reservations)
                .HasForeignKey(d => d.BookLocationId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Reservati__bookL__4D94879B");

            entity.HasOne(d => d.User).WithMany(p => p.Reservations)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Reservati__user___4CA06362");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.IdUser).HasName("PK__Users__D2D146372FB5736A");

            entity.HasIndex(e => e.Username, "UQ__Users__F3DBC572BB8EE5A8").IsUnique();

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
