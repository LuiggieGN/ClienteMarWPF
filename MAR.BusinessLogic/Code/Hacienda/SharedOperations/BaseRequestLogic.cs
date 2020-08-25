using MarConnectCliente.RequestModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MAR.DataAccess.EFRepositories.PMCuentasRepository;

namespace MAR.BusinessLogic.Code.Hacienda.SharedOperations
{
    public class BaseRequestLogic
    {
        public static BaseRequestModel CreaBaseRequest(CuentaProducto pCuenta, string pCodigoOperacion)
        {


            return new BaseRequestModel()
            {
                DiaOperacion = DateTime.Now.ToString("yyyy-MM-dd"),
                 FechaHoraSolicitud = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                 ServiceUrl = new Uri(pCuenta.SWProducto.URL),
                EstablecimientoID= pCuenta.PMCuenta.CueComercio,
                Usuario = pCuenta.PMCuenta.RecargaID,
                Password = pCuenta.PMCuenta.CueServidor,
                CodigoOperacion = pCodigoOperacion
            };
        }

        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }
    }
}
