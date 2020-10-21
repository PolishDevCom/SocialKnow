using MediatR;
using SK.Application.Common.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SK.Application.Discussions.Commands.CreateDiscussion
{
    public class CreateDiscussionCommandHandler : IRequestHandler<CreateDiscussionCommand, Guid>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUserService _currentUser;

        public CreateDiscussionCommandHandler(IApplicationDbContext context, ICurrentUserService currentUser)
        {
            _context = context;
            _currentUser = currentUser;
        }

        public Task<Guid> Handle(CreateDiscussionCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
