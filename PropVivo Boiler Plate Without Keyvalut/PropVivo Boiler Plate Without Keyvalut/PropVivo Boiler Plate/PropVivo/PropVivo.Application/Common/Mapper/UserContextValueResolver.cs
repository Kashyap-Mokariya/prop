using AutoMapper;
using PropVivo.Domain.Common;

namespace PropVivo.Application.Common.Mapper
{
    public class UserContextValueResolver<TSource, TDestination> : IValueResolver<TSource, TDestination, UserBase>
    {
        private readonly ClaimsPrincipalExtensions _userInfo;

        public UserContextValueResolver(ClaimsPrincipalExtensions userInfo)
        {
            _userInfo = userInfo ?? throw new ArgumentNullException(nameof(userInfo));
        }

        public UserBase Resolve(TSource source, TDestination destination, UserBase destMember, ResolutionContext context)
        {
            var userContext = (UserBase)typeof(TDestination).GetProperty("UserContext")?.GetValue(destination);
            return userContext != null
                ? _userInfo.GetUserBaseContext(userContext, DateTime.UtcNow)
                : _userInfo.GetUserBaseContext(DateTime.UtcNow, DateTime.UtcNow);
        }
    }
}