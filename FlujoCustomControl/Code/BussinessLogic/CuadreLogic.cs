using System;
using System.Linq;
using System.Collections.Generic;
using FlujoCustomControl.Helpers;
using FlujoCustomControl.Views;
using Flujo.Entities.WpfClient.POCO;
using Flujo.Entities.WpfClient.Enums;
using Flujo.Entities.WpfClient.RequestModel;
using Flujo.Entities.WpfClient.ResponseModels;
using FlujoCustomControl.Code.AppEnums;
using System.Globalization;
using Newtonsoft.Json;



namespace FlujoCustomControl.Code.BussinessLogic
{

    public static class CuadreLogic
    {


        public static LocalBL lcvr = new LocalBL();
        private static FlujoServices.mar_flujoSoapClient flujoSvr = lcvr.GetFlujoServiceClient(false);


        public static bool EstaBancaPoseeCuadreInicial(int bancaId)
        {

            string param1 = JsonConvert.SerializeObject(bancaId);
            FlujoServices.ArrayOfAnyType parametros = new FlujoServices.ArrayOfAnyType();
            parametros.Add(param1);

            var pResponse = flujoSvr.CallFlujoIndexFunction((int)RoutingFunctions.BancaTieneCuadreInicial, MainFlujoWindows.MarSession, parametros);
            bool poseeCuadreInicial = JsonConvert.DeserializeObject<bool>(pResponse.Respuesta, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });
            return poseeCuadreInicial;
        }
        public static decimal GetBancaACuadrarMontoReal(int pBancaID)
        {

            string param1 = JsonConvert.SerializeObject(pBancaID);
            FlujoServices.ArrayOfAnyType parametros = new FlujoServices.ArrayOfAnyType();
            parametros.Add(param1);

            var pResponse1 = flujoSvr.CallFlujoIndexFunction((int)RoutingFunctions.GetBancaUltimoCuadreId, MainFlujoWindows.MarSession, parametros);
            var pResponse2 = flujoSvr.CallFlujoIndexFunction((int)RoutingFunctions.GetTransaccionesBancaDesdeUltimoCuadre, MainFlujoWindows.MarSession, parametros);

            decimal TotalIngresos = 0,
                    TotalEgresos = 0,
                    MontoReal = 0;


            int? LastCuadreID = JsonConvert.DeserializeObject<int?>(pResponse1.Respuesta, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });
            List<BancaTransaccionesResponse> LaListaTransacciones = JsonConvert.DeserializeObject<List<BancaTransaccionesResponse>>(pResponse2.Respuesta, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });

            TotalIngresos = LaListaTransacciones.Where(b => b.IngresoOEgreso.Equals("i")).Sum(b => b.MontoAcumulado);
            TotalEgresos = LaListaTransacciones.Where(b => b.IngresoOEgreso.Equals("e")).Sum(b => b.MontoAcumulado);



            if (
                 LastCuadreID == null || (!LastCuadreID.HasValue)
               )
            {
                MontoReal = TotalIngresos - TotalEgresos;
            }
            else
            {
                string param2 = JsonConvert.SerializeObject(LastCuadreID.Value);
                FlujoServices.ArrayOfAnyType pListGroup1 = new FlujoServices.ArrayOfAnyType();
                pListGroup1.Add(param2);
                var pResponse3 = flujoSvr.CallFlujoIndexFunction((int)RoutingFunctions.GetCuadreById, MainFlujoWindows.MarSession, pListGroup1);
                CuadreModel LastCuadreBanca = JsonConvert.DeserializeObject<CuadreModel>(pResponse3.Respuesta, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });
                MontoReal = (LastCuadreBanca.Balance + TotalIngresos) - TotalEgresos;
            }
            return MontoReal;
        }
        public static CuadreResponse Procesar(int bancaID, CuadreModel cuadre, Accion accion, out string impresion, RutaAsignacion rutaAsignacion = null, RutaRecorrido rutaRecorrido = null)
        {
            PrintDocumentTransaccionConCuadre doc = new PrintDocumentTransaccionConCuadre();
            CuadreModel cm = new CuadreModel();
            impresion = "";

            try
            {
                bool esUnRetiro = accion.OpcionSeleccionada == EnumAccionCuadre.Retirar;

                cm.CajaID = cuadre.CajaID;
                cm.Tipo = cuadre.Tipo;
                cm.UsuarioCaja = cuadre.UsuarioCaja;
                cm.CajaOrigenID = cuadre.CajaOrigenID;
                cm.UsuarioOrigenID = cuadre.UsuarioOrigenID;
                cm.BalanceMinimo = cuadre.BalanceMinimo;
                cm.MontoPorPagar = cuadre.MontoPorPagar;
                cm.MontoContado = cuadre.MontoContado;
                cm.MontoRetirado = cuadre.MontoRetirado;
                cm.MontoDepositado = cuadre.MontoDepositado;
                cm.AuxMensajeroNombre = cuadre.AuxMensajeroNombre;


                FlujoServices.ArrayOfAnyType paramsCuadre = new FlujoServices.ArrayOfAnyType();
                var cuadreSerialized = JsonConvert.SerializeObject(cm);
                paramsCuadre.Add(cuadreSerialized);
                paramsCuadre.Add(esUnRetiro.ToString());

                var pResponse = flujoSvr.CallFlujoIndexFunction((int)RoutingFunctions.SubmitNuevoCuadre, MainFlujoWindows.MarSession, paramsCuadre);
                var response = JsonConvert.DeserializeObject<CuadreResponse>(pResponse.Respuesta, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });


                if (response.FueProcesado)
                {
                    if (rutaAsignacion != null && rutaRecorrido != null) // Si el cuadre esta atado a una asignacion de ruta
                    {
                        try
                        {
                            decimal gestorBalanceAlCuadre = CajaLogic.GetUsuarioCaja(cuadre.UsuarioOrigenID).BalanceActual;

                            for (int i = 0; i < rutaRecorrido.Terminales.Count(); i++)
                            {
                                if (rutaRecorrido.Terminales[i].BancaID == bancaID)
                                {
                                    rutaRecorrido.Terminales[i].FueRecorrida = true;
                                    rutaRecorrido.Terminales[i].BalanceAlCuadreGestor = gestorBalanceAlCuadre;
                                    break;
                                }
                            }

                            int cuentaRecogidas = rutaRecorrido.Terminales.Count(t => t.FueRecorrida);

                            string rutaEstado = cuentaRecogidas == rutaRecorrido.Terminales.Count() ? "Completada" : "En progreso";  // 'En progreso' o 'Completada'                         
                            int rutaUltimaLocalidad = bancaID;
                            string rutaOrdenRecorrido = JsonConvert.SerializeObject(rutaRecorrido);


                            int cuadreId = response.CuadreID.Value;  // Cuadre Nuevo Generado que se va atar a la asignacion pendiente mediante |rutaId|
                            int bancaCajaId = cm.CajaID;
                            int rutaId = rutaAsignacion.RutaId;

                            EnlazaCuadreConAsignacion(rutaEstado, rutaUltimaLocalidad, rutaOrdenRecorrido, cuadreId, bancaCajaId, rutaId);
                        }
                        catch
                        {
                        }
                    }  

                    string param1 = JsonConvert.SerializeObject(bancaID);
                    FlujoServices.ArrayOfAnyType pListGroup1 = new FlujoServices.ArrayOfAnyType();
                    pListGroup1.Add(param1);

                    var pResponse1 = flujoSvr.CallFlujoIndexFunction((int)RoutingFunctions.GetBancaByBancaId, MainFlujoWindows.MarSession, pListGroup1);
                    Banca banca = JsonConvert.DeserializeObject<Banca>(pResponse1.Respuesta, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });

                    #region Building Printing Document

                    doc.BanContacto = banca.BanContacto;
                    doc.BanDireccion = banca.BanDireccion;
                    doc.BalanceDeCajaSegunSistema = response.MontoReal.ToString("$#,##0.00", CultureInfo.CreateSpecificCulture("en-US"));
                    doc.EfectivoContadoEnCaja = cm.MontoContado.ToString("$#,##0.00", CultureInfo.CreateSpecificCulture("en-US"));

                    if (response.MontoReal <= 0)
                    {
                        if (cm.MontoContado == 0)
                        {
                            doc.FaltanteSobrante = FaltanteSobrante_Enum.Faltante;
                            doc.MontoFaltanteOMontoSobrante = (0).ToString("$#,##0.00", CultureInfo.CreateSpecificCulture("en-US"));
                            doc.AUX_MontoFaltanteOMontoSobrante = 0;
                        }
                        else if (cm.MontoContado > 0)
                        {
                            cm.AuxMontoAFavor = cm.MontoContado;
                            doc.FaltanteSobrante = FaltanteSobrante_Enum.Sobrante;
                            doc.MontoFaltanteOMontoSobrante = cm.AuxMontoAFavor.ToString("$#,##0.00", CultureInfo.CreateSpecificCulture("en-US"));
                            doc.AUX_MontoFaltanteOMontoSobrante = cm.AuxMontoAFavor;
                        }
                    }
                    else
                    {
                        if ((response.MontoReal - cm.MontoContado) < 0)
                        {
                            cm.AuxMontoAFavor = -1 * (response.MontoReal - cm.MontoContado);
                            doc.FaltanteSobrante = FaltanteSobrante_Enum.Sobrante;
                            doc.MontoFaltanteOMontoSobrante = cm.AuxMontoAFavor.ToString("$#,##0.00", CultureInfo.CreateSpecificCulture("en-US"));
                            doc.AUX_MontoFaltanteOMontoSobrante = cm.AuxMontoAFavor;
                        }
                        else if ((response.MontoReal - cm.MontoContado) >= 0)
                        {
                            cm.MontoFaltante = response.MontoReal - cm.MontoContado;
                            doc.FaltanteSobrante = FaltanteSobrante_Enum.Faltante;
                            doc.MontoFaltanteOMontoSobrante = (cm.MontoFaltante).ToString("$#,##0.00", CultureInfo.CreateSpecificCulture("en-US"));
                            doc.AUX_MontoFaltanteOMontoSobrante = cm.MontoFaltante;
                        }
                    }

                    doc.Responsable = cm.UsuarioCaja;
                    doc.BalanceFinalCaja = response.NuevoBalance.ToString("$#,##0.00", CultureInfo.CreateSpecificCulture("en-US"));

                    if (accion.OpcionSeleccionada == EnumAccionCuadre.Depositar)
                    {
                        cm.MontoRetirado = null;
                        doc.BanMontoRetiradoODepositado = cm.MontoDepositado.HasValue ? cm.MontoDepositado.Value.ToString("$#,##0.00", CultureInfo.CreateSpecificCulture("en-US")) : (0).ToString("$#,##0.00", CultureInfo.CreateSpecificCulture("en-US"));
                        doc.RecibidoPor = cm.UsuarioCaja;
                    }
                    else
                    {
                        cm.MontoDepositado = null;
                        doc.EsUnDeposito = false;
                        doc.BanMontoRetiradoODepositado = cm.MontoRetirado.HasValue ? cm.MontoRetirado.Value.ToString("$#,##0.00", CultureInfo.CreateSpecificCulture("en-US")) : (0).ToString("$#,##0.00", CultureInfo.CreateSpecificCulture("en-US"));
                        doc.RecibidoPor = cm.AuxMensajeroNombre;
                    }

                    doc.CuadreTransaccion = response.CuadreRef;
                    doc.FechaTransaccion = response.Fecha.ToString("dd MMMM yyyy hh:mm tt", new CultureInfo("es-ES"));
                    impresion = ReciboTemplateHelper.Get__ReciboTemplate__ConCuadre(doc);

                    #endregion


                }// Caudre Fue Procesado Fin

                return response;
            }
            catch (Exception ex)
            {
                return new CuadreResponse() { Error = ex.Message + ". " + ex.StackTrace, FueProcesado = false };
            }
        }

        public static decimal GetMontoTotalizadoTiketsPendientesDePagoSinReclamar(int pBancaID)
        {

            string param1 = JsonConvert.SerializeObject(pBancaID);

            FlujoServices.ArrayOfAnyType pListGroup1 = new FlujoServices.ArrayOfAnyType();
            pListGroup1.Add(param1);

            var pResponse1 = flujoSvr.CallFlujoIndexFunction((int)RoutingFunctions.GetMontoTotalizadoTicketsPendienteDePagoByBancaId, MainFlujoWindows.MarSession, pListGroup1);

            decimal MontoTotalPagosSinReclamar = JsonConvert.DeserializeObject<decimal>(pResponse1.Respuesta, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });

            return MontoTotalPagosSinReclamar;

        }



        private static bool EnlazaCuadreConAsignacion(string rutaEstado, int rutaUltimaLocalidad, string rutaOrdenRecorrido, int cuadreId, int bancaCajaId, int rutaId)
        {
            FlujoServices.ArrayOfAnyType parametros = new FlujoServices.ArrayOfAnyType();
            parametros.Add(rutaEstado);
            parametros.Add(rutaUltimaLocalidad);
            parametros.Add(rutaOrdenRecorrido);
            parametros.Add(cuadreId);
            parametros.Add(bancaCajaId);
            parametros.Add(rutaId);

            var consulta = flujoSvr.CallFlujoIndexFunction((int)RoutingFunctions.EnlazaCuadreConAsignacion, MainFlujoWindows.MarSession, parametros);


            if (consulta.OK == false)
            {
                throw new Exception("Ha occurrido un error al obtener la tarjeta");
            }

            if (consulta.Respuesta == null || consulta.Respuesta.Equals("null"))
            {
                return false;
            }



            bool fueEnlazado = JsonConvert.DeserializeObject<bool>(consulta.Respuesta, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });

            return fueEnlazado;

        }




    }
}















































