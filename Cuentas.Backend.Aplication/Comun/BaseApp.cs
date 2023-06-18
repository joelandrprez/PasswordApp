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

        protected async Task<StatusSimpleResponse> SimpleProcess(Func<Task> callback, string titulo)
        {
            var response = new StatusSimpleResponse();

            try
            {
                await callback();

                response.Success = true;
                response.Title = titulo;
            }
            catch (CustomException customEx)
            {
                this._logger.LogError(customEx, "Id: {0}", response.Id);
                response.Success = false;
                response.Title = customEx.Titulo;
                response.Detail = customEx.ToString();
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex, "Id: {0}", response.Id);
                response.Success = false;
                response.Title = "Sucedió un error inesperado.";
                response.Detail = ex.ToString();
            }

            return response;
        }


        protected async Task<StatusResponse<T>> ComplexProcess<T>(Func<Task<T>> callbackData, string titulo = "")
        {
            var response = new StatusResponse<T>();

            try
            {
                response.Data = await callbackData();

                response.Title = titulo;
                response.Success = true;
            }
            catch (CustomException customEx)
            {
                this._logger.LogError(customEx, "Id: {0}", response.Id);
                response.Title = customEx.Titulo;
                response.Detail = customEx.ToString();
                response.Success = false;
                //response.Errores = cuEx.Errores
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex, "Id: {0}", response.Id);
                response.Title = "Sucedió un error inesperado.";
                response.Detail = ex.ToString();
                response.Success = false;
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
        protected SqlConnection ConectionToTransaction()
        {
            return new SqlConnection(_connectionString);
        }

    }
}
