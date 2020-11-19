using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Flujo.Entities.WpfClient.ResponseModels.SorteosResponseModel;

namespace MAR.BusinessLogic.Code
{
    public class SorteosLogic
    {
        public static object  GetSorteosDisponibles()
        {
            try
            {
                var loteriasIdDisponibles = DataAccess.EFRepositories.LoteriaRepository.GetLoteriasVentasAbiertas();
                return new { OK = true, Respuesta = loteriasIdDisponibles };
            }
            catch(Exception e)
            {
                return new { OK = false, Respuesta = new { SorteosDisponibles = new SorteosDisponibles() } };
            }
        }
    }
}