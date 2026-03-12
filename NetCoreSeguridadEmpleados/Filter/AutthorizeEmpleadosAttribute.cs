using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace NetCoreSeguridadEmpleados.Filter
{
    public class AutthorizeEmpleadosAttribute : AuthorizeAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            //por ahora solamente nos interesa validar si existe o no el empleado
            var user = context.HttpContext.User;

            //necesitamos el action y el controller de donde el usuario ha pulsado
            //para ello tenemos RouteValues que contiene la info
            //RouteData["controller"]
            //RouteData["action"]
            //RouteData["idalgo"]
            string controller = context.RouteData.Values["controller"].ToString();
            string action = context.RouteData.Values["action"].ToString();

            var id = context.RouteData.Values["id"];

            ITempDataProvider provider = context.HttpContext.RequestServices.GetService<ITempDataProvider>();
            //esta clase contiene tempdata de nuestra app
            var temp = provider.LoadTempData(context.HttpContext);
            //almacenamos la info
            temp["controller"] = controller;
            temp["action"] = action;

            if (id != null)
            {
                temp["id"] = id.ToString();
            }
            else
            {
                //eliminamos la clave para que no se quede entre peticiones
                temp.Remove("id");
            }

            //reasignamos el tempdata para nuestra app
            provider.SaveTempData(context.HttpContext, temp);

            if (!user.Identity.IsAuthenticated)
            {
                context.Result = GetRoute("Managed", "Login");
            }
            //else
            //{
            //    //comprobamos los roles
            //    //tenemos en cuneta mayusculas
            //    if (!user.IsInRole("PRESIDENTE") && !user.IsInRole("DIRECTOR") && !user.IsInRole("ANALISTA"))
            //    {
            //        context.Result = GetRoute("Managed", "ErrorAcceso");
            //    }
            //}
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
