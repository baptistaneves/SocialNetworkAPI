using MediatR;
using SocialNetwork.Application.Models;

namespace SocialNetwork.Application.Identity.Commands
{
    public class RemoveAccountCommand : IRequest<OperationResult<bool>>
    {
        public Guid IdentityUserId { get; set; }
        public Guid RequestorId { get; set; }
    }
}
