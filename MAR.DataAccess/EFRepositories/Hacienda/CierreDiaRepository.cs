using Dapper;
using MAR.AppLogic.MARHelpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAR.DataAccess.EFRepositories.Hacienda
{
    public class CierreDiaRepository
    {

        public static List<DesgloseCierreValues> GetCierreDiaRequestValues(DateTime? pFecha)
        {
            try
            {
                using (var con = DALHelper.GetSqlConnection())
                {
                    string query = $@"select  TT.Nombre as Tipo, COUNT(T.TransaccionID) as CantidadOperaciones, sum(Monto) as MontoOperaciones
                                                    from TransaccionClienteHttp t join TipoTransaccion tt ON TT.TipoTransaccionID = t.TipoTransaccionID
                                                    where tt.Nombre != 'Inicio' and  tt.Nombre != 'CierreDia' and  Activo = 1 and (@Fecha is null or  CONVERT(date, t.fecha) = CONVERT(date, @Fecha))
                                                    group by tt.Nombre"
                                                                                      ;

                    var p = new DynamicParameters();
                    p.Add("@Fecha", pFecha);
                    var desgloseJugadas = con.Query<DesgloseCierreValues>(query, p, commandType: CommandType.Text).Distinct().ToList();
                    con.Close();
                    var desgloseClean = desgloseJugadas;
                    foreach (var item in desgloseJugadas)
                    {
                        if (item.Tipo == "Anulacion")
                        {
                            var montoApuestaOriginal = desgloseJugadas.Where(x => x.Tipo == "Apuesta").FirstOrDefault().MontoOperaciones;
                            var anulacionMonto = item.MontoOperaciones;

                            var cantidadApuestaOriginal = desgloseJugadas.Where(x => x.Tipo == "Apuesta").FirstOrDefault().CantidadOperaciones;


                            desgloseClean.Where(x => x.Tipo == "Apuesta").FirstOrDefault().MontoOperaciones = montoApuestaOriginal - anulacionMonto;
                            desgloseClean.Where(x => x.Tipo == "Apuesta").FirstOrDefault().CantidadOperaciones = cantidadApuestaOriginal - item.CantidadOperaciones;

                        }
                    }

                    return desgloseClean;
                }
            }
            catch (Exception e)
            {
                string t = e.Message;
                return null;
            }
        }

        public static bool SorteosAbiertos(DateTime? pFecha)
        {
            try
            {
                using (var con = DALHelper.GetSqlConnection())
                {
                    string query = $@"SELECT COUNT(*)
                                                     FROM HEstatusDias
                                                     where convert(date,EDiFecha) = convert(date,@Fecha) and EDiDiaCerrado = 0"
                                                                                      ;
                    var p = new DynamicParameters();
                    p.Add("@Fecha", pFecha);
                    var sorteosAbiertos = con.QueryFirst<int>(query, p, commandType: CommandType.Text);

                    con.Close();
                    if (sorteosAbiertos > 0)
                    {
                        return true;
                    }
                    return false;
                }
            }
            catch (Exception e)
            {
                string t = e.Message;
                return true;
            }
        }


        public class DesgloseCierreValues
        {
            public decimal MontoOperaciones { get; set; }
            public string Tipo { get; set; }
            public int CantidadOperaciones { get; set; }
        }

     




    }
}
