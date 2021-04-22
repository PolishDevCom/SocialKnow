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

namespace SK.Application.Tags.Commands.CreateTag
{
    public class CreateTagCommandHandler : IRequestHandler<CreateTagCommand, Guid>
    {
        private readonly IApplicationDbContext _context;
        private readonly IStringLocalizer<TagsResource> _localizer;
        private readonly IMapper _mapper;

        public CreateTagCommandHandler(IApplicationDbContext context, IStringLocalizer<TagsResource> localizer, IMapper mapper)
        {
            _context = context;
            _localizer = localizer;
            _mapper = mapper;
        }

        public async Task<Guid> Handle(CreateTagCommand request, CancellationToken cancellationToken)
        {
            var tag = _mapper.Map<Tag>(request);

            _context.Tags.Add(tag);
            var succes = await _context.SaveChangesAsync(cancellationToken) > 0;
            if (succes)
            {
                return tag.Id;
            }
            throw new RestException(HttpStatusCode.BadRequest, new { Tag = _localizer["TagSaveError"] });
        }
    }
}
