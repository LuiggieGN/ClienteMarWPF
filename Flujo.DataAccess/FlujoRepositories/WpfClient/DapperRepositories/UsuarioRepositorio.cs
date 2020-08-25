using System;
using Dapper;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using Flujo.Entities.WpfClient.POCO;
using Flujo.Entities.WpfClient.Enums; 
using Flujo.Entities.WpfClient.ResponseModels;
using MAR.AppLogic.MARHelpers;

namespace Flujo.DataAccess.FlujoRepositories.WpfClient.DapperRepositories
{
    public static class UsuarioRepositorio
    {
        public static MUsuario GetFirstSurperUsuario() 
        {
            try
            {
                MUsuario usuario = null;
 
                using (var con = DapperDBHelper.GetSqlConnection())
                {
                    con.Open();
                    usuario = con.Query<MUsuario>
                                       (
                                           @"                                           
                                               select top 1 
                                                  UsuarioID,     UsuNombre, UsuApellido,   UsuCedula,     UsuFechaNac,
                                                  UsuUserName,   UsuClave,  UsuVenceClave, UsuActivo,     UsuNivel,
                                                  UsuComentario, Email,     TipoUsuarioID, LoginFallidos, ToquenFallidos
                                               
                                                from dbo.MUsuarios where TipoUsuarioID = 0; 

                                           ",commandType: CommandType.Text).FirstOrDefault();

                    if (usuario == null)
                    {
                        throw new Exception("Error.no se pudo encontrar el Super Usuario");
                    }
                }

                return usuario;
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
                MUsuario usuario = null;
                DynamicParameters queryParametros = new DynamicParameters(); queryParametros.Add("@pin", pin);
                using (var con = DapperDBHelper.GetSqlConnection())
                {
                    con.Open();
                    usuario = con.Query<MUsuario>
                                       (
                                           @"                                           
                                              select 
                                                     UsuarioID,UsuNombre,UsuApellido, UsuCedula, UsuFechaNac
                                                    ,UsuUserName,UsuClave,UsuVenceClave,UsuActivo,UsuNivel
                                                    ,UsuComentario,Email,TipoUsuarioID,LoginFallidos,ToquenFallidos
                                                    ,UsuPin,UsuPuedeCuadrar,TipoAutenticacion
                                               from dbo.MUsuarios where UsuPin = @pin;
                                           ",queryParametros, commandType: CommandType.Text ).FirstOrDefault();

               
                    con.Close();
                }
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
                RutaAsignacion asignacion = null;
                DynamicParameters queryParametros = new DynamicParameters(); 
                queryParametros.Add("@usuarioid", gestorId);
                queryParametros.Add("@bancaId", bancaIdQueGestorTransita);

                using (var con = DapperDBHelper.GetSqlConnection())
                {
                    con.Open();
                    asignacion = con.Query<RutaAsignacion>("flujo.GetGestorAsignacionPendiente", queryParametros, commandType: CommandType.StoredProcedure).FirstOrDefault();


                    con.Close();
                }
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
                UsuarioTarjetaClave tarjeta = null;
                DynamicParameters queryParametros = new DynamicParameters();
                queryParametros.Add("@usuarioid", usuarioId);

                using (var con = DapperDBHelper.GetSqlConnection())
                {
                    con.Open();
                    tarjeta = con.Query<UsuarioTarjetaClave>
                                       (
                                           @"                                           
                                                 select TarjetaID 
                                                       ,UsuarioID 
                                                       ,Serial 
                                                       ,FechaCreacion 
                                                       ,Comentario 
                                                       ,Tokens 
                                                       ,Activo 
                                                  from flujo.UsuarioTarjetaClaves where UsuarioID = @usuarioid;
                                           ", queryParametros, commandType: CommandType.Text).FirstOrDefault();


                    con.Close();
                }
                return tarjeta;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        public static Usuario GetUsuarioResponsableDeCaja(int pCajaID)
        {
            try
            {
                DynamicParameters p = new DynamicParameters(); Usuario r = null;

                p.Add("@CajaID", pCajaID);

                string query = @"

                    select * from flujo.Usuario u where u.UsuarioID = (
                    
                       select top 1 c.UsuarioID from flujo.Caja c where c.CajaID = @CajaID  and c.TipoCajaID = (select  top 1 tc.TipoCajaID from flujo.TipoCaja tc where tc.Nombre = 'CAJA_VIRTUAL')
                    
                    )
                ";
                using (var con = DapperDBHelper.GetSqlConnection())
                {
                    con.Open();

                    List<Usuario> ColeccionUsuarios = con.Query<Usuario>(query, p, commandType: CommandType.Text).ToList();

                    if (ColeccionUsuarios.Any())
                    {
                        r = ColeccionUsuarios.First();
                    }

                    con.Close();
                }

                return r;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public static UsuarioResponseModel BuscarUsuarioPor(string pDocumento,  UsuarioTiposDocumento pTipoDocumento)
        {
            try
            {
                DynamicParameters p = new DynamicParameters();  UsuarioResponseModel r = null;

                p.Add("@Documento", pDocumento);
                p.Add("@TipoDocumento", (int)pTipoDocumento);

                string query = @"

                      select top 1 * from flujo.Usuario u Where u.Documento = @Documento and u.TipoDocumentoID = @TipoDocumento 

                ";
                using (var con = DapperDBHelper.GetSqlConnection())
                {
                    con.Open();

                    List<UsuarioResponseModel> ColeccionUsuarios = con.Query<UsuarioResponseModel>(query, p, commandType: CommandType.Text).ToList();

                    if (ColeccionUsuarios.Any())
                    {
                        r = ColeccionUsuarios.First();
                    }

                    con.Close();
                }

                return r;
            }
            catch (Exception ex)
            {
                return null;
            }

        }
        public static UsuarioResponseModel BuscarMensajeroUsuarioPor(string pDocumento, UsuarioTiposDocumento pTipoDocumento)
        {
            try
            {
                DynamicParameters p = new DynamicParameters(); UsuarioResponseModel r = null;

                p.Add("@Documento", pDocumento);
                p.Add("@TipoDocumento", (int)pTipoDocumento);

                string query = @"

                      select top 1 * from flujo.Usuario u Where u.Documento = @Documento and u.TipoDocumentoID = @TipoDocumento and u.TipoUsuarioID = (select top 1 tu.TipoUsuarioID from flujo.TipoUsuario tu where tu.Tipo = 'Mensajero')

                ";
                using (var con = DapperDBHelper.GetSqlConnection())
                {
                    con.Open();

                    List<UsuarioResponseModel> ColeccionUsuarios = con.Query<UsuarioResponseModel>(query, p, commandType: CommandType.Text).ToList();

                    if (ColeccionUsuarios.Any())
                    {
                        r = ColeccionUsuarios.First();
                    }

                    con.Close();
                }

                return r;
            }
            catch (Exception ex)
            {
                return null;
            }

        }
    }
}
