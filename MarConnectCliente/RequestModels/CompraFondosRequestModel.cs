using MarConnectCliente.IndividualModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarConnectCliente.RequestModels
{
    public partial class CompraFondosRequestModel : BaseRequestModel
    {
        public override string ToString()
        {
            return new
            {

                CodigoOperacion = this.CodigoOperacion ?? "",
                DiaOperacion = this.DiaOperacion ?? "",
                EstablecimientoID = this.EstablecimientoID ?? "",
                FechaHoraSolicitud = this.FechaHoraSolicitud ?? "",
                LocalID = LocalID,
                TerminalID = TerminalID,
                Jugador = (Jugador == null) ? "" : Jugador.ToString(),
                MontoOperacion = MontoOperacion,
                TipoOperacion = TipoOperacion ?? ""

            }.ToString();
        }
    }

    public partial class CompraFondosRequestModel : BaseRequestModel
    {
        //public CompraFondosRequestModel()
        //{
        //    base.TipoOperacion = "CompraFondo";
        //}
    }
    public partial class CompraFondosRequestModel : BaseRequestModel
    {
        public int LocalID { get; set; }
        public int TerminalID { get; set; }
        public Jugador Jugador { get; set; }
        public decimal MontoOperacion { get; set; }

        public string NumeroAutentificacionCalculado { get; set; }


        public object CompraFondo()
        {
            return new
            {

                CodigoOperacion = this.CodigoOperacion ?? "",
                DiaOperacion = this.DiaOperacion ?? "",
                EstablecimientoID = this.EstablecimientoID ?? "",
                FechaHoraSolicitud = this.FechaHoraSolicitud ?? "",
                LocalID = this.LocalID,
                TerminalID = this.TerminalID,
                Jugador = (this.Jugador == null) ? "" : this.Jugador.ToString(),
                MontoOperacion = this.MontoOperacion,
                TipoOperacion = this.TipoOperacion ?? ""

            };


        }
        public object CompraFondoWithNAUTC()
        {
            return new
            {

                CodigoOperacion = this.CodigoOperacion ?? "",
                DiaOperacion = this.DiaOperacion ?? "",
                EstablecimientoID = this.EstablecimientoID ?? "",
                FechaHoraSolicitud = this.FechaHoraSolicitud ?? "",
                LocalID = this.LocalID,
                TerminalID = this.TerminalID,
                Jugador = (this.Jugador == null) ? "" : this.Jugador.ToString(),
                MontoOperacion = this.MontoOperacion,
                TipoOperacion = this.TipoOperacion ?? "",
                NumeroAutentificacionCalculado = this.NumeroAutentificacionCalculado ?? ""

            };


        }


    }
}