//
//
// ** Metodo Original para procesar el Cuadre **  FAVOR Y NO BORRAR
//
// False si el cuadre no fue procesado de lo contrario true./
//public static bool ProcesarCuadre(int pBancaID, CuadreModel cuadre, Accion pAccion, out string pPrintCuadreStr)
//{
//    PrintDocumentTransaccionConCuadre printDoc = new PrintDocumentTransaccionConCuadre();

//    pPrintCuadreStr = "";

//    try
//    {
//        string param1 = JsonConvert.SerializeObject(pBancaID);
//        FlujoServices.ArrayOfAnyType pListGroup1 = new FlujoServices.ArrayOfAnyType();
//        pListGroup1.Add(param1);

//        var pResponse1 = flujoSvr.CallFlujoIndexFunction(7, MainFlujoWindows.MarSession, pListGroup1); // GetBanca()
//        Banca banca = JsonConvert.DeserializeObject<Banca>(pResponse1.Respuesta, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });
//        printDoc.BanContacto = banca.BanContacto;
//        printDoc.BanDireccion = banca.BanDireccion;

//        // Verifico si hay cuadre
//        if (cuadre == null)
//        {
//            return false;
//        }

//        decimal TotalIngresos = 0,
//                TotalEgresos = 0;


//        var pResponse2 = flujoSvr.CallFlujoIndexFunction(2, MainFlujoWindows.MarSession, pListGroup1); // GetBancaLastCuadre_ID()
//        int? LastCuadreID = JsonConvert.DeserializeObject<int?>(pResponse2.Respuesta, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });

