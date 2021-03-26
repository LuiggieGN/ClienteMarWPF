using ClienteMarWPFWin7.UI.ViewModels.ModelObservable;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ClienteMarWPFWin7.UI.Modules.Mensajeria
{
    /// <summary>
    /// Interaction logic for MensajeriaView.xaml
    /// </summary>
    public partial class MensajeriaView : UserControl
    {

        public static readonly DependencyProperty GetMensajesCommandProperty = DependencyProperty.Register("GetMensajesCommand", typeof(ICommand), typeof(MensajeriaView), new PropertyMetadata(null));

        public ICommand GetMensajesCommand
        {
            get { return (ICommand)GetValue(GetMensajesCommandProperty); }
            set { SetValue(GetMensajesCommandProperty, value); }
        }

        public MensajeriaView()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (GetMensajesCommand != null)
            {
                GetMensajesCommand.Execute(null);
            }
        }

    }
}
