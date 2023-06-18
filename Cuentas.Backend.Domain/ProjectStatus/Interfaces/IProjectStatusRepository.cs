using Cuentas.Backend.Domain.EstadoProyecto.Domain;
using Cuentas.Backend.Shared;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cuentas.Backend.Domain.EstadoProyecto.Interfaces
{
    public interface IProjectStatusRepository
    {
        Task Save(EProjectStatus estadoProject, SqlConnection conexion, SqlTransaction transaccion);

        Task Update(EProjectStatus estadoProject, SqlConnection conexion, SqlTransaction transaccion);

        Task<Pagination<EProjectStatus>> List(int page, int size, string? search, string? orderBy, string? orderDir);

    }
}
