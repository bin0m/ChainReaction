using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace ChainReactionBack.Models
{
    public class ChainReactionContext : DbContext   
    {
        private const string ConnectionStringName = "DefaultConnection";

        public ChainReactionContext() : base(ConnectionStringName)
        {
        }

        public static ChainReactionContext Create()
        {
            return new ChainReactionContext();
        }


        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<SmartContract> SmartContracts { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // configures one-to-many relationship
            modelBuilder.Entity<Product>()
                .HasRequired(s => s.User)
                .WithMany(g => g.Products)
                .HasForeignKey(s => s.UserId);

            // configures one-to-many relationship
            modelBuilder.Entity<Transaction>()
                .HasRequired(s => s.FromUser)
                .WithMany(g => g.FromTransactions)
                .HasForeignKey(s => s.FromUserId)
                .WillCascadeOnDelete(false);

            // configures one-to-many relationship
            modelBuilder.Entity<Transaction>()
                .HasRequired(s => s.ToUser)
                .WithMany(g => g.ToTransactions)
                .HasForeignKey(s => s.ToUserId)
                .WillCascadeOnDelete(false);
        }
    }
}
