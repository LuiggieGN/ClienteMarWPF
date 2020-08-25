using System;
using System.Globalization;
using System.Collections.Generic;

namespace Flujo.Entities.WebClient.POCO
{
    [Serializable()]
    public class MovimientoDatos
    {
        public MovimientoDatos()
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
                return Fecha.ToString("dd MMM yyyy hh:mm tt", CultureInfo.CreateSpecificCulture("es"));
            }
        }

        public string FechaEnFormato__FechaParte
        {
            get
            {
                return Fecha.ToString("dd MMM yyyy", CultureInfo.CreateSpecificCulture("es"));
            }
        }

        public string FechaEnFormato__TiempoParte
        {
            get
            {
                var t = Fecha.ToString("hh:mm tt", CultureInfo.CreateSpecificCulture("es"));

                return (t.Equals("12:00 AM") ? "" : t);
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

        public string Descripcion { get; set; }

        public double EntradaOSalida { get; set; }
        public string EntradaOSalidaEnFormato
        {
            get
            {
                if (CategoriaConcepto.Equals("Cuadre"))
                {
                    return "";
                }
                else
                {
                    if (Categoria == "Ingreso")
                    {
                        return "$ " + ((EntradaOSalida == 0) ? "0" : EntradaOSalida.ToString("#,##0.00", CultureInfo.CreateSpecificCulture("en-US"))); //(string.Format("{0:n}", EntradaOSalida))
                    }
                    else
                    {
                        return "$ " + ((EntradaOSalida == 0) ? "0" : "-" + EntradaOSalida.ToString("#,##0.00", CultureInfo.CreateSpecificCulture("en-US"))); // (string.Format("{0:n}", EntradaOSalida))
                    }
                }
            }
        }

        public double Balance { get; set; }
        public string BalanceEnFormato
        {
            get
            {
                return "$ " + ((Balance == 0) ? "0" : Balance.ToString("#,##0.00", CultureInfo.CreateSpecificCulture("en-US"))); // (string.Format("{0:n}", Balance))
            }
        }

        public string CategoriaClase
        {
            get
            {
                return EntradaOSalida == 0 ? "Indefinido" : Categoria;

            }
        }


        public string FYear
        {
            get
            {
                return Fecha.ToString("yyyy", CultureInfo.CreateSpecificCulture("es"));
            }

        }

        public string FMes
        {
            get
            {
                return Fecha.ToString("dd MMM", CultureInfo.CreateSpecificCulture("es")).ToUpper();
            }
        }

        public string FTiempo
        {
            get
            {
                var t = Fecha.ToString("hh:mm tt", CultureInfo.InvariantCulture);

                bool isMidnight = Fecha.TimeOfDay.Ticks == 0;

                return (isMidnight ? "" : t);
            }
        }


        public string CategoriaRef { get; set; }
        public string CategoriaRefNombre { get; set; }

    }//Fin Clase MovimientoDatos

}//Fin Namespace MovimientosDatos