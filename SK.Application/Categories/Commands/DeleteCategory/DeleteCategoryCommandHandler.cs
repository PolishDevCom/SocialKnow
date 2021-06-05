using MediatR;
using Microsoft.Extensions.Localization;
using SK.Application.Common.Exceptions;
using SK.Application.Common.Interfaces;
using SK.Application.Common.Resources.Categories;
using SK.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SK.Application.Categories.Commands.DeleteCategory
{
    public class DeleteCategoryCommandHandler : IRequestHandler<DeleteCategoryCommand>
    {
        private readonly IApplicationDbContext _context;
        private readonly IStringLocalizer<CategoriesResource> _localizer;

        public DeleteCategoryCommandHandler(IApplicationDbContext context, IStringLocalizer<CategoriesResource> localizer)
        {
            _context = context;
            _localizer = localizer;
        }

        public async Task<Unit> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
        {
            var categoryToDelete = await _context.Categories.FindAsync(request.Id) ?? throw new NotFoundException(nameof(Category), request.Id);
            _context.Categories.Remove(categoryToDelete);
            var success = await _context.SaveChangesAsync(cancellationToken) > 0;
            if (success)
            {
                return Unit.Value;
            }
            throw new RestException(HttpStatusCode.BadRequest, new { Tag = _localizer["CategorySaveError"] });
        }
    }
}
