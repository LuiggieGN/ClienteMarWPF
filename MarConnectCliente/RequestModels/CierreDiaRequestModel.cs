using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace MarConnectCliente.RequestModels
{
    public partial class CierreDiaRequestModel : BaseRequestModel
    {
        //public CierreDiaRequestModel()
        //{
        //    base.TipoOperacion = "cierre de dia";
        //}

        public override string ToString()
        {
            var _r = new
            {
                EstablecimientoID = base.EstablecimientoID ?? "",
                TipoOperacion = base.TipoOperacion,
                CodigoOperacion = base.CodigoOperacion ?? "",
                DiaOperacion = base.DiaOperacion ?? "",
                FechaHoraSolicitud = base.FechaHoraSolicitud ?? "",
                CantidadTotalOperacionesExitosas = this.CantidadTotalOperacionesExitosas,
                MontoTotalOperaciones = this.MontoTotalOperaciones,
                DesgloseTotalesCierreDia = this.DesgloseTotalesCierreDia == null ? "" : new JavaScriptSerializer().Serialize(this.DesgloseTotalesCierreDia)
            };

            return _r.ToString();
        }
    }

    public partial class CierreDiaRequestModel
    {
        public string DiaDeCierre { get; set; }
        public int CantidadTotalOperacionesExitosas { get; set; }
        public decimal MontoTotalOperaciones { get; set; }

        public List<DegloseCierreDia> DesgloseTotalesCierreDia { get; set; }

        public string NumeroAutentificacionCalculado { get; set; }
    }

    public class DegloseCierreDia
    {
        public string Tipo { get; set; }
        public int CantidadOperaciones { get; set; }
        public decimal MontoOperaciones { get; set; }
    }
}