//        // Total de Ingresos y Egreso Banca desde ultimo cuadre

//        var pResponse3 = flujoSvr.CallFlujoIndexFunction(3, MainFlujoWindows.MarSession, pListGroup1); // GetTransaccionesDesdeUltimoCuadre()

//        List<BancaTransaccionesResponse> LaListaTransacciones = JsonConvert.DeserializeObject<List<BancaTransaccionesResponse>>(pResponse3.Respuesta, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });
//        TotalIngresos = LaListaTransacciones.Where(b => b.IngresoOEgreso.Equals("i")).Sum(b => b.MontoAcumulado);
//        TotalEgresos = LaListaTransacciones.Where(b => b.IngresoOEgreso.Equals("e")).Sum(b => b.MontoAcumulado);

//        //Modelo de Cuadre

//        CuadreModel cm = new CuadreModel();

//        cm.CajaID = cuadre.CajaID;                   // Caja de Banca
//        cm.UsuarioCaja = cuadre.UsuarioCaja;
//        cm.CajaOrigenID = cuadre.CajaOrigenID;       // Caja de Mensajero
//        cm.UsuarioOrigenID = cuadre.UsuarioOrigenID;
//        cm.CuadreAnteriorID = LastCuadreID;          // Id del ultimo cuadre a menos que no sea un cuadre inicial
//        cm.BalanceMinimo = cuadre.BalanceMinimo;
//        cm.MontoPorPagar = cuadre.MontoPorPagar;
//        cm.MontoContado = cuadre.MontoContado;       // Monto contado por Mensajero   <--- Dinero contado por el mensajero en caja
//                                                     // cm.MontoReal                                 
//        cm.MontoRetirado = cuadre.MontoRetirado;   // Monto retirado por el Mensajero
//        cm.MontoDepositado = cuadre.MontoDepositado; // Monto depositado por el Mensajero       
//        cm.AuxMensajeroNombre = cuadre.AuxMensajeroNombre;

