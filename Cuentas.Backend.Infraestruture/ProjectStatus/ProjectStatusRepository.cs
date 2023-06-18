using Cuentas.Backend.Domain.EstadoProyecto.Domain;
using Cuentas.Backend.Domain.EstadoProyecto.Interfaces;
using Cuentas.Backend.Shared;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cuentas.Backend.Infraestruture.EstadoProyecto
{
    public class ProjectStatusRepository : IProjectStatusRepository
    {
        private readonly ICustomConnection _connection;

        public ProjectStatusRepository(ICustomConnection connection)
        {
            this._connection = connection;

        }

        public async Task<Pagination<EProjectStatus>> List(int page, int size, string? search, string? orderBy, string? orderDir)
        {
            Pagination<EProjectStatus> paginacion = null;
            DynamicParameters dinamycParams = new DynamicParameters();
            dinamycParams.Add("Page", page);
            dinamycParams.Add("Size", size);
            dinamycParams.Add("Search", search);
            dinamycParams.Add("OrderBy", orderBy);
            dinamycParams.Add("OrderDir", orderDir);
            dinamycParams.Add("TotalGlobal", null, DbType.Int32, ParameterDirection.Output);
            dinamycParams.Add("TotalFiltered", null, DbType.Int32, ParameterDirection.Output);
            using (var scope = await this._connection.BeginConnection())
            {
                try
                {
                    paginacion = new Pagination<EProjectStatus>();

                    IEnumerable<EProjectStatus> records = await scope.QueryAsync<EProjectStatus>(
                        "SEL_ListarEstadoEstadoProyecto", dinamycParams, commandType: CommandType.StoredProcedure);

                    paginacion.Records = records;
                    paginacion.TotalGlobal = dinamycParams.Get<int>("TotalGlobal");
                    paginacion.TotalFiltered = dinamycParams.Get<int>("TotalFiltered");

                    int lastPage = ((paginacion.TotalFiltered % size) == 0) ? paginacion.TotalFiltered / size : (paginacion.TotalFiltered / size) + 1;

                    paginacion.Next = MaestraConstante.URL_NEXT+ "/api/v1/estadoproyecto?sort=page=2&size=" + size;
                    if (page == 1)
                    {
                        paginacion.Previus = null;
                    }
                    else
                    {
                        paginacion.Previus = string.Format(MaestraConstante.URL_NEXT + "/api/v1/estadoproyecto?sort=&page={0}&size={1}", page - 1, size);
                    }

                    if (page >= lastPage)
                    {
                        paginacion.Next = null;
                    }
                    else
                    {
                        paginacion.Next = string.Format(MaestraConstante.URL_NEXT + "/api/v1/estadoproyecto?sort=&page={0}&size={1}", page + 1, size);
                    }
                }
                catch (Exception ex)
                {
                    throw new CustomException("Sucedió un error al realizar la operación", ex);
                }
            }
            return paginacion;
        }

        public async Task Save(EProjectStatus estadoProject, SqlConnection conexion, SqlTransaction transaccion)
        {
            DynamicParameters dinamycParams = new DynamicParameters();
            dinamycParams.Add("Estado", estadoProject.Estado);
            dinamycParams.Add("Descripcion", estadoProject.Descripcion);
            dinamycParams.Add("FechaCreacion", estadoProject.FechaCreacion);
            dinamycParams.Add("UsuarioCrea", estadoProject.UsuarioCrea);
            dinamycParams.Add("FechaModificacion", estadoProject.FechaCreacion);
            dinamycParams.Add("UsuarioModifica", estadoProject.UsuarioModifica);

            await conexion.QueryAsync("INS_RegistrarEstadoProyecto", dinamycParams, transaccion, commandType: CommandType.StoredProcedure);

        }

        public async Task Update(EProjectStatus estadoProject, SqlConnection conexion, SqlTransaction transaccion)
        {
            DynamicParameters dinamycParams = new DynamicParameters();
            dinamycParams.Add("Id", estadoProject.Id);
            dinamycParams.Add("Estado", estadoProject.Estado);
            dinamycParams.Add("Descripcion", estadoProject.Descripcion);
            dinamycParams.Add("FechaModificacion", estadoProject.FechaCreacion);
            dinamycParams.Add("UsuarioModifica", estadoProject.UsuarioModifica);

            await conexion.QueryAsync("UPD_ActualizarEstadoProyecto", dinamycParams, transaccion, commandType: CommandType.StoredProcedure);

        }
    }
}
