using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAR.BusinessLogic.Admin.Code
{
    public class LoteriasLogic
    {
        public static object AgregaLoteria(Models.RequestModels.LoteriaModels.CrearLoteriaModel pLoteriaModel)
        {
            try
            {
                if (SharedFuntions.Autorizaciones.AutorizaCorreccionPremios(pLoteriaModel.Autorizacion))
                {
                    bool loteriaCreada = DataAccess.AdminRepositories.LoteriasRepository.AgregaLoteria(pLoteriaModel.Nombre, pLoteriaModel.NombreResumido);
                    if (loteriaCreada)
                    {
                        return new { OK = true, Mensaje = "Nueva Loteria " + pLoteriaModel.Nombre + " agregada." };
                    }
                    else
                    {
                        return new { OK = false, Mensaje = "Fallo la transaccion" };
                    }
                }
                else
                {
                    return new { OK = false, Mensaje = "El Codigo de autorizacion no es valido." };
                }
            }
            catch (Exception)
            {
                return new { OK = false, Mensaje = "Fallo la transaccion" };
            }
        
        }
    }
}
