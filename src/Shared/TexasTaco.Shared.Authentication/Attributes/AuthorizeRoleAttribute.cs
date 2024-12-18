using Microsoft.AspNetCore.Authorization;

namespace TexasTaco.Shared.Authentication.Attributes
{
    public class AuthorizeRoleAttribute : AuthorizeAttribute
    {
        public AuthorizeRoleAttribute(Role roleEnum)
        {
            Roles = roleEnum.ToString();
        }

        public AuthorizeRoleAttribute(params Role[] rolesEnums)
        {
            Roles = string.Join(",", rolesEnums.Select(r => r.ToString()));
        }
    }
}
