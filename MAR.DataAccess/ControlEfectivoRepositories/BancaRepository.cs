using Dapper;

using System;
using System.Data;
using System.Linq;
using System.Collections.Generic;

using MAR.AppLogic.MARHelpers;
using MAR.DataAccess.ControlEfectivoRepositories.Helpers;
using MAR.DataAccess.Tables.ControlEfectivoDTOs;

namespace MAR.DataAccess.ControlEfectivoRepositories
{
    public static class BancaRepository
    {
        public static BancaConfiguracionDTO LeerBancaConfiguraciones(int bancaid)
        {
            try
            {

                var p = new DynamicParameters();
                p.Add("@bancaid", bancaid);

                var configuraciones = new BancaConfiguracionDTO();

                using (var db = DALHelper.GetSqlConnection())
                {
                    db.Open();

                    using (var queryMultiple = db.QueryMultiple(BancaHelper.SelectBancaConfiguraciones, p, commandType: CommandType.Text))
                    {
                        configuraciones.BancaDto = queryMultiple.Read<BancaDTO>().FirstOrDefault();
                        configuraciones.CajaEfectivoDto = queryMultiple.Read<CajaDTO>().FirstOrDefault();
                        configuraciones.ControlEfectivoConfigDto = queryMultiple.Read<ControlEfectivoDTO>().FirstOrDefault();
                    }

                    db.Close();
                }

                return configuraciones;
            }
            catch (Exception e)
            {
                throw e;
            }
        }


        public static int? LeerBancaLastCuadreId(int bancaid)
        {
            try
            {
                var p = new DynamicParameters();
                p.Add("@bancaid", bancaid);

                int? ultimocuadre = 0;

                using (var db = DALHelper.GetSqlConnection())
                {
                    db.Open();

                    List<int> ids = db.Query<int>(BancaHelper.SelectBancaUltimoCuadreId, p, commandType: CommandType.Text).ToList();

                    if (!ids.Any())
                    {
                        ultimocuadre = null;
                    }
                    else
                    {
                        ultimocuadre = ids.FirstOrDefault();
                    }
                    db.Close();
                }

                return ultimocuadre;
            }
            catch (Exception e)
            {
                throw e;
            }
        }


