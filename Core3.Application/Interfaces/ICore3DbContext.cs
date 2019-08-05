using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Core3.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Core3.Application.Interfaces
{
    public interface ICore3DbContext
    {
        DbSet<Note> Notes { get; set; }

        DbSet<User> Users { get; set; }

        DbSet<UserToken> UserTokens { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
