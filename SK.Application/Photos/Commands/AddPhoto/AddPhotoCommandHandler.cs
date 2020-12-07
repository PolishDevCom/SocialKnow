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

namespace SK.Application.Photos.Commands.AddPhoto
{
    public class AddPhotoCommandHandler : IRequestHandler<AddPhotoCommand, Photo>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUserService _currentUserService;
        private readonly IPhotoService _photoService;
        private readonly IStringLocalizer<PhotosResource> _localizer;

        public AddPhotoCommandHandler(
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

        public async Task<Photo> Handle(AddPhotoCommand request, CancellationToken cancellationToken)
        {
            var photoUploadResult = _photoService.AddPhoto(request.File);

            var user = await _context.Users
                .Include(u => u.Photos)
                .SingleOrDefaultAsync(x => x.UserName == _currentUserService.Username);

            var photo = new Photo
            {
                Url = photoUploadResult.Url,
                Id = photoUploadResult.PublicId
            };

            if (!user.Photos.Any(x => x.IsMain))
            {
                photo.IsMain = true;
            }

            user.Photos.Add(photo);

            var succes = await _context.SaveChangesAsync(cancellationToken) > 0;
            if (succes)
            {
                return photo;
            }
            throw new RestException(HttpStatusCode.BadRequest, new { Photo = _localizer["PhotoSaveError"] });
        }
    }
}
