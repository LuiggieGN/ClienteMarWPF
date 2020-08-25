using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace MarConnectCliente.RequestModels
{
        public partial class CierreSorteoRequestModel : BaseRequestModel
        {
            public override string ToString()
            {
                var _r = new
                {
                    EstablecimientoID = base.EstablecimientoID ?? "",
                    TipoOperacion = base.TipoOperacion,
                    CodigoOperacion = base.CodigoOperacion ?? "",
                    DiaOperacion = base.DiaOperacion ?? "",
                    FechaHoraSolicitud = base.FechaHoraSolicitud ?? "",
                    IdentificadorJuegoDeAzar = this.IdentificadorJuegoDeAzar ?? "",
                    FechaCierre = this.FechaCierre ?? "",
                    //CodigoSorteo = this.CodigoSorteo,
                    CantidadTotalOperacionesExitosas = this.CantidadTotalOperacionesExitosas,
                    MontoTotalOperaciones = this.MontoTotalApuestas,
                    DesgloseCierreSorteo = this.DesglosesTotales == null ? "" : new JavaScriptSerializer().Serialize(this.DesglosesTotales)
                };

                return _r.ToString();
            }

        }

        public partial class CierreSorteoRequestModel
        {
            public string FechaCierre { get; set; }
            public string IdentificadorJuegoDeAzar { get; set; }
            //public int    CodigoSorteo { get; set; }
            public int CantidadTotalOperacionesExitosas { get; set; }
            public decimal MontoTotalApuestas { get; set; }
            public string NumeroAutentificacionCalculado { get; set; }

            public List<DesgloseCierreSorteo> DesglosesTotales { get; set; }
        }

        public class DesgloseCierreSorteo
        {
            public string Jugada { get; set; }
            public int CantidadApuestas { get; set; }
            public decimal Monto { get; set; }
        }
    }