//        if (
//             LastCuadreID == null || (!LastCuadreID.HasValue)
//           )
//        {
//            // ### Cuadre Inicial
//            cm.MontoReal = TotalIngresos - TotalEgresos;
//            cm.BalanceAnterior = null;

//            //Seteando el Balance Inicial del documento a imprimir
//            printDoc.BalanceDeCajaSegunSistema = cm.MontoReal.ToString("$#,##0.00", CultureInfo.CreateSpecificCulture("en-US"));
//        }
//        else
//        {
//            // ### Cuadre Anteterior registrado

//            string param2 = JsonConvert.SerializeObject(LastCuadreID.Value);
//            FlujoServices.ArrayOfAnyType pListGroup2 = new FlujoServices.ArrayOfAnyType();
//            pListGroup2.Add(param2);

//            var pResponse4 = flujoSvr.CallFlujoIndexFunction(4, MainFlujoWindows.MarSession, pListGroup2); // GetCuadre()

//            //Se setea el cuadre Inicial 
//            CuadreModel LastCuadreBanca = JsonConvert.DeserializeObject<CuadreModel>(pResponse4.Respuesta, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });
//            cm.MontoReal = (LastCuadreBanca.Balance + TotalIngresos) - TotalEgresos;
//            cm.BalanceAnterior = LastCuadreBanca.Balance;

