using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAR.BusinessLogic.Code
{
    public class CuentasLogic
    {
        public static string GetRiferosIdsDeTarjetasPHVentas(string pRecargaID)
        {
            var cueServidor = DataAccess.EFRepositories.PMCuentasRepository.GetPMCuentas(x => x.RecargaID == pRecargaID && x.CueActiva == 1).Select(x => x.CueServidor).FirstOrDefault();
            if (cueServidor.Any())
            {
                var riferosIDs = DataAccess.EFRepositories.PMCuentasRepository.GetPMCuentas(x => x.CueServidor == cueServidor && x.CueActiva == 1).Select(x => x.RiferoID);
                if (riferosIDs.FirstOrDefault() != null)
                {
                    string riferosString = string.Join(",", riferosIDs.ToArray());
                    return "IN (" + riferosString + ")";
                }
                else
                {
                    var riferosIds = DataAccess.EFRepositories.PMCuentasRepository.GetPMCuentas(x => x.RiferoID != null && x.CueActiva == 1).Select(x => x.RiferoID);
                    string riferosString = string.Join(",", riferosIds);
                    return "NOT IN (" + riferosString + ")";
                }
            }
            else
            {
                return "IN (" + "" + ")";
            }
          
        }


    }
}
