using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MAR.BusinessLogic.Models.RequestModel;
using MAR.DataAccess.ViewModels;
using JuegosTemplate.DataAccess.Servicios.LoteriaServicio.Utilidades;
using MAR.DataAccess.ViewModels.Mappers;
using MAR.DataAccess.Tables.Enums;
using MAR.DataAccess.Tables.DTOs;
using MAR.DataAccess.EFRepositories.CLRepositories;

namespace MAR.BusinessLogic.Code.RFCode
{
    public class LoteriasNuevasIndexLogic
    {
        public static object GetSorteosLoteriasNuevas(int pBancaId)
        {
            List<DataAccess.Tables.Enums.DbEnums.SorteoReferencia> sorteosRef = new List<DataAccess.Tables.Enums.DbEnums.SorteoReferencia>();
            sorteosRef.Add(DataAccess.Tables.Enums.DbEnums.SorteoReferencia.Pega4Real);
            var result = DataAccess.EFRepositories.RFRepositories.RFSorteoRepository.GetRFSorteosDia(sorteosRef, pBancaId);
            return new
            {
                OK = true,
                Resultado = result
            };
        }

        public static object RealizaVenta(List<JuegosNuevosRequestModel.RFTransaccionRequestModel.RFTransacciones> pRfTransaccioneses, List<JuegosNuevosRequestModel.RFTransaccionRequestModel.CLReciboDetalleCampos> pReciboCampos, int pBancaID, int pUsuarioID, int pPrintW, string pBanca, string pDireccion, string pFood, string pHead, int pRiferoId)
        {
            try
            {

                var pmCuenta = DataAccess.EFRepositories.PMCuentasRepository.GetPMCuenta(pRiferoId, Enums.ProductosEnum.LoteriasNuevas.ToString(), 0,5);
                if (pmCuenta == null)
                {
                    return new
                    {
                        OK = false,
                        Mensaje = "Cuenta no configurada",
                        Err = "La Cuenta no esta configurada para este Rifero"
                    };
                }
                List<int> sortesId = new List<int>();
                foreach (var item in pRfTransaccioneses)   //***************VALIDA LOS LIMITES*****************
                {
                    var limite = DataAccess.EFRepositories.RFRepositories.RFLimitesRepository.ValidaLimites(item.SorteoDiaID, item.TransaccionJugadas.Select(x => x.Aposto).FirstOrDefault());
                    if (!limite.Valido)
                    {
                        return new
                        {
                            OK = false,
                            Mensaje = "Paso el Limite " + limite.TipoLimite + " de Ventas para el Sorteo " + limite.Sorteo + ", Solo puede vender " + Math.Round(limite.PuedeVender, 0)
                        };
                    }
                }
                //var resultados = ResultadoSorteos(DateTime.Now.Date.AddDays(-1));
                var jugadas = new List<JuegosNuevosRequestModel.ClienteJugadas>();
                foreach (var item in pRfTransaccioneses)
                {
                    jugadas.Add(new JuegosNuevosRequestModel.ClienteJugadas { CodigoJuego = item.Referencia, Jugada = item.TransaccionJugadas.FirstOrDefault().Numeros, Monto = int.Parse(item.TransaccionJugadas.FirstOrDefault().Aposto.ToString()) });
                }


               
                    var transaccionesValidas = new List<Models.RequestModel.JuegosNuevosRequestModel.RFTransaccionRequestModel.RFTransacciones>();
                    DateTime fechaRegistro = DateTime.Now;
                    int[] SorteosDiaAvalidar = pRfTransaccioneses.Select(x => x.SorteoDiaID).Distinct().ToArray();
                    var sorteosValidos = DataAccess.EFRepositories.RFRepositories.RFSorteoRepository.ValidarRFSorteosPorID(SorteosDiaAvalidar).Select(x => x.SorteoDiaID); // CAMBIAR A DAPPER

                    if (sorteosValidos.Any())
                    {
                        transaccionesValidas = pRfTransaccioneses.Where(x => sorteosValidos.Contains(x.SorteoDiaID)).ToList();
                    }
                    else
                    {
                        return new
                        {
                            OK = false,
                            Mensaje = "Los Sorteos no Disponibles"
                        };
                    }


                    for (int i = 0; i < transaccionesValidas.Count; i++)
                    {
                        transaccionesValidas[i].FechaIngreso = fechaRegistro;
                        transaccionesValidas[i].Estado = DataAccess.Tables.Enums.DbEnums.RFTransaccionEstado.Exitosa.ToString();
                        transaccionesValidas[i].Ingresos = transaccionesValidas[i].TransaccionJugadas.Sum(x => x.Aposto);
                    }

                    var transaccionViewModel = Models.Mappers.JuegosNuevosMapper.MapRfTransaccion(transaccionesValidas);


                  
                    var recibo = CLReciboRepository.AgregarCL_Recibo(new CL_Recibo
                    {
                        BancaID = pBancaID,
                        UsuarioID = pUsuarioID,
                        ClienteID = 1,
                        Descuentos = 0,
                        Fecha = fechaRegistro,
                        Impuestos = 0,
                        Ingresos = transaccionesValidas.Sum(x => x.Ingresos),
                        MonedaID = 1,
                        Referencia = "0",//Enums.ProductosEnum.Pega3Pega4.ToString(),
                        SolicitudID = 0,
                        RF_Transacciones = transaccionViewModel.ToList()
                      //  , CL_ReciboDetalle_Extra = recibosDeltallesExtras
                    },"B", true);

                    List<DataAccess.Tables.DTOs.CL_Recibo> recibos = new List<DataAccess.Tables.DTOs.CL_Recibo>();
                    recibos.Add(recibo);





                if (recibos.Any())
                {
                   

                   
                        recibo.Activo = true;
                        var transViewModel = Models.Mappers.JuegosNuevosMapper.MapRecibos(recibos, true).FirstOrDefault();
                        string firma = GeneraFirma(recibo.Fecha.ToShortDateString(), recibo.Fecha.ToString("t"), recibo.Referencia, recibo.RF_Transacciones.ToList());
                        var printData = PrintJobs.JuegosNuevosPrintJob.ImprimirJuegosNuevos(pPrintW, pBanca, pDireccion, recibo.Referencia, BaseViewModel.GeneraPinGanador(recibo.ReciboID), false, transViewModel, pFood, pHead, pBancaID, firma, "");
                        return new
                        {
                            OK = true,
                            PrintData = printData,
                            Transaccion = transViewModel
                        };
                  
                }

                else
                {
                    return new { OK = false, Mensaje = "Fallo la transaccion", Err = "Error en la transaccion." };
                }
            }
            catch (Exception e)
            {
                return new { OK = false, Mensaje = "Fallo la transaccion", Err = e.Message };
            }
      
        }


