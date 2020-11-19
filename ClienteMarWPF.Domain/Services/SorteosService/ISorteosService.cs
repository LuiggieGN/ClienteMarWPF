using HaciendaService;
using MarPuntoVentaServiceReference;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClienteMarWPF.Domain.Services.SorteosService
{
    public interface ISorteosService
    {
        MAR_HaciendaResponse GetSorteosDisponibles(HaciendaService.MAR_Session session);
        MAR_MultiBet RealizarApuesta(MarPuntoVentaServiceReference.MAR_Session session, MAR_MultiBet Apuestas);
    }
}
