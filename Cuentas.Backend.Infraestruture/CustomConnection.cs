using Cuentas.Backend.Shared;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace Cuentas.Backend.Infraestruture
{
    public class CustomConnection : ICustomConnection
    {
        private string _connectionString;
        private readonly IConfiguration _config;
        private SqlConnection con;

        public CustomConnection(IConfiguration config)
        {
            this._config = config;
            this._connectionString = this._config.GetConnectionString("CuentasSqlConnection");
        }

        public async Task<IDbConnection> BeginConnection()
        {
            if (this.con == null)
                this.con = new SqlConnection(this._connectionString);

            if (this.con.State != System.Data.ConnectionState.Open)
            {
                if (string.IsNullOrEmpty(this.con.ConnectionString))
                    this.con.ConnectionString = this._connectionString;
                try
                {
                    await this.con.OpenAsync();
                }
                catch (Exception ex)
                {
                    throw new CustomException(string.Format("connectionString: {0}", this._connectionString), ex);
                }
            }
            return this.con;
        }

        public async Task CloseConnection()
        {
            await Task.Run(() =>
            {
                this.con.Close();
                this.con.Dispose();
            });
        }

    }
}