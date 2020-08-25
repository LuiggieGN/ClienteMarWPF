using System;
using System.Globalization;
using System.Linq;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using MAR.BusinessLogic.Flujo.WebClient.Code;
using Flujo.DataAccess.FlujoRepositories.WebClient.DapperRepositories;
using Flujo.Entities.WebClient.POCO;
using Flujo.Entities.WebClient.RequestModels;
using Flujo.Entities.WebClient.ReponseModels;

namespace MAR.BusinessLogic.Flujo.WebClient.Code.Validations
{
    public static class MovimientoValidacion
    {

        public static MovimientoValidacionResponse GetErrors(MovimientoRequestModel model)
        {
            MovimientoValidacionResponse validacionResponse = new MovimientoValidacionResponse() { Errors = new List<FormMovimientoError>(), Request = model };

            #region Caja Origen
            if (validacionResponse.Request.JsonCajaDesde == null || Regex.Replace(validacionResponse.Request.JsonCajaDesde, @"\s+", "").Equals(string.Empty))
            {
                validacionResponse.Errors.Add(new FormMovimientoError() { __Control__KEY = "#contenedorInputCajaDesde", __Message = "Especificar caja de origen." });
            }
            else
            {
                try
                {
                    Caja cajaorigen = JsonConvert.DeserializeObject<Caja>(validacionResponse.Request.JsonCajaDesde);
                    bool estaDisponibleCajaOrigen = CajaLogic.VerificarDisponibilidadDeCaja(cajaorigen.CajaID);

                    if (!estaDisponibleCajaOrigen)
                    {
                        validacionResponse.Errors.Add(new FormMovimientoError() { __Control__KEY = "#contenedorInputCajaDesde", __Message = validacionResponse.Request.CategoriaDesde == 1 ? "Banca en proceso de cuadre. Caja no disponible" : "Caja No Disponible" });
                    }

                    validacionResponse.Request.CajaDesde = cajaorigen;
                }
                catch (Exception ex)
                {
                    validacionResponse.Request.CajaDesde = null;
                    validacionResponse.Errors.Add(new FormMovimientoError() { __Control__KEY = "#contenedorInputCajaDesde", __Message = "Caja de origen inválida." });
                }
            }
            #endregion

            #region Caja Destino
            if (validacionResponse.Request.JsonCajaHasta == null || Regex.Replace(validacionResponse.Request.JsonCajaHasta, @"\s+", "").Equals(string.Empty))
            {
                validacionResponse.Errors.Add(new FormMovimientoError() { __Control__KEY = "#contenedorInputCajaHasta", __Message = "Especificar caja de destino." });
            }
            else
            {
                try
                {
                    Caja cajadestino = JsonConvert.DeserializeObject<Caja>(validacionResponse.Request.JsonCajaHasta);
                    bool estaDisponibleCajaDestino = CajaLogic.VerificarDisponibilidadDeCaja(cajadestino.CajaID);
                    if (!estaDisponibleCajaDestino)
                    {
                        validacionResponse.Errors.Add(new FormMovimientoError() { __Control__KEY = "#contenedorInputCajaHasta", __Message = validacionResponse.Request.CategoriaHasta == 1 ? "Banca en proceso de cuadre. Caja no disponible" : "Caja No Disponible" });
                    }
                    validacionResponse.Request.CajaHasta = cajadestino;
                }
                catch (Exception ex)
                {
                    validacionResponse.Request.CajaHasta = null;
                    validacionResponse.Errors.Add(new FormMovimientoError() { __Control__KEY = "#contenedorInputCajaHasta", __Message = "Caja de destino inválida." });
                }
            }
            #endregion

            #region Input Descripcion
            if (validacionResponse.Request.Descripcion == null || Regex.Replace(validacionResponse.Request.Descripcion, @"\s+", "").Equals(string.Empty))
            {
                validacionResponse.Errors.Add(new FormMovimientoError() { __Control__KEY = "#contenedorInputDescripcion", __Message = "Especificar descripción." });
            }
            #endregion

            #region Monto
            if (validacionResponse.Request.strMonto == null || Regex.Replace(validacionResponse.Request.strMonto, @"\s+", "").Equals(string.Empty))
            {
                validacionResponse.Errors.Add(new FormMovimientoError() { __Control__KEY = "#contenedorInputMonto", __Message = "Especificar monto." });
            }
            else
            {
                decimal monto;

                bool elMontoEsValido = decimal.TryParse(validacionResponse.Request.strMonto, NumberStyles.Any, new CultureInfo("en-US"), out monto);
                if (!elMontoEsValido)
                {
                    validacionResponse.Errors.Add(new FormMovimientoError() { __Control__KEY = "#contenedorInputMonto", __Message = "Monto Inválido." });
                    validacionResponse.Request.Monto = 0;
                }
                else
                {
                    validacionResponse.Request.Monto = monto;
                }
            }
            #endregion


            if (!validacionResponse.Errors.Any())
            {
                decimal balanceCajaOrigen = CajaLogic.GetBalance(validacionResponse.Request.CajaDesde.CajaID);

                if ((balanceCajaOrigen - validacionResponse.Request.Monto) < 0)
                {
                    validacionResponse.Errors.Add(new FormMovimientoError() { __Control__KEY = "#contenedorInputMonto", __Message = $"El monto ({validacionResponse.Request.Monto.ToString("$#,##0.00", CultureInfo.CreateSpecificCulture("en-US"))}) excede el balance de la Caja Origen ({balanceCajaOrigen.ToString("$#,##0.00", CultureInfo.CreateSpecificCulture("en-US"))})" });
                }

            }



            return validacionResponse;
        }












        public static List<FormMovimientoError> GetFormMovimientosRegisterErrors(string pCategoria, string pEntidad, string pTipoFlujo, string pObjeto_TipoIngreso_TipoEgreso_Seleccionado, string pMonto)
        {
            List<FormMovimientoError> Errores = new List<FormMovimientoError>();


            //... Categoria |< Banca >| |< Usuario >|
            if (!(pCategoria.Equals("Banca") || pCategoria.Equals("Usuario"))) Errores.Add(new FormMovimientoError() { __Control__KEY = "#SelectCategoriaContainer", __Message = "Categoría Inválida." });


            if (pCategoria.Equals("Banca"))
            {

                //$$ pEntidad  :: Banca

                #region pEntidad ::

                bool IsInvalid_pEntidad = (pEntidad == null || Regex.Replace(pEntidad, @"\s+", "").Trim().Equals(string.Empty)) ? true : false;

                BancaBalance SubmittedBanca = null;

                try
                {
                    SubmittedBanca = JsonConvert.DeserializeObject<BancaBalance>(pEntidad); // Banca que fue enviada
                }
                catch (Exception ex)
                {
                    IsInvalid_pEntidad = true;
                }

                if (IsInvalid_pEntidad)
                {
                    Errores.Add(new FormMovimientoError() { __Control__KEY = "#Typed_Banca", __Message = "Seleccione una Banca." });
                }
                else
                {
                    bool BancaEnviadaEstaDisponible = CajaLogic.VerificarDisponibilidadDeCaja(SubmittedBanca.CajaID); //BancaLogic.BancaYCajaSonValidos(SubmittedBanca.BancaID, SubmittedBanca.CajaID);

                    if (!BancaEnviadaEstaDisponible)
                    {
                        Errores.Add(new FormMovimientoError() { __Control__KEY = "#Typed_Banca", __Message = "Banca en proceso de cadre. No disponible" });
                    }
                }

                #endregion

            }

            if (pCategoria.Equals("Usuario"))
            {
                #region pEntidad ::

                bool IsInvalid_pEntidad = (pEntidad == null || Regex.Replace(pEntidad, @"\s+", "").Trim().Equals(string.Empty)) ? true : false;

                ConsultaUsuarioBalance SubmittedUsuario = null;

                try
                {
                    SubmittedUsuario = JsonConvert.DeserializeObject<ConsultaUsuarioBalance>(pEntidad);
                }
                catch (Exception ex)
                {
                    IsInvalid_pEntidad = true;
                }

                if (IsInvalid_pEntidad)
                {
                    Errores.Add(new FormMovimientoError() { __Control__KEY = "#Typed_Usuario", __Message = "Seleccione un usuario." });
                }
                else
                {

                    Caja UsuarioCaja = CajaLogic.GetCajaVirtual(SubmittedUsuario.Posicion);

                    if (UsuarioCaja == null) Errores.Add(new FormMovimientoError() { __Control__KEY = "#Typed_Usuario", __Message = "Error usuario especificado." });
                }

                #endregion                
            }



            #region Validaciones Comunes

            #region pTipoFlujo ::

            if (
                  !(pTipoFlujo.Equals("Entrada") || pTipoFlujo.Equals("Salida"))
            )
            {
                Errores.Add(new FormMovimientoError() { __Control__KEY = "#TipoFlujoSeleccionado", __Message = "Error en acción especificada." });
            }
            else
            {
                bool Is_InvalidTipoIngresoOTipoEgreso = (pObjeto_TipoIngreso_TipoEgreso_Seleccionado == null || Regex.Replace(pObjeto_TipoIngreso_TipoEgreso_Seleccionado, @"\s+", "").Trim().Equals(string.Empty)) ? true : false;

                FlujoTipoCategoria SubmitteTipoIngresoOTipoEgreso = null;

                try
                {
                    SubmitteTipoIngresoOTipoEgreso = JsonConvert.DeserializeObject<FlujoTipoCategoria>(pObjeto_TipoIngreso_TipoEgreso_Seleccionado);
                }
                catch (Exception ex)
                {
                    Is_InvalidTipoIngresoOTipoEgreso = true;
                }

                if (Is_InvalidTipoIngresoOTipoEgreso)
                {
                    if (pTipoFlujo.Equals("Entrada"))
                    {
                        Errores.Add(new FormMovimientoError() { __Control__KEY = "#SelIngreso", __Message = "Seleccionar un tipo de ingreso" });
                    }

                    if (pTipoFlujo.Equals("Salida"))
                    {
                        Errores.Add(new FormMovimientoError() { __Control__KEY = "#SelEgreso", __Message = "Seleccionar un tipo de egreso." });
                    }
                }
                else
                {
                    List<FlujoTipoCategoria> laListaDeIngresos = FlujoTipoRepositorio.GetTiposDeIngreso();

                    List<FlujoTipoCategoria> laListaDeEgresos = FlujoTipoRepositorio.GetTiposDeEgresos();

                    if (pTipoFlujo.Equals("Entrada"))
                    {
                        if (
                             !(laListaDeIngresos.Any(b => b.Posicion == SubmitteTipoIngresoOTipoEgreso.Posicion))
                           )
                        {
                            Errores.Add(new FormMovimientoError() { __Control__KEY = "#SelIngreso", __Message = "Tipo de Ingreso seleccionado inválido." });
                        }
                    }

                    if (pTipoFlujo.Equals("Salida"))
                    {
                        if (
                             !(laListaDeEgresos.Any(b => b.Posicion == SubmitteTipoIngresoOTipoEgreso.Posicion))
                           )
                        {
                            Errores.Add(new FormMovimientoError() { __Control__KEY = "#SelEgreso", __Message = "Tipo de Egreso seleccionado inválido." });
                        }

                    }
                }


            }

            #endregion


            #region pMonto ::

            decimal n;

            bool isNumeric = decimal.TryParse(pMonto, out n);


            if (!isNumeric)
            {
                Errores.Add(new FormMovimientoError() { __Control__KEY = "#GroupTextBox", __Message = "Monto especificado inválido." });
            }


            #endregion


            #endregion




            return Errores;


        } // GetFormMovimientosRegisterErrors() ~

    } // MovimientoValidacion() ~ 


}