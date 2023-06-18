using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cuentas.Backend.Shared
{
    public class StatusSimpleResponse
    {
        public Guid Id { get; set; }
        public bool Success { get; set; }
        public string Title { get; set; }
        public string Detail { get; set; }
        public int StatusCode { get ; set; }
        public Dictionary<string, List<string>> Errors { get; set; }


        public StatusSimpleResponse() : this(true, "")
        {
            this.Id = Guid.NewGuid();
            this.Title = null;
            this.Detail = null;
            this.StatusCode = 200;

        }

        public StatusSimpleResponse(bool satisfactorio, string titulo)
        {
            this.Id = Guid.NewGuid();
            this.Success = satisfactorio;
            this.Title = titulo;
            this.StatusCode = 200;
        }

        public StatusSimpleResponse(bool satisfactorio, string titulo, string detalle)
        {
            this.Id = Guid.NewGuid();
            this.Success = satisfactorio;
            this.Title = titulo;
            this.Detail = detalle;
            this.StatusCode = 200;
        }
        public StatusSimpleResponse(bool satisfactorio, string titulo, string detalle,int status)
        {
            this.Id = Guid.NewGuid();
            this.Success = satisfactorio;
            this.Title = titulo;
            this.Detail = detalle;
            this.StatusCode = status;
        }
        public StatusSimpleResponse(bool satisfactorio, string titulo, string detalle, int status, Dictionary<string, List<string>> errors) 
        {
            this.Id = Guid.NewGuid();
            this.Success = satisfactorio;
            this.Title = titulo;
            this.Detail = detalle;
            this.StatusCode = status;
            this.Errors = errors;
        }

        public StatusSimpleResponse(string titulo, Dictionary<string, List<string>> errors)
        {
            this.Id = Guid.NewGuid();
            this.Success = false;
            this.Title = titulo;
            this.Errors = errors;
            this.StatusCode = 200;
        }

        public StatusSimpleResponse(string titulo, string detalle, Dictionary<string, List<string>> errors)
        {
            this.Id = Guid.NewGuid();
            this.Success = false;
            this.Title = titulo;
            this.Detail = detalle;
            this.Errors = errors;
            this.StatusCode = 200;
        }
    }
}
