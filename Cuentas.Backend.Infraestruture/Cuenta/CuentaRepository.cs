using Cuentas.Backend.Domain.Cuentas.Domain;
using Cuentas.Backend.Domain.Cuentas.Interfaces;
using Cuentas.Backend.Domain.Tipocuentas.Domain;
using Cuentas.Backend.Shared;
using Dapper;
using System.Data;
using System.Data.SqlClient;


namespace Cuentas.Backend.Infraestruture.Cuentas
{
    public class CuentaRepository : ICuentaRepository
    {
        private readonly ICustomConnection _connection;

        public CuentaRepository(ICustomConnection connection)
        {
            this._connection = connection;

        }

        public async Task Actualizar(ECuenta cuenta, SqlConnection conexion, SqlTransaction transaccion)
        {
            try
            {
                DynamicParameters parametros = new DynamicParameters();
                parametros.Add("Id", cuenta.Id);
                parametros.Add("TipoCuenta_Id", cuenta.TipoCuenta_Id);
                parametros.Add("Sitio", cuenta.Sitio);
                parametros.Add("Usuario", cuenta.Usuario);
                parametros.Add("Password", cuenta.Password);
                parametros.Add("Usuario_Id", cuenta.Usuario_Id);
                parametros.Add("Proyecto_Id", cuenta.Proyecto_Id);
                parametros.Add("FechaModificacion", cuenta.FechaCreacion);
                parametros.Add("UsuarioModificacion", cuenta.UsuarioModificacion);

                await conexion.QueryAsync("UPD_UpdateCuentas", parametros, transaccion, commandType: CommandType.StoredProcedure);

            }
            catch (Exception ex)
            {
                throw new CustomException("Sucedió un error al realizar la operación", ex);
            }
        }

        public async Task Registrar(ECuenta cuenta, SqlConnection conexion, SqlTransaction transaccion)
        {
            try
            {
                DynamicParameters parametros = new DynamicParameters();

                parametros.Add("TipoCuenta_Id", cuenta.TipoCuenta_Id);
                parametros.Add("Sitio", cuenta.Sitio);
                parametros.Add("Usuario", cuenta.Usuario);
                parametros.Add("Password", cuenta.Password);
                parametros.Add("Usuario_Id", cuenta.Usuario_Id);
                parametros.Add("Proyecto_Id", cuenta.Proyecto_Id);
                parametros.Add("FechaCreacion", cuenta.FechaCreacion);
                parametros.Add("UsuarioCrea", cuenta.UsuarioCrea);
                parametros.Add("FechaModificacion", cuenta.FechaCreacion);
                parametros.Add("UsuarioModificacion", cuenta.UsuarioModificacion);

                await conexion.QueryAsync("INS_RegistrarCuentas", parametros, transaccion, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                throw new CustomException("Sucedió un error al realizar la operación", ex);
            }
        }

        public async Task<Paginacion<ECuenta>> Listar(int page, int size, string? search, string? orderBy, string? orderDir)
        {
            Paginacion<ECuenta> paginacion = null;
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
                    paginacion = new Paginacion<ECuenta>();

                    IEnumerable<ECuenta> records = await scope.QueryAsync(
                        "SEL_ListarCuentas",
                         (ECuenta cuenta, EtipoCuenta tipocuenta, Domain.Proyectos.Domain.EProyecto proyecto) =>
                         {
                             cuenta.TipoCuentaDetalle = tipocuenta;
                             cuenta.ProyectoDetalle = proyecto;
                             return cuenta;
                         }, dinamycParams, splitOn: "Id,Id", commandType: CommandType.StoredProcedure);

                    paginacion.Registros = records;
                    paginacion.TotalGlobal = dinamycParams.Get<int>("TotalGlobal");
                    paginacion.TotalFiltrado = dinamycParams.Get<int>("TotalFiltered");

                    int lastPage = ((paginacion.TotalFiltrado % size) == 0) ? paginacion.TotalFiltrado / size : (paginacion.TotalFiltrado / size) + 1;

                    paginacion.Seguiente = MaestraConstante.URL_NEXT + "/api/v1/cuentas?sort=page=2&size=" + size;
                    if (page == 1)
                    {
                        paginacion.Previo = null;
                    }
                    else
                    {
                        paginacion.Previo = string.Format(MaestraConstante.URL_NEXT + "/api/v1/cuentas?sort=&page={0}&size={1}", page - 1, size);
                    }

                    if (page >= lastPage)
                    {
                        paginacion.Seguiente = null;
                    }
                    else
                    {
                        paginacion.Seguiente = string.Format(MaestraConstante.URL_NEXT + "/api/v1/cuentas?sort=&page={0}&size={1}", page + 1, size);
                    }
                }
                catch (Exception ex)
                {
                    throw new CustomException("Sucedió un error al realizar la operación", ex);
                }
            }
            return paginacion;
        }

        public async Task<ECuenta> ObtenerPassword(int id)
        {
            ECuenta cuenta = new ECuenta();
            try
            {
                using (var scope = await this._connection.BeginConnection())
                { 
                    DynamicParameters parametros = new DynamicParameters();
                    parametros.Add("id", id);

                    cuenta = await scope.QueryFirstOrDefaultAsync<ECuenta>("SEL_BuscarCuenta", parametros, commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception ex)
            {
                throw new CustomException("Sucedió un error al realizar la operación", ex);
            }
            return cuenta;
        }
    }
}
