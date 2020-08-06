using System;
using System.Collections.Generic;
using System.Text;

namespace ClienteMarWPF.UI.Extensions
{
    public static class ListExtensions
    {

        public static void AgregaListadoCadaElemento<T>( this List<T> listadoActual, List<T> nuevoListado ) 
        {
            if (nuevoListado != null && nuevoListado.Count > 0)
            {
                foreach (T item in nuevoListado)
                {
                    listadoActual.Add(item);
                }
            }       
        }




    }
}
