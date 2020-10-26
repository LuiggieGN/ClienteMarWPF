using System;
using System.Linq;
using System.Collections.Generic;


using FlujoCustomControl.Views;
using Flujo.Entities.WpfClient.POCO;
using Flujo.Entities.WpfClient.Enums;
using Flujo.Entities.WpfClient.RequestModel;
using Flujo.Entities.WpfClient.ResponseModels;

using Flujo.DataAccess.FlujoRepositories.WpfClient.DapperRepositories;

using System.Globalization;
using Newtonsoft.Json;


namespace FlujoCustomControl.Code.BussinessLogic
{
    public static class BancaLogic
    {
        public static LocalBL lcvr = new LocalBL();
        private static FlujoServices.mar_flujoSoapClient flujoSvr = lcvr.GetFlujoServiceClient(false);
        public static Banca GetBanca(int pBancaID)
        {
            var jsonStr = JsonConvert.SerializeObject(pBancaID);
            FlujoServices.ArrayOfAnyType parametros = new FlujoServices.ArrayOfAnyType();
            parametros.Add(jsonStr);
            var pResponse = flujoSvr.CallFlujoIndexFunction(7, MainFlujoWindows.MarSession, parametros);
            Banca banca = JsonConvert.DeserializeObject<Banca>(pResponse.Respuesta, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });
            return banca;
        }
    }
}
