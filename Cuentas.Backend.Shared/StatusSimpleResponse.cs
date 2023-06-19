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
        public bool Satisfactorio { get; set; }
        public string Titulo { get; set; }
        public string Detalle { get; set; }
        public int Codigo { get ; set; }
        public Dictionary<string, List<string>> Errors { get; set; }


        public StatusSimpleResponse() : this(true, "")
        {
            this.Id = Guid.NewGuid();
            this.Titulo = null;
            this.Detalle = null;
            this.Codigo = 200;

        }

        public StatusSimpleResponse(bool satisfactorio, string titulo)
        {
            this.Id = Guid.NewGuid();
            this.Satisfactorio = satisfactorio;
            this.Titulo = titulo;
            this.Codigo = 200;
        }

        public StatusSimpleResponse(bool satisfactorio, string titulo, string detalle)
        {
            this.Id = Guid.NewGuid();
            this.Satisfactorio = satisfactorio;
            this.Titulo = titulo;
            this.Detalle = detalle;
            this.Codigo = 200;
        }
        public StatusSimpleResponse(bool satisfactorio, string titulo, string detalle,int status)
        {
            this.Id = Guid.NewGuid();
            this.Satisfactorio = satisfactorio;
            this.Titulo = titulo;
            this.Detalle = detalle;
            this.Codigo = status;
        }
        public StatusSimpleResponse(bool satisfactorio, string titulo, string detalle, int status, Dictionary<string, List<string>> errors) 
        {
            this.Id = Guid.NewGuid();
            this.Satisfactorio = satisfactorio;
            this.Titulo = titulo;
            this.Detalle = detalle;
            this.Codigo = status;
            this.Errors = errors;
        }

        public StatusSimpleResponse(string titulo, Dictionary<string, List<string>> errors)
        {
            this.Id = Guid.NewGuid();
            this.Satisfactorio = false;
            this.Titulo = titulo;
            this.Errors = errors;
            this.Codigo = 200;
        }

        public StatusSimpleResponse(string titulo, string detalle, Dictionary<string, List<string>> errors)
        {
            this.Id = Guid.NewGuid();
            this.Satisfactorio = false;
            this.Titulo = titulo;
            this.Detalle = detalle;
            this.Errors = errors;
            this.Codigo = 200;
        }
    }
}
