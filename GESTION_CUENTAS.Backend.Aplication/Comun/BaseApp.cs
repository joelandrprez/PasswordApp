using Cuentas.Backend.Shared;
using FluentValidation.Results;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Data.SqlClient;


namespace Cuentas.Backend.Aplication.Comun
{
    public class BaseApp<T>
    {
        public readonly ILogger<BaseApp<T>> _logger;
        private readonly string _connectionString;
        private readonly IConfiguration _config;
        public BaseApp(ILogger<BaseApp<T>> logger)
        {
            _logger = logger;

        }
        public BaseApp(ILogger<BaseApp<T>> logger, IConfiguration configuracion)
        {
            _logger = logger;
            _config = configuracion;
            this._connectionString = this._config.GetConnectionString("CuentasSqlConnection");

        }

        protected async Task<StatusSimpleResponse> ProcesoSimple(Func<Task> callback, string titulo)
        {
            var response = new StatusSimpleResponse();

            try
            {
                await callback();

                response.Satisfactorio = true;
                response.Titulo = titulo;
            }
            catch (CustomException customEx)
            {
                this._logger.LogError(customEx, "Id: {0}", response.Id);
                response.Satisfactorio = false;
                response.Titulo = customEx.Titulo;
                response.Detalle = customEx.ToString();
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex, "Id: {0}", response.Id);
                response.Satisfactorio = false;
                response.Titulo = "Sucedió un error inesperado.";
                response.Detalle = ex.ToString();
            }

            return response;
        }


        protected async Task<StatusResponse<T>> ProcesoComplejo<T>(Func<Task<T>> callbackData, string titulo = "")
        {
            var response = new StatusResponse<T>();

            try
            {
                response.Data = await callbackData();

                response.Titulo = titulo;
                response.Satisfactorio = true;
            }
            catch (CustomException customEx)
            {
                this._logger.LogError(customEx, "Id: {0}", response.Id);
                response.Titulo = customEx.Titulo;
                response.Detalle = customEx.ToString();
                response.Satisfactorio = false;
                //response.Errores = cuEx.Errores
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex, "Id: {0}", response.Id);
                response.Titulo = "Sucedió un error inesperado.";
                response.Detalle = ex.ToString();
                response.Satisfactorio = false;
            }

            return response;
        }


        public Dictionary<string, List<string>> GetErrors(List<ValidationFailure> errors)
        {
            Dictionary<string, List<string>> result = new Dictionary<string, List<string>>();
            foreach (ValidationFailure failure in errors)
            {
                List<string> errorsList = null;
                if (!result.TryGetValue(failure.PropertyName, out errorsList))
                {
                    errorsList = new List<string>();
                    result[failure.PropertyName] = errorsList;
                }

                errorsList.Add(failure.ErrorMessage);
            }
            return result;
        }
        protected SqlConnection ConexionParaTransaccion()
        {
            return new SqlConnection(_connectionString);
        }

    }
}
