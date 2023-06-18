using Cuentas.Backend.Domain.EstadoProyecto.Domain;
using Cuentas.Backend.Domain.EstadoProyecto.Interfaces;
using Cuentas.Backend.Shared;
using Dapper;
using System.Data;
using System.Data.SqlClient;

namespace Cuentas.Backend.Infraestruture.EstadoProyecto
{
    public class EstadoProyectoRepository : IEstadoProyectoRepository
    {
        private readonly ICustomConnection _connection;

        public EstadoProyectoRepository(ICustomConnection connection)
        {
            this._connection = connection;

        }

        public async Task<Paginacion<EEstadoProyecto>> Listar(int page, int size, string? search, string? orderBy, string? orderDir)
        {
            Paginacion<EEstadoProyecto> paginacion = null;
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
                    paginacion = new Paginacion<EEstadoProyecto>();

                    IEnumerable<EEstadoProyecto> records = await scope.QueryAsync<EEstadoProyecto>(
                        "SEL_ListarEstadoEstadoProyecto", dinamycParams, commandType: CommandType.StoredProcedure);

                    paginacion.Registros = records;
                    paginacion.TotalGlobal = dinamycParams.Get<int>("TotalGlobal");
                    paginacion.TotalFiltrado = dinamycParams.Get<int>("TotalFiltered");

                    int lastPage = ((paginacion.TotalFiltrado % size) == 0) ? paginacion.TotalFiltrado / size : (paginacion.TotalFiltrado / size) + 1;

                    paginacion.Seguiente = MaestraConstante.URL_NEXT+ "/api/v1/estadoproyecto?sort=page=2&size=" + size;
                    if (page == 1)
                    {
                        paginacion.Previo = null;
                    }
                    else
                    {
                        paginacion.Previo = string.Format(MaestraConstante.URL_NEXT + "/api/v1/estadoproyecto?sort=&page={0}&size={1}", page - 1, size);
                    }

                    if (page >= lastPage)
                    {
                        paginacion.Seguiente = null;
                    }
                    else
                    {
                        paginacion.Seguiente = string.Format(MaestraConstante.URL_NEXT + "/api/v1/estadoproyecto?sort=&page={0}&size={1}", page + 1, size);
                    }
                }
                catch (Exception ex)
                {
                    throw new CustomException("Sucedió un error al realizar la operación", ex);
                }
            }
            return paginacion;
        }

        public async Task Registrar(EEstadoProyecto estadoProject, SqlConnection conexion, SqlTransaction transaccion)
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

        public async Task Actualizar(EEstadoProyecto estadoProject, SqlConnection conexion, SqlTransaction transaccion)
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
