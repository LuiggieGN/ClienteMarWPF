
using System.Collections.Generic;
using System.Collections.ObjectModel;
using ClienteMarWPFWin7.UI.ViewModels.ModelObservable;
using ClienteMarWPFWin7.Domain.Models.Dtos.EfectivoDtos;
using ClienteMarWPFWin7.Domain.MarPuntoVentaServiceReference;
using System;

namespace ClienteMarWPFWin7.UI.Extensions
{
    public static class ObservableExtensions
    {
        public static List<MovimientoObservable> ToListOfMovimientoObservable(this List<MovimientoDTO> source)
        {
            var mapped = new List<MovimientoObservable>();

            if (source != null && source.Count > 0)
            {
                foreach (var item in source)
                {
                    mapped.Add(new MovimientoObservable()
                    {
                        CajaId = item.CajaID,
                        Categoria = item.Categoria,
                        CategoriaSubTipoId = item.CategoriaSubTipoID,
                        CategoriaConcepto = item.CategoriaConcepto,
                        Orden = item.Orden,
                        MovimientoId = item.MovimientoID,
                        Fecha = item.Fecha,
                        Referencia = item.Referencia,
                        Descripcion = item.Descripcion == null ? string.Empty: item.Descripcion.Replace("&nbsp;", " ").Replace("<br/>", Environment.NewLine+ Environment.NewLine).Replace("Bal.", Environment.NewLine + "Bal.").Replace(Environment.NewLine+Environment.NewLine+"Com:","Com:") ,
                        EntradaOSalida = item.EntradaOSalida,
                        Balance = item.Balance
                    });
                }
            }
            return mapped;
        }// fin de metodo


        public static ObservableCollection<MAR_Bet> ToObservableMarBet(this List<MAR_Bet> source)
        {
            var mapped = new ObservableCollection<MAR_Bet>();

            if (source != null && source.Count > 0)
            {
                foreach (var item in source)
                {
                    mapped.Add(new MAR_Bet()
                    {
                        Cedula = item.Cedula,
                        Cliente = item.Cliente,
                        Costo = item.Costo,
                        Err = item.Err,
                        Grupo = item.Grupo,
                        Items = item.Items,
                        Loteria = item.Loteria,
                        Nulo = item.Nulo,
                        Pago = item.Pago,
                        Solicitud = item.Solicitud,
                        StrFecha = item.StrFecha,
                        StrHora = item.StrHora,
                        Ticket = item.Ticket,
                        TicketNo = item.TicketNo
                    });
                }
            }
            return mapped;
        }// fin de metodo




    }
}
