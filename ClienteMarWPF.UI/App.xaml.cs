#region Namespaces

using Microsoft.Extensions.DependencyInjection;
using ClienteMarWPF.UI.Extensions;
using ClienteMarWPF.UI.Views.Controls;
using System;
using System.ComponentModel;
using System.Windows;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Markup;
using System.Globalization;

#endregion

namespace ClienteMarWPF.UI
{

    public partial class App : Application
    {

        private static readonly TimeSpan InactivityTimeLimit = TimeSpan.FromMinutes(5);


        private readonly BackgroundWorker worker = new BackgroundWorker();
        public Window VentanaPrincipal { get; set; }
        public WindowAppLoding VentanaCargando { get; set; }


        [STAThread]
        protected override void OnStartup(StartupEventArgs e)
        {
            // ** Overrinding default Application |Culture Info to spanish|
            Thread.CurrentThread.CurrentCulture = new CultureInfo("es-DO");
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("es-DO");
            FrameworkElement.LanguageProperty.OverrideMetadata(typeof(FrameworkElement), new FrameworkPropertyMetadata(XmlLanguage.GetLanguage(CultureInfo.CurrentCulture.IetfLanguageTag)));
            

            base.OnStartup(e);


            worker.DoWork += worker_DoWork;
            worker.RunWorkerCompleted += worker_RunWorkerCompleted;
            

            this.VentanaCargando = new WindowAppLoding();
            this.VentanaCargando.Show();


            worker.RunWorkerAsync();
        }



        private void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            Thread.Sleep(1000);
        }


        private void worker_RunWorkerCompleted(object sender,
                                               RunWorkerCompletedEventArgs e)
        {
            try
            {
                var serviceProvider = CreateServiceProvider();
                if (serviceProvider == null)
                {
                    Application.Current.Shutdown();
                    return;
                }

                Window window = serviceProvider.GetRequiredService<MainWindow>();
                Current.MainWindow = window;
                VentanaPrincipal = window;


                window.TimedShowDialog(InactivityTimeLimit);
                //window.Show();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private IServiceProvider CreateServiceProvider()
        {
            try
            {
                var services = new ServiceCollection();
                return services.CreateAppServicesProvider(this, this.VentanaCargando);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public void CerrarAplicacion()
        {
            if (VentanaPrincipal != null)
            {
                VentanaPrincipal.Close();
            }
                       
            Application.Current.Shutdown(); 
        }







    }// fin de Clase App

}// fin de Namespace ClienteMarWPF.UI



