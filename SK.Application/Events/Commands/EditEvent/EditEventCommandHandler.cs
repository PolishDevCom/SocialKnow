using AutoMapper;
using MediatR;
using Microsoft.Extensions.Localization;
using SK.Application.Common.Exceptions;
using SK.Application.Common.Interfaces;
using SK.Application.Common.Resources.Events;
using SK.Domain.Entities;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace SK.Application.Events.Commands.EditEvent
{
    public class EditEventCommandHandler : IRequestHandler<EditEventCommand>
    {
        private readonly IApplicationDbContext _context;
        private readonly IStringLocalizer<EventsResource> _localize;
        private readonly IMapper _mapper;

        public EditEventCommandHandler(IApplicationDbContext context, IStringLocalizer<EventsResource> localize, IMapper mapper)
        {
            _context = context;
            _localize = localize;
            _mapper = mapper;
        }
        public async Task<Unit> Handle(EditEventCommand request, CancellationToken cancellationToken)
        {
            var eventToChange = await _context.Events.FindAsync(request.Id) ?? throw new NotFoundException(nameof(Event), request.Id);
            _mapper.Map(request, eventToChange);

            var success = await _context.SaveChangesAsync(cancellationToken) > 0;
            if (success)
            {
                return Unit.Value;
            }
            throw new RestException(HttpStatusCode.BadRequest, new { Event = _localize["EventSaveError"] });

        }
    }
}
