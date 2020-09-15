using MediatR;

namespace SK.Application.User.Commands.RegisterUser
{
    public class RegisterUserCommand : IRequest<User>
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
