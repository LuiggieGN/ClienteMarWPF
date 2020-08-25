using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarConnectCliente.RequestModels
{
    public partial  class AnulacionRequestModel : BaseRequestModel
    {
    }
    public partial class AnulacionRequestModel : BaseRequestModel
    {

        public int LocalID { get; set; }
        public int TerminalID { get; set; }
        public string CodigoRazonAnulacion { get; set; }
        public string CodigoOperacionReferencia { get; set; }          //|Codigo de Operacion| del Ticket Original 
        public string NumeroAutenticacionReferencia { get; set; }  //|Autorizacion                | del Ticket Original
        public decimal MontoOperacion { get; set; }                            //|Monto   Total            | del Ticket Original

        public string NumeroAutentificacionCalculado { get; set; }
        public override string ToString()
        {
            return new
            {
                EstablecimientoID = base.EstablecimientoID ?? "",
                CodigoOperacion = base.CodigoOperacion ?? "",
                NumeroAutenticacionReferencia = this.NumeroAutenticacionReferencia ?? "",
                CodigoOperacionReferencia = this.CodigoOperacionReferencia ?? "",
                CodigoRazonAnulacion = this.CodigoRazonAnulacion ?? "",
                MontoOperacion = this.MontoOperacion,
                FechaHoraSolicitud = base.FechaHoraSolicitud ?? "",
                DiaOperacion = base.DiaOperacion ?? "",
                LocalID = this.LocalID,
                TerminalID = this.TerminalID,
                NumeroAutentificacionCalculado = this.NumeroAutentificacionCalculado ?? "",
                TipoOperacion = base.TipoOperacion,

            }.ToString();
        }

        public object AnulacionWithNAUTC()
        {
            return new
            {
                EstablecimientoID = base.EstablecimientoID ?? "",
                CodigoOperacion = base.CodigoOperacion ?? "",
                NumeroAutenticacionReferencia = this.NumeroAutenticacionReferencia ?? "",
                CodigoOperacionReferencia = this.CodigoOperacionReferencia ?? "",
                CodigoRazonAnulacion = this.CodigoRazonAnulacion ?? "",
                MontoOperacion = this.MontoOperacion,
                FechaHoraSolicitud = base.FechaHoraSolicitud ?? "",
                DiaOperacion = base.DiaOperacion ?? "",
                LocalID = this.LocalID,
                TerminalID = this.TerminalID,
                NumeroAutentificacionCalculado = this.NumeroAutentificacionCalculado ?? "",
                TipoOperacion = base.TipoOperacion
            };
        }
        public object Anulacion()
        {
            return new
            {
                EstablecimientoID = base.EstablecimientoID ?? "",
                CodigoOperacion = base.CodigoOperacion ?? "",
                NumeroAutenticacionReferencia = this.NumeroAutenticacionReferencia ?? "",
                CodigoOperacionReferencia = this.CodigoOperacionReferencia ?? "",
                CodigoRazonAnulacion = this.CodigoRazonAnulacion ?? "",
                MontoOperacion = this.MontoOperacion,
                FechaHoraSolicitud = base.FechaHoraSolicitud ?? "",
                DiaOperacion = base.DiaOperacion ?? "",
                LocalID = this.LocalID,
                TerminalID = this.TerminalID,
                TipoOperacion = base.TipoOperacion
            };
        }

    }
}
