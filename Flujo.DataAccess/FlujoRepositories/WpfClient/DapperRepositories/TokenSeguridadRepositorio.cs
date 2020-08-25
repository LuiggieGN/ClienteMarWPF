using System;
using Dapper;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Flujo.Entities.WpfClient.Enums;
using Flujo.Entities.WpfClient.RequestModel;
using Flujo.Entities.WpfClient.ResponseModels;

using Flujo.DataAccess.FlujoRepositories.WpfClient.Helpers;
using MAR.AppLogic.MARHelpers;

namespace Flujo.DataAccess.FlujoRepositories.WpfClient.DapperRepositories
{
    public static class TokenSeguridadRepositorio
    {
        public static TokenDeSeguridadResponseModel ObtenerUnSoloTokenDeFormaAleatoria(int pUsuarioID)
        {
            try
            {
                TokenDeSeguridadResponseModel token = null;

                DynamicParameters p = new DynamicParameters();

                p.Add("@UsuarioID", pUsuarioID);

                string query = @"

                            select 
                             * from flujo.ToquenUsuario tu
                            where 
                              tu.TarjetaID =(        
                         		select top 1 ut.TarjetaID from flujo.UsuarioTarjeta ut where ut.UsuarioID = @UsuarioID and ut.Activo = 1	
                         	 )
                     and tu.Posicion = isnull
                                             (  
                         					  nullif(
                                                      floor
                                                      ( rand()*(  
                         							                (
                              select  isnull(count(*),0)+1 from flujo.ToquenUsuario tu  where tu.TarjetaID = ( select top 1 ut.TarjetaID from flujo.UsuarioTarjeta ut where ut.UsuarioID = @UsuarioID and ut.Activo = 1 )) -1
                         							                ) +1
                                                        ),
                         		   	                 0
                         					  ), 
                         	               1);
                ";

                using (var con = DapperDBHelper.GetSqlConnection())
                {
                    con.Open();

                    List<TokenDeSeguridadResponseModel> aux = con.Query<TokenDeSeguridadResponseModel>(query, p, commandType: CommandType.Text).ToList();

                    if (aux.Any())
                    {
                        token = aux.First();
                    }

                    con.Close();
                }
                
                return token;
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        public static IEnumerable<TokenDeSeguridadResponseModel> ConsultarTokensDeSeguridadPor(int pUsuarioID)
        {
            try
            {
                DynamicParameters p = new DynamicParameters(); List<TokenDeSeguridadResponseModel> ColeccionTokens = null;

                p.Add("@UsuarioID", pUsuarioID);

                string query = @"
                      select 
                       *   from flujo.ToquenUsuario tu
                      where 
                        tu.TarjetaID =(
                           
                   		select top 1 ut.TarjetaID from flujo.UsuarioTarjeta ut where ut.UsuarioID = @UsuarioID and ut.Activo = 1
                   	
                   	    );
                ";

                using (var con = DapperDBHelper.GetSqlConnection())
                {
                    con.Open();

                    List<TokenDeSeguridadResponseModel> aux = con.Query<TokenDeSeguridadResponseModel>(query, p, commandType: CommandType.Text).ToList();

                    if (aux.Any())
                    {
                        ColeccionTokens = aux;
                    }

                    con.Close();
                }

                return ColeccionTokens;
             }
            catch (Exception ex)
            {
                return null;
            }
        }

        


    }
}
