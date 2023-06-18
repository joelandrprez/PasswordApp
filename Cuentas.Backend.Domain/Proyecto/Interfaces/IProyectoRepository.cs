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
        Task<Paginacion<EProyecto>> Listar(int page, int size, string? search, string? orderBy, string? orderDir);

        Task Registrar(EProyecto proyecto);

        Task Actualizar(EProyecto proyecto);

    }
}
