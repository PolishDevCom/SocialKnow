using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using System;
using System.Threading.Tasks;

namespace SK.API.Extensions
{
    public class RouteDataRequestCultureProvider : RequestCultureProvider
    {
        public int IndexOfCulture;
        public int IndexOfUICulture;

        public override Task<ProviderCultureResult> DetermineProviderCultureResult(HttpContext httpContext)
        {
            string culture = null;
            string uiCulture = null;

            if (httpContext == null)
                throw new ArgumentNullException(nameof(httpContext));

            culture = uiCulture = httpContext.Request.Path.Value.Split('/')[IndexOfCulture]?.ToString();

            var providerResultCulture = new ProviderCultureResult(culture, uiCulture);

            return Task.FromResult(providerResultCulture);
        }
    }
}