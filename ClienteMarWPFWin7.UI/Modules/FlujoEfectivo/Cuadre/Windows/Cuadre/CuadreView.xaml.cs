using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using ClienteMarWPFWin7.Domain.Models.Dtos.EfectivoDtos;
using ClienteMarWPFWin7.UI.State.Authenticators;
using ClienteMarWPFWin7.UI.State.CuadreBuilders;

using System.Threading;
using System.Globalization;


namespace ClienteMarWPFWin7.UI.Modules.FlujoEfectivo.Cuadre.Windows.Cuadre
{

    public partial class CuadreView : Window
    {
        public Window ParentWindow;
        private IAuthenticator _aut;
        private ICuadreBuilder _builder;

        public CuadreView(Window parentWindow, object cuadreContext, IAuthenticator aut, ICuadreBuilder cuadreBuilder)
        {
            _aut = aut;
            _builder = cuadreBuilder;

            InitializeComponent();
            ParentWindow = parentWindow;
            DataContext = cuadreContext;

            inpMontoContado.Focus();
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            Left = ParentWindow.Left + (ParentWindow.Width - ActualWidth) / 2;
            Top = ParentWindow.Top + (ParentWindow.Height - ActualHeight) / 2;
        }

        private void OnCerrarVentanaClick(object sender, RoutedEventArgs e)
        {

            SetCajaDeBancaDisponible();
            Close();
        }

        private void OnCerrarVentanaClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            SetCajaDeBancaDisponible();
        }

        private void SetCajaDeBancaDisponible()
        {
            try
            {
                _builder.SetearCajaDisponibilidad(new CajaDisponibilidadDTO() { Cajaid = null, Bancaid = _aut.BancaConfiguracion.BancaDto.BancaID, Disponibilidad = true });
            }
            catch { }
        }




    }
}