//            //Seteando el Balance Inicial del documento a imprimir
//            printDoc.BalanceDeCajaSegunSistema = cm.MontoReal.ToString("$#,##0.00", CultureInfo.CreateSpecificCulture("en-US"));
//        }


//        printDoc.EfectivoContadoEnCaja = cm.MontoContado.ToString("$#,##0.00", CultureInfo.CreateSpecificCulture("en-US"));

//        //Calculo de Monto Faltante  y Balance
//        if (cm.MontoReal <= 0)
//        {
//            /**Banca en perdida*/
//            if (cm.MontoContado == 0)
//            {
//                cm.MontoFaltante = 0;

//                //Seteando el documento a imprimir como un Faltante
//                printDoc.FaltanteSobrante = FaltanteSobrante_Enum.Faltante;
//                printDoc.MontoFaltanteOMontoSobrante = (0).ToString("$#,##0.00", CultureInfo.CreateSpecificCulture("en-US"));
//                printDoc.AUX_MontoFaltanteOMontoSobrante = 0;

//            }
//            else if (cm.MontoContado > 0)
//            {
//                cm.AuxMontoAFavor = cm.MontoContado;
//                cm.MontoFaltante = -1 * cm.AuxMontoAFavor;

//                string param3 = JsonConvert.SerializeObject(cm.CajaID);
//                string param4 = JsonConvert.SerializeObject("Entrada");
//                string param5 = JsonConvert.SerializeObject(cm.AuxMontoAFavor);

