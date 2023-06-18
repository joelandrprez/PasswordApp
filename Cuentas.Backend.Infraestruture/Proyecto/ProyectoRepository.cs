using Cuentas.Backend.Domain.Proyectos.Interfaces;
using Cuentas.Backend.Shared;
using Dapper;
using System.Data;


namespace Cuentas.Backend.Infraestruture.Proyecto
{
    public class ProyectoRepository : IProyectoRepository
    {
        private readonly ICustomConnection _connection;

        public ProyectoRepository(ICustomConnection connection)
        {
            this._connection = connection;
        }

        public async Task Actualizar(Domain.Proyectos.Domain.EProyecto proyecto)
        {
            using (var scope = await this._connection.BeginConnection())
            {
                try
                {
                    DynamicParameters parametros = new DynamicParameters();
                    parametros.Add("Id", proyecto.Id);
                    parametros.Add("Descripcion", proyecto.Descripcion);
                    parametros.Add("EstadoProyecto_Id", proyecto.EstadoProyecto_Id);
                    parametros.Add("UsuarioModifica", proyecto.UsuarioModifica);
                    parametros.Add("FechaModificacion", proyecto.FechaModificacion);
                    await scope.QueryAsync("UDP_ActualizarProyecto", parametros, commandType: CommandType.StoredProcedure);

                }
                catch (Exception ex)
                {
                    throw new CustomException("Sucedió un error al realizar la operación", ex);
                }
            }
        }

        public async Task<Paginacion<Domain.Proyectos.Domain.EProyecto>> Listar(int page, int size, string? search, string? orderBy, string? orderDir)
        {
            Paginacion<Domain.Proyectos.Domain.EProyecto> paginacion = null;
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
                    paginacion = new Paginacion<Domain.Proyectos.Domain.EProyecto>();

                    IEnumerable<Domain.Proyectos.Domain.EProyecto> records = await scope.QueryAsync<Domain.Proyectos.Domain.EProyecto>(
                        "SEL_ListarProyectos", dinamycParams, commandType: CommandType.StoredProcedure);

                    paginacion.Registros = records;
                    paginacion.TotalGlobal = dinamycParams.Get<int>("TotalGlobal");
                    paginacion.TotalFiltrado = dinamycParams.Get<int>("TotalFiltered");

                    int lastPage = ((paginacion.TotalFiltrado % size) == 0) ? paginacion.TotalFiltrado / size : (paginacion.TotalFiltrado / size) + 1;

                    paginacion.Seguiente = MaestraConstante.URL_NEXT + "/api/v1/proyecto?sort=page=2&size=" + size;
                    if (page == 1)
                    {
                        paginacion.Previo = null;
                    }
                    else
                    {
                        paginacion.Previo = string.Format(MaestraConstante.URL_NEXT + "/api/v1/proyecto?sort=&page={0}&size={1}", page - 1, size);
                    }

                    if (page >= lastPage)
                    {
                        paginacion.Seguiente = null;
                    }
                    else
                    {
                        paginacion.Seguiente = string.Format(MaestraConstante.URL_NEXT + "/api/v1/proyecto?sort=&page={0}&size={1}", page + 1, size);
                    }
                }
                catch (Exception ex)
                {
                    throw new CustomException("Sucedió un error al realizar la operación", ex);
                }
            }
            return paginacion;
        }

        public async Task Registrar(Domain.Proyectos.Domain.EProyecto proyecto)
        {
            using (var scope = await this._connection.BeginConnection())
            {
                try
                {
                    DynamicParameters parametros = new DynamicParameters();
                    parametros.Add("Descripcion", proyecto.Descripcion);
                    parametros.Add("EstadoProyecto_Id", proyecto.EstadoProyecto_Id);
                    parametros.Add("UsuarioCrea", proyecto.UsuarioCrea);
                    parametros.Add("FechaCreacion", proyecto.FechaCreacion);
                    parametros.Add("UsuarioModifica", proyecto.UsuarioModifica);
                    parametros.Add("FechaModificacion", proyecto.FechaModificacion);
                    await scope.QueryAsync("INS_RegistrarProyecto", parametros, commandType: CommandType.StoredProcedure);

                }
                catch (Exception ex)
                {
                    throw new CustomException("Sucedió un error al realizar la operación", ex);
                }
            }
        }
    }
}
