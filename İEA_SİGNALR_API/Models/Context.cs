using Microsoft.EntityFrameworkCore;
using System.Data.Common;
using System.Security.Cryptography.X509Certificates;

namespace İEA_SİGNALR_API.Models
{
    public class Context:DbContext
    {
        public Context(DbContextOptions<Context> options) : base(options)
        {
        }
            public DbSet<Room>Rooms { get;set; }
            public DbSet<User>Users { get;set; }
        }
    }

