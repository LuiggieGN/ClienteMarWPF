using MAR.DataAccess.EFRepositories.StoreProcedures;
using MAR.DataAccess.Tables.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAR.DataAccess.AdminRepositories
{
    public class PreciosRepository
    {
        public static bool AgregaPrecios(string pNombre)
        {
            try
            {
                var sp = new SPContext();
                var agrega = sp.CrearLoteriaPrecio(2, pNombre);
                if (agrega > 0)
                {
                    return true;
                }
                return false;
            }
            catch (Exception e)
            {
                string t = e.Message;
                return false;
            }
        }
    }
}
