using Marketplace.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using SendGrid.Helpers.Errors.Model;

namespace Middleware
{
    [AttributeUsage(AttributeTargets.Method)]
    public class AuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        private readonly string _permission;

        public AuthorizeAttribute(string permission = "")
        {
            _permission = permission;
        }
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var allowAnonymous = context.ActionDescriptor.EndpointMetadata.OfType<AllowAnonymousAttribute>().Any();
            if (allowAnonymous) return;

            var roleId = context.HttpContext.Items["RoleId"];
            if (roleId == null)
                throw new UnauthorizedException();

            var db = context.HttpContext.RequestServices.GetService(typeof(DataContext)) as DataContext;

            if (!ValidatePrivilege(Convert.ToInt32(roleId), db))
                throw new ForbiddenException();
        }

        private bool ValidatePrivilege(int? roleId, DataContext? db)
        {
            if (roleId == null || db == null) return false;

            var roleExist = db.Roles.Any(x => x.id == roleId);
            if (!roleExist) return false;

            if (string.IsNullOrWhiteSpace(_permission)) return true;

            return db.RolePermissions
                .Any(x => x.role_id == roleId && x.permission == _permission);
        }
    }
}
