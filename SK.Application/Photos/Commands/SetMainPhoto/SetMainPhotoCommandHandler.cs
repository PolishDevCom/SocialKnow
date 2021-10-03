using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using SK.Application.Common.Exceptions;
using SK.Application.Common.Interfaces;
using SK.Application.Common.Resources.Photos;
using SK.Domain.Entities;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace SK.Application.Photos.Commands.SetMainPhoto
{
    public class SetMainPhotoCommandHandler : IRequestHandler<SetMainPhotoCommand>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUserService _currentUserService;
        private readonly IStringLocalizer<PhotosResource> _localizer;

        public SetMainPhotoCommandHandler(
            IApplicationDbContext context,
            ICurrentUserService currentUserService,
            IStringLocalizer<PhotosResource> localizer)
        {
            _context = context;
            _currentUserService = currentUserService;
            _localizer = localizer;
        }

        public async Task<Unit> Handle(SetMainPhotoCommand request, CancellationToken cancellationToken)
        {
            var user = await _context.Users
                .Include(u => u.Photos)
                .SingleOrDefaultAsync(x => x.UserName == _currentUserService.Username);

            var newMainPhoto = user.Photos.FirstOrDefault(p => p.Id == request.Id) ?? throw new NotFoundException(nameof(Photo), request.Id);
            var currentMainPhoto = user.Photos.FirstOrDefault(p => p.IsMain);

            currentMainPhoto.IsMain = false;
            newMainPhoto.IsMain = true;

            var succes = await _context.SaveChangesAsync(cancellationToken) > 0;
            if (succes)
            {
                return Unit.Value;
            }
            throw new RestException(HttpStatusCode.BadRequest, new { Photo = _localizer["PhotoSaveError"] });
        }
    }
}