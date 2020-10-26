using System;
using System.Linq;
using System.Windows.Input;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ServiceModel.Channels;
using System.Windows;
using FlujoCustomControl.Code.BussinessLogic;

using FlujoCustomControl.Helpers;
using Flujo.Entities.WpfClient.Enums;
using Flujo.Entities.WpfClient.POCO;
using Flujo.Entities.WpfClient.RequestModel;
using Flujo.Entities.WpfClient.ResponseModels;

using FlujoCustomControl.ViewModels.Commands;
using FlujoCustomControl.ViewModels.CommonViews;

using Flujo.DataAccess.FlujoRepositories.WpfClient.Helpers;


namespace FlujoCustomControl.ViewModels
{
    public partial class ConsultaMovimientoViewModel : CommonBase
    {
        private void InicializaComponentes()
        {
            this.ColeccionPeriodosDeTiempo = new ObservableCollection<PeriodoTiempo>();

            this.ColeccionPeriodosDeTiempo.AddRange(FechaHelper.DefaultPeriodosDeTiempo());

            this.PeriodoTiempo = this.ColeccionPeriodosDeTiempo[0];  // Default : Consulta los movimientos de Hoy 

            this.IsVisibleSeccionPeriodoCombobox = true;

            this.IsVisibleSeccionRangoFecha = false;

            this.BtnCerrarFiltroCommand = new RelayCommand(BtnCerrarFiltro_Click);

            this.BtnFiltrarPorRangoFechaCommand = new RelayCommand(BtnFiltrarPorRangoFecha_Click);

            this.BtnRefrescarCommand = new RelayCommand(BtnRefrescar__Click);

            //... Consultando movimientos de HOY            
            ConsultarMovimientosSeleccion(this.PeriodoTiempo);   //Consulta los movimientos segubn la seleccion

        }//Fin Inicializa Componentes()
    }

    public partial class ConsultaMovimientoViewModel
    {

