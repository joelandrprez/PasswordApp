﻿using Cuentas.Backend.Domain.Usuario.Domain;
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
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly ICustomConnection _connection;

        public UsuarioRepository(ICustomConnection connection)
        {
            this._connection = connection;
        }

        public async Task Registrar(UsuarioPortal usuario, SqlConnection conexion, SqlTransaction transaccion)
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

        public async Task Actualizar(UsuarioPortal usuario, int id, SqlConnection conexion, SqlTransaction transaccion) {
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


        public async Task<UsuarioPortal> ValidarExistenciaDeNombreDeUsuario(string nombreUsuario, SqlConnection conexion, SqlTransaction transaccion)
        {
            UsuarioPortal Usuario = new();
            try
            {
                DynamicParameters parametros = new DynamicParameters();
                parametros.Add("Usuario", nombreUsuario);

                Usuario = await conexion.QueryFirstOrDefaultAsync<UsuarioPortal>("SEL_BuscarCuenta", parametros, transaccion, commandType: CommandType.StoredProcedure);

            }
            catch (Exception ex)
            {
                throw new CustomException("Sucedió un error al realizar la operación", ex);
            }
            return Usuario;
        }
        public async Task<UsuarioPortal> ValidarExistenciaDeNombreDeUsuarioSinTransaccion(string nombreUsuario)
        {
            UsuarioPortal Usuario = new();

           DynamicParameters parametros = new DynamicParameters();
           parametros.Add("Usuario", nombreUsuario);

           using (var scope = await this._connection.BeginConnection())
           {
               try
               {
                   Usuario = await scope.QueryFirstOrDefaultAsync<UsuarioPortal>("SEL_BuscarCuenta", parametros, commandType: CommandType.StoredProcedure);

               }
               catch (Exception ex)
               {
                   throw new CustomException("Sucedió un error al realizar la operación", ex);
               }
           }


            return Usuario;
        }
    }
}
