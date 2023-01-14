using Cuentas.Backend.Domain.Cuentas.DTO;
using Cuentas.Backend.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cuentas.Backend.Domain.Cuentas.Interfaces
{
    public interface ICuentaRepository
    {
        Task<Pagination<OutCuenta>> Search(int page, int size, string? search, string? orderBy, string? orderDir);

    }
}