        private void ConsultarMovimientosSeleccion(PeriodoTiempo periodo)
        {
            int enumPeriodoCode = (int)periodo.PeriodoEnum; Dictionary<string,object> p = new Dictionary<string, object>();

            if (periodo != null)
            {
                if (enumPeriodoCode == (int)PeriodoTiempoDefaults.Hoy)
                {
                    p.Add("@CajaID", ReadOnly_BancaCajaID);
                    p.Add("@FechaInicio", DateTime.Now.ToString("yyyyMMdd"));
                    p.Add("@FechaFin", DateTime.Now.AddDays(1).ToString("yyyyMMdd"));

                    if (this.MovimientoPagerView == null)
                    {
                        this.MovimientoPagerView = new MovimientoPagerViewModel(p, SelectView.SelectRangoFechaMovimientos, "Orden", 20);
                    }
                    else
                    {
                        this.MovimientoPagerView.ResetPager(0, 20, "Orden", true, 0, SelectView.SelectRangoFechaMovimientos, p);
                    }
                }
                else if (enumPeriodoCode == (int)PeriodoTiempoDefaults.Periodo_7_dias_atras)
                {
                    p.Add("@CajaID", ReadOnly_BancaCajaID);
                    p.Add("@FechaInicio", DateTime.Now.AddDays(-6).ToString("yyyyMMdd"));
                    p.Add("@FechaFin", DateTime.Now.AddDays(1).ToString("yyyyMMdd"));

                    if (MovimientoPagerView == null)
                    {
                        this.MovimientoPagerView = new MovimientoPagerViewModel(p, SelectView.SelectRangoFechaMovimientos, "Orden", 20);
                    }
                    else
                    {
                        this.MovimientoPagerView.ResetPager(0, 20, "Orden", true, 0, SelectView.SelectRangoFechaMovimientos, p);
                    }

                }
                else if (enumPeriodoCode == (int)PeriodoTiempoDefaults.Periodo_30_dias_atras)
                {
                    p.Add("@CajaID", ReadOnly_BancaCajaID);
                    p.Add("@FechaInicio", DateTime.Now.AddDays(-29).ToString("yyyyMMdd"));
                    p.Add("@FechaFin", DateTime.Now.AddDays(1).ToString("yyyyMMdd"));

                    if (this.MovimientoPagerView == null)
                    {
                        this.MovimientoPagerView = new MovimientoPagerViewModel(p, SelectView.SelectRangoFechaMovimientos, "Orden", 20);
                    }
                    else
                    {
                        this.MovimientoPagerView.ResetPager(0, 20, "Orden", true, 0, SelectView.SelectRangoFechaMovimientos, p);
                    }

                }

                else if (enumPeriodoCode == (int)PeriodoTiempoDefaults.Rango_de_fecha)
                {
                    p.Add("@CajaID", ReadOnly_BancaCajaID);

                    #region  -> Estableciendo Rango de Fecha a Consultar Movimientos

                    if (FechaInicio == null || FechaFin == null)
                    {
                        p.Add("@FechaInicio", DateTime.MinValue.ToString("yyyyMMdd"));
                        p.Add("@FechaFin", DateTime.MinValue.ToString("yyyyMMdd"));
                    }
                    else
                    {
                        int comparison = DateTime.Compare(FechaInicio.Value, FechaFin.Value);

                        if (comparison < 0)              //FechaInicio < FechaFin
                        {
                            p.Add("@FechaInicio", FechaInicio.Value.ToString("yyyyMMdd"));
                            p.Add("@FechaFin", FechaFin.Value.AddDays(1).ToString("yyyyMMdd"));
                        }
                        else if (comparison == 0)   //Fecha Inicio = FechaFin
                        {
                            p.Add("@FechaInicio", FechaInicio.Value.ToString("yyyyMMdd"));
                            p.Add("@FechaFin", FechaInicio.Value.AddDays(1).ToString("yyyyMMdd"));
                        }
                        else                                       //Fecha Inicio  > Fecha Fin
                        {
                            p.Add("@FechaInicio", FechaFin.Value.ToString("yyyyMMdd"));
                            p.Add("@FechaFin", FechaInicio.Value.AddDays(1).ToString("yyyyMMdd"));
                        }
                    }

                    #endregion

                    #region -> Buscando Movimientos Segun Rango de Fechas Establecidos

                    if (MovimientoPagerView == null)
                    {
                        this.MovimientoPagerView = new MovimientoPagerViewModel(p, SelectView.SelectRangoFechaMovimientos, "Orden", 20);
                    }
                    else
                    {
                        this.MovimientoPagerView.ResetPager(0, 20, "Orden", true, 0, SelectView.SelectRangoFechaMovimientos, p);
                    }

                    #endregion


                }

            }
        }
        private void BtnCerrarFiltro_Click(object parameter)
        {
            // +Reset |Rango de Fecha|  seleccion
            this.IsVisibleSeccionRangoFecha = false;
            this.FechaInicio = null;
            this.FechaFin = null;

            // +Se muestra seccion seleccion periodo combobox [ Hoy, 30 dias atras, 60 dias atras, rango de fecha]

            this.IsVisibleSeccionPeriodoCombobox = true;                         // Se muestra la seccion de |Periodo combobox|
            this.PeriodoTiempo = this.ColeccionPeriodosDeTiempo[0];    //  Se selecciona el primer elemento para consultar los movimientos de Hoy
            ConsultarMovimientosSeleccion(this.PeriodoTiempo);            // Consulto los Movimientos de Hoy realizados por esta Banca
        }

        private void MostrarSeccionRangoFecha()
        {
            // +Reset |Periodo Combobox|            
            this.IsVisibleSeccionPeriodoCombobox = false;
            //this.PeriodoTiempo = this.ColeccionPeriodosDeTiempo[0];

            // + Se muestra seccion de  |Rango de Fecha|
            this.IsVisibleSeccionRangoFecha = true;
            this.FechaInicio = null;
            this.FechaFin = null;

            ConsultarMovimientosSeleccion(this.ColeccionPeriodosDeTiempo[3]);   // Se consulta los movimientos especificando un Periodo de Tiempo equivalenta a [Rango de Fecha]            
        }

        private void BtnFiltrarPorRangoFecha_Click(object parameter)
        {
            ConsultarMovimientosSeleccion(this.ColeccionPeriodosDeTiempo[3]); // Consultando movimiento por rango de fecha
        }


