using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Flujo.Entities.WpfClient.ResponseModels
{
    public class MovimientoResponseModel
    {
        public MovimientoResponseModel()
        {
            Fecha = DateTime.MinValue;
            Balance = 0;
            EntradaOSalida = 0;
        }


        public int CajaID { get; set; }
        public string Categoria { get; set; }
        public int CategoriaSubTipoID { get; set; }
        public string CategoriaConcepto { get; set; }
        public int Orden { get; set; }
        public int MovimientoID { get; set; }

        public DateTime Fecha { get; set; }
        public string FechaEnFormato
        {
            get
            {

                string f1 = Fecha.ToString("dd MMM yyyy", CultureInfo.CreateSpecificCulture("es"));

                string f2 = Fecha.ToString("hh:mm tt", CultureInfo.InvariantCulture);

                bool isMidnight = Fecha.TimeOfDay.Ticks == 0;

                f1 = f1 + " " + (isMidnight ? "" : f2);

                return f1;

            }
        }



        public string Referencia { get; set; }

        public string ReferenciaFormato
        {
            get
            {
                return "Referencia : " + Referencia;
            }
        }


        public string Descripcion
        {
            get
            {

                if (CategoriaConcepto.Equals("Cuadre"))
                {
                    string cuadreDes = " Balance al cuadre: " + System.Environment.NewLine + $" {this.BalanceEnFormato}";


                    return cuadreDes;
                }
                else
                {
                    return descripcion;
                }

            }
            set
            {
                descripcion = value;
            }
        }

        public double EntradaOSalida { get; set; }
        public string EntradaOSalidaEnFormato
        {
            get
            {

                if (CategoriaConcepto.Equals("Cuadre"))
                {
                    return "";
                }

                if (Categoria == "Ingreso")
                {
                    return "$ " +
                                 (
                                    (EntradaOSalida == 0) ? "0"
                                    :
                                    (
                                       EntradaOSalida.ToString("#,##0.00", CultureInfo.CreateSpecificCulture("en-US"))
                                    )
                                 );
                }
                else
                {
                    return "$ " +
                                 (
                                     (EntradaOSalida == 0)
                                   ? "0"
                                   : "-" + EntradaOSalida.ToString("#,##0.00", CultureInfo.CreateSpecificCulture("en-US"))

                                 );
                }
            }
        }

        public double Balance { get; set; }
        public string BalanceEnFormato
        {
            get
            {
                return "$ " + (
                                  (Balance == 0) ? "0" : Balance.ToString("#,##0.00", CultureInfo.CreateSpecificCulture("en-US"))
                              );
            }
        }


        private string descripcion;

    }
}