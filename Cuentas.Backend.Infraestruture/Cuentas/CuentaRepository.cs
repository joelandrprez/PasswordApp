using Cuentas.Backend.Domain.Cuentas.Domain;
using Cuentas.Backend.Domain.Cuentas.DTO;
using Cuentas.Backend.Domain.Cuentas.Interfaces;
using Cuentas.Backend.Domain.Proyectos.Domain;
using Cuentas.Backend.Domain.Tipocuentas.Domain;
using Cuentas.Backend.Shared;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using static System.Formats.Asn1.AsnWriter;

namespace Cuentas.Backend.Infraestruture.Cuentas
{
    public class CuentaRepository : ICuentaRepository
    {
        private readonly ICustomConnection _connection;

        public CuentaRepository(ICustomConnection connection)
        {
            this._connection = connection;

        }

        public async Task Actualizar(Cuenta cuenta, SqlConnection conexion, SqlTransaction transaccion)
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

        public async Task Registrar(Cuenta cuenta, SqlConnection conexion, SqlTransaction transaccion)
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

        public async Task<Pagination<Cuenta>> Listar(int page, int size, string? search, string? orderBy, string? orderDir)
        {
            Pagination<Cuenta> paginacion = null;
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
                    paginacion = new Pagination<Cuenta>();

                    IEnumerable<Cuenta> records = await scope.QueryAsync<Cuenta,TipoCuenta,ProyectoDominio,Cuenta>(
                        "SEL_ListarCuentas",
                         (cuenta, tipocuenta, proyecto) =>
                         {
                             cuenta.TipoCuentaDetalle = tipocuenta;
                             cuenta.ProyectoDetalle = proyecto;
                             return cuenta;
                         }, dinamycParams, splitOn: "Id,Id", commandType: CommandType.StoredProcedure);

                    paginacion.Records = records;
                    paginacion.TotalGlobal = dinamycParams.Get<int>("TotalGlobal");
                    paginacion.TotalFiltered = dinamycParams.Get<int>("TotalFiltered");

                    int lastPage = ((paginacion.TotalFiltered % size) == 0) ? paginacion.TotalFiltered / size : (paginacion.TotalFiltered / size) + 1;

                    paginacion.Next = MaestraConstante.URL_NEXT + "/api/v1/cuentas?sort=page=2&size=" + size;
                    if (page == 1)
                    {
                        paginacion.Previus = null;
                    }
                    else
                    {
                        paginacion.Previus = string.Format(MaestraConstante.URL_NEXT + "/api/v1/cuentas?sort=&page={0}&size={1}", page - 1, size);
                    }

                    if (page >= lastPage)
                    {
                        paginacion.Next = null;
                    }
                    else
                    {
                        paginacion.Next = string.Format(MaestraConstante.URL_NEXT + "/api/v1/cuentas?sort=&page={0}&size={1}", page + 1, size);
                    }
                }
                catch (Exception ex)
                {
                    throw new CustomException("Sucedió un error al realizar la operación", ex);
                }
            }
            return paginacion;
        }

        public async Task<Cuenta> GetPassword(int id)
        {
            Cuenta cuenta = new Cuenta();
            try
            {
                using (var scope = await this._connection.BeginConnection())
                { 
                    DynamicParameters parametros = new DynamicParameters();
                    parametros.Add("id", id);

                    cuenta = await scope.QueryFirstOrDefaultAsync<Cuenta>("SEL_BuscarCuenta", parametros, commandType: CommandType.StoredProcedure);
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
