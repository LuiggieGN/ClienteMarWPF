#region Namespaces
using System;
using System.Windows.Input;
using ClienteMarWPFWin7.UI.ViewModels;
using ClienteMarWPFWin7.UI.ViewModels.Base;
using ClienteMarWPFWin7.UI.ViewModels.Commands;
#endregion


namespace ClienteMarWPFWin7.UI.Modules.Reporte.Modal
{
    public class DialogoReporteViewModel : BaseViewModel
    {
        #region Fields
        private bool _muestro;
        private bool _soloTotales;
        private string _fechainicio;
        private string _fechaFin;

        #endregion

        #region Properties
        public bool MuestroDialogo
        {
            get
            {
                return _muestro;
            }
            private set
            {
                _muestro = value; NotifyPropertyChanged(nameof(MuestroDialogo));
            }
        }
 
        #endregion
 
        
        #region Comandos
        public ICommand AceptarCommand { get; }
        public ICommand CancelarCommand { get; }
        #endregion

        public DialogoReporteViewModel(ActionCommand cancelar, ActionCommand aceptar)
        {
            
            CancelarCommand = cancelar;
            
            AceptarCommand = aceptar;

            FechaInicio = Convert.ToDateTime(DateTime.Now).AddDays(-7).ToString();
            FechaFin = Convert.ToDateTime(DateTime.Now).ToString();
            SoloTotales = true;


        }

        public string FechaInicio
        {
            get { return _fechainicio; }
            set { _fechainicio = value; NotifyPropertyChanged(nameof(FechaInicio)); }
        }

        public string FechaFin
        {
            get { return _fechaFin; }
            set { _fechaFin = value; NotifyPropertyChanged(nameof(FechaFin)); }
        }

        public bool SoloTotales
        {
            get { return _soloTotales; }
            set { _soloTotales = value; NotifyPropertyChanged(nameof(SoloTotales)); }
        }

        public void Mostrar()
        {
            MuestroDialogo = true;
        }

        public void Ocultar()
        {
            MuestroDialogo = false;
        }

    } // fin de clase
}
