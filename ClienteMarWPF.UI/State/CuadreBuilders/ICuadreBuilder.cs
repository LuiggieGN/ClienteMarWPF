using System;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

using ClienteMarWPF.Domain.Models.Dtos;
using ClienteMarWPF.Domain.Models.Dtos.EfectivoDtos;
using ClienteMarWPF.Domain.Models.Entities;
using ClienteMarWPF.UI.Modules.FlujoEfectivo.Cuadre.Windows.Cuadre.ViewModels;
using ClienteMarWPF.UI.State.Authenticators;

namespace ClienteMarWPF.UI.State.CuadreBuilders
{
    public interface ICuadreBuilder
    {

        ConsultaInicialViewModel LeerCuadreConsultaInicial(IAuthenticator authenticator);
        CuadreRegistroResultDTO BuildCuadre(BancaDTO banca, CuadreRegistroDTO ope, bool enablePrinting, out string toPrint);
        decimal LeerCajaBalance(int cajaid);
        bool SetearCajaDisponibilidad(CajaDisponibilidadDTO disponibilidad);

    }
}
