using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using Flujo.Entities.WebClient.RequestModels;
using Flujo.Entities.WebClient.POCO;
using Flujo.Entities.WebClient.Enums;
using Flujo.DataAccess.FlujoRepositories.WebClient.DapperRepositories;
using System.Text.RegularExpressions;

namespace MAR.BusinessLogic.Flujo.WebClient.Code
{
    public static class MovimientoLogic
    {
        public static MovimientoInsertEstado InsertarMovimiento(MovimientoRequestModel peticion, Usuario usuario)
        {
            try
            {
                MovimientoInsertEstado estado = MovimientoRepositorio.CrearMovimiento(peticion, usuario);
                return estado;
            }
            catch (Exception ex)
            {
                MovimientoInsertEstado errorExcepcion = new MovimientoInsertEstado();
                errorExcepcion.FechaCreacion = DateTime.Now;
                errorExcepcion.MensajeError = ex.Message + " " + ex.StackTrace;
                errorExcepcion.MovimientoFueProcesado = false;
                errorExcepcion.MovimientoCajaOrigen = (int?)null;
                errorExcepcion.MovimientoCajaDestino = (int?)null;
                return errorExcepcion;
            }
        }

        /// <summary>
        ///     Registro los movimiento de entrada y salida entre @CajaVIrtual {Caja de Usuario} y  @CajaTerimanl {Caja de Banca}
        /// </summary>
        /// <param name="pTipoFlujo"> ( Entrada ) || ( Salida )</param>
        /// <param name="pTipoIngresoOTipoEgresoStrJson"> (TipoIngreso) || (TipoEgreso) seleccionado</param>
        /// <param name="pComentario">Comentario del movimiento</param>
        /// <param name="pMonto">Monto de la transaccion de movimiento a realizar</param>
        /// <param name="pCurrentUserID">Usuario que realiza el movimeinto</param>
        /// <param name="pSumittedBancaStrJson">Objeto Banca</param>
        /// <returns>Retorna la confirmacion del si el movimiento fue registrado</returns>
        /// 
        public static bool Mov_Usuario_Y_Banca(string pTipoFlujo, string pTipoIngresoOTipoEgresoStrJson, string pComentario, decimal pMonto, int pCurrentUserID, string pSumittedBancaStrJson)
        {
            try
            {
                List<FlujoTipoCategoria> laListaDeIngresos = FlujoTipoRepositorio.GetTiposDeIngreso();
                List<FlujoTipoCategoria> laListaDeEgresos = FlujoTipoRepositorio.GetTiposDeEgresos();

                BancaBalance SubmittedBanca = JsonConvert.DeserializeObject<BancaBalance>(pSumittedBancaStrJson); // Banca que fue enviada

                FlujoTipoCategoria pTipoFlujoSubTipos = JsonConvert.DeserializeObject<FlujoTipoCategoria>(pTipoIngresoOTipoEgresoStrJson); // TipoIngreso o TipoEgreso

                Caja BancaCaja = new Caja() { CajaID = SubmittedBanca.CajaID, BancaID = SubmittedBanca.BancaID };  //Caja Destino

                Caja UsuarioCaja = CajaLogic.GetCajaVirtual(pCurrentUserID); // Caja Origen

                if (BancaCaja == null || UsuarioCaja == null)
                {
                    return false;
                }

                // $$ Caja Usuario As [ Caja Origen + ] -> Caja Banca As [ Caja Destino - ]

                if (pTipoFlujo.Equals("Entrada"))
                {


                    FlujoTipoCategoria flujoTipoOrigen = laListaDeEgresos.Where(x => x.LogicaKey != null && x.LogicaKey == (int)TipoFlujoSubCategorias.SalidaDeCuadre).FirstOrDefault();
                    Usuario user = UsuarioLogic.GetUsuario(pCurrentUserID);

                    bool MovOrigenCompletado = CreaMovimiento(UsuarioCaja, "Entrada", pTipoFlujoSubTipos, pMonto, $" ({pTipoFlujoSubTipos.FlujoTipoNombre}) desde Banca : " + SubmittedBanca.Banca + ". " + pComentario, UsuarioCaja);

                    bool MovDestinoCompletado = CreaMovimiento(BancaCaja, "Salida", flujoTipoOrigen, pMonto, flujoTipoOrigen.FlujoTipoNombre + ". Por Usuario : " + user.Nombre, UsuarioCaja);  // Entrada Salida  flujoTipoOrigen pTipoFlujoSubTipos

                    return (MovOrigenCompletado && MovDestinoCompletado);


                }
                else if (pTipoFlujo.Equals("Salida"))
                {
                    //$$ Salida aplicadas a ( >.. Bancas ..<)
                    //$$ | < Usuario > Caja Origen ( - )  | < Banca > Caja Destino ( + )

                    FlujoTipoCategoria flujoTipoOrigen = laListaDeIngresos.Where(x => x.LogicaKey != null && x.LogicaKey == (int)TipoFlujoSubCategorias.DepositoDeCuadre).FirstOrDefault();
                    Usuario user = UsuarioLogic.GetUsuario(pCurrentUserID);

                    bool MovOrigenCompletado = CreaMovimiento(UsuarioCaja, "Salida", pTipoFlujoSubTipos, pMonto, $" ({pTipoFlujoSubTipos.FlujoTipoNombre}). Entrega a Banca : " + SubmittedBanca.Banca + ". " + pComentario, UsuarioCaja);

                    bool MovDestinoCompletado = CreaMovimiento(BancaCaja, "Entrada", flujoTipoOrigen, pMonto, flujoTipoOrigen.FlujoTipoNombre + ". Por Usuario : " + user.Nombre, UsuarioCaja);  // Entrada Salida flujoTipoOrigen pTipoFlujoSubTipos

                    return (MovOrigenCompletado && MovDestinoCompletado);

                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public static bool MovDe_Usuario_Y_Usuario(string pTipoFlujo, string pTipoIngresoOTipoEgresoStrJson, string pComentario, decimal pMonto, int pCurrentUserID, string pSumittedUsuarioStrJson, int? pCajaRefID = null)
        {
            try
            {

                // TipoIngreso o TipoEgreso
                FlujoTipoCategoria pTipoFlujoSubTipos = JsonConvert.DeserializeObject<FlujoTipoCategoria>(pTipoIngresoOTipoEgresoStrJson);

                //Submitted Usuario Caja Data
                ConsultaUsuarioBalance SubmittedUsuario = JsonConvert.DeserializeObject<ConsultaUsuarioBalance>(pSumittedUsuarioStrJson);


                //Inicializacion de Cajas
                Caja CajaOrigenUsuarioLogged = CajaLogic.GetCajaVirtual(pCurrentUserID);
                Caja CajaDestinoUsuarioSumitted = new Caja() { CajaID = SubmittedUsuario.CajaID, UsuarioID = SubmittedUsuario.Posicion };


                //Valido si las cajas fueron inicializadas
                if (CajaOrigenUsuarioLogged == null || CajaDestinoUsuarioSumitted == null)
                {
                    return false;
                }

                if (CajaOrigenUsuarioLogged.UsuarioID == CajaDestinoUsuarioSumitted.UsuarioID)
                {
                    //Este bloque se codigo es aplicado cuando los movimientos de Ingreso o Egreso son aplicados en la mismas cuenta del usuario logueado
                    bool MovFueRegistrado;


                    string pBuildComment = Regex.Replace(pComentario ?? "", @"\s+", "");


                    if (pTipoFlujo.Equals("Entrada"))
                    {
                        MovFueRegistrado = CreaMovimiento(CajaOrigenUsuarioLogged, "Entrada", pTipoFlujoSubTipos, pMonto, (pBuildComment == string.Empty ? pTipoFlujoSubTipos.FlujoTipoNombre : pComentario), CajaOrigenUsuarioLogged, pCajaRefID);

                        return MovFueRegistrado;
                    }
                    else
                    {
                        MovFueRegistrado = CreaMovimiento(CajaOrigenUsuarioLogged, "Salida", pTipoFlujoSubTipos, pMonto, (pBuildComment == string.Empty ? pTipoFlujoSubTipos.FlujoTipoNombre : pComentario), CajaOrigenUsuarioLogged, pCajaRefID);

                        return MovFueRegistrado;
                    }
                }

                List<FlujoTipoCategoria> laListaDeIngresos = FlujoTipoRepositorio.GetTiposDeIngreso();
                List<FlujoTipoCategoria> laListaDeEgresos = FlujoTipoRepositorio.GetTiposDeEgresos();

                // $$ CajaOrigenUsuarioLogged As [ Caja Origen ] -> CajaDestinoUsuarioSumitted As [ Caja Destino ]

                if (pTipoFlujo.Equals("Entrada"))
                {
                    //$$ Entradas a ( >.. UsuarioSumitted ..<)
                    //$$ | < Usuario > Caja Origen ( + )  | < Banca > Caja Destino ( - )

                    FlujoTipoCategoria flujoTipoOrigen = laListaDeEgresos.Where(x => x.LogicaKey != null && x.LogicaKey == (int)TipoFlujoSubCategorias.SalidaDeCuadre).FirstOrDefault();

                    Usuario user = UsuarioLogic.GetUsuario(pCurrentUserID);

                    bool MovOrigenCompletado = CreaMovimiento(CajaOrigenUsuarioLogged, "Entrada", pTipoFlujoSubTipos, pMonto, $" ({pTipoFlujoSubTipos.FlujoTipoNombre}) por usuario : " + SubmittedUsuario.Nombre + ". " + pComentario, CajaOrigenUsuarioLogged, null);
                    bool MovDestinoCompletado = CreaMovimiento(CajaDestinoUsuarioSumitted, "Salida", flujoTipoOrigen, pMonto, flujoTipoOrigen.FlujoTipoNombre + ". Por Usuario : " + user.Nombre, CajaOrigenUsuarioLogged, null);

                    return (MovOrigenCompletado && MovDestinoCompletado);

                }
                else if (pTipoFlujo.Equals("Salida"))
                {
                    //$$ Salida aplicadas a ( >.. UsuarioSumitted ..<)
                    //$$ | < Usuario > Caja Origen ( - )  | < Banca > Caja Destino ( + )

                    FlujoTipoCategoria flujoTipoOrigen = laListaDeIngresos.Where(x => x.LogicaKey != null && x.LogicaKey == (int)TipoFlujoSubCategorias.DepositoDeCuadre).FirstOrDefault();
                    Usuario user = UsuarioLogic.GetUsuario(pCurrentUserID);

                    bool MovOrigenCompletado = CreaMovimiento(CajaOrigenUsuarioLogged, "Salida", pTipoFlujoSubTipos, pMonto, $" ({pTipoFlujoSubTipos.FlujoTipoNombre}). Entrega a usuario : " + SubmittedUsuario.Nombre + ". " + pComentario, CajaOrigenUsuarioLogged, null);

                    bool MovDestinoCompletado = CreaMovimiento(CajaDestinoUsuarioSumitted, "Entrada", flujoTipoOrigen, pMonto, flujoTipoOrigen.FlujoTipoNombre + ". Por Usuario : " + user.Nombre, CajaOrigenUsuarioLogged, null);


                    return (MovOrigenCompletado && MovDestinoCompletado);

                }
                else
                {
                    return false;
                }



            }
            catch (Exception ex)
            {
                return false;
            }

        }

        private static bool CreaMovimiento(Caja pCajaDondeSeRealizaMovimiento, string pEntradaOSalida, FlujoTipoCategoria pTipoEntradaOTipoSalida, decimal pMonto, string pDescripcion, Caja pCajaQueRealizaMovimiento, int? pCajaRefID = null)
        {
            bool MovimientoFueRegistrado = MovimientoRepositorio.AplicarMovimientoACaja(
                pCajaDondeSeRealizaMovimiento,
                pEntradaOSalida,
                pTipoEntradaOTipoSalida,
                pMonto,
                pDescripcion,
                pCajaQueRealizaMovimiento,
                pCajaRefID
            );

            return MovimientoFueRegistrado;
        }


        public static List<MovimientoDatos> LastCajaXRecords(int pCajaID, int NoRecords)
        {
            List<MovimientoDatos> movs = MovimientoRepositorio.ConsultarLastXRecords(pCajaID, NoRecords);

            //if (movs != null && movs.Any())
            //{
            //    movs.Reverse();

            //    for (int i = 0; i < movs.Count; i++)
            //    {

            //        movs[i].Orden = i + 1;

            //    }

            //}


            return movs;
        }


    }
}