using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace NetCoreSeguridadEmpleados.Policies
{
    public class OverSalarioRequirement : AuthorizationHandler<OverSalarioRequirement>, IAuthorizationRequirement
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, OverSalarioRequirement requirement)
        {
            //podriamos preguntar si existe un claim o no
            if (!context.User.HasClaim(x => x.Type == "Salario"))
            {
                context.Fail();
            }
            else
            {
                string dato = context.User.FindFirstValue("Salario");
                int salario = int.Parse(dato);

                if (salario >= 300000)
                {
                    context.Succeed(requirement);
                }
                else
                {
                    context.Fail();
                }
            }

            return Task.CompletedTask;
        }
    }
}
