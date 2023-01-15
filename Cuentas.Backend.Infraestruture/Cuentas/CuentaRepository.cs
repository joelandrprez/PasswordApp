using Cuentas.Backend.Domain.Cuentas.Domain;
using Cuentas.Backend.Domain.Cuentas.DTO;
using Cuentas.Backend.Domain.Cuentas.Interfaces;
using Cuentas.Backend.Shared;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cuentas.Backend.Infraestruture.Cuentas
{
    public class CuentaRepository : ICuentaRepository
    {
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

        public Task<Pagination<OutCuenta>> Search(int page, int size, string? search, string? orderBy, string? orderDir)
        {
            throw new NotImplementedException();
        }
    }
}
