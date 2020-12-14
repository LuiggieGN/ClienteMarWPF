using ClienteMarWPF.DataAccess;
using ClienteMarWPF.Domain.Models.Dtos;
using ClienteMarWPF.Domain.Services.SorteosService;
using ClienteMarWPF.UI.Modules.Sorteos;
using ClienteMarWPF.UI.State.Authenticators;
using MAR.AppLogic.MARHelpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClienteMarWPF.UI.ViewModels.Commands.Sorteos
{
    public class GetUltimosSorteosCommand: ActionCommand
    {
        private readonly SorteosViewModel ViewModel;
        private readonly ISorteosService SorteosService;
        private readonly IAuthenticator Autenticador;

        public GetUltimosSorteosCommand(SorteosViewModel viewModel, IAuthenticator autenticador, ISorteosService sorteosService)
        {
            ViewModel = viewModel;
            Autenticador = autenticador;
            SorteosService = sorteosService;

            Action<object> comando = new Action<object>(GetUltimosSorteos);
            base.SetAction(comando);
        }

        private void GetUltimosSorteos(object parametro)
        {
            ViewModel.ganadores.Clear();
            try
            {
                var sorteos = SessionGlobals.LoteriasTodas;

                foreach (var item in sorteos)
                {
                    var ganadors = SorteosService.GetUltimosSorteos(Autenticador.CurrentAccount.MAR_Setting2.Sesion, item.Numero, FechaHelper.FormatFecha(DateTime.Today, FechaHelper.FormatoEnum.FechaBasico));
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
