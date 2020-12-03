using System;
using System.Linq;
using System.Globalization;
using System.Collections.Generic;
using MAR.AppLogic.MARHelpers;

using ClienteMarWPF.Domain.Helpers;
using ClienteMarWPF.Domain.Enums;
using ClienteMarWPF.Domain.Models.Dtos;
using ClienteMarWPF.Domain.Models.Dtos.EfectivoDtos;
using ClienteMarWPF.Domain.Models.Entities;
using ClienteMarWPF.Domain.Services.TieService;
using ClienteMarWPF.Domain.Exceptions;

using ClienteMarWPF.DataAccess.Services.Helpers;

using FlujoService;

namespace ClienteMarWPF.DataAccess.Services
{
    public class TieDataService : ITieService
    {
        public static SoapClientRepository soapClientesRepository;
        private static mar_flujoSoapClient efectivoSoapCliente;

        static TieDataService()
        {
            soapClientesRepository = new SoapClientRepository();
            efectivoSoapCliente = soapClientesRepository.GetCashFlowServiceClient(false);
        }


        public TieDTO LeerTiposAnonimos()
        {
            try
            {
                var llamada = efectivoSoapCliente.CallControlEfectivoFunciones((int)EfectivoFunciones.Tie_LeerTiposAnonimos, new ArrayOfAnyType());

                if (llamada == null || llamada.OK == false)
                {
                    throw new Exception("Ha ocurrido un error al leer los tipos ingresos o tipo egresos anonimos");
                }

                var anonimos = JSONHelper.CreateNewFromJSONNullValueIgnore<TieDTO>(llamada.Respuesta);

                return anonimos;
            }
            catch
            {
                var onError = new TieDTO();

                #region TIPOS INGRESOS ANONIMOS
                onError.TiposIngresosQueSonAnonimo = new List<TipoAnonimoDTO>();
                onError.TiposIngresosQueSonAnonimo.Add(new TipoAnonimoDTO
                {
                    Clave = 1,
                    Id = 1,
                    Tipo = "Depósito de cuadre",
                    TipoNombre = "Recibo Efectivo",
                    Descripcion = "Cuando un usuario deposita un dinero en una caja",
                    LogicaKey = 100001,
                    EsTipoSistema = false,
                    EsTipoAnonimo = true,
                    Activo = true
                });
                onError.TiposIngresosQueSonAnonimo.Add(new TipoAnonimoDTO
                {
                    Clave = 1,
                    Id = 2,
                    Tipo = "Otros abonos",
                    TipoNombre = "Otros abonos",
                    Descripcion = "Otro tipo de ingreso",
                    LogicaKey = 100002,
                    EsTipoSistema = false,
                    EsTipoAnonimo = true,
                    Activo = true
                });
                #endregion

                #region TIPOS EGRESOS ANONIMOS
                onError.TiposEgresosQueSonAnonimo = new List<TipoAnonimoDTO>();
                onError.TiposEgresosQueSonAnonimo.Add(new TipoAnonimoDTO
                {
                    Clave = 0,
                    Id = 1,
                    Tipo = "Salida de cuadre",
                    TipoNombre = "Entrego Efectivo",
                    Descripcion = "Cuando un usuario retira un dinero de una caja",
                    LogicaKey = 200001,
                    EsTipoSistema = false,
                    EsTipoAnonimo = true,
                    Activo = true
                });
                onError.TiposEgresosQueSonAnonimo.Add(new TipoAnonimoDTO
                {
                    Clave = 0,
                    Id = 2,
                    Tipo = "Compra",
                    TipoNombre = "Compra",
                    Descripcion = "Gasto General",
                    LogicaKey = 200002,
                    EsTipoSistema = false,
                    EsTipoAnonimo = true,
                    Activo = true
                });
                onError.TiposEgresosQueSonAnonimo.Add(new TipoAnonimoDTO
                {
                    Clave = 0,
                    Id = 3,
                    Tipo = "Otros gastos, especificar en comentario",
                    TipoNombre = "Gasto general",
                    Descripcion = "Gasto General",
                    LogicaKey = 200003,
                    EsTipoSistema = false,
                    EsTipoAnonimo = true,
                    Activo = true
                });
                #endregion

                return onError;

            }// fin de catch

        }// fin de metodo LeerTiposAnonimos()



    }//fin de clase
}
































