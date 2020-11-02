using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Flujo.Entities.WpfClient.Enums;
using Flujo.Entities.WpfClient.RequestModel;
using Flujo.Entities.WpfClient.ResponseModels;

using Flujo.DataAccess.FlujoRepositories.WpfClient.DapperRepositories;
using Flujo.Entities.WpfClient.POCO;
using FlujoCustomControl.Views;
using Newtonsoft.Json;

namespace FlujoCustomControl.Code.BussinessLogic
{
    public static class UsuarioTokenSeguridadLogic
    {
        public static LocalBL lcvr = new LocalBL();
        private static FlujoServices.mar_flujoSoapClient flujoSvr = lcvr.GetFlujoServiceClient(false);
        public static TokenDeSeguridadResponseModel ConsultaUnSoloTokenDeSeguridadAleatorio(int pUsuarioID)
        {
            var jsonStr = JsonConvert.SerializeObject(pUsuarioID);
            FlujoServices.ArrayOfAnyType parametros = new FlujoServices.ArrayOfAnyType();
            parametros.Add(jsonStr);
            var pResponse = flujoSvr.CallFlujoIndexFunction(14, MainFlujoWindows.MarSession, parametros);
            TokenDeSeguridadResponseModel token = JsonConvert.DeserializeObject<TokenDeSeguridadResponseModel>(pResponse.Respuesta, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });
            return token;
        }

              
        public static IEnumerable<TokenDeSeguridadResponseModel> ObtnerUsuarioCodigoSeguridad(int pUsuarioID)
        {
            var jsonStr = JsonConvert.SerializeObject(pUsuarioID);
            FlujoServices.ArrayOfAnyType parametros = new FlujoServices.ArrayOfAnyType();
            parametros.Add(jsonStr);
            var pResponse = flujoSvr.CallFlujoIndexFunction(15, MainFlujoWindows.MarSession, parametros);
            List<TokenDeSeguridadResponseModel> tokens = JsonConvert.DeserializeObject<List<TokenDeSeguridadResponseModel>>(pResponse.Respuesta, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });
            return tokens;
        }






    }
}
