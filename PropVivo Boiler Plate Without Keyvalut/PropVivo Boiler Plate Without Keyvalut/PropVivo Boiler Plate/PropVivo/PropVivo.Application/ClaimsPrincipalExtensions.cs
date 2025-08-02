using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace PropVivo.Application
{
    public class ClaimsPrincipalExtensions
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ClaimsPrincipalExtensions(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        }

        public string Email
        {
            get { return GetSingleClaimValue(ClaimTypes.Email); }
        }

        public string FirstName
        {
            get { return GetSingleClaimValue(ClaimTypes.GivenName); }
        }

        public bool? IsAuthenticated
        {
            get { return _httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated; }
        }

        public string LastName
        {
            get { return GetSingleClaimValue(ClaimTypes.Surname); }
        }

        public string Name
        {
            get { return GetSingleClaimValue(ClaimTypes.Name); }
        }

        public string PropertyId
        {
            get { return GetSingleClaimValue("PropertyId"); }
        }

        public string UserProfileId
        {
            get { return GetSingleClaimValue("id"); }
        }

        private IEnumerable<Claim>? FindClaims(string ClaimType)
        {
            if (IsAuthenticated.HasValue)
                return _httpContextAccessor.HttpContext?.User.Claims.Where(claim => claim.Type.Equals(ClaimType, StringComparison.Ordinal));

            return new List<Claim>();
        }

        private string GetSingleClaimValue(string claimType)
        {
            if (!string.IsNullOrEmpty(claimType))
            {
                Claim? c = FindClaims(claimType)?.FirstOrDefault();
                if (c != null)
                    return c.Value;
            }

            return string.Empty;
        }
    }
}