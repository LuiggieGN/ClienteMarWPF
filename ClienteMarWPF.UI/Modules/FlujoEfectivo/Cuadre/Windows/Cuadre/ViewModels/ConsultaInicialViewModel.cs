
using System.Globalization;
using ClienteMarWPF.UI.ViewModels.Base;

namespace ClienteMarWPF.UI.Modules.FlujoEfectivo.Cuadre.Windows.Cuadre.ViewModels
{
    public class ConsultaInicialViewModel : BaseViewModel
    {
        #region Fields
        private decimal _bancaBalance;
        private decimal _bancaBalanceMinimo;
        private decimal _bancaDeuda;
        #endregion

        #region Properties

        public bool EstaCargando { get; set; } = true;

        public decimal BancaBalance
        {
            get => _bancaBalance;
            set
            {
                _bancaBalance = value;
                NotifyPropertyChanged(nameof(BancaBalance),
                                      nameof(FBancaBalance),
                                      nameof(ForegroundBancaBalance));
            }
        }

        public decimal BancaBalanceMinimo
        {
            get => _bancaBalanceMinimo;
            set
            {
                _bancaBalanceMinimo = value;
                NotifyPropertyChanged(nameof(BancaBalanceMinimo),
                                      nameof(FBalanceMinimo),
                                      nameof(ForegroundBancaBalanceMinimo));
            }
        }

        public decimal BancaDeuda
        {
            get => _bancaDeuda;
            set
            {
                _bancaDeuda = value;
                NotifyPropertyChanged(nameof(BancaDeuda),
                                      nameof(FDeuda),
                                      nameof(ForegroundBancaDeuda));
            }
        }
        public string FBancaBalance => EstaCargando? "... Cargando": BancaBalance.ToString("$ #,##0.00", CultureInfo.CreateSpecificCulture("en-US"));
        public string FBalanceMinimo => EstaCargando ? "... Cargando":BancaBalanceMinimo.ToString("$ #,##0.00", CultureInfo.CreateSpecificCulture("en-US"));
        public string FDeuda => EstaCargando ? "... Cargando":BancaDeuda.ToString("$ #,##0.00", CultureInfo.CreateSpecificCulture("en-US"));

        public string ForegroundBancaBalance
        {
            get
            {
                string color = "#000";

                if (BancaBalance < 0)
                {
                    color = "#E02D1B";
                }
                else if(BancaBalance > 0)
                {
                    color = "#28A745";
                }
                return color;
            }
        }

        public string ForegroundBancaBalanceMinimo
        {
            get
            {
                string color = "#000";

                if (BancaBalanceMinimo < 0)
                {
                    color = "#E02D1B";
                }
                else if (BancaBalanceMinimo > 0)
                {
                    color = "#28A745";
                }
                return color;
            }
        }


        public string ForegroundBancaDeuda
        {
            get
            {
                string color = "#000";

                if (BancaDeuda != 0)
                {
                    color = "#E02D1B";
                }
 
                return color;
            }
        }
        #endregion





    }
}
