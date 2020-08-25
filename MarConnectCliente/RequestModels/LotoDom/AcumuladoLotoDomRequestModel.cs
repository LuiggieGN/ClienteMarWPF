using MarConnectCliente.IndividualModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Threading.Tasks;

namespace MarConnectCliente.RequestModels.LotoDom
{
 

    public partial class AcumuladoLotoDomRequestModel : BaseRequestModel
    {
   

       
        public object AcumuladoAnonima()
        {
            var anonModel = new
            {
                EstablecimientoID = base.EstablecimientoID ?? "",
                CodigoOperacion = base.CodigoOperacion ?? "",
                TipoOperacion = base.TipoOperacion,
                DiaOperacion = base.DiaOperacion ?? "",
                FechaHoraSolicitud = base.FechaHoraSolicitud ?? "",
            };
            return anonModel;
        }
       

    }





}



