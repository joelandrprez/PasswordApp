using Cuentas.Backend.Domain.Menu.Domain;
using Cuentas.Backend.Domain.Menu.DTO;
using Cuentas.Backend.Domain.Menu.Interfaces;
using Cuentas.Backend.Domain.Proyectos.Interfaces;
using Cuentas.Backend.Shared;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cuentas.Backend.Infraestruture.Menu
{
    public class MenuRepository : IMenuRepository 
    {
        private readonly ICustomConnection _connection;

        public MenuRepository(ICustomConnection connection)
        {
            this._connection = connection;
        }
        public async Task<List<OutMenu>> Listar(int rolId)
        {
            DynamicParameters dinamycParams = new DynamicParameters();
            dinamycParams.Add("RolId", rolId);
            Dictionary<int, OutMenu> menuDic = new Dictionary<int, OutMenu>();
            IEnumerable<EMenu> listaNivel1 = null;
            IEnumerable<EMenu> listaNivel2 = null;
            try
            {
                using (var scope = await this._connection.BeginConnection())
                {
                    using (var gridReader = await scope.QueryMultipleAsync("USP_SEL_ListarMenu", dinamycParams, commandType: CommandType.StoredProcedure))
                    {
                        listaNivel1 = await gridReader.ReadAsync<EMenu>();
                        listaNivel2 = await gridReader.ReadAsync<EMenu>();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new CustomException("Sucedió un error al realizar la operación", ex);
            }

            foreach (EMenu item in listaNivel1)
            {
                OutMenu menuNivel1 = new OutMenu()
                {
                    Id = item.Id,
                    Titulo = item.Titulo,
                    Descripcion = item.Descripcion,
                    Url = item.Url,
                    Icono = item.Icono,
                    ColorFondo = item.ColorFondo,
                    ColorTexto = item.ColorTexto,
                    Orden = item.Orden,
                    SubMenu = new List<OutMenu>()

                };
                menuDic.Add(menuNivel1.Id, menuNivel1);
            }

            foreach (EMenu item in listaNivel2)
            {
                if (menuDic.ContainsKey(item.MenuPadre_Id))
                {
                    menuDic[item.MenuPadre_Id].SubMenu.Add(new OutMenu()
                    {
                        Id = item.Id,
                        Titulo = item.Titulo,
                        Descripcion = item.Descripcion,
                        Url = item.Url,
                        Icono = item.Icono,
                        ColorFondo = item.ColorFondo,
                        ColorTexto = item.ColorTexto,
                        Orden = item.Orden
                    });
                }
            }
            return menuDic.Values.ToList();

        }
    }
}
