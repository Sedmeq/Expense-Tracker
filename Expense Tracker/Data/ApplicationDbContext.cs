using Expense_Tracker.Models;
using Microsoft.EntityFrameworkCore;

namespace Expense_Tracker.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; } = null!;
        public DbSet<Transaction> Transactions { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure Category entity
            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasKey(e => e.CategoryId);
                entity.Property(e => e.Title).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Icon).HasMaxLength(5);
                entity.Property(e => e.Type).IsRequired().HasMaxLength(10);

                // Add index for better performance
                entity.HasIndex(e => e.Title).HasDatabaseName("IX_Categories_Title");
                entity.HasIndex(e => e.Type).HasDatabaseName("IX_Categories_Type");
            });

            // Configure Transaction entity
            modelBuilder.Entity<Transaction>(entity =>
            {
                entity.HasKey(e => e.TransactionId);
                entity.Property(e => e.Amount).IsRequired();
                entity.Property(e => e.Note).HasMaxLength(75);
                entity.Property(e => e.Date).IsRequired();

                // Configure foreign key relationship
                entity.HasOne(d => d.Category)
                    .WithMany(p => p.Transactions)
                    .HasForeignKey(d => d.CategoryId)
                    .OnDelete(DeleteBehavior.Restrict); // Prevent cascade delete

                // Add indexes for better performance
                entity.HasIndex(e => e.Date).HasDatabaseName("IX_Transactions_Date");
                entity.HasIndex(e => e.CategoryId).HasDatabaseName("IX_Transactions_CategoryId");
            });
        }
        // Override SaveChanges to add audit functionality
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                return await base.SaveChangesAsync(cancellationToken);
            }
            catch (DbUpdateException ex)
            {
                // Log the error and throw a more user-friendly exception
                throw new InvalidOperationException("An error occurred while saving to the database.", ex);
            }
        }
    }
}