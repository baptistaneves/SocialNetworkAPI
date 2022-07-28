namespace SocialNetwork.Api.Contracts.Posts.Requests
{
    public class PostCreate
    {
        [Required]
        [StringLength(1000)]
        public string TextContent { get; set; }
    }
}
