using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MAR.DataAccess.Tables;

namespace MAR.DataAccess.Repositories
{
    public  class mar_HVP_LogComunicacionRepository : BaseRepository
    {
        public mar_HVP_LogComunicacionRepository(int pCurrentUserID):base(pCurrentUserID) {    }

        public List<HVP_LogComunicacionesRecord> GetTransaccionesDiasCerrados(int? SoloLogID, int? ProductoID, int? PinID, DateTime StartDate, DateTime EndDate)
        {
            return _Query<HVP_LogComunicacionesRecord>("mar_VP_And_HVP_Log_GetTransacciones", new { SoloLogID = SoloLogID, IsDiasAbiertos = (byte)0, ProductoID = ProductoID, StartDate = StartDate, EndDate = EndDate }).ToList(); ;
        }


        public int InsertTransaccionDiaCerrado(int ProductoId, int PinID, string Direccion, DateTime Fecha, Guid RawData)
        {
            Dapper.DynamicParameters pParams = new Dapper.DynamicParameters();
            pParams.Add("@IsDiaActivo", dbType: System.Data.DbType.Byte,     value: (byte)0);
            pParams.Add("@ProductoId" , dbType: System.Data.DbType.Int32,    value: ProductoId);
            pParams.Add("@PinID",       dbType: System.Data.DbType.Int32,    value: PinID);
            pParams.Add("@Direccion",   dbType: System.Data.DbType.String,   value: Direccion);
            pParams.Add("@Fecha",       dbType: System.Data.DbType.DateTime, value: Fecha);
            pParams.Add("@RawData",     dbType: System.Data.DbType.Guid,     value: RawData);
            pParams.Add("@LogId",       dbType: System.Data.DbType.Int32,    value: -1);
            return _Execute("mar_VP_HVP_LogInsertTransacciones", pParams, "@LogId");           
        }
    }
}