        public static object AnulaRFTransaccionLoteriasNuevas(int pReciboId, string pPin, int pRiferoId)
        {
            try
            {
                if (BaseViewModel.ComparaPinGanador(pReciboId, pPin))
                {
                    var campos = CLReciboRepository.GetCamposExtras("Autorizacion", pReciboId);
                    
                    var transaccionAnulada = DataAccess.EFRepositories.RFRepositories.RFTransaccionRepository.Anula_RfTransaccion(pReciboId, "Anulado por cliente", "No Requiere");
                        return new
                        {
                            OK = true,
                            Mensaje = transaccionAnulada
                        };
                    
                }
                else
                {
                    return new
                    {
                        OK = false,
                        Mensaje = "El Pin es incorrecto."
                    };
                }
            }
            catch
            {
                return new
                {
                    OK = false,
                    Mensaje = "Fallo la anulacion"
                };
            }
        }





        private static string GeneraFirma(string pFecha, string pHora, string pTicket, List<DataAccess.Tables.DTOs.RF_Transaccion> pJugadas)
        {
            try
            {
                var rdn = (new Random(DateTime.Now.Millisecond)).Next(31) + 1;
                var Cadena = String.Format("{0}{1}{2}{3}{4}21", pFecha, pHora, pTicket, pJugadas, rdn);
                long acum1 = 0;
                for (var i = 1; i <= Cadena.Length; i++)
                {
                    acum1 += (i * (int)Cadena[i - 1]);
                }

                var Source = (acum1 % 100000).ToString().PadLeft(5, '0');

                acum1 = 0;
                foreach (var item in pJugadas)
                {
                    Cadena = String.Format("{0}{1}{2}3", pJugadas,
                                            rdn,
                                            Convert.ToInt32(Math.Ceiling(item.Ingresos)));
                    for (var i = 0; i < Cadena.Length; i++)
                    {
                        acum1 += (int)Cadena[i];
                    }

                }


                Source = String.Format("{0}-{1}", Source, (acum1 % 10000000).ToString().PadLeft(7, '1'));

                var theCrosswalk = GetCrosswalk();
                var theResult = string.Empty;
                for (var cnt = 1; cnt <= 13; cnt++)
                {
                    var iSrcNo = 0;
                    var iSrc = Source[cnt - 1].ToString();
                    if (cnt == 6)
                    {
                        iSrcNo = rdn;
                    }
                    else
                    {
                        iSrcNo = theCrosswalk.Where(x => x.Value.Equals(iSrc)).Select(x => x.Key).FirstOrDefault();
                        iSrcNo = (iSrcNo + rdn + cnt) % 33;
                    }
                    theResult += theCrosswalk[iSrcNo];
                }
                return theResult;
            }
            catch
            {
                return "- no disponible -";
            }
        }
        private static Dictionary<int, string> GetCrosswalk()
        {
            return ("*0#Q*1#V*2#C*3#0*4#H*5#5*6#M*7#R*8#W*9#D*10#1*11#6*12#N*13#S*14#X*15#E*16#2*17#J*18#7*19#T*20#A*21#Y*22#F*23#3*24#K*25#8*26#P*27#U*28#B*29#L*30#G*31#4*32#9")
                       .Split('*').Where(x => x.Length > 1)
                       .Select(xx => new { key = Convert.ToInt32(xx.Split('#')[0]), value = xx.Split('#')[1] })
                       .ToDictionary(x => x.key, y => y.value);
        }



