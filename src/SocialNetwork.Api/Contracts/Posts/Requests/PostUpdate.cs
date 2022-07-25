namespace SocialNetwork.Api.Contracts.Posts.Requests
{
    public class PostUpdate
    {
        [Required]
        [StringLength(1000)]
        public string TextContent { get; set; }
    }
}
