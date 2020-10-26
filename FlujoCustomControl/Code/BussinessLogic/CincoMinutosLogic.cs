using Flujo.Entities.WpfClient.PublicModels;
using Flujo.Entities.WpfClient.RequestModel;
using Flujo.Entities.WpfClient.ResponseModel;
using FlujoCustomControl.Views;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static Flujo.Entities.WpfClient.PublicModels.ProductoResponseModel;
using static Flujo.Entities.WpfClient.RequestModel.CincoMinutosRequestModel;



namespace FlujoCustomControl.Code.BussinessLogic
{
    public class CincoMinutosLogic
    {
        public static LocalBL lcvr = new LocalBL();
        private static  BingoServices.mar_bingoSoapClient bingoSrv = lcvr.GetBingoServiceClient(false);
      
        public static ApuestaCincoMinutosResponseModel Apuesta(CincoMinutosRequestModel.TicketModel pTicketModel)
        {
            try
            {

                //for (int i = 0; i < 99; i++)
                //{
                //    pTicketModel.TicketDetalles.Add(new TicketDetalle { TipoJugadaID = 1, Monto = 2, Jugada = i.ToString() });
                //}


                pTicketModel.MontoOperacion = pTicketModel.TicketDetalles.Sum(x => x.Monto);
                BingoServices.ArrayOfAnyType paramsApuesta = new BingoServices.ArrayOfAnyType();
                var ticket = JsonConvert.SerializeObject(pTicketModel);
                var productoSelected = JsonConvert.SerializeObject(MainFlujoWindows.ProductoSelected);
                paramsApuesta.Add(ticket);
                paramsApuesta.Add(productoSelected);
                var pResponse = bingoSrv.CallJuegaMaxIndexFunction(16, MainFlujoWindows.MarBingoSession, paramsApuesta, 0);
                var apuesta = JsonConvert.DeserializeObject<ApuestaCincoMinutosResponseModel>(pResponse.Respuesta, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });
                return apuesta;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static ProductoViewModelResponse GetProductosDisponibles()
        {
            try
            {
                BingoServices.ArrayOfAnyType parasMov = new BingoServices.ArrayOfAnyType();
                var pResponse = bingoSrv.CallJuegaMaxIndexFunction(17, MainFlujoWindows.MarBingoSession, null, 0);
                var productosDisponibles = JsonConvert.DeserializeObject<ProductoViewModelResponse>(pResponse.Respuesta, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });
                return productosDisponibles;
            }
            catch (Exception e)
            {
                return null;
            }
        }
        public static ConsultaPagoResponseModel ConsultaPagoGanador(string pTicket, string pPin)
        {
            try
            {
                BingoServices.ArrayOfAnyType paramsApuesta = new BingoServices.ArrayOfAnyType();
                paramsApuesta.Add(pTicket);
                paramsApuesta.Add(pPin);
                var pResponse = bingoSrv.CallJuegaMaxIndexFunction(19, MainFlujoWindows.MarBingoSession, paramsApuesta, 0);
                var consultaPago = JsonConvert.DeserializeObject<ConsultaPagoResponseModel>(pResponse.Respuesta, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });
                return consultaPago;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static AnulaApuestaResponseModel AnulaApuesta(string pTicket, string pPin)
        {
            try
            {
                BingoServices.ArrayOfAnyType paramsApuesta = new BingoServices.ArrayOfAnyType();
                paramsApuesta.Add(pTicket);
                paramsApuesta.Add(pPin);
                var pResponse = bingoSrv.CallJuegaMaxIndexFunction(21, MainFlujoWindows.MarBingoSession, paramsApuesta, 0);
                var anula = JsonConvert.DeserializeObject<AnulaApuestaResponseModel>(pResponse.Respuesta, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });
                return anula;
            }
            catch (Exception e)
            {
                return null;
            }
        }
        public static ConsultaPagoResponseModel RealizaPagoGanador(string pTicket, string pPin, decimal pSaco)
        {
            try
            {
                BingoServices.ArrayOfAnyType paramsApuesta = new BingoServices.ArrayOfAnyType();
                paramsApuesta.Add(pTicket);
                paramsApuesta.Add(pPin);
                paramsApuesta.Add(pSaco);
                var jugadasGanadoras = JsonConvert.SerializeObject(MainFlujoWindows.DetalleJugadas);
                paramsApuesta.Add(jugadasGanadoras);
                var pResponse = bingoSrv.CallJuegaMaxIndexFunction(20, MainFlujoWindows.MarBingoSession, paramsApuesta, 0);
                var pagoGanador = JsonConvert.DeserializeObject<ConsultaPagoResponseModel>(pResponse.Respuesta, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });

                return pagoGanador;
            }
            catch (Exception e)
            {
                return null;
            }
        }
        public static TicketResponseModel GetVendidosHoy()
        {
            try
            {
                BingoServices.ArrayOfAnyType parasMov = new BingoServices.ArrayOfAnyType();
                var pResponse = bingoSrv.CallJuegaMaxIndexFunction(18, MainFlujoWindows.MarBingoSession, null, 0);
                var vendidosHoy = JsonConvert.DeserializeObject<TicketResponseModel>(pResponse.Respuesta, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });
                return vendidosHoy;
            }
            catch (Exception e)
            {
                return null;
            }
        }
        public static ProductoViewModel SetProducto(string productoTipo)
        {
            try
            {
               return (from p in MainFlujoWindows.ProductosDisponiblesList
                                     where p.Nombre.ToUpper().Contains(productoTipo.ToUpper())
                                     select new ProductoViewModel() {
                                         SuplidorID = p.SuplidorID,
                                         Referencia = p.Referencia,
                                         Nombre = p.Nombre,
                                         Monto = double.Parse(p.ProductoConfig.Where(x => x.ConfigKey.ToUpper() == "PRECIO").FirstOrDefault().ConfigValue),
                                         Cuenta = p.Cuenta,
                                         ProductoID = p.ProductoID,
                                         ProductoCampos = p.ProductoCampos }).FirstOrDefault();
            }
            catch (Exception e)
            {
                return null;
            }
        }

    }
}