//                FlujoServices.ArrayOfAnyType pListGroup3 = new FlujoServices.ArrayOfAnyType();
//                pListGroup3.Add(param3);
//                pListGroup3.Add(param4);
//                pListGroup3.Add(param5);

//                var pResponse5 = flujoSvr.CallFlujoIndexFunction(8, MainFlujoWindows.MarSession, pListGroup3); // RegistarMontoAfavorCuadre()
//                //CuadreRepositorio.RegistarMontoAfavorCuadre(cm.CajaID, "Entrada", cm.AuxMontoAFavor);

//                //Seteando el documento a imprimir como un Sobrante
//                printDoc.FaltanteSobrante = FaltanteSobrante_Enum.Sobrante;
//                printDoc.MontoFaltanteOMontoSobrante = cm.AuxMontoAFavor.ToString("$#,##0.00", CultureInfo.CreateSpecificCulture("en-US"));
//                printDoc.AUX_MontoFaltanteOMontoSobrante = cm.AuxMontoAFavor;

//            }
//        }
//        else
//        {
//            /**Banca con balance disponible**/
//            if ((cm.MontoReal - cm.MontoContado) < 0)
//            {
//                cm.AuxMontoAFavor = -1 * (cm.MontoReal - cm.MontoContado);
//                cm.MontoFaltante = -1 * cm.AuxMontoAFavor;


//                string param6 = JsonConvert.SerializeObject(cm.CajaID);
//                string param7 = JsonConvert.SerializeObject("Entrada");
//                string param8 = JsonConvert.SerializeObject(cm.AuxMontoAFavor);


//                FlujoServices.ArrayOfAnyType pListGroup4 = new FlujoServices.ArrayOfAnyType();
//                pListGroup4.Add(param6);
//                pListGroup4.Add(param7);
//                pListGroup4.Add(param8);

//                var pResponse6 = flujoSvr.CallFlujoIndexFunction(8, MainFlujoWindows.MarSession, pListGroup4); // RegistarMontoAfavorCuadre()

//                //CuadreRepositorio.RegistarMontoAfavorCuadre(cm.CajaID, "Entrada", cm.AuxMontoAFavor);

//                //Seteando el documento a imprimir como un Sobrante
//                printDoc.FaltanteSobrante = FaltanteSobrante_Enum.Sobrante;
//                printDoc.MontoFaltanteOMontoSobrante = cm.AuxMontoAFavor.ToString("$#,##0.00", CultureInfo.CreateSpecificCulture("en-US"));
//                printDoc.AUX_MontoFaltanteOMontoSobrante = cm.AuxMontoAFavor;

//            }
//            else if ((cm.MontoReal - cm.MontoContado) >= 0)
//            {
//                cm.MontoFaltante = cm.MontoReal - cm.MontoContado;

//                //Seteando el documento a imprimir como un Faltante
//                printDoc.FaltanteSobrante = FaltanteSobrante_Enum.Faltante;
//                printDoc.MontoFaltanteOMontoSobrante = (cm.MontoFaltante).ToString("$#,##0.00", CultureInfo.CreateSpecificCulture("en-US"));
//                printDoc.AUX_MontoFaltanteOMontoSobrante = cm.MontoFaltante;

//            }
//        }


//        cm.Balance = cm.MontoReal - cm.MontoRetirado.Value - cm.MontoFaltante;

