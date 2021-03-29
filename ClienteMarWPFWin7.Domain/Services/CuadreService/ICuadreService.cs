
using ClienteMarWPFWin7.Domain.Enums;
using ClienteMarWPFWin7.Domain.Models.Dtos;
using ClienteMarWPFWin7.Domain.Models.Dtos.EfectivoDtos;

namespace ClienteMarWPFWin7.Domain.Services.CuadreService
{
    public interface ICuadreService
    {
        CuadreRegistroResultDTO Registrar(BancaDTO banca, CuadreRegistroDTO ope, bool enablePrinting ,out string toPrint);

    }
}


