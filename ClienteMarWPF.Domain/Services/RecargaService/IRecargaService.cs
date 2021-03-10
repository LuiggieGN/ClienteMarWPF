using MarPuntoVentaServiceReference;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClienteMarWPF.Domain.Services.RecargaService
{
    public interface IRecargaService
    {
         MAR_Suplidor[] GetSuplidor(MAR_Session _Session);


       MAR_Pin GetRecarga(MAR_Session _Session, int user, string clave, int suplidor, string Numero, double Monto, int Solicitud);


        void ConfirmRecarga(MAR_Session _Session);


    

    }
}
