using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;


namespace MAR.Prueba
{
  
       
}

namespace MAR.AppLogic.MARHelpers
{
    public class Prueba
    {
        public static string ReadString(Config.ConfigEnums pConfigId)
        {
            return "";// AppLogic.Encryption.Encryptor.DecryptConfig(MAR.Config.Reader.ReadString(pConfigId));
        }
    }
    public class DALHelper
    {
        public static class ConfigReader
        {
            public static string ReadString(Config.ConfigEnums pConfigId)
            {
                return AppLogic.Encryption.Encryptor.DecryptConfig(MAR.Config.Reader.ReadString(pConfigId));
            }
          
        }
        public static System.Data.SqlClient.SqlConnection GetSqlConnection()
        {
            return new System.Data.SqlClient.SqlConnection(AppLogic.MARHelpers.DALHelper.ConfigReader.ReadString(Config.ConfigEnums.DBConnection2));
        }

      
        public static DataAccess.ViewModels.BaseViewModel.SqlDataResult GetDataTable(string pSqlQuery, List<System.Data.SqlClient.SqlParameter> pParams, CommandType pCommType)
        {
            try
            {
                var con = new System.Data.SqlClient.SqlConnection(AppLogic.MARHelpers.DALHelper.ConfigReader.ReadString(Config.ConfigEnums.DBConnection2));
                con.Open();
                SqlCommand cmd = new SqlCommand(pSqlQuery, con);
                cmd.CommandType = pCommType;
                if (pParams != null)
                {
                    cmd.Parameters.AddRange(pParams.ToArray());
                }
                System.Data.SqlClient.SqlDataReader reader = cmd.ExecuteReader();
                var tb = new System.Data.DataTable();
                tb.Load(reader);
                con.Close();
                return new DataAccess.ViewModels.BaseViewModel.SqlDataResult { OK = true, Result = tb };
            }
            catch (Exception e)
            {
                return new DataAccess.ViewModels.BaseViewModel.SqlDataResult { OK = false, Result = e.Message };
            }
        }

        public static DataAccess.ViewModels.BaseViewModel.SqlDataResult PostDataTable(string pSPName, List<SqlParameter> pParams, CommandType pCommType)
        {
            try
            {
                var con = new System.Data.SqlClient.SqlConnection(AppLogic.MARHelpers.DALHelper.ConfigReader.ReadString(Config.ConfigEnums.DBConnection2));
                con.Open();
                SqlCommand cmd = new SqlCommand(pSPName, con);
                cmd.CommandType = pCommType;
                if (pParams != null)
                {
                    cmd.Parameters.AddRange(pParams.ToArray());
                }
                cmd.ExecuteNonQuery();
                con.Close();
                return new DataAccess.ViewModels.BaseViewModel.SqlDataResult { OK = true, Result = null };
            }
            catch (Exception e)
            {
                return new DataAccess.ViewModels.BaseViewModel.SqlDataResult { OK = false, Result = e.Message };
            }
         
        }

        public static DataTable ToDataTable<T>(List<T> items)
        {
            var tb = new DataTable(typeof(T).Name);

            PropertyInfo[] props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var prop in props)
            {
                tb.Columns.Add(prop.Name, Nullable.GetUnderlyingType(
            prop.PropertyType) ?? prop.PropertyType);
            }

            foreach (var item in items)
            {
                var values = new object[props.Length];
                for (var i = 0; i < props.Length; i++)
                {
                    values[i] = props[i].GetValue(item, null);
                }

                tb.Rows.Add(values);
            }

            return tb;
        }

    }

}
