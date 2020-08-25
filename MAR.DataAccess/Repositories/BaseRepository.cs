using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dapper;
using System.Data.SqlClient;
using MAR.AppLogic.MARHelpers;

namespace MAR.DataAccess.Repositories
{
    public class BaseRepository
    {
        public int _currentUserID;

        public BaseRepository(int pCurrentUserID)
        {
            _currentUserID = pCurrentUserID;
        }

        public List<T> _Query<T>(string pSPName, object pParams)
        {
            using (var cn = new SqlConnection(DALHelper.ConfigReader.ReadString(Config.ConfigEnums.DBConnection2)))
            {
                List<T> myList = cn.Query<T>(pSPName,pParams,commandType:System.Data.CommandType.StoredProcedure).ToList();
                return myList;
            }
        } 


        public int _Execute(string pSPName, DynamicParameters pParams, string outParam) 
        {
            using (var cn = new SqlConnection(DALHelper.ConfigReader.ReadString(Config.ConfigEnums.DBConnection2)))
            {
                cn.Execute(pSPName, pParams, commandType: System.Data.CommandType.StoredProcedure);

                return pParams.Get<int>(outParam);
            }
                
        }


    }
}
