using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using SK.Application.Common.Exceptions;
using SK.Application.Common.Interfaces;
using SK.Application.Common.Resources.Profiles;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace SK.Application.Profiles.Commands.EditProfile
{
    public class EditProfileCommandHandler : IRequestHandler<EditProfileCommand>
    {
        private readonly IApplicationDbContext _context;
        private readonly IStringLocalizer<ProfilesResource> _localizer;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;

        public EditProfileCommandHandler(
            IApplicationDbContext context,
            IStringLocalizer<ProfilesResource> localizer,
            IMapper mapper,
            ICurrentUserService currentUserService)
        {
            _context = context;
            _localizer = localizer;
            _mapper = mapper;
            _currentUserService = currentUserService;
        }
        public async Task<Unit> Handle(EditProfileCommand request, CancellationToken cancellationToken)
        {
            var currentUser = _currentUserService.Username;
            var user = await _context.Users.FindAsync(currentUser) ?? throw new NotFoundException(nameof(Profile), currentUser);

            _mapper.Map(request, user);
            var success = await _context.SaveChangesAsync(cancellationToken) > 0;
            if (success)
            {
                return Unit.Value;
            }
            throw new RestException(HttpStatusCode.BadRequest, new { Profile = _localizer["ProfileSaveError"] });
        }
    }
}
