using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SocialNetwork.Domain.Aggregates.PostAggregate;

namespace SocialNetwork.Dal.Configurations
{
    internal class PostCommentConfig : IEntityTypeConfiguration<PostComment>
    {
        public void Configure(EntityTypeBuilder<PostComment> builder)
        {
            builder.HasKey(x => x.CommentId);
        }
    }
}
