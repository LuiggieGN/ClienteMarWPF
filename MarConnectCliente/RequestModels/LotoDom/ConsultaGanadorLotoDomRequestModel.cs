using MarConnectCliente.IndividualModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Threading.Tasks;

namespace MarConnectCliente.RequestModels.LotoDom
{
 

    public partial class ConsultaGanadorLotoDomRequestModel : BaseRequestModel
    {

        public string Sequence { get; set; }

        public object ConsultaGanadorAnonima()
        {
            var anonModel = new
            {
                EstablecimientoID = base.EstablecimientoID ?? "",
                CodigoOperacion = base.CodigoOperacion ?? "",
                Sequence = this.Sequence ?? "",
                TipoOperacion = base.TipoOperacion,
                DiaOperacion = base.DiaOperacion ?? "",
                FechaHoraSolicitud = base.FechaHoraSolicitud ?? "",
            };
            return anonModel;
        }
       

    }





}



