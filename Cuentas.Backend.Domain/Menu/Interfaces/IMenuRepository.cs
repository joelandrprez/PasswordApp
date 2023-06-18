using Cuentas.Backend.Domain.Menu.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cuentas.Backend.Domain.Menu.Interfaces
{
    public interface IMenuRepository
    {
        Task<List<OutMenu>> Listar(int rolId);
    }
}
