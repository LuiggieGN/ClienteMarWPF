
using System;
using System.Globalization;
using ClienteMarWPFWin7.UI.ViewModels.Base;

namespace ClienteMarWPFWin7.UI.ViewModels.ModelObservable
{
    public class MovimientoObservable : BaseViewModel
    {
        #region Fields
        private int _cajaid;
        private string _categoria;
        private int _categoria_subtipo_id;
        private string _categoria_concepto;
        private int _orden;
        private long _movimientoid;
        private DateTime _fecha;
        private string _referencia;
        private string _descripcion;
        private decimal _entradaosalida;
        private decimal _balance;
        #endregion

        public int CajaId
        {
            get => _cajaid;
            set
            {
                _cajaid = value; NotifyPropertyChanged(nameof(CajaId));
            }
        }
        public string Categoria
        {
            get => _categoria;
            set
            {
                _categoria = value; NotifyPropertyChanged(nameof(Categoria));
            }
        }
        public int CategoriaSubTipoId
        {
            get => _categoria_subtipo_id;
            set
            {
                _categoria_subtipo_id = value; NotifyPropertyChanged(nameof(CategoriaSubTipoId));
            }
        }
        public string CategoriaConcepto
        {
            get => _categoria_concepto;
            set
            {
                _categoria_concepto = value; NotifyPropertyChanged(nameof(CategoriaConcepto));
            }
        }
        public int Orden
        {
            get => _orden;
            set
            {
                _orden = value; NotifyPropertyChanged(nameof(Orden));
            }
        }
        public long MovimientoId
        {
            get => _movimientoid;
            set
            {
                _movimientoid = value; NotifyPropertyChanged(nameof(MovimientoId));
            }
        }

        #region Fecha
        public DateTime Fecha
        {
            get => _fecha;
            set
            {
                _fecha = value; NotifyPropertyChanged(nameof(Fecha),
                                                      nameof(FYear),
                                                      nameof(FMes),
                                                      nameof(FTiempo));
            }
        }
        public string FYear => Fecha.ToString("yyyy", CultureInfo.CreateSpecificCulture("es"));
        public string FMes => Fecha.ToString("dd MMM", CultureInfo.CreateSpecificCulture("es")).ToUpper();
        public string FTiempo
        {
            get
            {
                var t = Fecha.ToString("hh:mm tt", CultureInfo.InvariantCulture);

                bool isMidnight = Fecha.TimeOfDay.Ticks == 0;

                return (isMidnight ? "" : t);
            }
        }
        #endregion
        
        public string Referencia
        {
            get => _referencia;
            set
            {
                _referencia = value; NotifyPropertyChanged(nameof(Referencia));
            }
        }
        public string Descripcion
        {
            get => _descripcion;
            set
            {
                _descripcion = value; NotifyPropertyChanged(nameof(Descripcion));
            }
        }

        #region EntradaOSalida
        public decimal EntradaOSalida
        {
            get => _entradaosalida;
            set
            {
                _entradaosalida = value; NotifyPropertyChanged(nameof(EntradaOSalida),
                                                               nameof(FEntradaOSalida));
            }
        }
        public string FEntradaOSalida
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
                        return "$ " + ((EntradaOSalida == 0) ? "0" : EntradaOSalida.ToString("#,##0.00", CultureInfo.CreateSpecificCulture("en-US")));
                    }
                    else
                    {
                        return "$ " + ((EntradaOSalida == 0) ? "0" : "-" + EntradaOSalida.ToString("#,##0.00", CultureInfo.CreateSpecificCulture("en-US")));
                    }
                }
            }
        }
        public string EntradaOSalidaForeground => EntradaOSalida == 0 ? "#000" : ( Categoria == "Ingreso" ? "#41B415" : "#F54141");
        #endregion

        #region Balance
        public decimal Balance
        {
            get => _balance;
            set
            {
                _balance = value; NotifyPropertyChanged(nameof(Balance),
                                                        nameof(FBalance));
            }
        }
        public string FBalance => "$ " + ((Balance == 0) ? "0" : Balance.ToString("#,##0.00", CultureInfo.CreateSpecificCulture("en-US")));
        #endregion

        public bool IsCuadre => CategoriaConcepto.Equals("Cuadre");

    } 
}



 