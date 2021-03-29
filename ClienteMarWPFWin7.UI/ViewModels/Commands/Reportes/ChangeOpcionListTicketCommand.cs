﻿using ClienteMarWPFWin7.Domain.Services.ReportesService;
using ClienteMarWPFWin7.UI.Modules.Reporte;
using ClienteMarWPFWin7.UI.State.Authenticators;
using ClienteMarWPFWin7.UI.State.PinterConfig;
using ClienteMarWPFWin7.UI.ViewModels.ModelObservable;
using ClienteMarWPFWin7.Domain.MarPuntoVentaServiceReference;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace ClienteMarWPFWin7.UI.ViewModels.Commands.Reportes
{
    class ChangeOpcionListTicket : ActionCommand
    {

        private readonly ReporteViewModel ViewModel;
        private readonly IAuthenticator Autenticador;
        private readonly IReportesServices ReportesService;


        public ChangeOpcionListTicket(ReporteViewModel viewModel, IAuthenticator autenticador, IReportesServices reportesServices)
        {
            ViewModel = viewModel;
            Autenticador = autenticador;
            ReportesService = reportesServices;

            Action<object> comando = new Action<object>(ChangeOpcion);
            base.SetAction(comando);

        }

        private void ChangeOpcion(object parametro)
        {
            string OpcionSeleccionado =new ReporteView().ObtenerSeleccionTicket();
            ObservableCollection <ReporteListaTicketsObservable> observable = new ObservableCollection<ReporteListaTicketsObservable>() { };

            if (OpcionSeleccionado == "Válidos")
            {
                foreach (var ticket in ViewModel.ReporteAllDataListTicket)
                {
                    if (ticket.Nulo==false)
                    {
                        observable.Add(ticket);
                    }
                }
                ViewModel.ReporteListTicket = observable;
            }

            if (OpcionSeleccionado == "Nulos")
            {
                foreach (var ticket in ViewModel.ReporteAllDataListTicket)
                {
                    if (ticket.Nulo == true)
                    {
                        observable.Add(ticket);
                    }
                }
                ViewModel.ReporteListTicket = observable;
            }

            if (OpcionSeleccionado == "Todos" || OpcionSeleccionado == null)
            {
                ViewModel.ReporteListTicket = ViewModel.ReporteAllDataListTicket;
            }

        }
       
    }
}
