using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Simple.Smart.Chat.App.Models;

namespace Simple.Smart.Chat.App.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public DbSet<ChatMessage> ChatMessages { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ChatMessage>()
                .HasKey(e => e.Id);
            builder.Entity<ChatMessage>()
                .Property(e => e.DateSent)
                .ValueGeneratedOnAdd()
                .HasDefaultValueSql("GETDATE()"); 
            builder.Entity<ChatRoomUser>()
                .HasIndex(e => e.DisplayName)
                .IsUnique();
                
            builder.Entity<ChatRoomUser>()
                .HasMany(e => e.ChatMessages)
                .WithOne(e => e.ChatRoomUser)
                .HasForeignKey(e => e.UserId);
        }
    }
}
