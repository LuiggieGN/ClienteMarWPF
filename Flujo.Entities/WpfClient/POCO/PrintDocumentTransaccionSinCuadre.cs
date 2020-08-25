using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Flujo.Entities.WpfClient.POCO
{
    public class PrintDocumentTransaccionSinCuadre
    {
        public string BanContacto        { get; set; }
        public string BanDireccion       { get; set; }
        public string BanTransaccion     { get; set; }
        public string FechaTransaccion   { get; set; }
        public string BanMonto           { get; set; }
        public string Recibido__Por      { get; set; } //Banquera o Mensajero
        public bool   EsUnDeposito { get; set; }

    }
}
