using AutoMapper;
using MediatR;
using Microsoft.Extensions.Localization;
using SK.Application.Common.Exceptions;
using SK.Application.Common.Interfaces;
using SK.Application.Common.Resources.Articles;
using SK.Domain.Entities;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace SK.Application.Articles.Commands.EditArticle
{
    public class EditArticleCommandHandler : IRequestHandler<EditArticleCommand>
    {
        private readonly IApplicationDbContext _context;
        private readonly IStringLocalizer<ArticlesResource> _localizer;
        private readonly IMapper _mapper;

        public EditArticleCommandHandler(IApplicationDbContext context, IStringLocalizer<ArticlesResource> localizer, IMapper mapper)
        {
            _context = context;
            _localizer = localizer;
            _mapper = mapper;
        }

        public async Task<Unit> Handle(EditArticleCommand request, CancellationToken cancellationToken)
        {
            var article = await _context.Articles.FindAsync(request.Id) ?? throw new NotFoundException(nameof(Article), request.Id);

            _mapper.Map(request, article);
            var success = await _context.SaveChangesAsync(cancellationToken) > 0;
            if (success)
            {
                return Unit.Value;
            }
            throw new RestException(HttpStatusCode.BadRequest, new { Article = _localizer["ArticleSaveError"] });
        }
    }
}