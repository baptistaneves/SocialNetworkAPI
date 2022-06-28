using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SocialNetwork.Domain.Aggregates.UserProfileAggregate;

namespace SocialNetwork.Dal.Configurations
{
    internal class UserProfileConfig : IEntityTypeConfiguration<UserProfile>
    {
        public void Configure(EntityTypeBuilder<UserProfile> builder)
        {
            builder.OwnsOne(x => x.BasicInfo);
        }
    }
}
