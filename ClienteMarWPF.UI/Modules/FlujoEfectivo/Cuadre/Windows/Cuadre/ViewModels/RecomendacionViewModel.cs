
using System.Globalization;
using ClienteMarWPF.UI.ViewModels.Base;

namespace ClienteMarWPF.UI.Modules.FlujoEfectivo.Cuadre.Windows.Cuadre.ViewModels
{
    public class RecomendacionViewModel : BaseViewModel
    {
        #region Fields
        private int _codigo;
        private string _tipo;
        private decimal _montoRecomendado;
        private string _icono;
        private string _descripcion;
        private bool _estaCargando;
        #endregion

        #region Properties

        public bool EstaCargando
        {
            get => _estaCargando;
            set
            {
                _estaCargando = value; NotifyPropertyChanged(nameof(EstaCargando),
                                                            nameof(Codigo),
                                                            nameof(Tipo),
                                                            nameof(MontoRecomendado),
                                                            nameof(FMontoRecomendado),
                                                            nameof(Icono),
                                                            nameof(Descripcion),
                                                            nameof(Color));
            }
        }

        public int Codigo
        {
            get => _codigo;
            set { _codigo = value; NotifyPropertyChanged(nameof(Codigo)); }
        }
        public string Tipo
        {
            get => _tipo;
            set { _tipo = value; NotifyPropertyChanged(nameof(Tipo)); }
        }
        public decimal MontoRecomendado
        {
            get => _montoRecomendado;
            set
            {
                _montoRecomendado = value; NotifyPropertyChanged(nameof(MontoRecomendado),
                                                                 nameof(FMontoRecomendado),
                                                                 nameof(Color));
            }
        }
        public string FMontoRecomendado => EstaCargando ? string.Empty : MontoRecomendado.ToString("$ #,##0.00", CultureInfo.CreateSpecificCulture("en-US"));
        public string Icono
        {
            get => _icono;
            set { _icono = value; NotifyPropertyChanged(nameof(Icono)); }
        }
        public string Descripcion
        {
            get => _descripcion;
            set { _descripcion = value; NotifyPropertyChanged(nameof(Descripcion)); }
        }
        public string Color
        {
            get
            {
                string color = "#000";

                if (Codigo == 1) //Recomiendo Retirar
                {
                    color = "#E02D1B"; //ROJO
                }
                else if (Codigo == 0) //Recomiendo Depositar
                {
                    color = "#28A745";  //VERDE
                }
                return color;
            }
        }

        #endregion


        public RecomendacionViewModel()
        {
            _icono = "None";
            _descripcion = string.Empty;
            _estaCargando = true;
        }


    }
}
