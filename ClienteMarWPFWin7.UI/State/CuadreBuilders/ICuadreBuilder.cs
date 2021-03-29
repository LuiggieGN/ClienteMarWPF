using System;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

using ClienteMarWPFWin7.Domain.Models.Dtos;
using ClienteMarWPFWin7.Domain.Models.Dtos.EfectivoDtos;
using ClienteMarWPFWin7.Domain.Models.Entities;
using ClienteMarWPFWin7.UI.Modules.FlujoEfectivo.Cuadre.Windows.Cuadre.ViewModels;
using ClienteMarWPFWin7.UI.State.Authenticators;

namespace ClienteMarWPFWin7.UI.State.CuadreBuilders
{
    public interface ICuadreBuilder
    {

        ConsultaInicialViewModel LeerCuadreConsultaInicial(IAuthenticator authenticator);
        CuadreRegistroResultDTO BuildCuadre(BancaDTO banca, CuadreRegistroDTO ope, bool enablePrinting, out string toPrint);
        decimal LeerCajaBalance(int cajaid);
        bool SetearCajaDisponibilidad(CajaDisponibilidadDTO disponibilidad);

    }
}
