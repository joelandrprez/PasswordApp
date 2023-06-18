using Cuentas.Backend.Domain.Tipocuentas.Domain;
using Cuentas.Backend.Domain.Tipocuentas.Interfaces;
using Cuentas.Backend.Shared;


namespace Cuentas.Backend.Infraestruture.TipoCuentas
{
    public class TipoCuentaRepository : ITipoCuentaRepository
    {
        public Task<Paginacion<EtipoCuenta>> Listar(int page, int size, string? search, string? orderBy, string? orderDir)
        {
            throw new NotImplementedException();
        }
    }
}
