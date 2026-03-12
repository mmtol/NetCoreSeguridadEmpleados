using Microsoft.EntityFrameworkCore;
using NetCoreSeguridadEmpleados.Data;
using NetCoreSeguridadEmpleados.Models;

namespace NetCoreSeguridadEmpleados.Repositories
{
    public class RepositoryEmpleados
    {
        private HospitalContext context;

        public RepositoryEmpleados(HospitalContext context)
        {
            this.context = context;
        }

        public async Task<List<Empleado>> GetEmpleadosAsync()
        {
            return await context.Empleados.ToListAsync();
        }

        public async Task<Empleado> FindEmpleadoAsync(int idEmpleado)
        {
            return await context.Empleados.FirstOrDefaultAsync(e => e.IdEmpleado == idEmpleado);
        }

        public async Task<List<Empleado>> GetEmpleadosDepartamentoAsync(int idDepartamento)
        {
            return await context.Empleados.Where(e => e.IdDepartamento == idDepartamento).ToListAsync();
        }

        public async Task UpdateSalarioEmpleadoDeptAsync(int idDepartamento, int incremento)
        {
            List<Empleado> empleados = await GetEmpleadosDepartamentoAsync(idDepartamento);
            foreach (Empleado emp in empleados)
            {
                emp.Salario += incremento;
            }
            await context.SaveChangesAsync();
        }

        public async Task<Empleado> LogInEmpleadoAsync(string apellido, int idEmpleado)
        {
            Empleado emp = await context.Empleados.FirstOrDefaultAsync(e => e.Apellido == apellido && e.IdEmpleado == idEmpleado);
            return emp;
        }

        public async Task<List<Empleado>> GetSubordinadosAsync(int idEmpleado)
        {
            var consulta = from datos in context.Empleados
                           where datos.Dir == idEmpleado
                           select datos;
            return await consulta.ToListAsync();
        }

        public async Task DeleteEmpleado(int idEmpleado)
        {
            Empleado emp = await FindEmpleadoAsync(idEmpleado);
            context.Empleados.Remove(emp);
            context.SaveChanges();
        }
    }
}

