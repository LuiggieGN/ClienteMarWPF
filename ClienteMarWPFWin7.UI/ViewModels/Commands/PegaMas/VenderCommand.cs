
#region Namespace
using ClienteMarWPFWin7.UI.ViewModels.ModelObservable;
using ClienteMarWPFWin7.UI.ViewModels.Helpers;
using ClienteMarWPFWin7.UI.Modules.PegaMas;
using ClienteMarWPFWin7.Domain.Models.Dtos;
using ClienteMarWPFWin7.UI;
using System.Collections.Generic;
using System.Windows;
using System;
using System.Linq;
#endregion

namespace ClienteMarWPFWin7.UI.ViewModels.Commands.PegaMas
{
    public class VenderCommand : ActionCommand
    {
        private readonly PegaMasViewModel vm;

        public VenderCommand(PegaMasViewModel viewmodel) : base()
        {
            vm = viewmodel; base.SetAction(new Action<object>(Vender));
        }

        private void Vender(object parametro)
        {
            try
            {
                if (vm.Jugadas.Count > 0)
                {
                    var ticket = LeerTicketCreado();
                    var producto = vm.CincoMinServicio.SetProducto("CincoMinutos", vm.AutServicio.CurrentAccount);
                    var response = vm.CincoMinServicio.Apuesta(ticket, producto, vm.AutServicio.CurrentAccount);

                }
                else
                {
                    var ventana = Application.Current.MainWindow as ClienteMarWPFWin7.UI.MainWindow;
                    if (ventana != null)
                    {
                        ventana.MensajesAlerta("No hay jugadas en la lista debe agregar al menos una jugada", "Aviso!!");
                    }
                }
            }
            catch
            {
            }
        }




        private CincoMinutosRequestModel.TicketModel LeerTicketCreado() 
        {
            int terminalid = vm.AutServicio?.BancaConfiguracion?.BancaDto?.BancaID ?? 0;
            int montoOperacion = (int)vm.Jugadas.ToList().Sum(j => j.Monto);

            string codigoharcodeado = "PegaMas";
            int sorteoharcodeado = 0;
            int tipojugadaharcodeado = 1;

            var request = new CincoMinutosRequestModel.TicketModel();
            request.NoTicket = string.Empty;
            request.TicketID = 0;
            request.NautCalculado = string.Empty;
            request.Fecha = DateTime.Now;                      // Requerido
            request.TerminalID = terminalid;                   // Requerido
            request.AutenticacionReferencia = string.Empty;
            request.CodigoOperacionReferencia = string.Empty;
            request.LocalID = 0;
            request.MontoOperacion = montoOperacion;           // Requerido

            request.TicketDetalles = new List<CincoMinutosRequestModel.TicketDetalle>(); // Requerido

            foreach (var item in vm.Jugadas)
            {
                request.TicketDetalles.Add(new CincoMinutosRequestModel.TicketDetalle()
                {
                    TicketID = 0,
                    DetalleID = 0,
                    Codigo = codigoharcodeado,   // Requerido
                    SorteoID = sorteoharcodeado, // Requerido
                    Monto = (int)item.Monto,     // Requerido
                    Saco = 0,
                    Jugada = item.Jugada,        // Requerido
                    TipoJugadaID = tipojugadaharcodeado // Requerido
                });
            }

            return request;
        }




    }// Clase
}
