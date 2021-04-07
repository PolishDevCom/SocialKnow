using AutoMapper;
using MediatR;
using Microsoft.Extensions.Localization;
using SK.Application.Common.Exceptions;
using SK.Application.Common.Interfaces;
using SK.Application.Common.Resources.Articles;
using SK.Domain.Entities;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace SK.Application.Articles.Commands.CreateArticle
{ 
    public class CreateArticleCommandHandler : IRequestHandler<CreateArticleCommand, Guid>
    {
        private readonly IApplicationDbContext _context;
        private readonly IStringLocalizer<ArticlesResource> _localizer;
        private readonly IMapper _mapper;

        public CreateArticleCommandHandler(IApplicationDbContext context, IStringLocalizer<ArticlesResource> localizer, IMapper mapper)
        {
            _context = context;
            _localizer = localizer;
            _mapper = mapper;
        }
        public async Task<Guid> Handle(CreateArticleCommand request, CancellationToken cancellationToken)
        {
            var article = _mapper.Map<Article>(request);

            _context.Articles.Add(article);
            var succes = await _context.SaveChangesAsync(cancellationToken) > 0;
            if (succes)
            {
                return article.Id;
            }
            throw new RestException(HttpStatusCode.BadRequest, new { Article = _localizer["ArticleSaveError"] });
        }
    }
}