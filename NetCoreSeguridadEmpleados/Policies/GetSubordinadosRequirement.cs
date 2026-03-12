using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using NetCoreSeguridadEmpleados.Models;
using NetCoreSeguridadEmpleados.Repositories;
using System.Security.Claims;

namespace NetCoreSeguridadEmpleados.Policies
{
    public class GetSubordinadosRequirement : AuthorizationHandler<GetSubordinadosRequirement>, IAuthorizationRequirement
    {
        protected override async Task<Task> HandleRequirementAsync(AuthorizationHandlerContext context, GetSubordinadosRequirement requirement)
        {
            var filterContext = context.Resource as AuthorizationFilterContext;
            var httpContext = filterContext.HttpContext;

            RepositoryEmpleados repo = httpContext.RequestServices.GetService<RepositoryEmpleados>();

            if (!context.User.HasClaim(x => x.Type == ClaimTypes.NameIdentifier))
            {
                context.Fail();
            }
            else
            {
                string dato = context.User.FindFirstValue(ClaimTypes.NameIdentifier);
                int id = int.Parse(dato);

                List<Empleado> subs = await repo.GetSubordinadosAsync(id);

                if (subs.Count == 0)
                {
                    context.Fail();
                }
                else
                {
                    context.Succeed(requirement);
                }
            }

            return Task.CompletedTask;
        }
    }
}
