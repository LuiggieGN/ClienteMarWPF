using ClienteMarWPF.DataAccess;
using ClienteMarWPF.Domain.Models.Dtos;
using ClienteMarWPF.Domain.Services.SorteosService;
using ClienteMarWPF.UI.Modules.Sorteos;
using ClienteMarWPF.UI.State.Authenticators;
using MarPuntoVentaServiceReference;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClienteMarWPF.UI.ViewModels.Commands.Sorteos
{
    public class RealizarApuestaCommand: ActionCommand
    {
        private readonly SorteosViewModel ViewModel;
        private readonly ISorteosService SorteosService;
        private readonly IAuthenticator Autenticador;
        public RealizarApuestaCommand(SorteosViewModel viewModel, IAuthenticator autenticador, ISorteosService sorteosService)
        {
            ViewModel = viewModel;
            Autenticador = autenticador;
            SorteosService = sorteosService;

            Action<object> comando = new Action<object>(RealizarApuestas);
            base.SetAction(comando);
        }

        private void RealizarApuestas(object parametro)
        {
            var data = parametro as ApuestaResponse;
            if (data.Jugadas.Count == 1)
            {
                OnlyBet(data);
            }
            else if(data.Jugadas.Count > 1)
            {
                MultiBet(data);
            }

            SessionGlobals.GenerateNewSolicitudID(Autenticador.CurrentAccount.MAR_Setting2.Sesion.Sesion, true);
        }

        private void OnlyBet(ApuestaResponse apuesta)
        {
            var bet = new MAR_Bet();
            var itemBet = new List<MAR_BetItem>();

            foreach (var item in apuesta.Jugadas)
            {
                itemBet.Add(new MAR_BetItem
                {
                    Loteria = apuesta.LoteriaID,
                    Numero = item.Jugadas,
                    Costo = item.Monto,
                    Cantidad = item.Monto,
                    QP = item.TipoJugada.TrimStart().Substring(0, 1)

                });

            }

            bet.Items = itemBet.ToArray();
            bet.Solicitud = SessionGlobals.SolicitudID;
            bet.Loteria = apuesta.LoteriaID;


            var MarBetResponse = SorteosService.RealizarApuesta(Autenticador.CurrentAccount.MAR_Setting2.Sesion, bet, SessionGlobals.SolicitudID, true);
            var ticket = new ArrayOfInt() { MarBetResponse.Ticket  };
            SorteosService.ConfirmarMultiApuesta(Autenticador.CurrentAccount.MAR_Setting2.Sesion, ticket);
            //SorteosService.ConfirmarApuesta(Autenticador.CurrentAccount.MAR_Setting2.Sesion);
        }

        private void MultiBet(ApuestaResponse apuestas)
        {

            var multi = new MAR_MultiBet();
            var itemBet = new List<MAR_BetItem>();

            foreach (var item in apuestas.Jugadas)
            {
                itemBet.Add(new MAR_BetItem
                {
                    Loteria = apuestas.LoteriaID,
                    Numero = item.Jugadas,
                    Costo = item.Monto,
                    QP = item.TipoJugada.TrimStart().Substring(0, 1),
                    Cantidad = item.Monto

                });
            }
            multi.Items = itemBet.ToArray();
            multi.Headers = new MAR_BetHeader[] {
                new MAR_BetHeader {
                    Solicitud = SessionGlobals.SolicitudID,
                    Loteria = apuestas.LoteriaID,
                    Costo = apuestas.Jugadas.Sum(x => x.Monto)

                } };

            var MultiBetResponse = SorteosService.RealizarMultiApuesta(Autenticador.CurrentAccount.MAR_Setting2.Sesion, multi);
            var ticket = new ArrayOfInt();
            var headerResponse = MultiBetResponse.Headers.OfType<MAR_BetHeader>().ToList();
            if (headerResponse.Count > 0)
            {
                //foreach (var item in headerResponse)
                //{
                //    ticket.Add(item.Ticket);
                //}

            }

            SorteosService.ConfirmarMultiApuesta(Autenticador.CurrentAccount.MAR_Setting2.Sesion, ticket);
        }

    }
}
