using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ClienteMarWPFWin7.Domain.Models.Dtos.ConsultaPagoResponseModel._RespuestaApi;

namespace ClienteMarWPFWin7.Domain.Models.Dtos
{
    public class ApuestaCincoMinutosResponseModel
    {
        public _RespuestaApi RespuestaApi { get; set; }
        public class _RespuestaApi
        {

            public string FechaHoraRespuesta { get; set; }
            public string CodigoRespuesta { get; set; }
            public string MensajeRespuesta { get; set; }
            public object Peticion { get; set; }
            public string TipoAutentificacion { get; set; } //(Online - Offline)
            public string NumeroAutentificacion { get; set; } // naut
            public JugadorBalance JugadorBalance { get; set; }
            public List<int> SorteosNoDisponible { get; set; }
            public List<_TicketDetalle> TicketDetalle { get; set; }
            public override string ToString()
            {
                return this.ApuestaAnon().ToString();
            }

            public object ApuestaAnon()
            {

                var ModelAnnon = new
                {
                    NumeroAutentificacion = this.NumeroAutentificacion ?? "",
                    TipoAutentificacion = this.TipoAutentificacion ?? "",
                    CodigoRespuesta = this.CodigoRespuesta ?? "",
                    MensajeRespuesta = this.MensajeRespuesta ?? "",
                    JugadorBalance = this.JugadorBalance == null ? "" : this.JugadorBalance.ToString(),
                    SorteosNoDisponible = this.SorteosNoDisponible == null ? "[ ]" : $"[{  string.Join(",", SorteosNoDisponible) }]",
                    Peticion = this.Peticion
                };

                return ModelAnnon;
            }
        }

        public class JugadorBalance
        {
            public decimal? BalanceInical { get; set; }
            public decimal? BalanceActual { get; set; }
            public decimal MontoDebitado { get; set; }

            public override string ToString()
            {
                return new
                {
                    BalanceInical = BalanceInical.HasValue ? BalanceInical.Value : 0,
                    BalanceActual = BalanceActual.HasValue ? BalanceActual.Value : 0,
                    MontoDebitado = MontoDebitado

                }.ToString();
            }
        }
    }

}