        private void BtnRefrescar__Click(object parameter)
        {
            ConsultarMovimientosSeleccion(this.PeriodoTiempo);
        }

    }

    public partial class ConsultaMovimientoViewModel
    {
        public int UserID
        {
            get { return _UserID; }
            set { _UserID = value; }
        }

        public ObservableCollection<PeriodoTiempo> ColeccionPeriodosDeTiempo { get; set; }
        public PeriodoTiempo PeriodoTiempo
        {

            get
            {
                return perioTiempo;
            }

            set
            {
                if (value != null)
                {
                    perioTiempo = value; OnPropertyChanged("PeriodoTiempo");

                    if ((int)perioTiempo.PeriodoEnum == (int)PeriodoTiempoDefaults.Rango_de_fecha)
                    {
                        MostrarSeccionRangoFecha();
                    }
                    else
                    {
                        ConsultarMovimientosSeleccion(PeriodoTiempo);   //Consulta los movimientos segubn la seleccion
                    }

                }
            }
        }
        public MovimientoPagerViewModel MovimientoPagerView
        {
            get
            {
                return movimientoPagerView;
            }
            set
            {
                if (value != null)
                {
                    movimientoPagerView = value; OnPropertyChanged("MovimientoPagerView");
                }
            }
        }

        public DateTime? FechaInicio
        {
            get
            {
                return fechaInicio;
            }
            set
            {
                fechaInicio = value; OnPropertyChanged("FechaInicio");
            }
        }
        public DateTime? FechaFin
        {
            get
            {
                return fechaFin;
            }
            set
            {
                fechaFin = value; OnPropertyChanged("FechaFin");
            }
        }

        public bool IsVisibleSeccionPeriodoCombobox
        {
            get
            {
                return isVisibleSeccionPeriodoCombobox;
            }
            set
            {
                isVisibleSeccionPeriodoCombobox = value; OnPropertyChanged("IsVisibleSeccionPeriodoCombobox");
            }
        }
        public bool IsVisibleSeccionRangoFecha
        {
            get
            {
                return isVisibleSeccionRangoFecha;
            }
            set
            {
                isVisibleSeccionRangoFecha = value; OnPropertyChanged("IsVisibleSeccionRangoFecha");
            }
        }


        public ICommand BtnCerrarFiltroCommand { get; set; }
        public ICommand BtnFiltrarPorRangoFechaCommand { get; set; }
        public ICommand BtnRefrescarCommand { get; set; }
    }

    public partial class ConsultaMovimientoViewModel
    {
        public ConsultaMovimientoViewModel(int pBancaID, int pUsuerID, Action<string> accionMainTitle, Action<string> accionMainBancaBalance)
        {

            try
            {
                this.ReanOnly_BancaID = pBancaID;
                this._UserID = pUsuerID;
                this.ReadOnly_BancaCajaID = CajaLogic.GetBancaCajaID(pBancaID);

                Set_Windows_MainLabelTitle = accionMainTitle;
                Set_Windows_MainLabelBancaBalanceActual = accionMainBancaBalance;
                Set_Windows_MainLabelTitle?.Invoke("Consulta de Movimientos");// Seteando el titulo principal de la pantalla principal

                string strBancaBalance = CajaLogic.GetBancaBalanceActual(pBancaID);
                Set_Windows_MainLabelBancaBalanceActual?.Invoke("|Balance : " + strBancaBalance); // Seteando el balance de la Banca desde la pantalla principal
                this.InicializaComponentes();
            }
            catch (Exception e)
            {
                throw e;
            }
          
        }
    }

    public partial class ConsultaMovimientoViewModel
    {
        private readonly int ReanOnly_BancaID;
        private readonly int ReadOnly_BancaCajaID;
        private int _UserID;
        private PeriodoTiempo perioTiempo;
        private MovimientoPagerViewModel movimientoPagerView;

        private DateTime? fechaInicio;
        private DateTime? fechaFin;

        private bool isVisibleSeccionPeriodoCombobox;
        private bool isVisibleSeccionRangoFecha;

        private event Action<string> Set_Windows_MainLabelTitle;
        private event Action<string> Set_Windows_MainLabelBancaBalanceActual;

    }


}
