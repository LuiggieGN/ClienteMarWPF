using MarConnectCliente.IndividualModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace MarConnectCliente.RequestModels
{
    public partial class PagoGanadorRequestModel : BaseRequestModel
    {
        public PagoGanadorRequestModel()
        {
           // base.TipoOperacion = "PagoGanador";
        }
    }

    public partial class PagoGanadorRequestModel: BaseRequestModel
    {
        public int          LocalID                       { get; set; }
        public int          TerminalID                    { get; set; }
        public Jugador      Jugador                       { get; set; }                            
        public string       NumeroAutenticacionReferencia { get; set; }
        public string       CodigoOperacionReferencia     { get; set; }//Codigo De la operacion que origina la transaccion
        public decimal      MontoOperacion                { get; set; }
        public DesglosePago DesglosePago             { get; set; }

        public override string ToString()
        {
            return new 
            {
                TipoOperacion = TipoOperacion ?? "",
                EstablecimientoID = EstablecimientoID ?? "",
                DiaOperacion = base.DiaOperacion ?? "",
                FechaHoraSolicitud = FechaHoraSolicitud ?? "",
                Jugador = (this.Jugador == null) ? "{ }" : Jugador.ToString(),
                LocalID = this.LocalID,
                TerminalID = this.TerminalID,
                CodigoOperacion = base.CodigoOperacion ?? "",
                NumeroAutenticacionReferencia = this.NumeroAutenticacionReferencia?? "",
                CodigoOperacionReferencia = this.CodigoOperacionReferencia ?? "",
                MontoOperacion = MontoOperacion,
                DesglosePago = (this.DesglosePago == null)? "{ }" : DesglosePago.JugadaAnonima().ToString()
            }.ToString();
        }

    }


    public class DesglosePago
    {
        public string NoTicket            { get; set; }
        public string Fecha               { get; set; }
        public DetalleJugadasPago Detalle { get; set; }

        public object JugadaAnonima()
        {
            return new
            {
                NoTicket = this.NoTicket ?? "",
                Fecha = this.Fecha ?? "",
                Detalle = (this.Detalle == null) ? "" : this.Detalle.DetalleJuegoSerializado()
            };
        }
    }


    public class DetalleJugadasPago
    {
        public List<JuegoPago> Juego { get; set; }

        public object DetalleJuegoSerializado()
        {
            if (Juego == null) return "[ ]";

            return new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(Juego);
        }
    }

    public class JuegoPago :ICloneable
    {
        public int     TipoJugadaID     { get; set; }
        //public int     EsquemaPagoID    { get; set; }
        //public int     SorteoReferencia { get; set; }
        public string  Codigo           { get; set; }
        //public int     SorteoID         { get; set; }
        public decimal MontoApostado    { get; set; }
        public decimal MontoPagado      { get; set; }
        public string  Jugada           { get; set; }

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        public object JuegoAnonimo()
        {
            return new
            {
                TipoJugadaID = this.TipoJugadaID,
                Codigo = this.Codigo ?? "",
                //SorteoID = this.SorteoID,
                Jugada = this.Jugada ?? "",
                MontoApostado = this.MontoApostado,
                MontoPagado = this.MontoPagado,
                //EsquemaPagoID = this.EsquemaPagoID,
                //SorteoReferencia = this.SorteoReferencia
            };
        }
    }


}
