using Dapper;
using MAR.AppLogic.MARHelpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAR.DataAccess.EFRepositories
{

    public class SessionRepository
    {
        public static SesionValues GetSesionValue(int BancaId, int SesionActual)
        {
            var context = new Tables.DTOs.MARContext();
            var values = new SesionValues() { BancaId = BancaId, SesionActual = SesionActual };
            var banca = context.MBancas.FirstOrDefault(b => b.BancaID == BancaId && b.BanSesionActual == SesionActual);
            if (banca == null) return null;

            values.LastTicket = context.DTickets.Where(t => t.BancaID == BancaId && !t.TicNulo)
                                            .OrderByDescending(t => t.TicketID).Select(t => t.TicketID).FirstOrDefault();
            values.LastPin = context.PDPines.Where(p => p.BancaID == BancaId && p.PinNulo == 0)
                                            .OrderByDescending(p => p.PinID).Select(p => p.PinID).FirstOrDefault();
            values.UsuarioActual = banca.BanUsuarioActual;
            return values;
        }
        public static string GetUsuario(int pUsuarioId) 
        {
            try
            {
                using (var con = DALHelper.GetSqlConnection())
                {
                    string query = $@"SELECT distinct UsuNombre + ' ' + UsuApellido FROM MUsuarios where UsuarioID = @UsuarioID";
                    var p = new DynamicParameters();
                    p.Add("@UsuarioID", pUsuarioId);
                    string usuario = con.Query<string>(query, p, commandType: CommandType.Text).FirstOrDefault();
                    return usuario;
                }
            }
            catch (Exception e)
            {
                return "";
            }
         
        }


        public class SesionValues
        {
            public int BancaId { get; set; }
            public int SesionActual { get; set; }
            public int UsuarioActual { get; set; }
            public int LastPin { get; set; }
            public int LastTicket { get; set; }
        }

        //El siguiente codigo garantiza que la libreria del provider the SQLClient para Entity Framework sea copiado cuando se haga Publish
        //Referencia: http://stackoverflow.com/questions/21175713/no-entity-framework-provider-found-for-the-ado-net-provider-with-invariant-name
        private volatile Type _sqlProviderDependency;
        public SessionRepository()
        {
            _sqlProviderDependency = typeof(System.Data.Entity.SqlServer.SqlProviderServices);
        }

    }
}
