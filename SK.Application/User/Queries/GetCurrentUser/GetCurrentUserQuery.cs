using MediatR;

namespace SK.Application.User.Queries.GetCurrentUser
{
    public class GetCurrentUserQuery : IRequest<User>
    {
    }
}