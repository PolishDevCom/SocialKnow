using SK.Application.Common.Models;
using System;

namespace SK.Application.Common.Interfaces
{
    public interface IUriService
    {
        public Uri GetPageUri(PaginationFilter filter, string route);
    }
}
