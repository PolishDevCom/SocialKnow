using MediatR;
using SK.Application.Common.Mapping;
using SK.Domain.Entities;

namespace SK.Application.User.Commands.RegisterUser
{
    public class RegisterUserCommand : IRequest<User>, IMapTo<AppUser>
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public RegisterUserCommand()
        {
        }

        public RegisterUserCommand(RegisterUserDto registerCredentials)
        {
            Username = registerCredentials.Username;
            Email = registerCredentials.Email;
            Password = registerCredentials.Password;
        }
    }
}