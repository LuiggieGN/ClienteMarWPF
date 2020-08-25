using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAR.BusinessLogic.Models.Mappers
{
    public class EstatusDiasMapper
    {
        public static DataAccess.Tables.DTOs.VHEstatusDia Map_From_RFSorteoDia(int pSorteoDiaId)
        {
            var rfSorteoDia = DataAccess.EFRepositories.RFRepositories.RFSorteoRepository.Get_VHEstatusDia(pSorteoDiaId);
            return rfSorteoDia;
        }
    }
}
