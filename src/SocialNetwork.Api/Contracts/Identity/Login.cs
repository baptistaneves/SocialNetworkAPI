namespace SocialNetwork.Api.Contracts.Identity
{
    public class Login
    {
        [Required]
        [EmailAddress]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
