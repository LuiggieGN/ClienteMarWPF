using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAR.DataAccess.Tables.ControlEfectivoDTOs
{
    public class ControlEfectivoDTO
    {
        public bool PuedeUsarControlEfectivo { get; set; } /// = False => No puede |Control Efectivo|    
                                                           ///   True  => Si puede |Control Efectivo|
        public bool BancaYaInicioControlEfectivo { get; set; } /// = False => No ha iniciado flujo efectivo  
                                                               ///   True  =>  Ha iniciado flujo efectivo
    }
}
