using MediatR;
using SK.Application.Common.Interfaces;
using SK.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace SK.Application.TestValues.Commands.CreateTestValue
{
    public class CreateArticleCommandHandler : IRequestHandler<CreateArticleCommand, int>
    {
        private readonly IApplicationDbContext _context;

        public CreateArticleCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<int> Handle(CreateArticleCommand request, CancellationToken cancellationToken)
        {
            var testValue = new Article
            {
                Id = request.Id,
                Name = request.Name
            };

            _context.TestValues.Add(testValue);
            await _context.SaveChangesAsync(cancellationToken);

            return testValue.Id;
        }
    }
}