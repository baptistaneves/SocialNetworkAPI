using MediatR;
using SocialNetwork.Application.Identity.Dtos;
using SocialNetwork.Application.Models;
using System.Security.Claims;

namespace SocialNetwork.Application.Identity.Queries
{
    public class GetCurrentUserQuery : IRequest<OperationResult<IdentityUserProfileDto>>
    {
        public Guid IdentityId { get; set; }
    }
}
