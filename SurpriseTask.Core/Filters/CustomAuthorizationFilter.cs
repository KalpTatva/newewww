using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;

public class CustomAuthorizationFilter : Attribute, IAuthorizationFilter
{
    private readonly string[] _requiredRole;

    public CustomAuthorizationFilter(params string[] requiredRole)
    {
        _requiredRole = requiredRole;
    }

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        // Example: Check if the user is authenticated and has the required role
        var user = context.HttpContext.User;

        if (!user.Identity?.IsAuthenticated ?? true)
        {
            // Redirect to Error404 page
            context.Result = new RedirectToActionResult("Error403", "Home", null);
            return;
        }
        bool hasAccess = _requiredRole.Any(r => user.IsInRole(r));
        if(!hasAccess)
        {
            context.Result = new RedirectToActionResult("Error403", "Home", null);
            return;
        }
    }
}
