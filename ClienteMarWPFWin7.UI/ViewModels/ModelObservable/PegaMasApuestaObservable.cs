
#region Namespaces
using ClienteMarWPFWin7.UI.ViewModels.Base;
using System.Globalization;
using System;
#endregion

namespace ClienteMarWPFWin7.UI.ViewModels.ModelObservable
{
    public class PegaMasApuestaObservable : BaseViewModel
    {
        #region Campos
        private string d1;
        private string d2;
        private string d3;
        private string d4;
        private string d5;
        private decimal monto;
        private int id;
        #endregion

        #region Digitos
        public string D1
        {
            get => d1;
            set
            {
                d1 = value; NotifyPropertyChanged(nameof(D1),
                                                  nameof(Tombo1),
                                                  nameof(Jugada));
            }
        }
        public string D2
        {
            get => d2;
            set
            {
                d2 = value; NotifyPropertyChanged(nameof(D2),
                                                  nameof(Tombo1),
                                                  nameof(Jugada));
            }
        }
        public string D3
        {
            get => d3;
            set
            {
                d3 = value; NotifyPropertyChanged(nameof(D3),
                                                  nameof(Tombo2),
                                                  nameof(Jugada));
            }
        }
        public string D4
        {
            get => d4;
            set
            {
                d4 = value; NotifyPropertyChanged(nameof(D4),
                                                  nameof(Tombo2),
                                                  nameof(Jugada));
            }
        }
        public string D5
        {
            get => d5;
            set
            {
                d5 = value; NotifyPropertyChanged(nameof(D5),
                                                  nameof(Tombo3),
                                                  nameof(Jugada));
            }
        }
        #endregion

        public string Tombo1 { get => $"{D1 ?? string.Empty}-{D2 ?? string.Empty}"; }
        public string Tombo2 { get => $"{D3 ?? string.Empty}-{D4 ?? string.Empty}"; }
        public string Tombo3 { get => $"{D5 ?? string.Empty}"; }
        public string Jugada { get => $"{Tombo1 ?? string.Empty}-{Tombo2 ?? string.Empty}-{Tombo3 ?? string.Empty}"; }
        public decimal Monto
        {
            get => monto;
            set
            {
                monto = value;
                NotifyPropertyChanged(nameof(Monto));
            }
        }
        public string MontoStr { get => $"${Monto.ToString("N", CultureInfo.InvariantCulture)}"; }
        public int Id
        {
            get => id;
            set
            {
                id = value; NotifyPropertyChanged(nameof(Id));
            }
        }
        public static PegaMasApuestaObservable NuevaApuesta(int id, int d1, int d2, int d3, int d4, int d5, decimal monto = 25)
        {
            char cero = '0';
            return new PegaMasApuestaObservable
            {
                D1 = d1.ToString().PadLeft(2, cero),
                D2 = d2.ToString().PadLeft(2, cero),
                D3 = d3.ToString().PadLeft(2, cero),
                D4 = d4.ToString().PadLeft(2, cero),
                D5 = d5.ToString().PadLeft(2, cero),
                Monto = monto,
                Id = id
            };
        }

    }//Clase
}
