
using System.Collections.Generic;
using ClienteMarWPFWin7.Domain.Enums;
using ClienteMarWPFWin7.Domain.Models.Dtos;
using ClienteMarWPFWin7.Domain.Models.Dtos.EfectivoDtos;
using ClienteMarWPFWin7.Domain.JuegaMasService;
using System;

namespace ClienteMarWPFWin7.Domain.Services.JuegaMasService
{
    public interface IJuegaMasService
    {
        MAR_JuegaMasResponse LeerReporteEstadoDePremiosJuegaMas(MAR_Session sesion,DateTime Fecha);
    }
}
