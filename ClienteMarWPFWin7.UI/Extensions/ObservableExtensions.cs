
using System.Collections.Generic;
using System.Collections.ObjectModel;
using ClienteMarWPFWin7.UI.ViewModels.ModelObservable;
using ClienteMarWPFWin7.Domain.Models.Dtos.EfectivoDtos;

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
                        Descripcion = item.Descripcion,
                        EntradaOSalida = item.EntradaOSalida,
                        Balance = item.Balance
                    });
                }
            }
            return mapped;
        }// fin de metodo




    }
}
