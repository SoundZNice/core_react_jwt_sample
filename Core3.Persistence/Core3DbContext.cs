using System;
using System.Collections.Generic;
using System.Text;
using Core3.Application.Interfaces;
using Core3.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Core3.Persistence
{
    public class Core3DbContext : DbContext, ICore3DbContext
    {
        public Core3DbContext(DbContextOptions<Core3DbContext> options)
            :base(options)
        {
        }

        public DbSet<Note> Notes { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<UserToken> UserTokens { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(Core3DbContext).Assembly);
        }
    }
}
