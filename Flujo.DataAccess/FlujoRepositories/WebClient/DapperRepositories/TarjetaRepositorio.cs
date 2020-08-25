using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;
using Flujo.Entities.WebClient.POCO;
using Flujo.DataAccess.FlujoRepositories.WebClient.Helpers;
using MAR.AppLogic.MARHelpers;
namespace Flujo.DataAccess.FlujoRepositories.WebClient.DapperRepositories
{
    public static class TarjetaRepositorio
    {
        private static string GetTarjetaPin()
        {
            try
            {
                string firma = null;
                string query = @"
                declare @pin bigint;
                
                select  @pin = year(getdate())*(100000000000000) + ( CAST(ABS(CHECKSUM(NEWID())) % 10000000  AS bigint) * CAST(ABS(CHECKSUM(NEWID())) % 10000000 AS bigint)) 
                
                select @pin;
            ";
                string queryConsultaTarjeta = @"
                 select top 1 ut.TarjetaID from flujo.UsuarioTarjeta ut where ut.Serial = @Serial;
            ";
                using (var con = DapperDBHelper.GetSqlConnection())
                {
                    con.Open();
                    List<string> LasFirmas = con.Query<string>(query, commandType: CommandType.Text).ToList();
                    if (LasFirmas.Any())
                    {
                        firma = LasFirmas.First();
                    }
                    if (firma != null)
                    {
                        int Limite = 0;
                        DynamicParameters p1 = new DynamicParameters();
                        p1.Add("@Serial", firma);
                        List<int> LasTarjetasIDs = con.Query<int>(queryConsultaTarjeta, p1, commandType: CommandType.Text).ToList();
                        while (LasTarjetasIDs.Any())
                        {
                            ++Limite;
                            List<string> Firmas__NuevoIntento = con.Query<string>(query, commandType: CommandType.Text).ToList();
                            if (Firmas__NuevoIntento.Any())
                            {
                                string Firma_Nueva = Firmas__NuevoIntento.First();
                                DynamicParameters p2 = new DynamicParameters();
                                p2.Add("@Serial", Firma_Nueva);
                                LasTarjetasIDs = con.Query<int>(queryConsultaTarjeta, p2, commandType: CommandType.Text).ToList();
                                if (LasTarjetasIDs != null && (!LasTarjetasIDs.Any()))
                                {
                                    firma = Firma_Nueva;
                                    break;
                                }
                            }
                            else
                            {
                                firma = null;
                                break;
                            }
                            if (Limite == 10)
                            {
                                firma = null;
                                break;
                            }
                        }
                    }
                }
                return firma;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public static bool CrearTarjeta(int pUsuarioID, List<SecurityToken> pTokens)
        {
            try
            {
                string firma = GetTarjetaPin(); bool FueCreada = false;
                if (firma == null)
                {
                    return false;
                }
                string insertTarjetaQuery = @"
                    insert into  flujo.UsuarioTarjeta
                               ( 
                                 UsuarioID
                               , Serial
                               , FechaCreacion
                               , Comentario
                               , Activo
                               )
                         VALUES
                               (
                                 @UsuarioID
                               , @Serial
                               , getdate()
                               , ''
                               , 1
                               );
                    
                    select cast(scope_identity() as int)
                ";
                string insertTokenStart = @"
                    insert into flujo.ToquenUsuario 
                               (
                                 TarjetaID
                               , Posicion
                               , Toquen
                               )
                    values 
                ";
                DynamicParameters p1 = new DynamicParameters(); p1.Add("@UsuarioID", pUsuarioID); p1.Add("@Serial", firma);
                using (var con = DapperDBHelper.GetSqlConnection())
                {
                    con.Open();
                    List<int> TarjetaIDs = con.Query<int>(insertTarjetaQuery, p1, commandType: CommandType.Text).ToList();
                    if (TarjetaIDs.Any())
                    {
                        int CurrentTarjetaID = TarjetaIDs.First();
                        if (pTokens != null && pTokens.Count > 0)
                        {
                            DynamicParameters p2 = new DynamicParameters();
                            p2.Add("@TarjetaID", CurrentTarjetaID);
                            for (int i = 0; i < pTokens.Count(); i++)
                            {
                                insertTokenStart += $"(@TarjetaID,  @Pos{i}, @Tok{i}),";
                                p2.Add($"@Pos{i}", pTokens[i].Posicion);
                                p2.Add($"@Tok{i}", pTokens[i].Token);
                            }
                            insertTokenStart = insertTokenStart.Substring(0, insertTokenStart.Length - 1) + ";";
                            var affected = con.Execute(insertTokenStart, p2, commandType: CommandType.Text);
                            if (affected <= 0)
                            {
                                return false;
                            }
                        }
                        else
                        {
                            return false;
                        }
                        FueCreada = true;
                    }
                }
                return FueCreada;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public static Tarjeta GetTarjeta(int pUsuarioID)
        {
            try
            {
                Tarjeta t = new Tarjeta();
                DynamicParameters p = new DynamicParameters();
                p.Add("@UsuarioID", pUsuarioID);
                string queryTarjetaSerial = @"
                  select top 1 ut.Serial from flujo.UsuarioTarjeta ut where ut.UsuarioID =  @UsuarioID;
                ";
                string queryTokens = @"
                    select TResult.Posicion As Posicion, TResult.Toquen As Token  from(
                    
                      select * from flujo.ToquenUsuario tu where tu.TarjetaID = (
                         select top 1  ut.TarjetaID from flujo.UsuarioTarjeta ut where ut.Serial = @Serial
                      ) 
                    
                    ) As TResult Order By TResult.Posicion ASC                
                ";
                using (var con = DapperDBHelper.GetSqlConnection())
                {
                    con.Open();
                    List<string> Seriales = con.Query<string>(queryTarjetaSerial, p, commandType: CommandType.Text).ToList();
                    if (Seriales.Any())
                    {
                        t.Pin = Seriales.First();
                        DynamicParameters p2 = new DynamicParameters();
                        p2.Add("@Serial", t.Pin);
                        List<SecurityToken> LosTokens = con.Query<SecurityToken>(queryTokens, p2, commandType: CommandType.Text).ToList();
                        if (LosTokens.Any() && LosTokens.Count == 40)
                        {
                            t.Tokens = new List<SecurityToken>();
                            foreach (SecurityToken Tok in LosTokens)
                            {
                                t.Tokens.Add(Tok);
                            }
                        }
                        else
                        {
                            t = null;
                        }
                    }
                    else
                    {
                        t = null;
                    }
                }
                return t;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        // true  : Si el update fue completado
        // false : Si el update no fue completado
        public static bool ActualizarTarjeta(int pUsuarioID, List<SecurityToken> pTokens)
        {
            try
            {
                bool TokenFueronActualizados = false;
                string query = @"
                update flujo.ToquenUsuario set Toquen = @newToken where Posicion = @pos and  TarjetaID = ( select top 1 ut.TarjetaID from flujo.UsuarioTarjeta ut where ut.UsuarioID = @UsuarioID )
             ";
                using (var con = DapperDBHelper.GetSqlConnection())
                {
                    con.Open();
                    foreach (SecurityToken item in pTokens)
                    {
                        DynamicParameters p = new DynamicParameters();
                        p.Add("@newToken", item.Token);
                        p.Add("@pos", item.Posicion);
                        p.Add("@UsuarioID", pUsuarioID);
                        con.Execute(query, p, commandType: CommandType.Text);
                    }
                    TokenFueronActualizados = true;
                }
                return TokenFueronActualizados;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}