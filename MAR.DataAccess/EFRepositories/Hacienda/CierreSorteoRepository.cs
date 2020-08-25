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
    public class CierreSorteoRepository
    {

        public static List<DesgloseCierreValues> GetCierreSorteoRequestValues(int pSorteoDiaID)
        {
            try
            {
                using (var con = DALHelper.GetSqlConnection())
                {
                    string query = $@"select COUNT(*) as  CantidadApuestas, EdiFecha as Fecha, e.EDiCierreVentaFecha as FechaCierre, h.Codigo as IdentificadorJuegoDeAzar, replace(REPLACE(SUBSTRING(t.Peticion,PATINDEX('%Detalle%',t.Peticion),LEN(t.Peticion)), 'Detalle = ', ''),' " + @"} }','') as Peticion from HEstatusDias e join HaciendaSorteo h on h.MARSorteoID = e.LoteriaID
                                                join TransaccionClienteHttp t on convert(date,t.Fecha) = convert(date,e.EDiFecha)
                                                where EstatusDiaID =@SorteoDiaID and t.Activo = 1 and t.TipoTransaccionID = 3
												group by EDiFecha, Codigo, Peticion, EDiCierreVentaFecha"
                                                 ;

                    var p = new DynamicParameters();
                    p.Add("@SorteoDiaID", pSorteoDiaID);
                    var desgloseJugadas = con.Query<DesgloseCierreValues>(query, p, commandType: CommandType.Text).Distinct().ToList();
                    con.Close();
                    return desgloseJugadas;
                }
            }
            catch (Exception e)
            {
                string t = e.Message;
                return null;
            }
        }



        public class DesgloseCierreValues
        {
            public string Peticion { get; set; }
            public DateTime Fecha { get; set; }
            public int CantidadApuestas { get; set; }
            public DateTime FechaCierre { get; set; }
            public string IdentificadorJuegoDeAzar { get; set; }
        }

     




    }
}
