
#region Namespaces
using ClienteMarWPFWin7.Domain.Services.CincoMinutosService;
using ClienteMarWPFWin7.UI.ViewModels.Base;
using ClienteMarWPFWin7.UI.ViewModels.Commands.PegaMas;
using ClienteMarWPFWin7.UI.ViewModels.ModelObservable;
using ClienteMarWPFWin7.UI.State.Authenticators;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System;
#endregion

namespace ClienteMarWPFWin7.UI.Modules.PegaMas
{
    public class PegaMasViewModel : BaseViewModel
    {
        #region Campos

        private string d1;
        private string d2;
        private string d3;
        private string d4;
        private string d5;
        private string total_jugado;
        private const int maximo_digito = 26;
        private ObservableCollection<PegaMasApuestaObservable> jugadas;
        #endregion

        #region Propiedades

        public string D1
        {
            get => d1;
            set
            {
                d1 = value;

                int aux_d1;

                bool es = int.TryParse(d1, out aux_d1);

                if (es)
                {
                    if (aux_d1 > maximo_digito)
                    {
                        d1 = $"{maximo_digito}";
                    }
                }

                NotifyPropertyChanged(nameof(D1));
            }
        }
        public string D2
        {
            get => d2;
            set
            {
                d2 = value;

                int aux_d2;

                bool es = int.TryParse(d2, out aux_d2);

                if (es)
                {
                    if (aux_d2 > maximo_digito)
                    {
                        d2 = $"{maximo_digito}";
                    }
                }

                NotifyPropertyChanged(nameof(D2));
            }
        }
        public string D3
        {
            get => d3;
            set
            {
                d3 = value;

                int aux_d3;

                bool es = int.TryParse(d3, out aux_d3);

                if (es)
                {
                    if (aux_d3 > maximo_digito)
                    {
                        d3 = $"{maximo_digito}";
                    }
                }

                NotifyPropertyChanged(nameof(D3));
            }
        }
        public string D4
        {
            get => d4;
            set
            {
                d4 = value;

                int aux_d4;

                bool es = int.TryParse(d4, out aux_d4);

                if (es)
                {
                    if (aux_d4 > maximo_digito)
                    {
                        d4 = $"{maximo_digito}";
                    }
                }

                NotifyPropertyChanged(nameof(D4));
            }
        }
        public string D5
        {
            get => d5;
            set
            {
                d5 = value;

                int aux_d5;

                bool es = int.TryParse(d5, out aux_d5);

                if (es)
                {
                    if (aux_d5 > maximo_digito)
                    {
                        d5 = $"{maximo_digito}";
                    }
                }

                NotifyPropertyChanged(nameof(D5));
            }
        }
        public string TotalJugado
        {
            get => total_jugado;
            set
            {
                total_jugado = value;
                NotifyPropertyChanged(nameof(TotalJugado));
            }
        }
        public ObservableCollection<PegaMasApuestaObservable> Jugadas { get => jugadas; set { jugadas = value; NotifyPropertyChanged(nameof(Jugadas)); } }
        public IAuthenticator AutServicio { get; set; }
        public ICincoMinutosServices CincoMinServicio { get; set; }
        #endregion

        #region Comandos
        public ICommand AgregaApuestaCommand { get; set; }
        public ICommand VenderCommand { get; set; }
        public ICommand RemoverJugadasCommand { get; set; }
        public ICommand RemoverSoloUnaJugadaCommand { get; set; }
        public ICommand CalcularMontoTotalJugadoCommand { get; set; }
        #endregion

        #region Acciones
        public Action FocusEnPrimerInput { get; set; }
        public Action FocusEnUltimoInput { get; set; }
        public Action EscrolearHaciaAbajoGridApuesta { get; set; }
        #endregion

        public PegaMasViewModel(IAuthenticator s1,
                                ICincoMinutosServices s2)
        {
            AutServicio = s1;
            CincoMinServicio = s2;

            jugadas = new ObservableCollection<PegaMasApuestaObservable>();
            
            AgregaApuestaCommand = new AgregaApuestaCommand(this);
            VenderCommand = new VenderCommand(this);
            RemoverJugadasCommand = new RemoverJugadasCommand(this);
            RemoverSoloUnaJugadaCommand = new RemoverSoloUnaJugadaCommand(this);
            CalcularMontoTotalJugadoCommand = new CalcularMontoTotalJugadoCommand(this);
            CalcularMontoTotalJugadoCommand?.Execute(null);
        }

        public void TriggerJugadasUpd() => NotifyPropertyChanged(nameof(Jugadas));

    }//Clase
}
