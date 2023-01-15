using Cuentas.Backend.Domain.Proyectos.Domain;
using Cuentas.Backend.Domain.Proyectos.Interfaces;
using Cuentas.Backend.Shared;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cuentas.Backend.Infraestruture.Proyecto
{
    public class ProyectoRepository : IProyectoRepository
    {
        private readonly ICustomConnection _connection;

        public ProyectoRepository(ICustomConnection connection)
        {
            this._connection = connection;
        }

        public async Task Actualizar(ProyectoDominio proyecto)
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

        public async Task<Pagination<ProyectoDominio>> Listar(int page, int size, string? search, string? orderBy, string? orderDir)
        {
            Pagination<ProyectoDominio> paginacion = null;
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
                    paginacion = new Pagination<ProyectoDominio>();

                    IEnumerable<ProyectoDominio> records = await scope.QueryAsync<ProyectoDominio>(
                        "SEL_ListarProyectos", dinamycParams, commandType: CommandType.StoredProcedure);

                    paginacion.Records = records;
                    paginacion.TotalGlobal = dinamycParams.Get<int>("TotalGlobal");
                    paginacion.TotalFiltered = dinamycParams.Get<int>("TotalFiltered");

                    int lastPage = ((paginacion.TotalFiltered % size) == 0) ? paginacion.TotalFiltered / size : (paginacion.TotalFiltered / size) + 1;

                    paginacion.Next = MaestraConstante.URL_NEXT + "/api/v1/proyecto?sort=page=2&size=" + size;
                    if (page == 1)
                    {
                        paginacion.Previus = null;
                    }
                    else
                    {
                        paginacion.Previus = string.Format(MaestraConstante.URL_NEXT + "/api/v1/proyecto?sort=&page={0}&size={1}", page - 1, size);
                    }

                    if (page >= lastPage)
                    {
                        paginacion.Next = null;
                    }
                    else
                    {
                        paginacion.Next = string.Format(MaestraConstante.URL_NEXT + "/api/v1/proyecto?sort=&page={0}&size={1}", page + 1, size);
                    }
                }
                catch (Exception ex)
                {
                    throw new CustomException("Sucedió un error al realizar la operación", ex);
                }
            }
            return paginacion;
        }

        public async Task Registrar(ProyectoDominio proyecto)
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
