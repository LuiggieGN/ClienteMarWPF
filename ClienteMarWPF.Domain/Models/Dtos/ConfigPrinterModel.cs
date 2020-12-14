using System;
using System.Collections.Generic;
using System.Text;

namespace ClienteMarWPF.Domain.Models.Dtos
{

    public class ConfigPrinterModel
    {
        public string ConfigKey { get; set; }
        public string ConfigValue { get; set; }

    }

    public class ConfigPrinterValue
    {
        public string Key { get; set; }
        public List<ConfigPrinterValueCode> Value { get; set; }
        public string Content { get; set; }
        public int Size { get; set; }
        public string Aligment { get; set; }
        public string FontStyle { get; set; }
        public int FormatOneSorteo { get; set; }
        public int FormatAnySorteo { get; set; }

    }

    public class ConfigPrinterValueCode
    {
        public int Codigo { get; set; }
        public string Content { get; set; }

    }

    public class TicketValue
    {


        /// <summary>
        /// Crear la jugada y pasar a la lista 
        /// <seealso cref="TicketValue.ListJugadas"/>
        /// </summary>
        public static JugadaPrinter MapJugada(string Numeros, int Monto)
        {
            var jugada = new JugadaPrinter();
            jugada.Numeros = Numeros;
            jugada.Monto = Monto;
            return jugada;
        }


        /// <summary>
        /// Asignarle un tipo, loteria y pasar las la lista de jugadas.
        /// <seealso cref="TicketValue.ListJugadas"/>
        /// </summary>
        public static TicketJugadas MapTicketJugadas(string NombreResumidoLoteria, string TipoJudaga, JugadaPrinter Jugada)
        {
            var Jugadas = new TicketJugadas();
            //Jugadas.NombreResumidoLoteria = NombreResumidoLoteria;
            Jugadas.TipoJudaga = TipoJudaga;
            Jugadas.Jugada = Jugada;

            return Jugadas;
        }

        /// <summary>
        /// Asignarle una loteria, Ticket y Pin. pasar las la lista de jugadas.
        /// <seealso cref="TicketValue.ListJugadas"/>
        /// </summary>
        /// <param name="Loteria"></param>
        /// <param name="Ticket"></param>
        /// <param name="Pin"></param>
        /// <returns></returns>
        public static LoteriaTicketPin MapLoteriaTicketPin(string Loteria, string Ticket, string Pin)
        {
            var LoteriaTicketPin = new LoteriaTicketPin();
            LoteriaTicketPin.Loteria = Loteria;
            LoteriaTicketPin.Ticket = Ticket;
            LoteriaTicketPin.Pin = Pin;

            return LoteriaTicketPin;
        }

        /// <summary>
        /// Llenar esta lista con el metodo 
        /// <seealso cref="TicketValue.MapLoteriaTicketPin(string, string, string)"/>
        /// </summary>
        public static List<LoteriaTicketPin> ListLoteriaTicketPin = new List<LoteriaTicketPin>();

        /// <summary>
        /// Llenar esta lista con el metodo 
        /// <seealso cref="TicketValue.MapTicketJugadas(string, string, JugadaPrinter)"/>
        /// </summary>
        public static List<TicketJugadas> TicketJugadas = new List<TicketJugadas>();

        /// <summary>
        /// Llenar los parametros necesarios y pasar la lista de TicketJugadas.
        /// <seealso cref="TicketValue.TicketJugadas"/>
        /// </summary>
        public static TicketValue MapTicketValue(
             string BancaNombre, string FechaActual, string Telefono, string Firma, string Total,
             List<LoteriaTicketPin> LoteriaTicketPin, List<TicketJugadas> Jugadas,
             string Direccion = "", string Texto = "", string AutorizacionHacienda = "", string UrlLogo = ""
            )
        {
            var ticket = new TicketValue();

            ticket.BanNombre = BancaNombre;
            ticket.FechaActual = FechaActual;
            ticket.Telefono = Telefono;
            ticket.Firma = Firma;
            // ticket.Ticket = NumeroTicket;
            ticket.Total = Total;
            //ticket.Pin = Pin;
            //ticket.Loteria = Loteria;
            ticket.LoteriaTicketPin = LoteriaTicketPin;
            ticket.Direccion = Direccion;
            ticket.AutorizacionHacienda = AutorizacionHacienda;
            ticket.Logo = UrlLogo;
            ticket.Jugadas = Jugadas;
            ticket.Texto = Texto;


            return ticket;
        }
        public string Logo { get; set; }
        public string BanNombre { get; set; }
        public string Direccion { get; set; }
        public string FechaActual { get; set; }
        public string Telefono { get; set; }
        public string Firma { get; set; }
        public string AutorizacionHacienda { get; set; }
        public string Total { get; set; }
        public List<LoteriaTicketPin> LoteriaTicketPin { get; set; }
        public List<TicketJugadas> Jugadas { get; set; }
        public string Texto { get; set; }

    }

    public class LoteriaTicketPin
    {
        public string Loteria { get; set; }
        public string Ticket { get; set; }
        public string Pin { get; set; }
    }

    public class TicketJugadas
    {
        //public string NombreResumidoLoteria { get; set; }
        public string TipoJudaga { get; set; }
        public JugadaPrinter Jugada { get; set; }
    }

    public class JugadaPrinter
    {
        public string Numeros { get; set; }
        public int Monto { get; set; }
    }


}
