using PropVivo.Domain.Common;

namespace PropVivo.Application
{
    internal static class UserContextExtension
    {
        internal static UserBase GetUserBaseContext(this ClaimsPrincipalExtensions userInfo, DateTime createdOn, DateTime modifiedOn)
        {
            return new UserBase
            {
                CreatedByUserId = userInfo.UserProfileId,
                CreatedByUserName = userInfo.Name,
                CreatedOn = createdOn,
                ModifiedByUserId = userInfo.UserProfileId,
                ModifiedByUserName = userInfo.Name,
                ModifiedOn = modifiedOn
            };
        }

        internal static UserBase GetUserBaseContext(this ClaimsPrincipalExtensions userInfo, DateTime createdOn)
        {
            return new UserBase
            {
                CreatedByUserId = userInfo.UserProfileId,
                CreatedByUserName = userInfo.Name,
                CreatedOn = createdOn
            };
        }

        internal static UserBase GetUserBaseContext(this ClaimsPrincipalExtensions userInfo, UserBase userContext, DateTime modifiedOn)
        {
            userContext.ModifiedByUserId = userInfo.UserProfileId;
            userContext.ModifiedByUserName = userInfo.Name;
            userContext.ModifiedOn = modifiedOn;

            return userContext;
        }
    }
}