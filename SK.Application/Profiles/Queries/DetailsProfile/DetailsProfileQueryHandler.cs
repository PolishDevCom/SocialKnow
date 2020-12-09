using MediatR;
using Microsoft.EntityFrameworkCore;
using SK.Application.Common.Exceptions;
using SK.Application.Common.Interfaces;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SK.Application.Profiles.Queries.DetailsProfile
{
    public class DetailsProfileQueryHandler : IRequestHandler<DetailsProfileQuery, ProfileDto>
    {
        private readonly IApplicationDbContext _context;

        public DetailsProfileQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<ProfileDto> Handle(DetailsProfileQuery request, CancellationToken cancellationToken)
        {
            var user = await _context.Users.SingleOrDefaultAsync(u => u.UserName == request.Username) 
                ??
                throw new NotFoundException(nameof(User), request.Username);

            return new ProfileDto()
            {
                Username = user.UserName,
                Image = user.Photos.FirstOrDefault(p => p.IsMain)?.Url,
                Bio = user.Bio,
                Photos = user.Photos
            };
        }
    }
}
