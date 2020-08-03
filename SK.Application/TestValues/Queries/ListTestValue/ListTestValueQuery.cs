using MediatR;
using System.Collections.Generic;

namespace SK.Application.TestValues.Queries.ListTestValue
{
    public class ListTestValueQuery : IRequest<List<TestValueDto>> { }
}