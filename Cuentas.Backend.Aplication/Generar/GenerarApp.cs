using FluentValidation.Results;
using Cuentas.Backend.Shared;
using Cuentas.Backend.Aplication.Comun;
using Microsoft.Extensions.Logging;
using Cuentas.Backend.Domain.AppAngular.DTO;
using System.Diagnostics;

namespace Cuentas.Backend.Aplication.Generar
{
    public class GenerarApp : BaseApp<GenerarApp>
    {
        public GenerarApp(ILogger<BaseApp<GenerarApp>> logger) : base(logger)
        {

        }
        public async Task<StatusSimpleResponse> GenerarEstructura(InGenerarApp datoApp)
        {
            StatusSimpleResponse Respuesta = new StatusSimpleResponse();
            try
            {

                InGenerarAppValidator validator = new InGenerarAppValidator();
                ValidationResult result = validator.Validate(datoApp);
                if (!result.IsValid)
                {
                    Respuesta.Satisfactorio = false;
                    Respuesta.Titulo = "Los datos enviados no son válidos";
                    Respuesta.Errores = this.GetErrors(result.Errors);
                    Respuesta.Status = StatusCodes.Status400BadRequest;
                    return Respuesta;
                }

                if (!Directory.Exists(datoApp.Ruta))
                {
                    Respuesta.Satisfactorio = false;
                    Respuesta.Titulo = "Ruta no existe por favor cree la ruta :" + datoApp.Ruta;
                    Respuesta.Status = StatusCodes.Status500InternalServerError;
                    return Respuesta;
                }

                ProcessStartInfo startInfo = new ProcessStartInfo
                {
                    FileName = "cmd.exe",
                    RedirectStandardInput = true,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true,
                    UseShellExecute = false
                };

                Process process = new Process { StartInfo = startInfo };

                if (datoApp.Tipo == MaestraConstante.TIPO_ESTRUCTURA_CAPAS_ANGULAR) {

                    process.Start();
                    process.StandardInput.WriteLine("cd " + datoApp.Ruta);
                    process.StandardInput.WriteLine("D:");
                    process.StandardInput.WriteLine("ng g interceptor interceptors/header");
                    process.StandardInput.WriteLine("ng g c layouts/default");
                    process.StandardInput.WriteLine("ng g c layouts/default/components/header");
                    process.StandardInput.WriteLine("ng g c layouts/default/components/sidebar");
                    process.StandardInput.WriteLine("ng g c layouts/default/components/footer");
                    process.StandardInput.WriteLine("ng g c layouts/default/components/breadcrumbs");
                    process.StandardInput.Flush();
                    process.StandardInput.Close();
                    string output = process.StandardOutput.ReadToEnd();
                    process.WaitForExit();

                    string rutaArchivoDefault = Path.Combine(datoApp.Ruta, "src\\app\\layouts\\default") ;

                    //process.StandardInput.WriteLine("cd " + rutaArchivoDefault);

                    if (!Directory.Exists(rutaArchivoDefault))
                    {
                        Respuesta.Satisfactorio = false;
                        Respuesta.Titulo = "Ruta no existe por favor cree la ruta :" + datoApp.Ruta;
                        Respuesta.Status = StatusCodes.Status500InternalServerError;
                        return Respuesta;
                    }

                    string rutadefaulthtml = rutaArchivoDefault + "\\default.component.html";
                    string contenidodefaulthtml =   "<app-header></app-header> \n"+
                                                    "<div>\n" +
                                                    "    <div> \n" +
                                                    "        <app-sidebar></app-sidebar> \n" +
                                                    "    </div> \n" +
                                                    "    <div > \n" +
                                                    "        <main> \n" +
                                                    "            <router-outlet></router-outlet> \n" +
                                                    "        </main> \n" +
                                                    "    </div> \n" +
                                                    "</div>\n";
                    File.WriteAllText(rutadefaulthtml, contenidodefaulthtml);

                    string rutaArchivoAppModule = Path.Combine(datoApp.Ruta, "src\\app\\app.module.ts");
                    string[] contenidoEnlinea = File.ReadAllLines(rutaArchivoAppModule);
                    List<string> enLista = contenidoEnlinea.ToList();


                    foreach (string linea in enLista) {
                        if (linea.Contains(MaestraConstante.SECCION_IMPORT_MODULOS_ANGULAR)) { 
                            
                        }
                    }

                    File.WriteAllText(rutadefaulthtml, contenidodefaulthtml);

                }


                if (datoApp.Tipo == MaestraConstante.TIPO_ESTRUCTURA_CAPAS_ANGULAR)
                {
                }










                if (!Respuesta.Satisfactorio)
                    Respuesta.Status = StatusCodes.Status500InternalServerError;


            }
            catch (Exception)
            {

                throw;
            }
            Respuesta.Titulo = MaestraConstante.MENSAJE_OPERACION_EXITOSA;
            return Respuesta;

        }

    }
}
