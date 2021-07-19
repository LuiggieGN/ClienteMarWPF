
#region Namespace
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
            vm = viewmodel;
            base.SetAction(new Action<object>(Vender));
        }

        private void Vender(object parametro)
        {
            if (vm.Jugadas.Count > 0)
            {
                try
                {
                    var ticket = LeerTicketCreado();
                    var producto = vm.CincoMinServicio.SetProducto("CincoMinutos", vm.AutServicio.CurrentAccount); // Ojo Aqui va PegaMas ;; pendiente configurar DB

                    if(producto == null)
                    {
                        DesplegarMensaje(mensaje: "El producto Pega Mas no esta disponible", encabezado: "Aviso !!");
                        vm.FocusEnPrimerInput?.Invoke();
                        return;
                    }

                    var respuesta = vm.CincoMinServicio.Apuesta(ticket, producto, vm.AutServicio.CurrentAccount);

                    if (respuesta.OK == false || (respuesta.Error != null && respuesta.Error != string.Empty))
                    {            
                        DesplegarMensaje(mensaje: respuesta.Error??"Ha ocurrido un error al procesar la operación", encabezado: "Error");
                        vm.FocusEnPrimerInput?.Invoke();
                        return;
                    }

                    if (respuesta.RespuestaApi.MensajeRespuesta != null && respuesta.RespuestaApi.MensajeRespuesta.ToLower().Contains("error"))
                    {                       
                        DesplegarMensaje(mensaje: respuesta.RespuestaApi.MensajeRespuesta, encabezado: "Error");
                        vm.FocusEnPrimerInput?.Invoke();
                        return;
                    }



                    ResetearTodo();
                    DesplegarMensaje(mensaje: "Jugadas realizadas satisfactoriamente.", encabezado: "Excelente");

                }
                catch
                {
                    DesplegarMensaje(mensaje: "Ha ocurrido un error al procesar la operación. Verificar Conexion de Internet. ", encabezado: "Error");
                    vm.FocusEnPrimerInput?.Invoke();
                }
            }
            else
            {
                DesplegarMensaje(mensaje: "No hay jugadas en la lista debe agregar al menos una jugada", encabezado: "Aviso !!");
                vm.FocusEnPrimerInput?.Invoke();
            }
        }




        private void DesplegarMensaje(string mensaje, string encabezado)
        {
            var ventana = Application.Current.MainWindow as ClienteMarWPFWin7.UI.MainWindow;
            ventana.MensajesAlerta(mensaje, encabezado);
        }

        private CincoMinutosRequestModel.TicketModel LeerTicketCreado()
        {
            int terminalid = vm.AutServicio?.BancaConfiguracion?.BancaDto?.BancaID ?? 0;
            int montoOperacion = (int)vm.Jugadas.ToList().Sum(j => j.Monto);

            string codigoharcodeado = "JMS";
            int sorteoharcodeado = 0; //100027
            int tipojugadaharcodeado = 4;

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

        }//LeerTicketCreado( )

        private void ResetearTodo()
        {
            vm.D1 = string.Empty;
            vm.D2 = string.Empty;
            vm.D3 = string.Empty;
            vm.D4 = string.Empty;
            vm.D5 = string.Empty;
            vm.Jugadas?.Clear();
            vm.CalcularMontoTotalJugadoCommand?.Execute(null);
            vm.FocusEnPrimerInput?.Invoke();
        }

    }//Clase
}
