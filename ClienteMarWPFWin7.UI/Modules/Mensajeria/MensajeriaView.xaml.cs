
#region Namespaces

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

#endregion

namespace ClienteMarWPFWin7.UI.Modules.Mensajeria
{
    public partial class MensajeriaView : UserControl
    {
        public Action EscrolearHaciaAbajo { get; set; }

        public static readonly DependencyProperty GetMensajesCommandProperty = DependencyProperty.Register("GetMensajesCommand", typeof(ICommand), typeof(MensajeriaView), new PropertyMetadata(null));

        public ICommand GetMensajesCommand
        {
            get { return (ICommand)GetValue(GetMensajesCommandProperty); }
            set { SetValue(GetMensajesCommandProperty, value); }
        }


        public MensajeriaView()
        {
            InitializeComponent();

            EscrolearHaciaAbajo = () =>
             {
                 if (VisualTreeHelper.GetChildrenCount(listMensajeChat) > 0)
                 {
                     Border borde = VisualTreeHelper.GetChild(listMensajeChat, 0) as Border;

                     if (borde != null)
                     {
                         ScrollViewer scroll = VisualTreeHelper.GetChild(borde, 0) as ScrollViewer;
                         
                         if (scroll != null)
                         {
                             scroll.ScrollToBottom();
                         }

                     }//fin de If

                 }//fin de If

             };//fin de EscrolearHaciaAbajo( )
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            var vm = DataContext as MensajeriaViewModel;

            if (vm != null)
            {
                vm.EscrolearHaciaAbajo = this.EscrolearHaciaAbajo;
            }

            if (GetMensajesCommand != null)
            {
                GetMensajesCommand.Execute(null);
            }

            txtChatMensaje.Focus();
            txtChatMensaje.UpdateLayout();
        }

    }
}
