using Microsoft.AspNetCore.Mvc;
using NetCoreSeguridadEmpleados.Filter;
using NetCoreSeguridadEmpleados.Models;
using NetCoreSeguridadEmpleados.Repositories;
using System.Security.Claims;

namespace NetCoreSeguridadEmpleados.Controllers
{
    public class EmpleadosController : Controller
    {
        private RepositoryEmpleados repo;

        public EmpleadosController(RepositoryEmpleados repo)
        {
            this.repo = repo;
        }

        public async Task<IActionResult> Index()
        {
            List<Empleado> empleados = await repo.GetEmpleadosAsync();
            return View(empleados);
        }

        [AutthorizeEmpleados]
        public async Task<IActionResult> Details(int id)
        {
            Empleado emp = await repo.FindEmpleadoAsync(id);
            return View(emp);
        }

        [AutthorizeEmpleados]
        public async Task<IActionResult> Perfil()
        {
            return View();
        }

        [AutthorizeEmpleados(Policy = "JEFES")]
        public async Task<IActionResult> Compis()
        {
            //recuperamos el claim del user validado
            string dato = HttpContext.User.FindFirstValue("Dept");
            int idDepartamento = int.Parse(dato);
            List<Empleado> compis = await repo.GetEmpleadosDepartamentoAsync(idDepartamento);
            return View(compis);
        }

        [AutthorizeEmpleados(Policy = "JEFES")]
        [HttpPost]
        public async Task<IActionResult> Compis(int incremento)
        {
            //recuperamos el claim del user validado
            string dato = HttpContext.User.FindFirstValue("Dept");
            int idDepartamento = int.Parse(dato);
            await repo.UpdateSalarioEmpleadoDeptAsync(idDepartamento, incremento);
            List<Empleado> compis = await repo.GetEmpleadosDepartamentoAsync(idDepartamento);
            return View(compis);
        }

        [AutthorizeEmpleados(Policy = "ADMIN")]
        public async Task<IActionResult> AdminEmpleados()
        {
            return View();
        }

        [AutthorizeEmpleados(Policy = "RICOS")]
        public async Task<IActionResult> ZonaNoble()
        {
            return View();
        }


    }
}

