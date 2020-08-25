using MAR.DataAccess.Tables.DTOs;
using MAR.DataAccess.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAR.BusinessLogic.Models.Mappers
{
    public class JuegosNuevosMapper
    {
        public static IEnumerable<DataAccess.Tables.DTOs.RF_Transaccion> MapRfTransaccion(List<RequestModel.JuegosNuevosRequestModel.RFTransaccionRequestModel.RFTransacciones> pRFTransRequestModel)
        {
            DateTime fechaIngreso = DateTime.Now;
            List<RF_TransaccionJugada> jugadas = new List<RF_TransaccionJugada>();
           
            IEnumerable<RF_Transaccion> trans = (from p in pRFTransRequestModel
                                                 select new RF_Transaccion
                                                 {
                                                     FechaIngreso = fechaIngreso,
                                                     SorteoDiaID = p.SorteoDiaID,
                                                     EsquemaPagoID = p.EsquemaPagoID,
                                                     Activo = true,
                                                     Estado = p.Estado,
                                                     Ingresos = p.Ingresos,
                                                     Referencia = p.Referencia,
                                                     ReciboID = p.ReciboID,
                                                     Serie = " ",
                                                     RFTransaccionJugadas = (from x in p.TransaccionJugadas select new RF_TransaccionJugada()
                                                     {Aposto = x.Aposto,
                                                     Numeros = x.Numeros, SorteoTipoJugadaID = x.SorteoTipoJugadaID, Opciones = x.Opciones
                                                     }).ToArray()
                                                 });

            return trans;
        }


        public static IEnumerable<ResponseModel.JuegosNuevosResponseModel.TicketViewModel> MapRecibos(IEnumerable<CL_Recibo> pRecibos, bool pWithPin)
        {

            IEnumerable<ResponseModel.JuegosNuevosResponseModel.TicketViewModel> recibos = from p in pRecibos
                                                                                  select new ResponseModel.JuegosNuevosResponseModel.TicketViewModel
                                                                                  {
                                                                                      Activo = p.Activo,
                                                                                      TicNumero = p.Referencia.ToString(),
                                                                                      TicketID = p.ReciboID,
                                                                                      TicFecha = p.Fecha.ToString("t"),
                                                                                      Hora = p.Fecha.ToString("t"),
                                                                                      TicCosto = p.Ingresos,
                                                                                      TicNulo = !p.RF_Transacciones.FirstOrDefault().Activo,
                                                                                      Pin = (pWithPin && p.ReciboID > 0 ? BaseViewModel.GeneraPinGanador(Convert.ToInt32(p.ReciboID)) : string.Empty),
                                                                                      Jugadas = (from x in p.RF_Transacciones select new ResponseModel.JuegosNuevosResponseModel.TicketViewModel.JugadaJuegosNuevos
                                                                                      {
                                                                                          Cantidad = 1, Jugada = x.RFTransaccionJugadas.FirstOrDefault().Numeros,  Monto = x.RFTransaccionJugadas.FirstOrDefault().Aposto, Sorteo = x.RFTransaccionJugadas.FirstOrDefault().Opciones,
                                                                                          TipoJugada = x.RFTransaccionJugadas.FirstOrDefault().Opciones, SorteoTipoJugadaID = x.RFTransaccionJugadas.FirstOrDefault().SorteoTipoJugadaID,
                                                                                          Referencia = x.Referencia
                                                                                      }).ToArray()
                                                                                    
                                                                                  };

            return recibos;
        }


    }
}
