using ClienteMarWPFWin7.Domain.Services.CincoMinutosService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ClienteMarWPFWin7.Domain.Models.Dtos.ProdutosDTO;
using MAR.AppLogic.MARHelpers;
using ClienteMarWPFWin7.Domain.Enums;
using ClienteMarWPFWin7.Domain.Models.Dtos;
using ClienteMarWPFWin7.Domain.Models.Dtos.EfectivoDtos;
using ClienteMarWPFWin7.Domain.Models.Entities;
using ClienteMarWPFWin7.Domain.Services.BancaService;
using ClienteMarWPFWin7.Domain.Exceptions;
using ClienteMarWPFWin7.Domain.Helpers;
using ClienteMarWPFWin7.Data.Services.Helpers;
using ClienteMarWPFWin7.Domain.FlujoService;
using System.Collections.ObjectModel;
using ClienteMarWPFWin7.Domain.MarPuntoVentaServiceReference;
using ClienteMarWPFWin7.Domain.BingoService;
using MAR_Session = ClienteMarWPFWin7.Domain.MarPuntoVentaServiceReference.MAR_Session;
using Newtonsoft.Json;

namespace ClienteMarWPFWin7.Data.Services
{
    public class CincoMinutosDataService : ICincoMinutosServices
    {
        public static SoapClientRepository soapClientesRepository;
        public static mar_bingoSoapClient bingoSrv;

        static CincoMinutosDataService()
        {
            soapClientesRepository = new SoapClientRepository();
            bingoSrv = soapClientesRepository.GetBingoServiceClient(false);
        }



        public ProductoViewModelResponse GetProductosDisponibles(CuentaDTO cuenta)
        {

            try
            {

                var sessionBingo = new ClienteMarWPFWin7.Domain.BingoService.MAR_Session();

                MAR_Session sessionPuntoVenta = cuenta.MAR_Setting2.Sesion;
                sessionBingo.Banca = sessionPuntoVenta.Banca;
                sessionBingo.Usuario = sessionPuntoVenta.Usuario;
                sessionBingo.Sesion = sessionPuntoVenta.Sesion;
                sessionBingo.Err = sessionPuntoVenta.Err;
                sessionBingo.LastTck = sessionPuntoVenta.LastTck;
                sessionBingo.LastPin = sessionPuntoVenta.LastPin;
                sessionBingo.PrinterSize = sessionPuntoVenta.PrinterSize;
                sessionBingo.PrinterHeader = sessionPuntoVenta.PrinterHeader;
                sessionBingo.PrinterFooter = sessionPuntoVenta.PrinterFooter;


                var pResponse = bingoSrv.CallJuegaMaxIndexFunction(17, sessionBingo, null, 0);
                var productosDisponibles = JsonConvert.DeserializeObject<ProductoViewModelResponse>(pResponse.Respuesta, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });
                return productosDisponibles;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public ProductoViewModel SetProducto(string producto, CuentaDTO cuenta)
        {
            try
            {
                var productosDisponiblesList = GetProductosDisponibles(cuenta).Productos ?? (List<ProductoViewModel>)null;

                if (productosDisponiblesList == null || productosDisponiblesList.Count == 0)
                {
                    return null;
                }

                return (from p in productosDisponiblesList
                        where p.Nombre.ToUpper().Contains(producto.ToUpper())
                        select new ProductoViewModel()
                        {
                            SuplidorID = p.SuplidorID,
                            Referencia = p.Referencia,
                            Nombre = p.Nombre,
                            Monto = double.Parse(p.ProductoConfig.Where(x => x.ConfigKey.ToUpper() == "PRECIO").FirstOrDefault().ConfigValue),
                            Cuenta = p.Cuenta,
                            ProductoID = p.ProductoID,
                            ProductoCampos = p.ProductoCampos
                        }).FirstOrDefault();
            }
            catch (Exception e)
            {
                return null;
            }
        }
    }
}
