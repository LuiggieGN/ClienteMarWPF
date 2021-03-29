
using ClienteMarWPFWin7.Domain.Enums;
using ClienteMarWPFWin7.Domain.Models.Dtos;
using ClienteMarWPFWin7.Domain.Models.Dtos.EfectivoDtos;

namespace ClienteMarWPFWin7.Domain.Services.TieService
{
    public interface ITieService
    {
        TieDTO LeerTiposAnonimos();
    }
}
