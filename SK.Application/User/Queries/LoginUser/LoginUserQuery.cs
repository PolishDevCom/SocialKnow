using MediatR;

namespace SK.Application.User.Queries.LoginUser
{
    public class LoginUserQuery : IRequest<User>
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
