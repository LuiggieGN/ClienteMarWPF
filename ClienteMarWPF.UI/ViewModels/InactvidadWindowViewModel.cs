using ClienteMarWPF.UI.ViewModels.Base;
using ClienteMarWPF.UI.ViewModels.Commands;

using ClienteMarWPF.UI.Extensions;
using ClienteMarWPF.UI.State.Authenticators;

using System.Windows.Threading;
using System.Windows;
using System;
using System.Globalization;
using System.Windows.Input;

namespace ClienteMarWPF.UI.ViewModels
{
    public class InactvidadWindowViewModel : BaseViewModel
    {
    
        Action CerrarSesion;
        Action ContinuarSesion;
        DispatcherTimer _timer;
        int _segundosAContar = 30;



        public Window ControlVentanaInactividad { get; set; }
        public TimeSpan _segundosRestante { get; set; }
        public string SegundosAMostrar { get; set; }


        public ICommand CancelarCierreDeSesionCommand { get; }


        public InactvidadWindowViewModel(Action cierreSesion, TimeSpan bancaTiempoInactividad)
        {
            CerrarSesion = () =>
            {
                cierreSesion?.Invoke();

                ControlVentanaInactividad?.Close();
            };

            ContinuarSesion = () =>
            {
                if (_timer != null && _timer.IsEnabled)
                {
                    _timer.Stop();
                }

                //InactividadExtension.RemoveInactividad();

                //InactividadExtension.SetInactividad(ventana: Application.Current.MainWindow, tiempo: bancaTiempoInactividad);

                ControlVentanaInactividad?.Close();
            };


            _segundosRestante = TimeSpan.FromSeconds(_segundosAContar);


            _timer = new DispatcherTimer(new TimeSpan(0, 0, 1), DispatcherPriority.Normal, delegate
            {

                UpdateSegundosAMostrar(_segundosRestante.ToString("ss", CultureInfo.InvariantCulture));

                if (_segundosRestante == TimeSpan.Zero) // Valida Si Los Segundos han llegado a 0 seg
                {
                    _timer.Stop();

                    CerrarSesion?.Invoke();

                    return;
                }

                _segundosRestante = _segundosRestante.Add(TimeSpan.FromSeconds(-1));


            }, Application.Current.Dispatcher);


            CancelarCierreDeSesionCommand = new ActionCommand((object x) =>
            {
                ContinuarSesion?.Invoke();
            });


            _timer.Start();
        }




        private void UpdateSegundosAMostrar(string segundoActual)
        {
            SegundosAMostrar = $"Cancelar - {segundoActual} seg";
            NotifyPropertyChanged(nameof(SegundosAMostrar));
        }








    }//fin de clase InactvidadWindowViewModel
}
