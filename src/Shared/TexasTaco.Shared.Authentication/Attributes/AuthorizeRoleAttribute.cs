using Microsoft.AspNetCore.Authorization;

namespace TexasTaco.Shared.Authentication.Attributes
{
    public class AuthorizeRoleAttribute : AuthorizeAttribute
    {
        public AuthorizeRoleAttribute(Role roleEnum)
        {
            Roles = roleEnum.ToString().Replace(" ", string.Empty);
        }
    }
}
