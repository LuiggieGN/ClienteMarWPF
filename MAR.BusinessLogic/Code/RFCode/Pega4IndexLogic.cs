using MAR.DataAccess.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAR.BusinessLogic.Code.RFCode
{
    public class Pega4IndexLogic
    {

        public static object GetTicketDetalle(object pTransaccionId, int pBancaId)
        {
            var transaccion = DataAccess.EFRepositories.CLRepositories.CLReciboRepository.Get_Recibos(pTransaccionId, 0).FirstOrDefault();  //.CLReciboRepository.GetRecibos(x => x.ReciboID == pTransaccionId, includeProperties: "RF_Transacciones,RF_Transacciones.RFTransaccionJugadas").FirstOrDefault();
            List<DataAccess.Tables.DTOs.CL_Recibo> transacciones = new List<DataAccess.Tables.DTOs.CL_Recibo>();
            transacciones.Add(transaccion);
            var transViewModel = Models.Mappers.Pega4Mapper.MapRecibos(transacciones, true).FirstOrDefault();
            return new
            {
                OK = true,
                Ticket = transViewModel
            };
        }

        public static object GetSorteoTiposJugada()
        {
            var sorteosTiposJugadas = DataAccess.EFRepositories.RFRepositories.RFSorteoRepository.GetRFSorteoTiposJugada();
            return new
            {
                OK = true,
                TiposJugadas = sorteosTiposJugadas
            };
        }
        public static object GetSorteoTiposJugadaPega4()
        {
            var sorteosTiposJugadas = DataAccess.EFRepositories.RFRepositories.RFSorteoRepository.GetRFSorteoTiposJugadaPega4();
            return new
            {
                OK = true,
                TiposJugadas = sorteosTiposJugadas
            };
        }
        public static object GetSorteosPega4(int pBancaId)
        {
            List<DataAccess.Tables.Enums.DbEnums.SorteoReferencia> sorteosRef = new List<DataAccess.Tables.Enums.DbEnums.SorteoReferencia>();
            sorteosRef.Add(DataAccess.Tables.Enums.DbEnums.SorteoReferencia.Pega3Dia);
            sorteosRef.Add(DataAccess.Tables.Enums.DbEnums.SorteoReferencia.Pega3Noche);
            sorteosRef.Add(DataAccess.Tables.Enums.DbEnums.SorteoReferencia.Pega4Dia);
            sorteosRef.Add(DataAccess.Tables.Enums.DbEnums.SorteoReferencia.Pega4Noche);
            var result = DataAccess.EFRepositories.RFRepositories.RFSorteoRepository.GetRFSorteosDia(sorteosRef, pBancaId);
            return new
            {
                OK = true,
                Resultado = result
            };
        }
        public static object AnulaRFTransaccion(int pTransaccionId, string pPin)
        {
            try
            {
                if (BaseViewModel.ComparaPinGanador(pTransaccionId, pPin))
                {
                    var transaccionAnulada = DataAccess.EFRepositories.RFRepositories.RFTransaccionRepository.Anula_RfTransaccion(pTransaccionId, "Anulado por cliente", null);
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

        public static object RealizaVenta(List<Models.RequestModel.Pega4RequestModel.RFTransaccionRequestModel.RFTransacciones> pRfTransaccioneses, int pBancaID, int pUsuarioID, int pPrintW, string pBanca, string pDireccion, string pFood, string pHead)
        {
            try
            {
                
                foreach (var item in pRfTransaccioneses)   //***************VALIDA LOS LIMITES*****************
                {
                    var limite = DataAccess.EFRepositories.RFRepositories.RFLimitesRepository.ValidaLimites(item.SorteoDiaID, item.TransaccionJugadas.Select(x => x.Aposto).FirstOrDefault());
                    if (!limite.Valido)
                    {
                        return new
                        {
                            OK = false,
                            Mensaje = "Paso el Limite " + limite.TipoLimite + " de Ventas para el Sorteo "  + limite.Sorteo + ", Solo puede vender " + Math.Round(limite.PuedeVender,0)
                        };
                    }
                }
             
                var transaccionesValidas = new List<Models.RequestModel.Pega4RequestModel.RFTransaccionRequestModel.RFTransacciones>();
                DateTime fechaRegistro = DateTime.Now;
                int[] SorteosDiaAvalidar = pRfTransaccioneses.Select(x => x.SorteoDiaID).Distinct().ToArray();
                var sorteosValidos = DataAccess.EFRepositories.RFRepositories.RFSorteoRepository.ValidarRFSorteosPorID(SorteosDiaAvalidar).Select(x => x.SorteoDiaID); // CAMBIAR A DAPPER

                if (sorteosValidos.Any())
                {
                      transaccionesValidas =  pRfTransaccioneses.Where(x => sorteosValidos.Contains(x.SorteoDiaID)).ToList();
                }
                else
                {
                    return new
                    {
                        OK = false,
                        Mensaje = "Los Sorteos no Disponibles"
                    };
                }

                List<DataAccess.Tables.DTOs.RF_Transaccion> transacciones = new List<DataAccess.Tables.DTOs.RF_Transaccion>();

                for (int i = 0; i < transaccionesValidas.Count; i++)
                {
                    transaccionesValidas[i].FechaIngreso = fechaRegistro;
                    transaccionesValidas[i].Estado = DataAccess.Tables.Enums.DbEnums.RFTransaccionEstado.Exitosa.ToString();
                    transaccionesValidas[i].Ingresos = transaccionesValidas[i].TransaccionJugadas.Sum(x => x.Aposto);
                }

                var transaccionViewModel = Models.Mappers.Pega4Mapper.MapRfTransaccion(transaccionesValidas);
 

                var recibo = DataAccess.EFRepositories.CLRepositories.CLReciboRepository.AgregarCL_Recibo(new DataAccess.Tables.DTOs.CL_Recibo
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
                },"A",true);

                transacciones.AddRange(recibo.RF_Transacciones);
               List<DataAccess.Tables.DTOs.CL_Recibo> recibos = new List<DataAccess.Tables.DTOs.CL_Recibo>();
                recibos.Add(recibo);
             
                var transViewModel = Models.Mappers.Pega4Mapper.MapRecibos(recibos, true).FirstOrDefault();
                string firma = GeneraFirma(recibo.Fecha.ToShortDateString(), recibo.Fecha.ToString("t"), recibo.Referencia, recibo.RF_Transacciones.ToList());
                var printData = PrintJobs.Pega4PrintJob.ImprimirPaga4(pPrintW, pBanca, pDireccion, recibo.Referencia, BaseViewModel.GeneraPinGanador(recibo.ReciboID),false, transViewModel, pFood, pHead, pBancaID, firma);
                return new
                {
                    OK = true,
                    PrintData = printData,
                    Transaccion = transViewModel
                };
            }
            catch (Exception e)
            {
                return new { OK = false };
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


        public static object GetTicket_VendidosHoy(int pRiferoId, int pBancaId)
        {



            DateTime desde = DateTime.Today;
            DateTime hasta = DateTime.Today.AddDays(1);
            try
            {

                var transacciones = DataAccess.EFRepositories.CLRepositories.CLReciboRepository.Get_Recibos(null, pBancaId); //.GetRecibos(null, includeProperties: "RF_Transacciones.RFTransaccionJugadas");
                var transViewModel = Models.Mappers.Pega4Mapper.MapRecibos(transacciones, true).OrderByDescending(x => x.TicketID);

                return new { OK = true, Err = string.Empty, Tickets = transViewModel };
            }
            catch (Exception e)
            {
                return new
                {
                    OK = false,
                    Mensaje = "Fallo la transaccion intente mas tarde",
                    Err = e.ToString()
                };
            }
        }

        public static object ReimprimirTicket(int theTransId, int pUsuarioID, int pPrintW, string pBanca, string pDireccion, string pFood, string pHead, int pBancaId)
        {


            var recibo = DataAccess.EFRepositories.CLRepositories.CLReciboRepository.Get_Recibos(theTransId, pBancaId).FirstOrDefault();
            var recibos = new List<DataAccess.Tables.DTOs.CL_Recibo>();
            recibos.Add(recibo);
            var transViewModel = Models.Mappers.Pega4Mapper.MapRecibos(recibos, true).FirstOrDefault();
            string firma = GeneraFirma(recibo.Fecha.ToShortDateString(), recibo.Fecha.ToString("t"), recibo.Referencia, recibo.RF_Transacciones.ToList());

            var printData = PrintJobs.Pega4PrintJob.ImprimirPaga4(pPrintW, pBanca, pDireccion, recibo.Referencia, BaseViewModel.GeneraPinGanador(recibo.ReciboID), false, transViewModel, pFood, pHead, recibo.BancaID, firma);

            return new
            {
                OK = true,
                PrintData = printData
            };
        }

        public static object ConsultaPagoTicket(int pReciboId, string pPin)
        {
            if (BaseViewModel.ComparaPinGanador(pReciboId, pPin))
            {
                var consultaPago = DataAccess.EFRepositories.RFRepositories.RFPagosRepository.ConsultarPago(pReciboId);
                return new
                {
                    OK = true,
                    ConsultaPago = consultaPago
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
