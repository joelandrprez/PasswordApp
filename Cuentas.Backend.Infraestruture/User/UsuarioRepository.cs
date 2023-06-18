using Cuentas.Backend.Domain.Usuario.Domain;
using Cuentas.Backend.Domain.Usuario.Interfaces;
using Cuentas.Backend.Shared;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cuentas.Backend.Infraestruture.Usuario
{
    public class UsuarioRepository : IUserRepository
    {
        private readonly ICustomConnection _connection;

        public UsuarioRepository(ICustomConnection connection)
        {
            this._connection = connection;
        }

        public async Task Registrar(Domain.Usuario.Domain.EUser usuario, SqlConnection conexion, SqlTransaction transaccion)
        {
            try {
                DynamicParameters parametros = new DynamicParameters();
                parametros.Add("Usuario", usuario.Usuario);
                parametros.Add("Password", usuario.Password);
                parametros.Add("FechaCreacion", usuario.FechaCreacion);
                parametros.Add("UsuarioCreacion", usuario.UsuarioCreacion);
                parametros.Add("FechaModificacion", usuario.FechaModificacion);
                parametros.Add("UsuarioModificacion", usuario.UsuarioModificacion);

                await conexion.QueryAsync("INS_RegistrarUsuario", parametros, transaccion, commandType: CommandType.StoredProcedure);

            }
            catch (Exception ex) {
                throw new CustomException("Sucedió un error al realizar la operación", ex);
            }
        }

        public async Task Actualizar(Domain.Usuario.Domain.EUser usuario, int id, SqlConnection conexion, SqlTransaction transaccion) {
            try
            {
                DynamicParameters parametros = new DynamicParameters();
                parametros.Add("Id", usuario.Id);
                parametros.Add("Password", usuario.Password);
                parametros.Add("FechaModificacion", usuario.FechaModificacion);
                parametros.Add("UsuarioModificacion", usuario.UsuarioModificacion);

                await conexion.QueryAsync("UPD_ActualizarUsuario", parametros, transaccion, commandType: CommandType.StoredProcedure);

            }
            catch (Exception ex)
            {
                throw new CustomException("Sucedió un error al realizar la operación", ex);
            }
        }


        public async Task<EUser> ValidarExistenciaDeNombreDeUsuario(string nombreUsuario, SqlConnection conexion, SqlTransaction transaccion)
        {
            EUser Usuario = new();
            try
            {
                DynamicParameters parametros = new DynamicParameters();
                parametros.Add("Usuario", nombreUsuario);

                Usuario = await conexion.QueryFirstOrDefaultAsync<EUser>("SEL_BuscarUsuario", parametros, transaccion, commandType: CommandType.StoredProcedure);

            }
            catch (Exception ex)
            {
                throw new CustomException("Sucedió un error al realizar la operación", ex);
            }
            return Usuario;
        }
        public async Task<EUser> ValidarExistenciaDeNombreDeUsuarioSinTransaccion(string nombreUsuario)
        {
            EUser Usuario = new();

           DynamicParameters parametros = new DynamicParameters();
           parametros.Add("Usuario", nombreUsuario);

           using (var scope = await this._connection.BeginConnection())
           {
               try
               {
                   Usuario = await scope.QueryFirstOrDefaultAsync<EUser>("SEL_BuscarUsuario", parametros, commandType: CommandType.StoredProcedure);

               }
               catch (Exception ex)
               {
                   throw new CustomException("Sucedió un error al realizar la operación", ex);
               }
           }


            return Usuario;
        }

        public async Task<Pagination<EUser>> Listar(int page, int size, string? search, string? orderBy, string? orderDir)
        {
            Pagination<EUser> paginacion = null;
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
                    paginacion = new Pagination<EUser>();

                    IEnumerable<EUser> records = await scope.QueryAsync<EUser>(
                        "SEL_ListarUsuarios", dinamycParams, commandType: CommandType.StoredProcedure);

                    paginacion.Records = records;
                    paginacion.TotalGlobal = dinamycParams.Get<int>("TotalGlobal");
                    paginacion.TotalFiltered = dinamycParams.Get<int>("TotalFiltered");

                    int lastPage = ((paginacion.TotalFiltered % size) == 0) ? paginacion.TotalFiltered / size : (paginacion.TotalFiltered / size) + 1;

                    paginacion.Next = MaestraConstante.URL_NEXT + "/api/v1/usuario?sort=page=2&size=" + size;
                    if (page == 1)
                    {
                        paginacion.Previus = null;
                    }
                    else
                    {
                        paginacion.Previus = string.Format(MaestraConstante.URL_NEXT + "/api/v1/usuario?sort=&page={0}&size={1}", page - 1, size);
                    }

                    if (page >= lastPage)
                    {
                        paginacion.Next = null;
                    }
                    else
                    {
                        paginacion.Next = string.Format(MaestraConstante.URL_NEXT + "/api/v1/usuario?sort=&page={0}&size={1}", page + 1, size);
                    }
                }
                catch (Exception ex)
                {
                    throw new CustomException("Sucedió un error al realizar la operación", ex);
                }
            }
            return paginacion;
        }
    }
}
