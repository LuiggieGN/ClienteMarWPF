using ClienteMarWPF.DataAccess.Services.Helpers;
using ClienteMarWPF.Domain.Services.SorteosService;
using HaciendaService;
using MarPuntoVentaServiceReference;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClienteMarWPF.DataAccess.Services
{
    public class SorteosDataService: ISorteosService
    {
        private static SoapClientRepository SoapClientesRepository;
        private static mar_haciendaSoapClient haciendacliente;
        private static PtoVtaSoapClient clientePuntoDeVenta;

        static SorteosDataService()
        {
            SoapClientesRepository = new SoapClientRepository();
            haciendacliente = SoapClientesRepository.GetHaciendaServiceClient(false);
            clientePuntoDeVenta = SoapClientesRepository.GetMarServiceClient(false);
        }

        public MAR_HaciendaResponse GetSorteosDisponibles(HaciendaService.MAR_Session session)
        {
            ArrayOfAnyType Params = new ArrayOfAnyType();
            return haciendacliente.CallHaciendaFuncion(21, session, Params);
        }

        public MAR_MultiBet RealizarApuesta(MarPuntoVentaServiceReference.MAR_Session session, MAR_MultiBet Apuestas)
        {
            return clientePuntoDeVenta.PlaceMultiBet(session, Apuestas);
        }


    }
}