        public static DataAccess.ViewModels.Mappers.LotteryVIPMapper.LotteryVIP_Response AutorizaJugada(JuegosNuevosRequestModel.ClienteDatosSuplidos pClienteDatos, LoteriaServicioParametros pLoteriaServicio)                  /////////////////////La salida puede ser solo en XML| chequiar => JSON
        {
            List<LoteriaServicioParametros.LoteriaTicket.DetalleDeJuego> detalleJugadas = ListameDetalleDeJuegos(pClienteDatos.Jugadas);
            pLoteriaServicio.Consorcio = pClienteDatos.Consorcio;
            pLoteriaServicio.Usuario = pClienteDatos.Usuario;
            pLoteriaServicio.Password = pClienteDatos.Password;
            pLoteriaServicio.Ticket.Juegos = detalleJugadas;
            return LotteryVIPMapper.AutorizaJugada(pLoteriaServicio, LotteryVIPMapper.SalidaEn.XML);
        }//End AutorizaJugada()~

        public static async Task<string> AutorizaJugadaAsync(JuegosNuevosRequestModel.ClienteDatosSuplidos c) /////////////////////La salida puede ser solo en XML| chequiar => JSON
        {

            LoteriaServicioParametros sp = new LoteriaServicioParametros();

            List<LoteriaServicioParametros.LoteriaTicket.DetalleDeJuego> detalleJugadas = ListameDetalleDeJuegos(c.Jugadas);

            sp.Consorcio = c.Consorcio;
            sp.Usuario = c.Usuario;
            sp.Password = c.Password;
            sp.Ticket = new LoteriaServicioParametros.LoteriaTicket
                            ()
            {
                CodigoDeConsorcio = c.Consorcio,
                CodigoDeAgencia = "1",
                NombreDeAgencia = "Lexus",
                CodigoDeCaja = "17",
                NombreDelSupervisor = "Fulanito",
                CedulaDelSupervisor = "001-123456-1",
                NombreDelCajero = "Cajero",
                CedulaDelCajero = "001-123585-9",
                NumeroDeTicket = "38294",
                Fecha = DateTime.Now,
                MontoJugada = "15",
                Juegos = detalleJugadas
            };

            return await LotteryVIPMapper.AutorizaJugadaAsync(sp, LotteryVIPMapper.SalidaEn.XML);


        }//End AutorizaJugadaAsync()~

        //...............................................................................................................................................................(AnulaJugada|Async)

        public static LotteryVIPMapper.LotteryVIP_Response  AnulaJugada(LoteriaServicioParametros pLoteriaServicio)                                 /////////////////////La salida puede  ser en XML|JSON
        {
            return LotteryVIPMapper.AnulaJugada(pLoteriaServicio, LotteryVIPMapper.SalidaEn.XML);

        }//End AnulaJugada()~

        public static async Task<string> AnulaJugadaAsync(JuegosNuevosRequestModel.ClienteDatosSuplidos c)                /////////////////////La salida puede  ser en XML|JSON
        {
            LoteriaServicioParametros sp = new LoteriaServicioParametros();

            //Setiando parametros de servicios
            sp.Consorcio = c.Consorcio;
            sp.Usuario = c.Usuario;
            sp.Password = c.Password;
            sp.Autorizacion = "4BD3863B-2DA6-4FEE-6DB798F4EC";

            string resultado = await LotteryVIPMapper.AnulaJugadaAsync(sp, LotteryVIPMapper.SalidaEn.JSON);

            return resultado;

        }//End AnulaJugadaAsync()~

