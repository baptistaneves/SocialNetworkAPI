using MediatR;
using SocialNetwork.Application.Identity.Dtos;
using SocialNetwork.Application.Models;

namespace SocialNetwork.Application.Identity.Commands
{
    public class LoginCommand : IRequest<OperationResult<IdentityUserProfileDto>>
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
