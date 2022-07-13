using SocialNetwork.Application.Enums;

namespace SocialNetwork.Application.Models
{
    public class Error
    {
        public ErrorCode Code { get; set; }
        public string Message { get; set; }
    }
}
