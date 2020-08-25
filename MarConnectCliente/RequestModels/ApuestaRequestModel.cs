using MarConnectCliente.IndividualModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Threading.Tasks;

namespace MarConnectCliente.RequestModels
{
 

    public partial class ApuestaRequestModel : BaseRequestModel
    {
        public int LocalID { get; set; }
        public int TerminalID { get; set; }
        public Jugador Jugador { get; set; }
        public bool UsaCuenta { get; set; }
        public decimal MontoOperacion { get; set; }
        public Jugada DesgloseOperacion { get; set; }

        public string NumeroAutentificacionCalculado { get; set; }

        //public override string ToString()
        //{
        //    return this.ApuestaAnonimaWithNAUTC().ToString();
        //}

        public object ApuestaAnonimaWithNAUTC()
        {
            var anonModel = new
            {
                EstablecimientoID = base.EstablecimientoID ?? "",
                CodigoOperacion = base.CodigoOperacion ?? "",
                TipoOperacion = base.TipoOperacion,
                DiaOperacion = base.DiaOperacion ?? "",
                FechaHoraSolicitud = base.FechaHoraSolicitud ?? "",
                LocalID = this.LocalID,
                TerminalID = this.TerminalID,
                MontoOperacion = this.MontoOperacion,
                UsaCuenta = this.UsaCuenta,
                Jugador = (this.Jugador == null) ? "" : this.Jugador.ToString(),
                DesgloseOperacion = this.DesgloseOperacion.JugadaAnonima(),
                NumeroAutentificacionCalculado = this.NumeroAutentificacionCalculado ?? ""
            };
            return anonModel;
        }
        public object ApuestaAnonima()
        {
            var anonModel = new
            {
                EstablecimientoID = base.EstablecimientoID ?? "",
                CodigoOperacion = base.CodigoOperacion ?? "",
                TipoOperacion = base.TipoOperacion,
                DiaOperacion = base.DiaOperacion ?? "",
                FechaHoraSolicitud = base.FechaHoraSolicitud ?? "",
                LocalID = this.LocalID,
                TerminalID = this.TerminalID,
                MontoOperacion = this.MontoOperacion,
                UsaCuenta = this.UsaCuenta,
                Jugador = (this.Jugador == null) ? "" : this.Jugador.ToString(),
                DesgloseOperacion = this.DesgloseOperacion.JugadaAnonima(),
                //NumeroAutentificacionCalculado = base.NumeroAutentificacionCalculado ?? ""
            };
            return anonModel;
        }

    }

}

public class Jugada
{
    public string NoTicket { get; set; }

    public string Fecha { get; set; }
    //public decimal MontoJugada { get; set; }
    public DetalleJugada Detalle { get; set; }

    public object JugadaAnonima()
    {
        return new
        {
            NoTicket = this.NoTicket ?? "",
        
            Fecha = this.Fecha ?? "",
            //MontoJugada = this.MontoJugada,
            Detalle = (this.Detalle == null) ? "" : this.Detalle.DetalleJuegoSerializado()
        };
    }
}

public class DetalleJugada
{
    public List<Juego> Juego { get; set; }

    public object DetalleJuegoSerializado()
    {
        if (Juego == null) return "[ ]";

        return new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(Juego);
    }
}

public class Juego
{
    public int TipoJugadaID { get; set; }

    //public int SorteoReferencia { get; set; }
    public string Codigo { get; set; }
    //public int SorteoID { get; set; }
    public int Monto { get; set; }
    public string Jugada { get; set; }

    public object JuegoAnonimo()
    {
        return new
        {
            Jugada = this.Jugada ?? "",
            Codigo = this.Codigo ?? "",
            //EsquemaPagoID = this.EsquemaPagoID,
            Monto = this.Monto,
            //SorteoID = this.SorteoID,
            //SorteoReferencia = this.SorteoReferencia,
            TipoJugadaID = this.TipoJugadaID
        };
    }
}




