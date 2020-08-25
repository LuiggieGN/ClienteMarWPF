using Flujo.DataAccess.FlujoRepositories.WebClient.DapperRepositories;
using Flujo.Entities.WebClient.POCO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAR.BusinessLogic.Flujo.WebClient.Code
{
    public class DashBoardLogic
    {
        public static EstadoConsorcio GetConsorcioEstado(int? RiferoID)
        {
            EstadoConsorcio estado = DashBoardRepositoriocs.GetConsorcioEstado(RiferoID);

            return estado;
        }
    }
}