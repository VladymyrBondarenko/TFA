using Microsoft.EntityFrameworkCore;
using System.Drawing;
using TFA.Storage.Models;

namespace TFA.Storage
{
    public class ForumDbContext : DbContext
    {
        public ForumDbContext(DbContextOptions<ForumDbContext> opt) : base(opt)
        {
        }

        public DbSet<Forum> Forums { get; set; }

        public DbSet<Topic> Topics { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<Comment> Comments { get; set; }
    }
}