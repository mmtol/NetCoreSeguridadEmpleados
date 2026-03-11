using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace NetCoreSeguridadEmpleados.Filter
{
    public class AutthorizeEmpleadosAttribute : AuthorizeAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            //por ahora solamente nos interesa validar si existe o no el empleado
            var user = context.HttpContext.User;
            if (!user.Identity.IsAuthenticated)
            {
                context.Result = GetRoute("Managed", "Login");
            }
            else
            {
                //comprobamos los roles
                //tenemos en cuneta mayusculas
                if (!user.IsInRole("PRESIDENTE") && !user.IsInRole("DIRECTOR") && !user.IsInRole("ANALISTA"))
                {
                    context.Result = GetRoute("Managed", "ErrorAcceso");
                }
            }
        }

        //en algun momento tendremos mas redirecciones que solo al login
        //por lo que creamos un metodo para redireccionar
        private RedirectToRouteResult GetRoute(string controller, string action)
        {
            RouteValueDictionary ruta = new RouteValueDictionary
                (
                    new
                    {
                        controller = controller,
                        action = action
                    }
                );
            RedirectToRouteResult result = new RedirectToRouteResult(ruta);
            return result;
        }
    }
}
