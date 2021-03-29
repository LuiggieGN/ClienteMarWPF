using ClienteMarWPFWin7.Data;
using ClienteMarWPFWin7.Domain.Models.Dtos;
using ClienteMarWPFWin7.Domain.Services.SorteosService;
using ClienteMarWPFWin7.UI.Modules.Sorteos;
using ClienteMarWPFWin7.UI.State.Authenticators;
using MAR.AppLogic.MARHelpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClienteMarWPFWin7.UI.ViewModels.Commands.Sorteos
{
    public class GetGanadoresCommand: ActionCommand
    {
        private readonly SorteosViewModel ViewModel;
        private readonly ISorteosService SorteosService;
        private readonly IAuthenticator Autenticador;

        public GetGanadoresCommand(SorteosViewModel viewModel, IAuthenticator autenticador, ISorteosService sorteosService)
        {
            ViewModel = viewModel;
            Autenticador = autenticador;
            SorteosService = sorteosService;

            Action<object> comando = new Action<object>(Ganadores3);
            base.SetAction(comando);
        }

        private void Ganadores3(object parametro)
        {
            ViewModel.ganadores.Clear();
            try
            {
                var sorteos = SessionGlobals.LoteriasTodas;

                foreach (var item in sorteos)
                {
                    var ganadors = SorteosService.Ganadores3(Autenticador.CurrentAccount.MAR_Setting2.Sesion, item.Numero, FechaHelper.FormatFecha(DateTime.Today, FechaHelper.FormatoEnum.FechaBasico));
                    if (ganadors.Fecha != null)
                    {
                        ViewModel.ganadores.Add(new UltimosSorteos { 
                            Sorteo = item.NombreResumido, 
                            Primero = ganadors.Primero,
                            Segundo = ganadors.Segundo,
                            Tercero = ganadors.Tercero,
                            //Hora = ganadors.Hora
                        });
                    }
                }
            }
            catch (Exception)
            {

            }
        }
    }
}
