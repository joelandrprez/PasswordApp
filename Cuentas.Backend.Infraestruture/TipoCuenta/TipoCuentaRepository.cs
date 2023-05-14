﻿using Cuentas.Backend.Domain.Tipocuentas.Domain;
using Cuentas.Backend.Domain.Tipocuentas.Interfaces;
using Cuentas.Backend.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cuentas.Backend.Infraestruture.TipoCuentas
{
    public class TipoCuentaRepository : ITipoCuentaRepository
    {
        public Task<Pagination<TipoCuenta>> Listar(int page, int size, string? search, string? orderBy, string? orderDir)
        {
            throw new NotImplementedException();
        }
    }
}