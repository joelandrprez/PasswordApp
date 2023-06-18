using Cuentas.Backend.Aplication.Comun;
using Cuentas.Backend.Domain.Menu.Domain;
using Cuentas.Backend.Domain.Menu.Interfaces;
using Cuentas.Backend.Shared;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cuentas.Backend.Aplication.Menu
{
    public class MenuApp : BaseApp<MenuApp>
    {
        private readonly IMenuRepository _menuRepository;
        public MenuApp(ILogger<BaseApp<MenuApp>> logger, IMenuRepository menuRepository) : base(logger)
        {
            this._menuRepository = menuRepository;
        }

        public async Task<StatusResponse<List<EMenu>>> List(int? page, int? size, string? search, string? orderBy, string? orderDir)
        {
            StatusResponse<List<EMenu>> respuesta = null;

            return respuesta;
        }
    }
}
