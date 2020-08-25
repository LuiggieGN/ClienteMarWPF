using MarConnectCliente.IndividualModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarConnectCliente.RequestModels
{
    public class VentaCompraMayoreoRequestModel : BaseRequestModel
    {

        //Consorcio Que comprara en caso de venta, 
        //Consorcion Que Vendio en caso de Compra
        public string EstablecimientoReferencia { get; set; }

        //Corresponde al Codigo de la operacion enviado en la Venta
        public string CodigoOperacionReferencia { get; set; }
        public string NumeroAutenticacionReferencia { get; set; }

        public decimal MontoOperacion { get; set; }
        public Jugada DesgloseOperacion { get; set; }

        public string NumeroAutentificacionCalculado { get; set; }

        //metodo utilizado para devolver un objeto Anonimo y poder hacer un ToString() a la hora de guardar la transaccion
        public object VentaAnonima()
        {

            var ModelAnnon = new
            {
                EstablecimientoID = this.EstablecimientoID,
                EstablecimientoReferencia = this.EstablecimientoReferencia,
                DiaOperacion = this.DiaOperacion,
                FechaHoraSolicitud = this.FechaHoraSolicitud,
                CodigoOperacion = this.CodigoOperacion,
                DesgloseOperacion = this.DesgloseOperacion,
                TipoOperacion = this.TipoOperacion,
                MontoOperacion = this.MontoOperacion
            };

            return ModelAnnon;
        }
        public object VentaAnonimaWithNAUTC()
        {

            var ModelAnnon = new
            {
                EstablecimientoID = this.EstablecimientoID,
                EstablecimientoReferencia = this.EstablecimientoReferencia,
                DiaOperacion = this.DiaOperacion,
                FechaHoraSolicitud = this.FechaHoraSolicitud,
                CodigoOperacion = this.CodigoOperacion,
                DesgloseOperacion = this.DesgloseOperacion,
                TipoOperacion = this.TipoOperacion,
                MontoOperacion = this.MontoOperacion,
                NumeroAutentificacionCalculado = this.NumeroAutentificacionCalculado
            };

            return ModelAnnon;
        }

        //metodo utilizado para devolver un objeto Anonimo y poder hacer un ToString() a la hora de guardar la transaccion
        public object CompraAnonima()
        {

            var ModelAnnon = new
            {
                EstablecimientoID = this.EstablecimientoID,
                EstablecimientoReferencia = this.EstablecimientoReferencia,
                DiaOperacion = this.DiaOperacion,
                FechaHoraSolicitud = this.FechaHoraSolicitud,
                CodigoOperacion = this.CodigoOperacion,
                DesgloseOperacion = this.DesgloseOperacion,
                TipoOperacion = this.TipoOperacion,
                MontoOperacion = this.MontoOperacion,
                CodigoOperacionReferencia = this.CodigoOperacionReferencia,
                NumeroAutenticacionReferencia = this.NumeroAutenticacionReferencia
            };

            return ModelAnnon;
        }
        public object CompraAnonimaWithNAUTC()
        {

            var ModelAnnon = new
            {
                EstablecimientoID = this.EstablecimientoID,
                EstablecimientoReferencia = this.EstablecimientoReferencia,
                DiaOperacion = this.DiaOperacion,
                FechaHoraSolicitud = this.FechaHoraSolicitud,
                CodigoOperacion = this.CodigoOperacion,
                DesgloseOperacion = this.DesgloseOperacion,
                TipoOperacion = this.TipoOperacion,
                MontoOperacion = this.MontoOperacion,
                CodigoOperacionReferencia = this.CodigoOperacionReferencia,
                NumeroAutenticacionReferencia = this.NumeroAutenticacionReferencia,
                NumeroAutentificacionCalculado = this.NumeroAutentificacionCalculado
            };

            return ModelAnnon;
        }






    }
}
