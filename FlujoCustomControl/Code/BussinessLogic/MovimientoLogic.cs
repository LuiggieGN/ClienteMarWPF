using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using Flujo.Entities.WpfClient.POCO;
using Flujo.Entities.WpfClient.Enums;
using Flujo.Entities.WpfClient.RequestModel;
using Flujo.Entities.WpfClient.ResponseModels;
using Flujo.DataAccess.FlujoRepositories.WpfClient.DapperRepositories;
using FlujoCustomControl.Views;
using Newtonsoft.Json;


namespace FlujoCustomControl.Code.BussinessLogic
{
    public partial class MovimientoLogic
    {

        public static LocalBL lcvr = new LocalBL();
        private static FlujoServices.mar_flujoSoapClient flujoSvr = lcvr.GetFlujoServiceClient(false);


        public static MovimientoInsertEstado InsertarMovimientoSinUsuarioAutorizado(MovimientoBancaData pMovimiento, int pBancaUsuarioID)
        {
            try
            {

                //Dim pBancaID = Integer.Parse(pParams(0).ToString())
                //Dim pBancaUsuarioID = Integer.Parse(pParams(1).ToString())
                //Dim pMonto = Decimal.Parse(pParams(2).ToString())
                //Dim pDescripcion = AppLogic.MARHelpers.JSONHelper.CreateNewFromJSON(Of String)(pParams(3).ToString())
                //Dim pIngresoOEgreso = Integer.Parse(pParams(4).ToString())
                //Dim pTipoIngresoOTipoEgreso = Integer.Parse(pParams(5).ToString())
                //Dim pUsuarioExternoId = Integer.Parse(pParams(6).ToString())


                string BancaID = JsonConvert.SerializeObject(pMovimiento.BancaID);
                string BancaUsuarioID = JsonConvert.SerializeObject(pBancaUsuarioID);
                string Monto = JsonConvert.SerializeObject(pMovimiento.Monto);
                string Descripcion = JsonConvert.SerializeObject(pMovimiento.Descripcion);
                string IngresoOEgreso = JsonConvert.SerializeObject((int)pMovimiento.TipoFlujo);
                string TipoIngresoOTipoEgreso = JsonConvert.SerializeObject(pMovimiento.TipoFlujoTipoID);
           
                FlujoServices.ArrayOfAnyType parasMov = new FlujoServices.ArrayOfAnyType();
                parasMov.Add(BancaID);
                parasMov.Add(BancaUsuarioID);
                parasMov.Add(Monto);
                parasMov.Add(Descripcion);
                parasMov.Add(IngresoOEgreso);
                parasMov.Add(TipoIngresoOTipoEgreso);
                var pResponse = flujoSvr.CallFlujoIndexFunction(25, MainFlujoWindows.MarSession, parasMov);

                MovimientoInsertEstado insertestado = JsonConvert.DeserializeObject<MovimientoInsertEstado>(pResponse.Respuesta, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });

                return insertestado;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static MovimientoInsertEstado InsertaMovimientoConUsuarioAutorizado(MovimientoBancaData pMovimiento, int pBancaUsuarioID, UsuarioResponseModel pUsuarioExterno)
        {
            try
            {
                string BancaID = JsonConvert.SerializeObject(pMovimiento.BancaID);
                string BancaUsuarioID = JsonConvert.SerializeObject(pBancaUsuarioID);
                string Monto = JsonConvert.SerializeObject(pMovimiento.Monto);
                string Descripcion = JsonConvert.SerializeObject(pMovimiento.Descripcion);
                string IngresoOEgreso = JsonConvert.SerializeObject((int)pMovimiento.TipoFlujo);
                string TipoIngresoOTipoEgreso = JsonConvert.SerializeObject(pMovimiento.TipoFlujoTipoID);
                string UsuarioExternoId = JsonConvert.SerializeObject(pUsuarioExterno.UsuarioID);

                FlujoServices.ArrayOfAnyType parasMov = new FlujoServices.ArrayOfAnyType();
                parasMov.Add(BancaID);
                parasMov.Add(BancaUsuarioID);
                parasMov.Add(Monto);
                parasMov.Add(Descripcion);
                parasMov.Add(IngresoOEgreso);
                parasMov.Add(TipoIngresoOTipoEgreso);
                parasMov.Add(UsuarioExternoId);
                var pResponse = flujoSvr.CallFlujoIndexFunction(26, MainFlujoWindows.MarSession, parasMov);

                MovimientoInsertEstado insertestado = JsonConvert.DeserializeObject<MovimientoInsertEstado>(pResponse.Respuesta, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });

                return insertestado;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public static List<TipoIngresoResponseModel> GetTiposDeIngresos()
        {
            FlujoServices.ArrayOfAnyType parametros = new FlujoServices.ArrayOfAnyType();
            var pResponse = flujoSvr.CallFlujoIndexFunction(21, MainFlujoWindows.MarSession, parametros);
            List< TipoIngresoResponseModel > lista = JsonConvert.DeserializeObject<List<TipoIngresoResponseModel>>(pResponse.Respuesta, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });
            return lista;
        }

        public static List<TipoEgresoResponseModel> GetTiposDeEgresos()
        {
            FlujoServices.ArrayOfAnyType parametros = new FlujoServices.ArrayOfAnyType();
            var pResponse = flujoSvr.CallFlujoIndexFunction(22, MainFlujoWindows.MarSession, parametros);
            List<TipoEgresoResponseModel> lista = JsonConvert.DeserializeObject<List<TipoEgresoResponseModel>>(pResponse.Respuesta, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });
            return lista;
        }



    }// Clase Parcial MovimientoLogic Fin




}










































//public static ObservableCollection<MovimientoResponseModel> GetMovimientosBanca(int pBancaID, int pLastXRecords)
//{
//    List<MovimientoResponseModel> UltimosXMovimientos = new List<MovimientoResponseModel>();

//    ObservableCollection<MovimientoResponseModel> o = new ObservableCollection<MovimientoResponseModel>();

//    UltimosXMovimientos.AddRange(  MovimientoRepositorio.ConsultarLastXRecords( pBancaID, pLastXRecords)     );

//    foreach (var item in UltimosXMovimientos)
//    {
//        o.Add(item);
//    }

//    return o;
//}