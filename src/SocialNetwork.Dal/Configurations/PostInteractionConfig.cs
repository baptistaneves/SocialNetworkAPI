using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SocialNetwork.Domain.Aggregates.PostAggregate;

namespace SocialNetwork.Dal.Configurations
{
    internal class PostInteractionConfig : IEntityTypeConfiguration<PostInteraction>
    {
        public void Configure(EntityTypeBuilder<PostInteraction> builder)
        {
            builder.HasKey(x=> x.InteractionId);
        }
    }
}
