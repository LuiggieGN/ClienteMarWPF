using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAR.BusinessLogic.Admin.Code
{
    public class PreciosLogic
    {
        public static object AgregaEsquemaPrecio(Models.RequestModels.PreciosModels.CrearPreciosModel pPreciosModel)
        {
            try
            {
                if (SharedFuntions.Autorizaciones.AutorizaCorreccionPremios(pPreciosModel.Autorizacion))
                {
                    bool loteriaCreada = DataAccess.AdminRepositories.PreciosRepository.AgregaPrecios(pPreciosModel.Nombre);
                    if (loteriaCreada)
                    {
                        return new { OK = true, Mensaje = "Nueva Esquema de Precios: " + pPreciosModel.Nombre + " agregado." };
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