//        printDoc.Responsable = cm.UsuarioCaja;
//        printDoc.BalanceFinalCaja = cm.Balance.ToString("$#,##0.00", CultureInfo.CreateSpecificCulture("en-US"));


//        // ##  Movimientos Adicionales
//        if (cm.MontoRetirado > 0)     // Hubo un retiro en la Banca
//        {

//            string param9 = JsonConvert.SerializeObject(cm.CajaID);
//            string param10 = JsonConvert.SerializeObject("Salida");
//            string param11 = JsonConvert.SerializeObject(cm.MontoRetirado.Value);


//            FlujoServices.ArrayOfAnyType pListGroup5 = new FlujoServices.ArrayOfAnyType();
//            pListGroup5.Add(param9);
//            pListGroup5.Add(param10);
//            pListGroup5.Add(param11);

//            var pResponse7 = flujoSvr.CallFlujoIndexFunction(9, MainFlujoWindows.MarSession, pListGroup5); // RegistarMovimientoCuadre()

//            string param12 = JsonConvert.SerializeObject(cm.CajaOrigenID);
//            string param13 = JsonConvert.SerializeObject("Entrada");
//            string param14 = JsonConvert.SerializeObject(cm.MontoRetirado.Value);


//            FlujoServices.ArrayOfAnyType pListGroup6 = new FlujoServices.ArrayOfAnyType();
//            pListGroup6.Add(param12);
//            pListGroup6.Add(param13);
//            pListGroup6.Add(param14);

//            var pResponse8 = flujoSvr.CallFlujoIndexFunction(9, MainFlujoWindows.MarSession, pListGroup6); // RegistarMovimientoCuadre()

//            // Salida a Banca  y Entrada a Mensajero
//            //CuadreRepositorio.RegistarMovimientoCuadre(cm.CajaID, "Salida", cm.MontoRetirado.Value);
//            //CuadreRepositorio.RegistarMovimientoCuadre(cm.CajaOrigenID, "Entrada", cm.MontoRetirado.Value);

//        }
//        else if (cm.MontoRetirado < 0) // Hubo un deposito en la Banca
//        {

//            string param15 = JsonConvert.SerializeObject(cm.CajaID);
//            string param16 = JsonConvert.SerializeObject("Entrada");
//            string param17 = JsonConvert.SerializeObject((-1 * cm.MontoRetirado.Value));


//            FlujoServices.ArrayOfAnyType pListGroup7 = new FlujoServices.ArrayOfAnyType();
//            pListGroup7.Add(param15);
//            pListGroup7.Add(param16);
//            pListGroup7.Add(param17);

//            var pResponse9 = flujoSvr.CallFlujoIndexFunction(9, MainFlujoWindows.MarSession, pListGroup7); // RegistarMovimientoCuadre()

//            string param18 = JsonConvert.SerializeObject(cm.CajaOrigenID);
//            string param19 = JsonConvert.SerializeObject("Salida");
//            string param20 = JsonConvert.SerializeObject((-1 * cm.MontoRetirado.Value));


//            FlujoServices.ArrayOfAnyType pListGroup8 = new FlujoServices.ArrayOfAnyType();
//            pListGroup8.Add(param18);
//            pListGroup8.Add(param19);
//            pListGroup8.Add(param20);

//            var pResponse10 = flujoSvr.CallFlujoIndexFunction(9, MainFlujoWindows.MarSession, pListGroup8); // RegistarMovimientoCuadre()

//            //  Entrada a Banca y Salida a Mensajero
//            //CuadreRepositorio.RegistarMovimientoCuadre(cm.CajaID, "Entrada", (-1 * cm.MontoRetirado.Value));
//            //CuadreRepositorio.RegistarMovimientoCuadre(cm.CajaOrigenID, "Salida", (-1 * cm.MontoRetirado.Value));
//        }




//        var pResponse11 = flujoSvr.CallFlujoIndexFunction(10, MainFlujoWindows.MarSession, pListGroup1); // AsentarTransaccionesDesdeUltimoCuadre()

