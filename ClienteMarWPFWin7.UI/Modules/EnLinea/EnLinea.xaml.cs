using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ClienteMarWPFWin7.UI.Modules.EnLinea
{
    /// <summary>
    /// Lógica de interacción para EnLinea.xaml
    /// </summary>
    public partial class EnLinea : UserControl
    {
        //private BackgroundWorker _bgWorker = new BackgroundWorker();

        //private int _workerState;

        //public int WorkerState
        //{
        //    get
        //    {
        //        return _workerState;
        //    }
        //    set
        //    {
        //        _workerState = value;
        //        if (PropertyChanged != null)
        //            PropertyChanged(this, new PropertyChangedEventArgs("WorkerState"));
        //    }
        //}
        public EnLinea()
        {
            InitializeComponent();
            //browser.Visibility = Visibility.Hidden;
            //barra.Visibility = Visibility.Visible;

            //_bgWorker.DoWork += (s, e) =>
            //{
            //    for (int i = 0; i <= 100; i++)
            //    {
            //        System.Threading.Thread.Sleep(100);
            //        WorkerState = i;
            //    }
            //    MessageBox.Show("Estoy aqui!!");
                
            //};

            
            //_bgWorker.RunWorkerAsync();
            //barra.Visibility = Visibility.Hidden;
            //browser.Visibility = Visibility.Visible;
        }

    //#region PropertyChangedMember

    //public event PropertyChangedEventHandler PropertyChanged;

    //#endregion

    }
}
