
using System.Globalization;
using ClienteMarWPFWin7.UI.ViewModels.Base;

namespace ClienteMarWPFWin7.UI.Modules.FlujoEfectivo.Cuadre.Windows.Cuadre.ViewModels
{
    public class ArqueoResultanteViewModel : BaseViewModel
    {
        #region Fields
        private int _codigo;
        private string _arqueo;
        private decimal _dineroResultante;
        private bool _estaCargando;
        #endregion

        #region Properties

        public bool EstaCargando
        {
            get => _estaCargando;
            set
            {
                _estaCargando = value; NotifyPropertyChanged(nameof(EstaCargando),
                                                            nameof(DineroResultante),
                                                            nameof(FDineroResultante),
                                                            nameof(ColorFDineroResultante));
            }
        }

        public int Codigo
        {
            get => _codigo;
            set { _codigo = value; NotifyPropertyChanged(nameof(Codigo)); }
        }

        public string Arqueo
        {
            get => _arqueo;
            set { _arqueo = value; NotifyPropertyChanged(nameof(Arqueo)); }
        }

        public decimal DineroResultante
        {
            get => _dineroResultante;
            set
            {
                _dineroResultante = value; NotifyPropertyChanged(nameof(DineroResultante),
                                                                 nameof(FDineroResultante),
                                                                 nameof(ColorFDineroResultante));
            }
        }

        public string FDineroResultante => EstaCargando ? "... Cargando" : DineroResultante.ToString("$ #,##0.00", CultureInfo.CreateSpecificCulture("en-US"));
        public string ColorFDineroResultante
        {
            get
            {
                string color = "#000";

                if (Codigo == 0)
                {
                    color = "#E02D1B";
                }
                else if (Codigo == 1)
                {
                    color = "#28A745";
                }
                return color;
            }
        }

        #endregion

        public ArqueoResultanteViewModel()
        {
            _arqueo = "Arqueo";
            _codigo = -1;
            _estaCargando = true;
        }


    }
}
