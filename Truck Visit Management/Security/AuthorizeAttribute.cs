using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Linq;
using Truck_Visit_Management.Entities;
using Truck_Visit_Management.Security;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class AuthorizeAttribute : Attribute, IAuthorizationFilter
{
    private readonly string[] _roles;

    public AuthorizeAttribute(params string[] roles)
    {
        _roles = roles;
    }

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        // skip authorization if action is decorated with [AllowAnonymous] attribute
        var allowAnonymous = context.ActionDescriptor.EndpointMetadata.OfType<AllowAnonymousAttribute>().Any();
        if (allowAnonymous)
            return;

        // authorization
        var user = (User)context.HttpContext.Items["User"];
        if (user == null)
        {
            context.Result = new JsonResult(new { error = "Unauthorized", message = "You are not authorized to access this resource." })
            {
                StatusCode = StatusCodes.Status401Unauthorized
            };

            return;
        }

        // Check roles if specified
        if (_roles != null && _roles.Length > 0)
        {
            bool hasRole = _roles.Any(role => user.Role.Contains(role));
            if (!hasRole)
            {
                context.Result = new JsonResult(new { error = "Access Denied", message = "You do not have the required permission to access this resource." })
                {
                    StatusCode = StatusCodes.Status403Forbidden
                };
                return;
            }
        }
    }
}
