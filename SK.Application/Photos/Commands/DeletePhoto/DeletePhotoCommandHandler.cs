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

namespace SK.Application.Photos.Commands.DeletePhoto
{
    public class DeletePhotoCommandHandler : IRequestHandler<DeletePhotoCommand>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUserService _currentUserService;
        private readonly IPhotoService _photoService;
        private readonly IStringLocalizer<PhotosResource> _localizer;

        public DeletePhotoCommandHandler(
            IApplicationDbContext context,
            ICurrentUserService currentUserService,
            IPhotoService photoService,
            IStringLocalizer<PhotosResource> localizer)
        {
            _context = context;
            _currentUserService = currentUserService;
            _photoService = photoService;
            _localizer = localizer;
        }

        public async Task<Unit> Handle(DeletePhotoCommand request, CancellationToken cancellationToken)
        {
            var user = await _context.Users
                .Include(u => u.Photos)
                .SingleOrDefaultAsync(x => x.UserName == _currentUserService.Username);

            var photo = user.Photos.FirstOrDefault(p => p.Id == request.Id) ?? throw new NotFoundException(nameof(Photo), request.Id);

            if (photo.IsMain)
            {
                throw new RestException(HttpStatusCode.BadRequest, new { Photo = _localizer["PhotoDeleteMainPhotoError"] });
            }

            var result = _photoService.DeletePhoto(photo.Id) ?? throw new RestException(HttpStatusCode.BadRequest, new { Photo = _localizer["PhotoDeletError"] });

            user.Photos.Remove(photo);

            var succes = await _context.SaveChangesAsync(cancellationToken) > 0;
            if (succes)
            {
                return Unit.Value;
            }
            throw new RestException(HttpStatusCode.BadRequest, new { Photo = _localizer["PhotoSaveError"] });
        }
    }
}