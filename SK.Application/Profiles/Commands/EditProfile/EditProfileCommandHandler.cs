using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace SK.Application.Profiles.Commands.EditProfile
{
    public class EditProfileCommandHandler : IRequestHandler<EditProfileCommand>
    {
        public Task<Unit> Handle(EditProfileCommand request, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}
