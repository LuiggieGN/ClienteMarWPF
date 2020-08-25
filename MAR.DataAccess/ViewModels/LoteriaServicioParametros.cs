using System;
using System.Collections.Generic;
using System.Xml;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


namespace JuegosTemplate.DataAccess.Servicios.LoteriaServicio.Utilidades

{
    public class LoteriaServicioParametros
    {

        //..........................................Estos parametros son siempre requeridos para utlizar cualquier metodo del servicio de la loteria execepto para el metodo |ResultadoSorteos()|
        private string        _fConsorcio;
        private string        _fUsuario;
        private string        _fPassword;     
        
        //..........................................Este parametro solo es requerido para |anular jugadas|
        private string        _fAutorizacion;

        //..........................................Este parametro solo es requerido para |consultar jugada|
        private string        _fFechaDeJugadas;
        private int           _fPPagina;

        //..........................................Este paramtero  solo es requerido para |consultar resultado de soteos|
        private string       _fFechaDeResultadoDeSorteos;
        private string _fEndPointAddress;
      
                                           //..........................................Este parametro solo es requerido para |autorizar jugada|

        //%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%(<TICKET>)

        private LoteriaTicket _ticket;    

        public class  LoteriaTicket {

            public class DetalleDeJuego
            {
                public string Codigo { get; set; }
                public string Monto  { get; set; }
                public string Jugada { get; set; }
            }

            public string               CodigoDeConsorcio   { get; set; }
            public string               CodigoDeAgencia     { get; set; }
            public string               NombreDeAgencia     { get; set; }
            public string               CodigoDeCaja        { get; set; }
            public string               NombreDelSupervisor { get; set; }
            public string               CedulaDelSupervisor { get; set; }
            public string               NombreDelCajero     { get; set; }
            public string               CedulaDelCajero     { get; set; }
            public string               NumeroDeTicket      { get; set; }
            public DateTime             Fecha               { get; set; }
            public string               MontoJugada         { get; set; }
            public List<DetalleDeJuego> Juegos              { get; set; }

        }//End Inner Class /LoteriaTicket/~

//%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%(</TICKET>)


        public string        Consorcio {

            get {

                return _fConsorcio;
            }

            set {

                _fConsorcio = value;
            }
        }
                             
        public string        Usuario {
            get
            {
                return _fUsuario;
            }

            set {
                _fUsuario = value;
            }
        }
                             
        public string        Password {

            get {
                return _fPassword;
            }

            set {
                _fPassword = value;
            }

        }        

        public LoteriaTicket Ticket {

            get {
                return _ticket;
            }

            set {
                _ticket = value;
            }

        }
        public string EndPointAddress
        {
            get
            {
                return _fEndPointAddress;
            }

            set
            {
                _fEndPointAddress = value;
            }
        }

        public XmlDocument   Ticket_En_XML() {

            XmlDocument result = new XmlDocument();

            if (_ticket == null)
            {
                return null;
            }

            string strXML = "<?xml version=\"1.0\"?>" +
                            "<Ticket>" +
                              "<CodigoConsorcio>"  + _ticket.CodigoDeConsorcio   +"</CodigoConsorcio>"  +
                              "<CodigoAgencia>"    + _ticket.CodigoDeAgencia     +"</CodigoAgencia>"    +
                              "<NombreAgencia>"    + _ticket.NombreDeAgencia     +"</NombreAgencia>"    +
                              "<CodigoCaja>"       + _ticket.CodigoDeCaja        +"</CodigoCaja>"       +
                              "<NombreSupervisor>" + _ticket.NombreDelSupervisor +"</NombreSupervisor>" +
                              "<CedulaSupervisor>" + _ticket.CedulaDelSupervisor +"</CedulaSupervisor>" +
                              "<NombreCajero>"     + _ticket.NombreDelCajero     +"</NombreCajero>"     +
                              "<CedulaCajero>"     + _ticket.CedulaDelCajero     +"</CedulaCajero>"     +
                              "<NoTicket>"         + _ticket.NumeroDeTicket      +"</NoTicket>" +
                              "<Fecha>"            + _ticket.Fecha.ToString(format: "yyyy-MM-dd HH':'mm':'ss")+"</Fecha>" +
                              "<MontoJugada>"      + _ticket.MontoJugada         +"</MontoJugada>";

            if (_ticket.Juegos != null && _ticket.Juegos.Count > 0) {

                strXML += "<Detalle>";

                foreach (LoteriaTicket.DetalleDeJuego juegoDettalle in _ticket.Juegos)
                {
                    strXML += "<Juego Codigo=\""+juegoDettalle.Codigo+"\" Monto=\""+juegoDettalle.Monto+"\" Jugada=\""+juegoDettalle.Jugada+"\" />";  //Jugada
                }

                strXML += "</Detalle>";
            }

            strXML += "</Ticket>";

            result.LoadXml(strXML);

            return result;

        }//End TicketXML()~

        public JObject       Ticket_En_JSON() {

            if (_ticket == null)
            {
                return null;
            }

            JArray arr = new JArray();

            if (_ticket.Juegos != null && _ticket.Juegos.Count > 0) {

                foreach (var juegos in _ticket.Juegos)
                {
                    JObject jproperty = new JObject(
                      
                        new JProperty("Codigo", juegos.Codigo),
                        new JProperty("Monto" , juegos.Monto),
                        new JProperty("Jugada", juegos.Jugada)                    
                        
                    );

                    arr.Add(jproperty);
                }
            }
            
            JObject rss = new JObject(              
                new JProperty("Ticket", 
                    new JObject(                         
                        new JProperty("CodigoConsorcio",  _ticket.CodigoDeConsorcio  ),
                        new JProperty("CodigoAgencia",    _ticket.CodigoDeAgencia    ),
                        new JProperty("NombreAgencia",    _ticket.NombreDeAgencia    ),
                        new JProperty("CodigoCaja",       _ticket.CodigoDeCaja       ),
                        new JProperty("NombreSupervisor", _ticket.NombreDelSupervisor),
                        new JProperty("CedulaSupervisor", _ticket.CedulaDelSupervisor),
                        new JProperty("NombreCajero",     _ticket.NombreDelCajero    ),
                        new JProperty("CedulaCajero",     _ticket.CedulaDelCajero    ),
                        new JProperty("NoTicket",         _ticket.NumeroDeTicket     ),
                        new JProperty("Fecha",            _ticket.Fecha.ToString(format: "yyyy-MM-dd HH':'mm':'ss")),
                        new JProperty("MontoJugada",      _ticket.MontoJugada),
                        new JProperty("Detalle",          
                            new JObject(
                                new JProperty(
                                    "Juego", arr
                                )                              
                            )
                        )                        
                    )
                )    
            ); return rss;

        }//End CrearTicketFormatoJson()~

        public string        Autorizacion{

            get {

                return _fAutorizacion;
            }

            set {

                _fAutorizacion = value;
            }

        }

        public string        FechaDeJugadas {
            get {

                return _fFechaDeJugadas;
            }
            set {

                _fFechaDeJugadas = value;
            }
        }

        public int           PPagina {
            get {
                return _fPPagina;
            }
            set {
                _fPPagina = value;
            }
        }

        public string        FechaDeResultadosDeSorteos {
            get {
                return _fFechaDeResultadoDeSorteos;
            }
            set {
                _fFechaDeResultadoDeSorteos = value;
            }
        }

    }//End Class /LotteryServiceParameter/ ~

}//End Namespace JuegosTemplate.ServicesClient.WCFServices.ProxyServiceUtilities ~