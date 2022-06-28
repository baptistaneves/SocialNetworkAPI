using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SocialNetwork.Domain.Aggregates.PostAggregate;
using SocialNetwork.Domain.Aggregates.UserProfileAggregate;

namespace SocialNetwork.Dal.Context
{
    public class DataContext : IdentityDbContext
    {
        public DataContext(DbContextOptions options) : base(options) {}

        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<Post> Posts { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfigurationsFromAssembly(typeof(DataContext).Assembly);
        }
    }
}
