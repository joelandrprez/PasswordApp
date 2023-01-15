using Cuentas.Backend.Domain.Proyectos.Domain;
using Cuentas.Backend.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cuentas.Backend.Domain.Proyectos.Interfaces
{
    public interface IProyectoRepository
    {
        Task<Pagination<ProyectoDominio>> Listar(int page, int size, string? search, string? orderBy, string? orderDir);

        Task Registrar(ProyectoDominio proyecto);

        Task Actualizar(ProyectoDominio proyecto);

    }
}