        //...............................................................................................................................................................(ConsultaJugadaFecha|Async)

        public static string ConsultaJugadasFecha(JuegosNuevosRequestModel.ClienteDatosSuplidos c)                        /////////////////////La salida puede  ser en XML|JSON
        {

            LoteriaServicioParametros sp = new LoteriaServicioParametros();

            sp.Consorcio = c.Consorcio;
            sp.Usuario = c.Usuario;
            sp.Password = c.Password;

            sp.FechaDeJugadas = "2018-01-05";
            sp.PPagina = 1;

            return LotteryVIPMapper.ConsultaJugadasFecha(sp, LotteryVIPMapper.SalidaEn.XML);

        }//End ConsultaJugadasFecha()~

        public static async Task<string> ConsultaJugadasFechaAsync(JuegosNuevosRequestModel.ClienteDatosSuplidos c)       /////////////////////La salida puede  ser en XML|JSON
        {

            LoteriaServicioParametros sp = new LoteriaServicioParametros();

            sp.Consorcio = c.Consorcio;
            sp.Usuario = c.Usuario;
            sp.Password = c.Password;

            sp.FechaDeJugadas = "2018-01-05";
            sp.PPagina = 1;

            return await LotteryVIPMapper.ConsultaJugadasFechaAsync(sp, LotteryVIPMapper.SalidaEn.XML);

        }//End ConsultaJugadasFechaAsync()~

        //...............................................................................................................................................................(ResultadoSorteos|Async)

        public static LotteryVIPMapper.LotteryVIP_Response ResultadoSorteos(DateTime pFechaSorteo)                                                  /////////////////////La salida es en XML
        {
            string fechaSorteo = pFechaSorteo.ToString(format: "yyyy-MM-dd");

            LoteriaServicioParametros sp = new LoteriaServicioParametros();

            sp.FechaDeResultadosDeSorteos = fechaSorteo; //Cada viernes

            return LotteryVIPMapper.ResultadoSorteos(sp);

        }//End ResultadoSorteos()~

        public static async Task<string> ResultadoSorteosAsync()                                 /////////////////////La salida es en XML
        {

            LoteriaServicioParametros sp = new LoteriaServicioParametros();

            sp.FechaDeResultadosDeSorteos = "2017-12-01";

            return await LotteryVIPMapper.ResultadoSorteosAsync(sp);

        }//End ResultadoSorteosAsync()~

        private static List<LoteriaServicioParametros.LoteriaTicket.DetalleDeJuego> ListameDetalleDeJuegos(List<JuegosNuevosRequestModel.ClienteJugadas> j)
        {

            List<LoteriaServicioParametros.LoteriaTicket.DetalleDeJuego> d = new List<LoteriaServicioParametros.LoteriaTicket.DetalleDeJuego>();

            foreach (var x in j)
            {

                d.Add(

                    new LoteriaServicioParametros.LoteriaTicket.DetalleDeJuego()
                    {

                        Codigo = x.CodigoJuego,
                        Monto = Convert.ToString(x.Monto),
                        Jugada = x.Jugada

                    }
                );

            }
            return d;

        }//End ListameDetalleDeJuegos()~


        private static bool ErrorRespuesta(int pCod)
        {
            if (pCod == (int)ProductosExternosEnums.JuegosNuevosStatus.OperacionSatisFactoria)
            {
                return false;
            }
            return true;
        }

        public static object RealizaPagoTicket(string pDireccion, string pBanca, int pPrintW, int pReciboId, string pPin, int pBancaID, string pBancaJugada)
        {
            if (int.Parse(pBancaJugada) != pBancaID)
            {
                return new
                {
                    OK = false,
                    Err = "No puede Pagar. El Ticket debe ser pagado donde realizo la Jugada."
                };
            };

            if (BaseViewModel.ComparaPinGanador(pReciboId, pPin))
            {
                var reciboPago = DataAccess.EFRepositories.RFRepositories.RFPagosRepository.RealizaPago(pReciboId, pBancaID);
                var printData = PrintJobs.Pega4PrintJob.ImprimirReciboPago(pDireccion, pBanca, pPrintW, pReciboId, reciboPago);
                return new
                {
                    OK = true,
                    Mensaje = "Pago Realizado Exitosamente",
                    PrintData = printData
                };
            }
            else
            {
                return new
                {
                    OK = false,
                    Mensaje = "El Pin es incorrecto."
                };
            }
        }
    }
}
