using AutoMapper;
using MediatR;
using Microsoft.Extensions.Localization;
using SK.Application.Common.Exceptions;
using SK.Application.Common.Interfaces;
using SK.Application.Common.Resources.Tags;
using SK.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SK.Application.Tags.Commands.EditTag
{
    public class EditTagCommandHandler : IRequestHandler<EditTagCommand>
    {
        private readonly IApplicationDbContext _context;
        private readonly IStringLocalizer<TagsResource> _localizer;
        private readonly IMapper _mapper;

        public EditTagCommandHandler(IApplicationDbContext context, IStringLocalizer<TagsResource> localizer, IMapper mapper)
        {
            _context = context;
            _localizer = localizer;
            _mapper = mapper;
        }

        public async Task<Unit> Handle(EditTagCommand request, CancellationToken cancellationToken)
        {
            var tag = await _context.Tags.FindAsync(request.Id) ?? throw new NotFoundException(nameof(Tag), request.Id);

            _mapper.Map(request, tag);
            var success = await _context.SaveChangesAsync(cancellationToken) > 0;
            if (success)
            {
                return Unit.Value;
            }
            throw new RestException(HttpStatusCode.BadRequest, new { Tag = _localizer["TagSaveError"] });
        }
    }
}
