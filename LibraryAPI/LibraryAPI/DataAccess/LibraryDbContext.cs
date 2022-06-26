using LibraryAPI.Domain.Authors;
using LibraryAPI.Domain.Books;
using LibraryAPI.Domain.StatusHistories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SL = LibraryAPI.DataAccess.Consts.StringLengths;

namespace LibraryAPI.DataAccess
{
    public class LibraryDbContext : DbContext
    {
        public DbSet<Author> Authors => Set<Author>();
        public DbSet<Book> Books => Set<Book>();
        public DbSet<StatusHistory> StatusHistory => Set<StatusHistory>();

        public LibraryDbContext(DbContextOptions<LibraryDbContext> options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            ConfigureAuthors(builder);
            ConfigureBooks(builder);
            ConfigureStatusHistory(builder);
        }

        private static void ConfigureAuthors(ModelBuilder builder)
        {
            builder.Entity<Author>(cfg =>
            {
                cfg.HasKey(e => e.Id);

                cfg.HasMany(e => e.Books);
            });
        }

        private static void ConfigureBooks(ModelBuilder builder)
        {
            builder.Entity<Book>(cfg =>
            {
                cfg.HasKey(e => e.Id);

                cfg.Property(e => e!.Language)
                    .HasMaxLength(SL.TinyString);

                cfg.Property(e => e.Genre)
                    .HasConversion(new EnumToStringConverter<BookGenres>());

                // After seeding data
                cfg.HasOne(e => e.CurrentStatus);

                cfg.HasMany(e => e.StatusHistory)
                    .WithOne(e => e.Book);

                cfg.HasMany(e => e.Authors)
                    .WithMany(e => e.Books)
                    .UsingEntity(e =>
                    {
                        e.Property("AuthorsId").HasColumnName("AuthorId");
                        e.Property("BooksId").HasColumnName("BookId");
                        e.ToTable("BookAuthor");
                    });
            });
        }

        private static void ConfigureStatusHistory(ModelBuilder builder)
        {
            builder.Entity<StatusHistory>(cfg =>
            {
                cfg.HasKey(e => e.Id);

                cfg.HasOne(e => e.Book);

                cfg.Property(e => e.Status)
                    .HasConversion(new EnumToStringConverter<Statuses>());
            });
        }
    }
}