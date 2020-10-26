using System; 
using System.Globalization;
using Flujo.Entities.WpfClient.POCO;
using FlujoCustomControl.Code.AppEnums;
using Newtonsoft.Json;
using FlujoCustomControl.Views;

namespace FlujoCustomControl.Code.BussinessLogic
{
    public static class CajaLogic
    {

        public static LocalBL lcvr = new LocalBL();
        private static FlujoServices.mar_flujoSoapClient flujoSvr = lcvr.GetFlujoServiceClient(false);


        public static void ConfigurarCajaDisponibilidad(int pBancaID, bool pDisponible)
        {
            try
            {
                string jsonStr = JsonConvert.SerializeObject(pBancaID);
                string disponible = JsonConvert.SerializeObject(pDisponible);
                FlujoServices.ArrayOfAnyType parametros1 = new FlujoServices.ArrayOfAnyType();
                parametros1.Add(jsonStr);
                parametros1.Add(disponible);
                var pResponse1 = flujoSvr.CallFlujoIndexFunction((int)RoutingFunctions.SetCajaDisponibilidad, MainFlujoWindows.MarSession, parametros1); //CajaRepositorio.CajaFueAsignadaABanca()
                bool fueConfigurada = JsonConvert.DeserializeObject<bool>(pResponse1.Respuesta, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });

             
                if (!fueConfigurada)    
                {
                    throw new Exception("Error. Configuración de disponibilidad de caja no pudo ser completada");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //: True si asigno una caja a una Banca
        public static bool Asigna__CajaABanca(int pBancaID)
        {

            string jsonStr = JsonConvert.SerializeObject(pBancaID);
            FlujoServices.ArrayOfAnyType parametros1 = new FlujoServices.ArrayOfAnyType();
            parametros1.Add(jsonStr);
            var pResponse1 = flujoSvr.CallFlujoIndexFunction( (int)RoutingFunctions.BancaTieneCajaAsignada , MainFlujoWindows.MarSession, parametros1); //CajaRepositorio.CajaFueAsignadaABanca()
            
            bool BancaPoseeCaja = JsonConvert.DeserializeObject<bool>(pResponse1.Respuesta, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });
            
            if (BancaPoseeCaja)
            {
                return true;
            }
            
            
            var pResponse2 = flujoSvr.CallFlujoIndexFunction((int)RoutingFunctions.CrearCajaABanca, MainFlujoWindows.MarSession, parametros1); //CajaRepositorio.CreaCajaABanca()
            bool CajaFueAsignada = JsonConvert.DeserializeObject<bool>(pResponse2.Respuesta, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });
            return CajaFueAsignada;

 
        }

        public static int GetBancaCajaID(int pBancaID)
        {

             string jsonStr = JsonConvert.SerializeObject(pBancaID);
             FlujoServices.ArrayOfAnyType parametros = new FlujoServices.ArrayOfAnyType();
             parametros.Add(jsonStr);
             var pResponse = flujoSvr.CallFlujoIndexFunction((int) RoutingFunctions.GetBancaCajaId, MainFlujoWindows.MarSession, parametros);  
             
             int cajaID = JsonConvert.DeserializeObject<int>(pResponse.Respuesta, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });
             return cajaID;
        }

        public static decimal GetBancaBalance(int pBancaID)
        {

            int CurrentBancaCajaID = GetBancaCajaID(pBancaID);
            
            if (CurrentBancaCajaID == -1) // Si la BANCA no tiene caja creada en flujo
            {
                return 0;
            }
            
            string jsonStr1 = JsonConvert.SerializeObject(CurrentBancaCajaID);
            FlujoServices.ArrayOfAnyType parametros1 = new FlujoServices.ArrayOfAnyType();
            parametros1.Add(jsonStr1);
            var pResponse1 = flujoSvr.CallFlujoIndexFunction((int)RoutingFunctions.GetCajaBalanceActual, MainFlujoWindows.MarSession, parametros1); //CajaRepositorio.GetCajaBalanceActual()
            
            decimal CajaBalanceActual = JsonConvert.DeserializeObject<decimal>(pResponse1.Respuesta, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });
            return CajaBalanceActual;

        }

        public static string GetBancaBalanceActual(int pBancaID)
        {
            try
            {
                int CurrentBancaCajaID = GetBancaCajaID(pBancaID);

                if (CurrentBancaCajaID == -1) // Si la BANCA no tiene caja creada en flujo
                {
                    return "$ 0.00";
                }

                string jsonStr1 = JsonConvert.SerializeObject(CurrentBancaCajaID);
                FlujoServices.ArrayOfAnyType parametros1 = new FlujoServices.ArrayOfAnyType();
                parametros1.Add(jsonStr1);
                var pResponse1 = flujoSvr.CallFlujoIndexFunction((int)RoutingFunctions.GetCajaBalanceActual, MainFlujoWindows.MarSession, parametros1); //CajaRepositorio.GetCajaBalanceActual()

                decimal CajaBalanceActual = JsonConvert.DeserializeObject<decimal>(pResponse1.Respuesta, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });
                return "$ " + ((CajaBalanceActual == 0) ? "0.00" : CajaBalanceActual.ToString("#,##0.00", CultureInfo.CreateSpecificCulture("en-US")));
            }
            catch (Exception e)
            {
                throw e;
            }
           
        }

        public static Caja GetUsuarioCaja(int usuarioId)
        {

            try
            {
                string paramUsuarioId = JsonConvert.SerializeObject(usuarioId);
                FlujoServices.ArrayOfAnyType coleccionParametros = new FlujoServices.ArrayOfAnyType();
                coleccionParametros.Add(paramUsuarioId);
                
                var consulta = flujoSvr.CallFlujoIndexFunction((int)RoutingFunctions.GetCajaVirtualByUsuarioId, MainFlujoWindows.MarSession, coleccionParametros); //CajaRepositorio.GetCajaVirtual

                if (consulta.OK == false )
                {
                    throw new Exception("Ha ocurrido un error en procesar la operacion.");
                }

                if (consulta.Respuesta == null || consulta.Respuesta.Equals("null"))
                {
                    return null;
                }

                Caja c = JsonConvert.DeserializeObject<Caja>(consulta.Respuesta, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });

                return c;
            }
            catch (Exception ex)
            {
                throw ex;
            }


        }

        public static decimal GetBancaBalanceMinimo(int pBancaID)
        {
            int cajaid = GetBancaCajaID(pBancaID);
            
            string jsonStr1 = JsonConvert.SerializeObject(cajaid);
            FlujoServices.ArrayOfAnyType parametros1 = new FlujoServices.ArrayOfAnyType();
            parametros1.Add(jsonStr1);
            var pResponse1 = flujoSvr.CallFlujoIndexFunction((int)RoutingFunctions.GetCajaBalanceMinimoByCajaId , MainFlujoWindows.MarSession, parametros1);  
            
            decimal BalanceMinimo =  JsonConvert.DeserializeObject<decimal>(pResponse1.Respuesta, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });
            return BalanceMinimo;
 
        }


    }
}
