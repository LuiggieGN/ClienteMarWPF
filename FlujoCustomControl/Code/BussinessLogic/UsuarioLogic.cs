using System;
using System.Collections.Generic;
using Flujo.Entities.WpfClient.POCO;
using Flujo.Entities.WpfClient.Enums;
using Flujo.Entities.WpfClient.ResponseModels;
using FlujoCustomControl.Code.AppEnums;
using FlujoCustomControl.Views;
using Newtonsoft.Json;

namespace FlujoCustomControl.Code.BussinessLogic
{
    public static class UsuarioLogic
    {
        public static LocalBL lcvr = new LocalBL();
        private static FlujoServices.mar_flujoSoapClient flujoSvr = lcvr.GetFlujoServiceClient(false);


        public static MUsuario GetFirstSurperUsuario()
        {
            try
            {
                var consulta = flujoSvr.CallFlujoIndexFunction((int)RoutingFunctions.GetFirstSurperUsuario, MainFlujoWindows.MarSession, null);

                if (consulta.OK == false || consulta.Respuesta == null || consulta.Respuesta.Equals("null"))
                {
                    throw new Exception("Ha ocurrido un error al obtener el super usuario");
                }

                MUsuario responsable = JsonConvert.DeserializeObject<MUsuario>(consulta.Respuesta);
                return responsable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static MUsuario GetUsuarioByPin(string pin) 
        {
            try
            {
                FlujoServices.ArrayOfAnyType parametros = new FlujoServices.ArrayOfAnyType();
                parametros.Add(pin);

                FlujoServices.MAR_FlujoResponse consulta = flujoSvr.CallFlujoIndexFunction((int)RoutingFunctions.GetMUsuarioByPin, MainFlujoWindows.MarSession, parametros);

                if (consulta.OK == false || consulta.Respuesta == null || consulta.Respuesta.Equals("null"))
                {
                    return null;
                }

                MUsuario usuario = JsonConvert.DeserializeObject<MUsuario>(consulta.Respuesta);

                return usuario;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static RutaAsignacion GetGestorAsignacionPendiente(int gestorId, int bancaIdQueGestorTransita)
        {
            try
            {
                FlujoServices.ArrayOfAnyType parametros = new FlujoServices.ArrayOfAnyType();
                parametros.Add(gestorId);
                parametros.Add(bancaIdQueGestorTransita);

                FlujoServices.MAR_FlujoResponse consulta = flujoSvr.CallFlujoIndexFunction((int)RoutingFunctions.GetGestorAsignacionPendiente, MainFlujoWindows.MarSession, parametros);

                if (consulta.OK == false )
                {
                    throw new Exception("Ha occurrido un error en obtener la asignacion");                  
                }

                if (consulta.Respuesta == null || consulta.Respuesta.Equals("null"))
                {
                    return null;
                }

                RutaAsignacion asignacion = JsonConvert.DeserializeObject<RutaAsignacion>(consulta.Respuesta);

                return asignacion;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static UsuarioTarjetaClave GetUsuarioTarjeta(int usuarioId)
        {
            try
            {
                FlujoServices.ArrayOfAnyType parametros = new FlujoServices.ArrayOfAnyType();
                parametros.Add(usuarioId);

                FlujoServices.MAR_FlujoResponse consulta = flujoSvr.CallFlujoIndexFunction((int)RoutingFunctions.GetUsuarioTarjeta, MainFlujoWindows.MarSession, parametros);

                if (consulta.OK == false)
                {
                    throw new Exception("Ha occurrido un error al obtener la tarjeta");
                }

                if (consulta.Respuesta == null || consulta.Respuesta.Equals("null"))
                {
                    return null;
                }

                UsuarioTarjetaClave tarjeta = JsonConvert.DeserializeObject<UsuarioTarjetaClave>(consulta.Respuesta);

                return tarjeta;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static TokenDeSeguridadResponseModel GetTarjetaTokenAleatorio(UsuarioTarjetaClave tarjeta) 
        {
            try
            {
                Random aleatorio = new Random();
                TokenDeSeguridadResponseModel tokenAleatorio = new TokenDeSeguridadResponseModel();

                int[][] valores = JsonConvert.DeserializeObject<int[][]>(tarjeta.Tokens);

                int posicion = 0;
                Dictionary<int, int> tokens = new Dictionary<int, int>();

                foreach (int[] tokensEnColumna in valores)
                {
                    foreach (int token in tokensEnColumna)
                    {
                        tokens.Add((++posicion), token);
                    }
                }

                int azar = aleatorio.Next(1, tokens.Count + 1);

                tokenAleatorio.TarjetaID = tarjeta.TarjetaID;
                tokenAleatorio.Posicion = azar;
                tokenAleatorio.Toquen = tokens[azar] + "";

                return tokenAleatorio;

            }
            catch (Exception ex)
            {
                throw ex;
            }        
        }
        public static Usuario GetUsuarioResponsableDeCaja (int pCajaID)
        {
            var jsonStr = JsonConvert.SerializeObject(pCajaID);
            FlujoServices.ArrayOfAnyType parametros = new FlujoServices.ArrayOfAnyType();
            parametros.Add(jsonStr);
            var pResponse = flujoSvr.CallFlujoIndexFunction(11, MainFlujoWindows.MarSession, parametros);
            Usuario responsable = JsonConvert.DeserializeObject<Usuario>(pResponse.Respuesta, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });
            return responsable;
        }
        public static UsuarioResponseModel BuscarUsuarioPorDocumento(string pin) //string pDocumento, UsuarioTiposDocumento pTipoDocumento
        {
            try
            {
                MUsuario musuario = UsuarioLogic.GetUsuarioByPin(pin);

                if (musuario == null)
                {
                    return null;
                }
                else
                {
                    UsuarioResponseModel response = new UsuarioResponseModel();
                    response.Activo = musuario.UsuActivo;
                    response.Documento = musuario.UsuCedula;
                    response.Nombre = musuario.UsuNombre + " " + musuario.UsuApellido;
                    response.TipoDocumentoID = 1;
                    response.TipoUsuarioID = musuario.TipoUsuarioID.HasValue ? musuario.TipoUsuarioID.Value : -1;
                    response.UsuarioID = musuario.UsuarioID;
                    response.ZonaID = 0;

                    return response;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
            //string param9 = JsonConvert.SerializeObject(pDocumento);
            //string param10 = JsonConvert.SerializeObject((int)pTipoDocumento);
            //FlujoServices.ArrayOfAnyType parametros = new FlujoServices.ArrayOfAnyType();
            //parametros.Add(param9);
            //parametros.Add(param10);

            //var pResponse = flujoSvr.CallFlujoIndexFunction(12, MainFlujoWindows.MarSession, parametros);
            //UsuarioResponseModel r = JsonConvert.DeserializeObject<UsuarioResponseModel>(pResponse.Respuesta, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });
        }

        public static UsuarioResponseModel BuscarUsuarioMensajeroDocumento(string pDocumento, UsuarioTiposDocumento pTipoDocumento)
        {
            string param9 = JsonConvert.SerializeObject(pDocumento);
            string param10 = JsonConvert.SerializeObject((int)pTipoDocumento);
            FlujoServices.ArrayOfAnyType parametros = new FlujoServices.ArrayOfAnyType(); 
            parametros.Add(param9);
            parametros.Add(param10);
            var pResponse = flujoSvr.CallFlujoIndexFunction(13, MainFlujoWindows.MarSession, parametros);
            UsuarioResponseModel r = JsonConvert.DeserializeObject<UsuarioResponseModel>(pResponse.Respuesta, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });
            return r;
    
        }




    }
}
