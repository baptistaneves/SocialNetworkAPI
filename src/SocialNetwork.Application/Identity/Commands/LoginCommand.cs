using MediatR;
using SocialNetwork.Application.Models;

namespace SocialNetwork.Application.Identity.Commands
{
    public class LoginCommand : IRequest<OperationResult<string>>
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
