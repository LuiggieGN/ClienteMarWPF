using System;
using System.Collections.Generic;
using System.Text;

namespace ClienteMarWPF.UI.Extensions
{
    public static class ListExtensions
    {

        public static void AgregaListadoCadaElemento<T>( this List<T> listadoActual, List<T> nuevoListado ) 
        {
            if (listadoActual != null && listadoActual.Count > 0)
            {
                foreach (var item in nuevoListado)
                {
                    listadoActual.Add(item);
                }                     

            }  
        
        }




    }
}
