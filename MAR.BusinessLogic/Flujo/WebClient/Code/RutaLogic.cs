
using System;
using System.Collections.Generic;

using Flujo.Entities.WebClient.Enums;
using Flujo.Entities.WebClient.POCO;

using Flujo.DataAccess.FlujoRepositories.WebClient.DapperRepositories;

using MAR.BusinessLogic.Flujo.WebClient.Code.Validations;

namespace MAR.BusinessLogic.Flujo.WebClient.Code
{
    public static class RutaLogic
    {
        public static CreacionRutaEstato GuardarRuta(FormularioRuta pFormulario)
        {
            CreacionRutaEstato EstadoCreacionRuta = new CreacionRutaEstato();
            RutasValidacion       Validacion                  = new RutasValidacion();

            List<string> errores = Validacion.ValidarFormularioRuta(pFormulario);

            if (errores.Count > 0)
            {
                EstadoCreacionRuta.RutaFueProcesada = false;
                EstadoCreacionRuta.Errores = errores;
                return EstadoCreacionRuta;
            }
            
            try
            {
                bool FueInsertadaLaRuta = RutaRepository.CrearNuevaRuta(pFormulario);

                if (FueInsertadaLaRuta)
                {
                    EstadoCreacionRuta.RutaFueProcesada = true;
                    EstadoCreacionRuta.Errores = null;
                    return EstadoCreacionRuta;
                }
                else
                {
                    EstadoCreacionRuta.RutaFueProcesada = false;
                    EstadoCreacionRuta.Errores = new List<string> {"Hubo un error en al creación de ruta. favor intentar mas tarde." };
                    return EstadoCreacionRuta;
                }
            }
            catch (Exception ex)
            {
                EstadoCreacionRuta.RutaFueProcesada = false;
                EstadoCreacionRuta.Errores = new List<string>() { "Hubo un error en al creación de ruta. favor intentar mas tarde." };
                return EstadoCreacionRuta;
            }
        }
    }
}
