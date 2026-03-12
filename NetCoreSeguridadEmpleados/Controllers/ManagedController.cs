using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using NetCoreSeguridadEmpleados.Models;
using NetCoreSeguridadEmpleados.Repositories;
using System.Security.Claims;

namespace NetCoreSeguridadEmpleados.Controllers
{
    public class ManagedController : Controller
    {
        private RepositoryEmpleados repo;

        public ManagedController(RepositoryEmpleados repo)
        {
            this.repo = repo;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string name, string pass)
        {
            int idEmpleado = int.Parse(pass);
            Empleado emp = await repo.LogInEmpleadoAsync(name, idEmpleado);
            if (emp != null)
            {
                ClaimsIdentity identity = new ClaimsIdentity
                    (
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        ClaimTypes.Name, ClaimTypes.Role
                    );

                //empleado ARROYO: 7499 sera el admin
                if (emp.IdEmpleado == 7499)
                {
                    Claim claimAdmin = new Claim("Admin", "Soy el anmo de la empresa");
                    identity.AddClaim(claimAdmin);
                }

                Claim claimName = new Claim(ClaimTypes.Name, name);
                identity.AddClaim(claimName);
                Claim claimId = new Claim(ClaimTypes.NameIdentifier, pass);
                identity.AddClaim(claimId);
                //como rol vamos a utilizar el oficio
                Claim claimRol = new Claim(ClaimTypes.Role, emp.Oficio);
                identity.AddClaim(claimRol);
                Claim claimSalario = new Claim("Salario", emp.Salario.ToString());
                identity.AddClaim(claimSalario);
                Claim claimDept = new Claim("Dept", emp.IdDepartamento.ToString());
                identity.AddClaim(claimDept);
                ClaimsPrincipal userPrincipal = new ClaimsPrincipal(identity);
                await HttpContext.SignInAsync
                    (
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        userPrincipal
                    );

                string controller = TempData["controller"].ToString();
                string action = TempData["action"].ToString();

                //por ahora lo enviamos a una vista que haremos en breve
                return RedirectToAction(action, controller);
            }
            else
            {
                ViewData["mensaje"] = "Credenciales incorrectas";
                return View();
            }
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

        public IActionResult ErrorAcceso()
        {
            return View();
        }
    }
}