//        // ## Asentando transacciones en flujo
//        //CuadreRepositorio.AsentarTransaccionesDesdeUltimoCuadre(pBancaID);


//        if (pAccion.OpcionSeleccionada == EnumAccionCuadre.Depositar)
//        {
//            cm.MontoRetirado = null;

//            //Seteando el documento a imprimir como un deposito
//            printDoc.EsUnDeposito = true;
//            printDoc.BanMontoRetiradoODepositado = cm.MontoDepositado.HasValue ? cm.MontoDepositado.Value.ToString("$#,##0.00", CultureInfo.CreateSpecificCulture("en-US")) : (0).ToString("$#,##0.00", CultureInfo.CreateSpecificCulture("en-US"));
//            printDoc.RecibidoPor = cm.UsuarioCaja;

//        }
//        else
//        {
//            cm.MontoDepositado = null;

//            //Seteando el documento a imprimir como un retiro
//            printDoc.EsUnDeposito = false;
//            printDoc.BanMontoRetiradoODepositado = cm.MontoRetirado.HasValue ? cm.MontoRetirado.Value.ToString("$#,##0.00", CultureInfo.CreateSpecificCulture("en-US")) : (0).ToString("$#,##0.00", CultureInfo.CreateSpecificCulture("en-US"));
//            printDoc.RecibidoPor = cm.AuxMensajeroNombre;

//        }

//        // ## Registrando Cuadre
//        bool CuadreFueRegistrado = RegistrarCuadre(cm);

//        if (CuadreFueRegistrado)
//        {


//            var pResponse12 = flujoSvr.CallFlujoIndexFunction(2, MainFlujoWindows.MarSession, pListGroup1);
//            int? LCID = JsonConvert.DeserializeObject<int?>(pResponse12.Respuesta, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });

//            string param21 = JsonConvert.SerializeObject(LCID.Value);
//            FlujoServices.ArrayOfAnyType pListGroup9 = new FlujoServices.ArrayOfAnyType();
//            pListGroup9.Add(param21);

//            var pResponse13 = flujoSvr.CallFlujoIndexFunction(4, MainFlujoWindows.MarSession, pListGroup9); // GetCuadre()


//            CuadreModel UltCuadreRegistrado = JsonConvert.DeserializeObject<CuadreModel>(pResponse13.Respuesta, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });


//            //Buscando el Ultimo cuadre registrado
//            // CuadreModel UltCuadreRegistrado = CuadreRepositorio.GetCuadre( CuadreRepositorio.GetBancaLastCuadre_ID(pBancaID).Value );

//            //Creando la referencia del ultimo cuadre creado
//            string cuadreRef = Convert.ToString(UltCuadreRegistrado.CuadreID).PadLeft(13, '0');

//            //Llenando los valores restantes del documento a imprimir
//            printDoc.CuadreTransaccion = banca.BancaID + "-" + cuadreRef;
//            printDoc.FechaTransaccion = UltCuadreRegistrado.Fecha.ToString("dd MMMM yyyy hh:mm tt", new CultureInfo("es-ES"));


//            pPrintCuadreStr = ReciboTemplateHelper.Get__ReciboTemplate__ConCuadre(printDoc);


//        }

//        return CuadreFueRegistrado;

//    }
//    catch (Exception ex)
//    {
//        return false;
//    }
//}

//private static bool RegistrarCuadre(CuadreModel cuadre)
//{

//    string param1 = JsonConvert.SerializeObject(cuadre);

//    FlujoServices.ArrayOfAnyType pListGroup1 = new FlujoServices.ArrayOfAnyType();
//    pListGroup1.Add(param1);

//    var pResponse1 = flujoSvr.CallFlujoIndexFunction(5, MainFlujoWindows.MarSession, pListGroup1);

//    bool CuadreFueRegistrado = JsonConvert.DeserializeObject<bool>(pResponse1.Respuesta, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });
//    return CuadreFueRegistrado;

//}