        public static List<BancaControlEfectivoTransaccionDTO> LeerBancaLastTransaccionesApartirDelUltimoCuadre(int bancaid)
        {
            try
            {
                DynamicParameters p = new DynamicParameters(); List<BancaControlEfectivoTransaccionDTO> transacciones = null;

                p.Add("@BanID", bancaid);

                using (var db = DALHelper.GetSqlConnection())
                {
                    db.Open();

                    transacciones = db.Query<BancaControlEfectivoTransaccionDTO>(
                        BancaHelper.Procedure_SP_GET_BANCA_TRANSACCIONES_A_PARTIR_DE_ULTIMO_CUADRE,
                        p,
                        commandType: CommandType.StoredProcedure).ToList();

                    if (!transacciones.Any())
                    {
                        transacciones = null;
                    }

                    db.Close();
                }

                return transacciones;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static CuadreDTO LeerBancaCuadrePorCuadreId(int cuadreid)
        {
            try
            {
                DynamicParameters p = new DynamicParameters(); CuadreDTO cuadre = null;

                p.Add("@cuadreid", cuadreid);

                using (var db = DALHelper.GetSqlConnection())
                {
                    db.Open();

                    List<CuadreDTO> cuadres = db.Query<CuadreDTO>(BancaHelper.SelectBancaCuadrePorCuadreId, p, commandType: CommandType.Text).ToList();

                    if (cuadres.Any())
                    {
                        cuadre = cuadres.First();
                    }

                    db.Close();
                }

                return cuadre;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public static bool BancaUsaControlEfectivo(int bancaid, bool incluyeConfig)
        {
            try
            {
                DynamicParameters p = new DynamicParameters(); bool bancaUsaControlEfectivo = false;

                p.Add("@bancaid", bancaid);
                p.Add("@incluyeconfig", incluyeConfig);

                using (var db = DALHelper.GetSqlConnection())
                {
                    db.Open();

                    List<bool> results = db.Query<bool>(BancaHelper.BancaUsaControlEfectivo, p, commandType: CommandType.Text).ToList();

                    if (results.Any())
                    {
                        bancaUsaControlEfectivo = results.First();
                    }

                    db.Close();
                }

                return bancaUsaControlEfectivo;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static decimal LeerDeudaDeBanca(int bancaid)
        {
            try
            {
                var p = new DynamicParameters(); decimal deuda = 0;
                p.Add("@bancaid", bancaid);

                using (var db = DALHelper.GetSqlConnection())
                {
                    db.Open();

                    deuda = db.Query<decimal>(BancaHelper.QueryParaLeerDeudaDeBanca, p, commandType: CommandType.Text).First();

                    db.Close();
                }

                return deuda;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public static int LeerBancaInactividad(int bancaid)
        {
            try
            {
                var p = new DynamicParameters(); int inactividad = 0;
                p.Add("@bancaid", bancaid);

                using (var db = DALHelper.GetSqlConnection())
                {
                    db.Open();

                    inactividad = db.Query<int>(BancaHelper.SelectBancaInactividad, p, commandType: CommandType.Text).First();

                    db.Close();
                }

                return inactividad;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<MarOperacionDTO> LeerBancaMarOperacionesDia(int bancaid, DateTime dia)
        {
            try
            {
                var p = new DynamicParameters(); List<MarOperacionDTO> operaciones;
                p.Add("@bancaid", bancaid);
                p.Add("@fcompleta", dia);

                using (var db = DALHelper.GetSqlConnection())
                {
                    db.Open();

                    operaciones = db.Query<MarOperacionDTO>(BancaHelper.SelectMarOperaciones, p, commandType: CommandType.Text)?.ToList() ?? new List<MarOperacionDTO>();

                    db.Close();
                }

                return operaciones;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public static string LeerBancaRemoteCmdCommand(int bancaid)
        {
            try
            {
                DynamicParameters p = new DynamicParameters(); string comando = string.Empty;

                p.Add("@bancaid", bancaid);

                using (var db = DALHelper.GetSqlConnection())
                {
                    db.Open();

                    List<string> comandos = db.Query<string>(BancaHelper.LeerBancaRemoteCmdCommand, p, commandType: CommandType.Text).ToList();

                    if (comandos.Any())
                    {
                        comando = comandos.First();
                    }

                    db.Close();
                }

                return comando;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public static bool LeerEstadoBancaEstaActiva(int bancaid)
        {
            try
            {
                DynamicParameters p = new DynamicParameters(); bool bancaEstaActiva = false;
                p.Add("@bancaid", bancaid);

                using (var db = DALHelper.GetSqlConnection())
                {
                    db.Open();

                    List<bool> estados = db.Query<bool>(BancaHelper.LeerEstadoBancaEstaActiva, p, commandType: CommandType.Text).ToList();

                    if (estados.Any())
                    {
                        bancaEstaActiva = estados.First();
                    }

                    db.Close();
                }

                return bancaEstaActiva;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public static decimal LeerVentaDeHoyDeLoterias(int bancaid)
        {
            try
            {
                var p = new DynamicParameters(); decimal totalvendido = 0;
                p.Add("@bancaid", bancaid);

                using (var db = DALHelper.GetSqlConnection())
                {
                    db.Open();

                    totalvendido = db.Query<decimal>(BancaHelper.LeerBancaVentaDeHoyDeLoterias, p, commandType: CommandType.Text).First();

                    db.Close();
                }

                return totalvendido;
            } 
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public static decimal LeerVentaDeHoyDeProductos(int bancaid)
        {
            try
            {
                var p = new DynamicParameters(); decimal totalvendido = 0;
                p.Add("@bancaid", bancaid);

                using (var db = DALHelper.GetSqlConnection())
                {
                    db.Open();

                    totalvendido = db.Query<decimal>(BancaHelper.LeerBancaVentaDeHoyDeProductos, p, commandType: CommandType.Text).First();

                    db.Close();
                }

                return totalvendido;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        public static bool LeerBancaTicketFueAnulado(string noTicket)
        {
            try
            {
                DynamicParameters p = new DynamicParameters(); bool ticketFueAnuladoEstado = false;
                p.Add("@noTicket", noTicket??string.Empty);

                using (var db = DALHelper.GetSqlConnection())
                {
                    db.Open();

                    List<bool> estados = db.Query<bool>(BancaHelper.LeerTicketFueAnuladoEstadoPorNoTicket, p, commandType: CommandType.Text).ToList();

                    if (estados.Any())
                    {
                        ticketFueAnuladoEstado = estados.First();
                    }

                    db.Close();
                }

                return ticketFueAnuladoEstado;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public static bool Rel(int bancaid, string hwkey, string query)
        {
            try
            {
                DynamicParameters p = new DynamicParameters(); bool procesada = false;
                p.Add("@bancaid", bancaid);
                p.Add("@hwkey", hwkey);

                using (var db = DALHelper.GetSqlConnection())
                {
                    db.Open();

                    List<bool> estados = db.Query<bool>(query, p, commandType: CommandType.Text).ToList();

                    if (estados.Any())
                    {
                        procesada = estados.First();
                    }

                    db.Close();
                }

                return procesada;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


    }//fin de clase
}
