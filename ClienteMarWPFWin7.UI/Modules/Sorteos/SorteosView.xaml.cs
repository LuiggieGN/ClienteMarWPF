using ClienteMarWPFWin7.Data;
using ClienteMarWPFWin7.Data.Services;
using ClienteMarWPFWin7.Data.Services.Helpers;
using ClienteMarWPFWin7.Domain.BingoService;
using ClienteMarWPFWin7.Domain.MarPuntoVentaServiceReference;
using ClienteMarWPFWin7.Domain.Models.Dtos;
using ClienteMarWPFWin7.UI.Modules.CincoMinutos;
using ClienteMarWPFWin7.UI.Modules.Sorteos.Modal;
using ClienteMarWPFWin7.UI.State.PinterConfig;
using ClienteMarWPFWin7.UI.ViewModels.Helpers;
using ClienteMarWPFWin7.UI.ViewModels.ModelObservable;
using ClienteMarWPFWin7.UI.Views.WindowsModals;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Threading;
using static ClienteMarWPFWin7.Domain.Models.Dtos.CincoMinutosRequestModel;
using static ClienteMarWPFWin7.Domain.Models.Dtos.ProdutosDTO;
using static ClienteMarWPFWin7.Domain.Models.Dtos.SorteosDisponibles;


namespace ClienteMarWPFWin7.UI.Modules.Sorteos
{
    /// <summary>
    /// Interaction logic for SorteosView.xaml
    /// </summary>
    public partial class SorteosView : UserControl
    {
        public List<SorteosObservable> SorteosBinding;
        public List<SorteosObservable> represorteoseleccionados;
        private List<SorteosObservable> SuperPales;
        private List<SuperPaleDisponible> combinations;
        private List<Jugada> ListJugadas;
        private DateTime lastKeyPress;
        private List<string> NumerosJugados;
        private static string ticketSeleccionado;
        private string teclaSeleccionada = "";
        public static DispatcherTimer Timer { get; set; }
        public static DispatcherTimer Timer2 { get; set; }
        public string vista { get; set; }

        public int loteria1 { get; set; }
        public int loteria2 { get; set; }
        public string NombreLoteria1 { get; set; }
        public string NombreLoteria2 { get; set; }
        public ObservableCollection<SorteosAvender> ListSorteosVender { get; set; }
        public int SeleccionadasLista { get; set; }
        public string nombreSuperPales { get; set; }
        public SorteosObservable objetoAgregar = new SorteosObservable();
        public IEnumerable<int> loteriaCombinada { get; set; }

        public bool SorteoItemFocused { get; set; }
        public bool TxtMontoFocus { get; set; }
        public bool TxtJugadaFocus { get; set; }
        public bool CrearSuperFocus { get; set; }
        public int contador { get; set; }
        public int indice1 { get; set; }
        public int indice2 { get; set; }
        public double cantidadTotal2 { get; set; }
        public double cantidadTotal { get; set; }

        public double almacenandoMontos = 0;
        public List<Tuple<double, int>> precioYdia = new List<Tuple<double, int>>();
        public int numeroLoteria = 0;
        public int totalMontos = 0;
        public PermisosDTO permisoCamion = new PermisosDTO();

        public static readonly DependencyProperty RealizarApuestaCommandProperty = DependencyProperty.Register("RealizarApuestaCommand", typeof(ICommand), typeof(SorteosView), new PropertyMetadata(null));
        //public static readonly DependencyProperty GetListadoTicketsCommandProperty = DependencyProperty.Register("GetListadoTicketsCommand", typeof(ICommand), typeof(SorteosView), new PropertyMetadata(null));
        public static readonly DependencyProperty ValidarPagoTicketCommandProperty = DependencyProperty.Register("ValidarPagoTicketCommand", typeof(ICommand), typeof(SorteosView), new PropertyMetadata(null));
        public static readonly DependencyProperty CopiarTicketCommandProperty = DependencyProperty.Register("CopiarTicketCommand", typeof(ICommand), typeof(SorteosView), new PropertyMetadata(null));
        public static readonly DependencyProperty GetListadoTicketsCommandProperty = DependencyProperty.Register("GetListadoTicketsCommand", typeof(ICommand), typeof(SorteosView), new PropertyMetadata(null));


        public ICommand RealizarApuestaCommand
        {
            get { return (ICommand)GetValue(RealizarApuestaCommandProperty); }
            set { SetValue(RealizarApuestaCommandProperty, value); }
        }
        public ICommand GetListadoTicketsCommand
        {
            get { return (ICommand)GetValue(GetListadoTicketsCommandProperty); }
            set { SetValue(GetListadoTicketsCommandProperty, value); }
        }
        public ICommand ValidarPagoTicketCommand
        {
            get { return (ICommand)GetValue(ValidarPagoTicketCommandProperty); }
            set { SetValue(ValidarPagoTicketCommandProperty, value); }
        }
        public ICommand CopiarTicketCommand
        {
            get { return (ICommand)GetValue(CopiarTicketCommandProperty); }
            set { SetValue(CopiarTicketCommandProperty, value); }
        }


        public SorteosView()
        {
            InitializeComponent();
            ListJugadas = new List<Jugada>();
            NumerosJugados = new List<string>();

            var camion = SessionGlobals.LoteriasDisponibles.Where(x => x.Nombre.Contains("Camion millonario"));
            if (SessionGlobals.permisos && !camion.Any())
            {
                SessionGlobals.LoteriasDisponibles.Add(new MAR_Loteria2()
                {
                    CieDom = 0,
                    CieSab = 0,
                    CieVie = 0,
                    CieJue = 0,
                    CieMie = 0,
                    CieMar = 0,
                    CieLun = 0,
                    Imagen = null,
                    LoteriaKey = 0,
                    Nombre = "Camion millonario",
                    NombreResumido = "CM5",
                    Numero = 0,
                    Oculta = false
                });
            }


            if (SessionGlobals.permisos)
            {
                ConsultarCM5.Visibility = Visibility.Visible;
            }
            else
            {
                ConsultarCM5.Visibility = Visibility.Hidden;
            }


            SorteosBinding = ConvertToObservables(SessionGlobals.LoteriasDisponibles);
            SuperPales = ConvertToObservables(SessionGlobals.LoteriasDisponibles);
            combinations = SessionGlobals.SuperPaleDisponibles;
            ListSorteosVender = new ObservableCollection<SorteosAvender>();

            // Agregar underline y texto de regulares (Toogle)
            TextBlockRegulares.Inlines.Add(new Underline(new Run("R")));
            TextBlockRegulares.Inlines.Add(new Run("egulares"));

            // Agregar underline y texto en super pales (Toogle)

            TextBlockSuper.Inlines.Add(new Underline(new Run("S")));
            TextBlockSuper.Inlines.Add(new Run("uperPale"));

            // Agregar underline y texto en boton quitar todos
            botonQuitarTodo.Inlines.Add(new Underline(new Run("Q")));
            botonQuitarTodo.Inlines.Add(new Run("uitar todos"));

            //if (CrearSuper.IsChecked == false)
            //{
            listSorteo.DataContext = SorteosBinding;
            Spinner.Visibility = Visibility.Collapsed; // Spinner de boton vender
            SpinnerConsulta.Visibility = Visibility.Collapsed; // Spinner de boton consultar
            //}
            //else if (CrearSuper.IsChecked == true)
            //{
            //    listSorteo.DataContext = SuperPales;
            //}

            if (Timer != null)
            {
                Timer.Stop();
            }
            //Timer que corre cada x segundos
            Timer = new DispatcherTimer();
            Timer.Tick += new EventHandler(RunEachTime);
            Timer.Interval = TimeSpan.FromSeconds(1);

            Timer.Start();
            //MostrarSorteos();

            SeleccionadasLista = ListSorteosVender.Count();
            CantidadSorteos.Content = $"{SeleccionadasLista} Sorteos seleccionados";

            contador = 0;

            //SetProducto("CincoMinutos");
        }



        private void RunEachTime(object sender, EventArgs e)
        {
            try
            {
                var sorteos = new SorteosDataService();
                var sessionHacienda = new ClienteMarWPFWin7.Domain.HaciendaService.MAR_Session();

                var vm = DataContext as SorteosViewModel;

                if (vm != null)
                {

                    List<int> SoloLoteriasExistentes = new List<int>();
                    List<int> SoloSuperPaleExistentes = new List<int>();

                    var clonLoterias = ConvertToObservables(SessionGlobals.LoteriasDisponibles);
                    var clonSuperPales = ConvertToObservablesSuperPales(SessionGlobals.SuperPaleDisponibles);

                    SoloLoteriasExistentes.Add(0);
                    foreach (var loteria in clonLoterias)
                    {
                        SoloLoteriasExistentes.Add(loteria.LoteriaID);

                    }

                    foreach (var super in clonSuperPales)
                    {
                        SoloSuperPaleExistentes.Add(super.LoteriaIDDestino);

                    }

                    var setting = vm.Autenticador.CurrentAccount.MAR_Setting2;

                    var sessionPuntoVenta = setting.Sesion;
                    sessionHacienda.Banca = sessionPuntoVenta.Banca;
                    sessionHacienda.Usuario = sessionPuntoVenta.Usuario;
                    sessionHacienda.Sesion = sessionPuntoVenta.Sesion;
                    sessionHacienda.Err = sessionPuntoVenta.Err;
                    sessionHacienda.LastTck = sessionPuntoVenta.LastTck;
                    sessionHacienda.LastPin = sessionPuntoVenta.LastPin;
                    sessionHacienda.PrinterSize = sessionPuntoVenta.PrinterSize;
                    sessionHacienda.PrinterHeader = sessionPuntoVenta.PrinterHeader;
                    sessionHacienda.PrinterFooter = sessionPuntoVenta.PrinterFooter;
                    var sorteosdisponibles = sorteos.GetSorteosDisponibles(sessionHacienda);
                    if (sorteosdisponibles.OK == true)
                    {
                        SessionGlobals.GetLoteriasDisponibles(setting.Loterias, sorteosdisponibles);

                    }
                    combinations = SessionGlobals.SuperPaleDisponibles;


                    foreach (var i in SoloLoteriasExistentes) // Este foreach elimina los sorteos tradicionales no disponibles, tanto para loterias tradicionales y super pales
                    {

                        var idLoteriaSuperPales = SuperPales.FindIndex(x => x.LoteriaID == i);

                        if (i == 0)
                        {
                            SuperPales.RemoveAt(idLoteriaSuperPales);
                        }

                        if (SessionGlobals.LoteriasDisponibles.FindIndex(x => x.Numero == i) == -1)
                        {
                            var idLoteriaTradicional = SorteosBinding.FindIndex(y => y.LoteriaID == i);
                            var idLoteriaCamion = SorteosBinding.Where(x => x.Loteria == "Camion millonario").Select(y => y.LoteriaID).ToList();
                            var nombreCamion = SorteosBinding.Where(x => x.LoteriaID == 0).Select(y => y.Loteria);
                            //var idLoteriaSuperPales = SuperPales.FindIndex(x => x.LoteriaID == i);

                            if (idLoteriaTradicional >= 0)
                            {
                                if (i == 0)
                                {
                                    return;
                                }

                                SorteosBinding.RemoveAt(idLoteriaTradicional);
                            }

                            if (idLoteriaSuperPales >= 0)
                            {

                                if (i == 0)
                                {
                                    return;
                                }

                                SuperPales.RemoveAt(idLoteriaSuperPales);
                            }

                            int sorteoAVenderIndice = -1;
                            int cuentaIndice = 0;
                            foreach (var item in ListSorteosVender)
                            {
                                if (item.Sorteo.LoteriaID == i)
                                {
                                    sorteoAVenderIndice = cuentaIndice;

                                    if (item.Sorteo.LoteriaID == 0)
                                    {
                                        return;
                                    }

                                    break;
                                }
                                ++cuentaIndice;
                            }

                            if (sorteoAVenderIndice >= 0)
                            {

                                if (i == 0)
                                {
                                    return;
                                }

                                ListSorteosVender.RemoveAt(sorteoAVenderIndice);
                                sorteosSeleccionados.ItemsSource = ListSorteosVender;
                                SeleccionadasLista = ListSorteosVender.Count();
                                CantidadSorteos.Content = $"{SeleccionadasLista} Sorteos seleccionados";
                            }
                            listSorteo.Items.Refresh();
                        }
                    }


                    int cuentaIndice2 = 0;
                    foreach (var item in ListSorteosVender)
                    {
                        if (item.Sorteo.Tipo.Equals("S", StringComparison.OrdinalIgnoreCase))
                        {
                            if (SessionGlobals.SuperPaleDisponibles.FindIndex(x => x.LoteriaIDDestino == item.Sorteo.LoteriaID) == -1)
                            {
                                try
                                {
                                    ListSorteosVender.RemoveAt(cuentaIndice2);
                                    sorteosSeleccionados.ItemsSource = ListSorteosVender;
                                    SeleccionadasLista = ListSorteosVender.Count();
                                    CantidadSorteos.Content = $"{SeleccionadasLista} Sorteos seleccionados";
                                    listSorteo.Items.Refresh();
                                }
                                catch
                                {

                                }
                            }
                        }
                        ++cuentaIndice2;
                    }


                    foreach (var i in SessionGlobals.LoteriasDisponibles)
                    {
                        SorteosObservable objetoAgregarTradicional = new SorteosObservable() { Date = DateTime.Now, IsSelected = false, Loteria = i.Nombre, LoteriaID = i.Numero };
                        SorteosObservable objetoAgregarSuper = new SorteosObservable() { Date = DateTime.Now, IsSelected = false, Loteria = i.Nombre, LoteriaID = i.Numero };
                        if (SoloLoteriasExistentes.FindIndex(x => x == i.Numero) == -1)
                        {

                            SuperPales.Add(objetoAgregarSuper);
                            SorteosBinding.Add(objetoAgregarTradicional);
                            listSorteo.Items.Refresh();
                        }
                    }

                    if (CrearSuper.IsChecked == false)
                    {
                        listSorteo.DataContext = SorteosBinding;

                    }
                    if (CrearSuper.IsChecked == true)
                    {
                        listSorteo.DataContext = SuperPales;

                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        #region LOGICA PARA SORTEOS
        private void ValidateSelectOnlyTwo(object sender)
        {

            if (CrearSuper.IsChecked == true)
            {

                var cuentaSuper = SuperPales.Where(x => x.IsSelected == true).Count();
                var combinacion = SuperPales.Where(x => x.IsSelected == true).Select(y => y.LoteriaID);

                if (cuentaSuper > 2)
                {
                    foreach (var i in SuperPales)
                    {
                        if (i.LoteriaID != loteria1 && i.LoteriaID != loteria2)
                        {
                            i.IsSelected = false;
                        }
                        else
                        {
                            i.IsSelected = true;

                        }

                    }
                    return;
                }

                int count = 0;
                var loteriaSeleccionadas = SuperPales.Where(x => x.IsSelected == true).ToArray();

                if (SuperPales.Where(x => x.IsSelected == true).Count() == 1 && loteria1 == 0)
                {
                    loteria1 = loteriaSeleccionadas[0].LoteriaID;
                    var InfoLoteria1 = SessionGlobals.LoteriasYSupersDisponibles.Where(x => x.Numero == loteria1).ToArray();
                    NombreLoteria1 = InfoLoteria1[0].NombreResumido;
                }
                else if (SuperPales.Where(x => x.IsSelected == true).Count() == 2 && loteria1 != 0)
                {
                    loteria1 = loteriaSeleccionadas[0].LoteriaID;
                    loteria2 = loteriaSeleccionadas[1].LoteriaID;
                    var InfoLoteria2 = SessionGlobals.LoteriasYSupersDisponibles.Where(x => x.Numero == loteria2).ToArray();
                    NombreLoteria2 = InfoLoteria2[0].NombreResumido;
                    var InfoLoteria1 = SessionGlobals.LoteriasYSupersDisponibles.Where(x => x.Numero == loteria1).ToArray();
                    NombreLoteria1 = InfoLoteria1[0].NombreResumido;
                }

                if (loteriaSeleccionadas.Count() == 2 && loteria1 != 0 && loteria2 != 0)
                {
                    int? va = null, vb = null;


                    loteriaCombinada = combinations.Where(x => (x.LoteriaID1 == loteria1 && x.LoteriaID2 == loteria2) || (x.LoteriaID1 == loteria2 && x.LoteriaID2 == loteria1)).Select(y => y.LoteriaIDDestino);


                    if (loteriaCombinada.Any())
                    {
                        va = combinations.Where(x => x.LoteriaID1 == loteria1 && x.LoteriaID2 == loteria2).Select(y => y.LoteriaIDDestino).FirstOrDefault();
                        vb = combinations.Where(x => x.LoteriaID1 == loteria2 && x.LoteriaID2 == loteria1).Select(y => y.LoteriaIDDestino).FirstOrDefault();

                        if (va.HasValue)
                        {
                            nombreSuperPales = $"SP {NombreLoteria1}-{NombreLoteria2}";
                        }

                        if (vb.HasValue)
                        {
                            nombreSuperPales = $"SP {NombreLoteria2}-{NombreLoteria1}";
                        }

                        RefrescarMonto(true, loteriaCombinada.ToArray()[0]);
                    }
                    else
                    {
                        nombreSuperPales = string.Empty;

                        //if (loteriaCombinada.ToArray()[0] != 0)
                        //{
                        //    RefrescarMonto(false, loteriaCombinada.ToArray()[0]);
                        //}

                    }

                }
            }



        }
        private string SepararNumeros(string jugadaIn)
        {
            var jugada = jugadaIn;
            StringBuilder builder = new StringBuilder(jugada);
            var startIndex = builder.Length - (builder.Length % 2);
            for (int i = startIndex; i >= 2; i += -2)
            {
                if (i < 6)
                    builder.Insert(i, '-');
            }
            jugada = builder.ToString();
            if (jugada.Substring(jugada.Length - 1) == "-")
            {
                var newText = jugada.Substring(0, jugada.Length - 1);
                jugada = newText;
            }
            StringBuilder numerosBuilder = new StringBuilder(10);
            var numerosSplited = jugada.Split('-');
            var loopTo = jugada.Split('-').Length - 1;
            for (var index = 0; index <= loopTo; index++)
            {
                var t = numerosSplited[index].PadLeft(2, '0');
                numerosBuilder.Append(numerosSplited[index].PadLeft(2, '0'));
            }
            var startIndex2 = numerosBuilder.Length - (numerosBuilder.Length % 2);
            for (int i = startIndex2; i >= 2; i += -2)
            {
                if (i < 6)
                    numerosBuilder.Insert(i, '-');
            }
            jugada = numerosBuilder.ToString();
            if (jugada.Substring(jugada.Length - 1) == "-")
            {
                var newText = jugada.Substring(0, jugada.Length - 1);
                jugada = newText;
            }

            return jugada;
        }
        private string TipoJugada(int tipojujgada)
        {
            string tipojugada = tipojujgada == 1 ? "  Quiniela" : tipojujgada == 2 ? "  Pale" : tipojujgada == 3 ? "  Tripleta" : " ";
            return tipojugada;
        }
        private void MostrarSorteos()
        {
            var mostrarSorteos = ltJugada.Items.Count > 0;
            if (mostrarSorteos)
            {
                SorteosYLoterias.Visibility = Visibility.Visible;
                VentasYConsulta.Visibility = Visibility.Collapsed;
            }
            else
            {
                SorteosYLoterias.Visibility = Visibility.Collapsed;
                VentasYConsulta.Visibility = Visibility.Visible;
            }
        }
        private void LimpiarApuesta()
        {
            // Limpiando todo 
            ListJugadas = new List<Jugada>();
            NumerosJugados = new List<string>();
            RemoveAllSeletion();
            ltJugada.ItemsSource = new List<Jugada>();
            txtMontoTotal.Content = "$0.00";
            ListSorteosVender.Clear();
            //MostrarSorteos();
        }
        private List<SorteosObservable> ConvertToObservables(List<MAR_Loteria2> Sorteos)
        {

            //var VM = DataContext as SorteosViewModel;

            var SorteosObservables = new List<SorteosObservable>();

            //if (VM != null)
            //{
            //    var MoreOptions = VM.Autenticador.CurrentAccount.MAR_Setting2.MoreOptions.ToList();
            //    if (MoreOptions.Contains("BANCA_VENDE_CINCOMINUTOS|TRUE"))
            //    {
            //        permisoCamion.CincoMinutos = true;
            //    }
            //}

            foreach (var item in Sorteos)
            {
                SorteosObservables.Add(new SorteosObservable { LoteriaID = item.Numero, Loteria = item.Nombre, IsSelected = false, Date = DateTime.Now });
            }

            //if (permisoCamion.CincoMinutos)
            //{
            //    var camion = SorteosBinding.Where(x => x.Loteria.Contains("Camion millonario"));
            //    if (!camion.Any())
            //    {
            //        SorteosObservables.Add(new SorteosObservable { LoteriaID = 0, Loteria = "Camion millonario", IsSelected = false, Date = DateTime.Now });
            //        SorteosBinding = SorteosObservables;
            //    }
            //}

            return SorteosObservables;
        }

        private List<SuperPaleDisponible> ConvertToObservablesSuperPales(List<SuperPaleDisponible> superPale)
        {
            var SuperPaleObservables = new List<SuperPaleDisponible>();

            foreach (var item in superPale)
            {
                SuperPaleObservables.Add(new SuperPaleDisponible { LoteriaID1 = item.LoteriaID1, LoteriaID2 = item.LoteriaID2, LoteriaIDDestino = item.LoteriaIDDestino });
            }


            return SuperPaleObservables;
        }

        private List<MAR_Loteria2> ConvertToSorteos(List<SorteosObservable> SorteosObservable)
        {
            var Sorteos = new List<MAR_Loteria2>();
            foreach (var item in SorteosObservable)
            {
                Sorteos.Add(new MAR_Loteria2 { LoteriaKey = item.LoteriaID, Nombre = item.Loteria });
            }

            return Sorteos;
        }
        private int FindCombinations(List<int> LotteryIDs)
        {
            int response = -1;

            if (combinations.Any())
            {
                if (LotteryIDs.Count == 2)
                {
                    int loteria1 = LotteryIDs.First();
                    int loteria2 = LotteryIDs.Last();
                    var hasCombinations = combinations.Where(x => (x.LoteriaID1 == loteria1 && x.LoteriaID2 == loteria2) || (x.LoteriaID1 == loteria2 && x.LoteriaID2 == loteria1));
                    if (hasCombinations.Any())
                    {
                        int combinationID = hasCombinations.Select(x => x.LoteriaIDDestino).FirstOrDefault();
                        response = combinationID;

                    }
                }
            }


            return response;
        }
        private void FindCombinationsNotExist()
        {
            if (CrearSuper.IsChecked == true)
            {
                var sorteos = SessionGlobals.LoteriasDisponibles;
                var CombinationsAllSorteo = GetCombinations(sorteos.Select(x => x.Numero).ToList(), 2);
                var combinationNotPossibility = new List<List<int>>();

                string combinationNotPossibilityString = "Lista de combinaciones no disponibles, favor solicitar:" + Environment.NewLine + Environment.NewLine;

                foreach (var item in CombinationsAllSorteo)
                {
                    var first = item.First();
                    var last = item.Last();
                    var hasCombinations = combinations.Where(x => (x.LoteriaID1 == first && x.LoteriaID2 == last) || (x.LoteriaID1 == last && x.LoteriaID2 == first));
                    if (!hasCombinations.Any())
                    {
                        combinationNotPossibility.Add(new List<int> { first, last });

                    }
                }

                if (combinationNotPossibility.Count > 0)
                {
                    foreach (var combo in combinationNotPossibility)
                    {
                        string loteria1 = sorteos.Where(x => x.Numero == combo.First()).Select(x => x.Nombre).FirstOrDefault();
                        string loteria2 = sorteos.Where(x => x.Numero == combo.Last()).Select(x => x.Nombre).FirstOrDefault();
                        combinationNotPossibilityString += "-- " + loteria1 + " + " + loteria2 + " --";
                    }

                    MessageBox.Show(combinationNotPossibilityString, "  Aviso  ", MessageBoxButton.OK, MessageBoxImage.Information);
                }



            }
        }


        public double GetPrecioPorDia(string tipoJugada, int loteria)
        {
            var collecion = SessionGlobals.LoteriasYSupersDisponibles.Where(x => x.Numero == loteria);
            var diaActual = DateTime.Now.DayOfWeek.ToString();

            switch (diaActual)
            {
                case "Monday":
                    if (tipoJugada == "  Quiniela" || tipoJugada == "Quiniela") { precioYdia = collecion.Select(x => new Tuple<double, int>(x.PrecioQ, x.CieLun)).ToList(); }
                    else if (tipoJugada == "  Pale" || tipoJugada == "Pale") { precioYdia = collecion.Select(x => new Tuple<double, int>(x.PrecioP, x.CieLun)).ToList(); }
                    else if (tipoJugada == "  Tripleta" || tipoJugada == "Tripleta") { precioYdia = collecion.Select(x => new Tuple<double, int>(x.PrecioT, x.CieLun)).ToList(); }
                    break;

                case "Tuesday":
                    if (tipoJugada == "  Quiniela" || tipoJugada == "Quiniela") { precioYdia = collecion.Select(x => new Tuple<double, int>(x.PrecioQ, x.CieMar)).ToList(); }
                    else if (tipoJugada == "  Pale" || tipoJugada == "Pale") { precioYdia = collecion.Select(x => new Tuple<double, int>(x.PrecioP, x.CieMar)).ToList(); }
                    else if (tipoJugada == "  Tripleta" || tipoJugada == "Tripleta") { precioYdia = collecion.Select(x => new Tuple<double, int>(x.PrecioT, x.CieMar)).ToList(); }
                    break;

                case "Wednesday":
                    if (tipoJugada == "  Quiniela" || tipoJugada == "Quiniela") { precioYdia = collecion.Select(x => new Tuple<double, int>(x.PrecioQ, x.CieMie)).ToList(); }
                    else if (tipoJugada == "  Pale" || tipoJugada == "Pale") { precioYdia = collecion.Select(x => new Tuple<double, int>(x.PrecioP, x.CieMie)).ToList(); }
                    else if (tipoJugada == "  Tripleta" || tipoJugada == "Tripleta") { precioYdia = collecion.Select(x => new Tuple<double, int>(x.PrecioT, x.CieMie)).ToList(); }
                    break;

                case "Thursday":
                    if (tipoJugada == "  Quiniela" || tipoJugada == "Quiniela") { precioYdia = collecion.Select(x => new Tuple<double, int>(x.PrecioQ, x.CieJue)).ToList(); }
                    else if (tipoJugada == "  Pale" || tipoJugada == "Pale") { precioYdia = collecion.Select(x => new Tuple<double, int>(x.PrecioP, x.CieJue)).ToList(); }
                    else if (tipoJugada == "  Tripleta" || tipoJugada == "Tripleta") { precioYdia = collecion.Select(x => new Tuple<double, int>(x.PrecioT, x.CieJue)).ToList(); }
                    break;

                case "Friday":
                    if (tipoJugada == "  Quiniela" || tipoJugada == "Quiniela") { precioYdia = collecion.Select(x => new Tuple<double, int>(x.PrecioQ, x.CieVie)).ToList(); }
                    else if (tipoJugada == "  Pale" || tipoJugada == "Pale") { precioYdia = collecion.Select(x => new Tuple<double, int>(x.PrecioP, x.CieVie)).ToList(); }
                    else if (tipoJugada == "  Tripleta" || tipoJugada == "Tripleta") { precioYdia = collecion.Select(x => new Tuple<double, int>(x.PrecioT, x.CieVie)).ToList(); }
                    break;

                case "Saturday":
                    if (tipoJugada == "  Quiniela" || tipoJugada == "Quiniela") { precioYdia = collecion.Select(x => new Tuple<double, int>(x.PrecioQ, x.CieSab)).ToList(); }
                    else if (tipoJugada == "  Pale" || tipoJugada == "Pale") { precioYdia = collecion.Select(x => new Tuple<double, int>(x.PrecioP, x.CieSab)).ToList(); }
                    else if (tipoJugada == "  Tripleta" || tipoJugada == "Tripleta") { precioYdia = collecion.Select(x => new Tuple<double, int>(x.PrecioT, x.CieSab)).ToList(); }
                    break;

                case "Sunday":
                    if (tipoJugada == "  Quiniela" || tipoJugada == "Quiniela") { precioYdia = collecion.Select(x => new Tuple<double, int>(x.PrecioQ, x.CieDom)).ToList(); }
                    else if (tipoJugada == "  Pale" || tipoJugada == "Pale") { precioYdia = collecion.Select(x => new Tuple<double, int>(x.PrecioP, x.CieDom)).ToList(); }
                    else if (tipoJugada == "  Tripleta" || tipoJugada == "Tripleta") { precioYdia = collecion.Select(x => new Tuple<double, int>(x.PrecioT, x.CieDom)).ToList(); }
                    break;

            }

            return precioYdia.ToArray()[0].Item1;
        }



        public void RefrescarMonto(bool sumar, int loteria)
        {
            var VM = DataContext as SorteosViewModel;
            var cincoMinutoService = new CincoMinutosDataService();
            var setProductoPrecio = cincoMinutoService.SetProducto("CincoMinutos", VM.Autenticador.CurrentAccount);

            var loteriasSeleccionadas = SorteosBinding.Where(x => x.IsSelected == true);
            var loteriasSeleccionadasS = combinations.Select(x => x.LoteriaIDDestino);

            var listadoLoteria = ListSorteosVender.Where(x => x.Sorteo.LoteriaID == loteria);

            var seleccion = SorteosBinding.Where(x => x.IsSelected == true).Select(y => y.LoteriaID);
            totalMontos = ListJugadas?.Sum(x => x.Monto) ?? 0;


            try
            {
                if (loteria != 0)
                {
                    if (sumar)
                    {
                        foreach (var item in ListJugadas)
                        {
                            almacenandoMontos += item.Monto * GetPrecioPorDia(item.TipoJugada, loteria);
                        }
                        txtMontoTotal.Content = (Decimal.Parse(almacenandoMontos.ToString(CultureInfo.InvariantCulture))).ToString("C");
                    }
                    else
                    {
                        foreach (var item in ListJugadas)
                        {
                            almacenandoMontos -= item.Monto * GetPrecioPorDia(item.TipoJugada, loteria);
                        }
                        txtMontoTotal.Content = (Decimal.Parse(almacenandoMontos.ToString(CultureInfo.InvariantCulture))).ToString("C");
                    }
                }
                else if (loteria == 0)
                {
                    almacenandoMontos = 0;

                    foreach (var i in ListSorteosVender)
                    {
                        if (!i.SorteoNombre.Contains("Camion millonario"))
                        {
                            foreach (var item in ListJugadas)
                            {
                                almacenandoMontos += item.Monto * GetPrecioPorDia(item.TipoJugada, i.Sorteo.LoteriaID);
                                txtMontoTotal.Content = (Decimal.Parse(almacenandoMontos.ToString(CultureInfo.InvariantCulture))).ToString("C");
                            }
                        }

                        if (i.SorteoNombre.Contains("Camion millonario") && ListJugadas.Count > 0)
                        {
                            if (VM != null)
                            {
                                foreach (var camion in ListJugadas)
                                {
                                    if (sumar)
                                    {
                                        almacenandoMontos += camion.Monto * setProductoPrecio.Monto;
                                    }
                                    else
                                    {
                                        almacenandoMontos -= camion.Monto * setProductoPrecio.Monto;
                                    }

                                    txtMontoTotal.Content = (Decimal.Parse(almacenandoMontos.ToString(CultureInfo.InvariantCulture))).ToString("C");
                                }

                            }
                        }

                    }
                }

            }
            catch
            {

            }
        }

        public void RefreshListJugadas()
        {
            ltJugada.ItemsSource = new List<Jugada>();
            ltJugada.ItemsSource = ListJugadas;
            ltJugada.Items.Refresh();

            if (ListSorteosVender.Count == 0 || ltJugada.Items.Count == 0)
            {
                txtMontoTotal.Content = "$0.00";
            }

        }


        //public void RefreshListJugadas()
        //{
        //    ltJugada.ItemsSource = new List<Jugada>();
        //    ltJugada.ItemsSource = ListJugadas;
        //    ltJugada.Items.Refresh();
        //    var total = ListJugadas?.Sum(x => x.Monto) ?? 0;
        //    var cantidadLoterias = ListSorteosVender?.Count() ?? 0;
        //    cantidadTotal = (total * cantidadLoterias);
        //    txtMontoTotal.Content = (Decimal.Parse(cantidadTotal.ToString())).ToString("C");

        //}

        public void AddItem(Jugada NuevaJugada = null)
        {
            var loteriasSeleccionadas = SorteosBinding.Where(x => x.IsSelected == true);

            if (!txtJugada.Text.Trim().Equals(string.Empty) && !txtMonto.Text.Trim().Equals(string.Empty))
            {
                string jugada = SepararNumeros(txtJugada.Text);
                var numeros = jugada.Split('-');
                jugada = "";
                Array.Sort(numeros);
                for (var i = 0; i < numeros.Length; i++)
                {
                    jugada = jugada + numeros[i];
                    if (i + 1 != numeros.Length)
                    {
                        jugada = jugada + "-";
                    }
                }
                int tipo = numeros.Count();
                var nuevajugada = new Jugada { Jugadas = jugada, Monto = Convert.ToInt32(txtMonto.Text), TipoJugada = TipoJugada(tipo) };


                foreach (var item in numeros)
                {
                    NumerosJugados.Add(item);

                }

                var existeJugada = ListJugadas.Where(x => x.Jugadas == nuevajugada.Jugadas).Any();
                if (existeJugada)
                {
                    Jugada jugadaExistente = ListJugadas.Where(x => x.Jugadas == nuevajugada.Jugadas).FirstOrDefault();
                    jugadaExistente.Monto += nuevajugada.Monto;
                }
                else
                {
                    ListJugadas.Add(nuevajugada);

                }
            }
            else if (NuevaJugada != null)
            {
                var existeJugada = ListJugadas.Where(x => x.Jugadas == NuevaJugada.Jugadas).Any();
                if (existeJugada)
                {
                    Jugada jugadaExistente = ListJugadas.Where(x => x.Jugadas == NuevaJugada.Jugadas).FirstOrDefault();
                    jugadaExistente.Monto += NuevaJugada.Monto;
                }
                else
                {
                    ListJugadas.Add(NuevaJugada);
                }
            }
            else
            {
                ((MainWindow)Window.GetWindow(this)).MensajesAlerta("No ha realizado jugada o puesto monto", "Aviso");
            }

            RefreshListJugadas();
            RefrescarMonto(true, 0);

        }
        private void RemoveItem()
        {
            if (ltJugada.Items.Count > 0)
            {
                var jugada = ltJugada.SelectedItem;
                if (jugada != null)
                {
                    var JugadaModel = jugada as Jugada;
                    ListJugadas.Remove(JugadaModel);
                }
                else
                {
                    ListJugadas.Remove(ListJugadas.LastOrDefault());
                }
            }

            ltJugada.Items.Refresh();
            RefreshListJugadas();
            RefrescarMonto(false, 0);
        }
        private void SelectItem()
        {
            if (listSorteo.SelectedItem != null)
            {
                var item = listSorteo.SelectedItem as SorteosObservable;
                var sorteoChangeSelect = SorteosBinding.Where(x => x.LoteriaID == item.LoteriaID).Select(x => { x.IsSelected = !x.IsSelected; x.Date = DateTime.Now; return x; }).FirstOrDefault();
                //if (sorteoChangeSelect.IsSelected == false && CrearSuper.IsChecked == false)
                //{
                //    SorteosSave.LoteriasResponse.Remove(sorteoChangeSelect.LoteriaID);
                //}
                var sorteosIsSelect = SorteosBinding.Where(x => x.IsSelected == true).ToList();

                if (sorteosIsSelect.Count == 3 && CrearSuper.IsChecked == true)
                {

                    var listLastModify = SorteosBinding.Where(x => x.LoteriaID != item.LoteriaID && x.IsSelected == true).ToList();
                    var firstDate = listLastModify.First().Date.TimeOfDay;
                    var lastDate = listLastModify.Last().Date.TimeOfDay;


                    if (firstDate < lastDate)
                    {
                        listLastModify.First().IsSelected = false;
                    }
                    else if (lastDate < firstDate)
                    {
                        listLastModify.Last().IsSelected = false;
                    }
                    else
                    {
                        item.Date = DateTime.Now;
                    }

                }
                item.Date = DateTime.Now;
            }

            RefreshListJugadas();
        }


        private void OpenCombinacion()
        {
            var NumerosJugado = new List<string>();
            if (ltJugada.Items.Count != 0)
            {
                NumerosJugado = NumerosJugados.Distinct().ToList();
            }

            var combinacion = new CombinacionWindowsModal(NumerosJugado);
            combinacion.Jugadas += delegate (List<Jugada> Jugadas)
            {
                foreach (var item in Jugadas)
                {
                    AddItem(item);
                }

                // MostrarSorteos();
            };

            combinacion.ShowDialog();
            txtMonto.Focus();
        }
        private void MostrarSuper(bool visible = true)
        {
            if (visible)
            {
                listSorteo.DataContext = SorteosBinding;
            }
            else
            {
                listSorteo.DataContext = SuperPales;
            }
        }
        private void RemoveAllSeletion()
        {
            foreach (var item in SorteosBinding)
            {
                item.IsSelected = false;
            }
            listSorteo.Focus();

        }
        private IEnumerable<IEnumerable<T>> GetCombinations<T>(IEnumerable<T> items, int count)
        {
            int i = 0;
            foreach (var item in items)
            {
                if (count == 1)
                    yield return new T[] { item };
                else
                {
                    foreach (var result in GetCombinations(items.Skip(i + 1), count - 1))
                        yield return new T[] { item }.Concat(result);
                }

                ++i;
            }
        }
        #endregion

        private void OnKeyUp(object sender, KeyEventArgs e)
        {


            switch (e.Key)
            {

                case Key.Enter:

                    teclaSeleccionada = "Enter";
                    if (listSorteo.SelectedItem != null)
                    {
                        if (CrearSuper.IsChecked == true)
                        {
                            var item = listSorteo.SelectedItem as SorteosObservable;

                            if (item.IsSelected == false)
                            {

                                item.IsSelected = true;

                            }
                            else if (item.IsSelected == true)
                            {
                                item.IsSelected = false;

                            }
                            ValidateSelectOnlyTwo(sender);

                        }

                    }

                    break;

                case Key.D0:
                    teclaSeleccionada = "D0";
                    if (listSorteo.SelectedItem != null)
                    {
                        if (CrearSuper.IsChecked == true)
                        {
                            var item = listSorteo.SelectedItem as SorteosObservable;

                            if (item.IsSelected == false)
                            {

                                item.IsSelected = true;

                            }
                            else if (item.IsSelected == true)
                            {
                                item.IsSelected = false;

                            }
                            ValidateSelectOnlyTwo(sender);

                        }

                    }

                    break;


                case Key.Space:

                    teclaSeleccionada = "Espacio";
                    if (listSorteo.SelectedItem != null)
                    {
                        if (CrearSuper.IsChecked == true)
                        {

                            var item = listSorteo.SelectedItem as SorteosObservable;

                            if (item.IsSelected == false)
                            {
                                item.IsSelected = true;
                            }
                            else if (item.IsSelected == true)
                            {
                                item.IsSelected = false;
                            }
                            ValidateSelectOnlyTwo(sender);

                        }

                    }
                    break;

                    //case Key.Left:

                    //    if (SorteoItemFocused == true)
                    //    {
                    //        indice1 = listSorteo.SelectedIndex;

                    //        if (contador == 0 && indice1 != indice2)
                    //        {
                    //            contador = 1;
                    //            return;
                    //        }

                    //        if (contador > 0)
                    //        {
                    //            contador = 2;
                    //            indice2 = listSorteo.SelectedIndex;
                    //        }

                    //        if (contador > 1 && indice1 == indice2)
                    //        {
                    //            e.Handled = true;
                    //            txtJugada.Focus();
                    //            listSorteo.SelectedIndex = -1;
                    //            e.Handled = false;
                    //            listSorteo.Items.Refresh();
                    //            TriggerButtonClickEvent(btnSeleccionaJugada);
                    //            contador = 0;
                    //        }
                    //    }
                    //    break;
            }
        }


        private void PressTecla(object sender, KeyEventArgs e)
        {
            var strToday = DateTime.Now.ToString("yyyyMMddHHmmss", CultureInfo.InvariantCulture);
            var today = DateTime.ParseExact(strToday, "yyyyMMddHHmmss", CultureInfo.InvariantCulture);
            var listado = (IEnumerable<SorteosAvender>)ListSorteosVender;
            var lista = new List<SorteosAvender>(listado);

            switch (e.Key)
            {
                case Key.Delete:
                    RemoveItem();
                    if (ltJugada.Items.Count == 0)
                    {
                        txtMonto.Focus();
                    }
                    else
                    {
                        txtMonto.Focus();
                    }
                    break;

                case Key.Subtract:
                    RemoveItem();
                    teclaSeleccionada = "";
                    txtMonto.Focus();
                    //MostrarSorteos();
                    break;

                case Key.Multiply:
                    teclaSeleccionada = "";
                    btnCombinar(sender, e);
                    break;

                case Key.S://Para cambiar a venta de super pales
                    teclaSeleccionada = "";
                    e.Handled = true;
                    CrearSuper.IsChecked = true;
                    listSorteo.Focus();
                    e.Handled = false;
                    break;

                case Key.R://Para cambiar a venta de loterias normales
                    teclaSeleccionada = "";
                    CrearSuper.IsChecked = false;

                    e.Handled = true;
                    listSorteo.Focus();
                    e.Handled = false;
                    break;

                case Key.Q:
                    BorrarTodo(sender, e);
                    break;

                case Key.Add:
                    teclaSeleccionada = "";
                    Vender(sender, e);
                    break;

                case Key.F5:
                    teclaSeleccionada = "";
                    ticketSeleccionado = null;
                    //ValidarPagoTicketCommand.Execute(null);
                    Consultar(sender, e);
                    break;

                case Key.F9:
                    teclaSeleccionada = "";
                    //RemoveItem();
                    ListJugadas.Clear();
                    if (ltJugada.Items.Count == 0)
                    {
                        txtMonto.Focus();
                    }
                    RefreshListJugadas();

                    break;

                case Key.F12:
                    teclaSeleccionada = "";
                    Vender(sender, e);
                    break;

                case Key.F11:
                    ConsultarCM5_Click(sender, e);
                    break;

                case Key.Tab:

                    if (SorteoItemFocused == true)
                    {
                        e.Handled = true;
                        listSorteo.Focus();
                        if (listSorteo.SelectedIndex == (listSorteo.Items.Count - 1))
                        {
                            listSorteo.SelectedIndex = 0;
                            e.Handled = false;
                            listSorteo.Items.Refresh();
                            return;
                        }
                        else
                        {
                            listSorteo.SelectedIndex += 1;
                            listSorteo.Items.Refresh();
                        }
                        return;
                    }

                    if (txtMonto.Text != "" && txtJugada.Text == "")
                    {
                        AgregaJugada(sender, e);
                        txtJugada.Focus();
                    }
                    else if (txtJugada.IsFocused)
                    {
                        AgregaJugada(sender, e);
                        listSorteo.Focus();
                        listSorteo.SelectedIndex = 0;
                    }

                    break;

                case Key.Down:

                    if (txtMonto.IsFocused || txtJugada.IsFocused)
                    {
                        TriggerButtonClickEvent(BajadaGrid);
                    }

                    break;

                case Key.Up:

                    TriggerButtonClickEvent(SubidaGrid);

                    break;

                case Key.Enter:

                    teclaSeleccionada = "Enter";
                    if (listSorteo.SelectedItem != null)
                    {
                        if (CrearSuper.IsChecked == false)
                        {
                            var item = listSorteo.SelectedItem as SorteosObservable;
                            var sorteoChangeSelect = SorteosBinding.Where(x => x.LoteriaID == item.LoteriaID).Select(x => { x.IsSelected = !x.IsSelected; x.Date = today; return x; });
                            var VM = DataContext as SorteosViewModel;
                            if (item.IsSelected == false)
                            {
                                item.IsSelected = true;

                                var posicionLoteriaEliminar = VM.LoteriasMultiples.IndexOf(item.LoteriaID);
                                if (posicionLoteriaEliminar == -1)
                                {
                                    VM.LoteriasMultiples.Add(item.LoteriaID);
                                }

                                AgregarSorteoAVender(item);
                                RefrescarMonto(true, item.LoteriaID);
                            }
                            else if (item.IsSelected == true)
                            {
                                int loteriaid = item.LoteriaID;
                                item.IsSelected = false;
                                var posicionLoteriaEliminar = VM.LoteriasMultiples.IndexOf(item.LoteriaID);
                                if (posicionLoteriaEliminar != -1)
                                {
                                    VM.LoteriasMultiples.RemoveAt(posicionLoteriaEliminar);
                                }
                                RemoverSorteoAVender(loteriaId: loteriaid, indice: lista.FindIndex(x => x.Sorteo.LoteriaID == loteriaid));
                                RefrescarMonto(false, loteriaid);
                            }
                        }
                    }

                    if (txtJugada.Text != "" && txtMonto.Text != "")
                    {
                        SelectItem();
                        //AgregaJugada(sender, e);
                    }
                    else if (txtMonto.Text != "" && txtJugada.Text == "" && txtMonto.IsFocused)
                    {
                        txtJugada.Focus();
                        AgregaJugada(sender, e);
                    }
                    else if (txtMonto.Text == "" && txtJugada.Text != "" && txtJugada.IsFocused)
                    {
                        txtMonto.Focus();
                        AgregaJugada(sender, e);
                    }

                    //RefreshListJugadas();

                    break;

                case Key.Left:
                    teclaSeleccionada = "";


                    if (listSorteo.SelectedIndex == 0)
                    {
                        e.Handled = true;
                        txtJugada.Focus();
                        listSorteo.SelectedIndex = -1;
                        e.Handled = false;
                        listSorteo.Items.Refresh();
                        //SeleccionarJugada(sender, e);
                        TriggerButtonClickEvent(btnSeleccionaJugada);
                        return;
                    }



                    if (TxtJugadaFocus == true)
                    {
                        e.Handled = true;
                        txtMonto.Focus();
                        e.Handled = false;
                        listSorteo.Items.Refresh();
                        //SeleccionarMonto(sender, e);
                        TriggerButtonClickEvent(btnSeleccionaMonto);
                        return;
                    }

                    break;

                case Key.Right:
                    teclaSeleccionada = "";

                    if (TxtMontoFocus == true)
                    {
                        e.Handled = true;
                        txtJugada.Focus();
                        e.Handled = false;
                        listSorteo.Items.Refresh();
                        //SeleccionarJugada(sender, e);
                        TriggerButtonClickEvent(btnSeleccionaJugada);
                        return;
                    }

                    if (TxtJugadaFocus == true)
                    {
                        listSorteo.Focus();
                        listSorteo.SelectedIndex = 0;
                        listSorteo.Items.Refresh();
                    }

                    //if (SorteoItemFocused == true)
                    //{
                    //    e.Handled = true;
                    //    listSorteo.Focus();
                    //    if (listSorteo.SelectedIndex == (listSorteo.Items.Count - 1))
                    //    {
                    //        listSorteo.SelectedIndex = 0;
                    //        e.Handled = false;
                    //        listSorteo.Items.Refresh();
                    //        return;
                    //    }
                    //    else
                    //    {
                    //        listSorteo.SelectedIndex += 1;
                    //        listSorteo.Items.Refresh();
                    //    }
                    //    return;
                    //}

                    //if (TxtMontoFocus == true)
                    //{
                    //    AgregaJugada(sender, e);
                    //    TriggerButtonClickEvent(btnSeleccionaJugada);
                    //    txtJugada.Focus();
                    //}
                    //else if (TxtJugadaFocus == true)
                    //{
                    //    AgregaJugada(sender, e);
                    //    listSorteo.Focus();
                    //    listSorteo.SelectedIndex = 0;
                    //}

                    break;

                case Key.Space:

                    teclaSeleccionada = "Espacio";
                    if (listSorteo.SelectedItem != null)
                    {
                        if (CrearSuper.IsChecked == false)
                        {
                            var item = listSorteo.SelectedItem as SorteosObservable;
                            var sorteoChangeSelect = SorteosBinding.Where(x => x.LoteriaID == item.LoteriaID).Select(x => { x.IsSelected = !x.IsSelected; x.Date = today; return x; });
                            var VM = DataContext as SorteosViewModel;
                            if (item.IsSelected == false)
                            {
                                item.IsSelected = true;

                                var posicionLoteriaEliminar = VM.LoteriasMultiples.IndexOf(item.LoteriaID);
                                if (posicionLoteriaEliminar == -1)
                                {
                                    VM.LoteriasMultiples.Add(item.LoteriaID);
                                }
                                AgregarSorteoAVender(item);
                                RefrescarMonto(true, item.LoteriaID);
                            }
                            else if (item.IsSelected == true)
                            {
                                int loteriaid = item.LoteriaID;
                                item.IsSelected = false;
                                var posicionLoteriaEliminar = VM.LoteriasMultiples.IndexOf(item.LoteriaID);
                                if (posicionLoteriaEliminar != -1)
                                {
                                    VM.LoteriasMultiples.RemoveAt(posicionLoteriaEliminar);
                                }
                                RemoverSorteoAVender(loteriaId: loteriaid, indice: lista.FindIndex(x => x.Sorteo.LoteriaID == loteriaid));
                                RefrescarMonto(false, loteriaid);
                            }
                        }
                    }

                    RefreshListJugadas();
                    break;

                case Key.D0:
                    teclaSeleccionada = "D0";
                    if (listSorteo.SelectedItem != null)
                    {
                        if (CrearSuper.IsChecked == false)
                        {
                            var item = listSorteo.SelectedItem as SorteosObservable;
                            var sorteoChangeSelect = SorteosBinding.Where(x => x.LoteriaID == item.LoteriaID).Select(x => { x.IsSelected = !x.IsSelected; x.Date = today; return x; });
                            var VM = DataContext as SorteosViewModel;
                            if (item.IsSelected == false)
                            {
                                item.IsSelected = true;

                                var posicionLoteriaEliminar = VM.LoteriasMultiples.IndexOf(item.LoteriaID);
                                if (posicionLoteriaEliminar == -1)
                                {
                                    VM.LoteriasMultiples.Add(item.LoteriaID);
                                }

                                AgregarSorteoAVender(item);
                                RefrescarMonto(true, item.LoteriaID);
                            }
                            else if (item.IsSelected == true)
                            {
                                int loteriaid = item.LoteriaID;
                                item.IsSelected = false;
                                var posicionLoteriaEliminar = VM.LoteriasMultiples.IndexOf(item.LoteriaID);
                                if (posicionLoteriaEliminar != -1)
                                {
                                    VM.LoteriasMultiples.RemoveAt(posicionLoteriaEliminar);
                                }
                                RemoverSorteoAVender(loteriaId: loteriaid, indice: lista.FindIndex(x => x.Sorteo.LoteriaID == loteriaid));
                                RefrescarMonto(false, loteriaid);
                            }
                        }
                    }

                    RefreshListJugadas();
                    break;
            }


        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
        private void listSorteo_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            //    var item = sender as SorteosObservable;
            //    if (e.Key == Key.Enter)
            //    {
            //        teclaSeleccionada = "Enter";
            //        }
            //    else if (e.Key == Key.Space)
            //    {
            //        teclaSeleccionada = "Espacio";
            //        }
            //ValidateSelectOnlyTwo(item);
        }
        private void CheckBox_Click(object sender, RoutedEventArgs e)
        {

            //var item = listSorteo.SelectedItem as SorteosObservable;

            //if (CrearSuper.IsChecked == true)
            //{
            //    if (item.IsSelected == false)
            //    {
            //        if (SuperPales.Count(x => x.IsSelected == true) < 2)
            //        {
            //            item.IsSelected = true;

            //        }
            //    }
            //}


        }
        private void Regulares_MouseDown(object sender, MouseButtonEventArgs e)
        {
            botonSuper.Visibility = Visibility.Collapsed;
            CrearSuper.IsChecked = false;
            foreach (var item in SorteosBinding)
            {
                item.IsSelected = false;
            }
        }
        private void SuperPales_MouseDown(object sender, MouseButtonEventArgs e)
        {
            botonSuper.Visibility = Visibility.Visible;
            CrearSuper.IsChecked = true;
        }
        private void CrearSuper_Unchecked(object sender, RoutedEventArgs e)
        {
            MostrarSuper();
            listSorteo.Focus();
            //listSorteo.SelectedIndex = 0;
            botonSuper.Visibility = Visibility.Collapsed;
        }
        private void CrearSuper_Checked(object sender, RoutedEventArgs e)
        {
            MostrarSuper(false);
            listSorteo.Focus();
            //listSorteo.SelectedIndex = 0;
            botonSuper.Visibility = Visibility.Visible;
        }
        private void SelectCampo(object sender, RoutedEventArgs e)
        {

            txtMonto.Focus();
            if (DataContext != null)
            {
                var vm = DataContext as SorteosViewModel;
                vm.SorteoViewClass = this;
            }
        }


        private void AgregaJugada(object sender, KeyEventArgs e)
        {

            var modal = new ConfirmarMontoWindow();

            if (e.Key == Key.Enter)
            {

                TextBox s = e.Source as TextBox;
                if (s != null)
                {

                    if (txtMonto.Text != "0")
                    {
                        if (txtMonto.Text != "" && txtJugada.Text != "")
                        {


                            if (Convert.ToInt32(txtMonto.Text) <= 500)
                            {

                                AddItem();
                                txtMonto.Text = "";
                                txtJugada.Text = "";

                            }
                            else
                            {
                                modal.Owner = Application.Current.MainWindow;

                                modal.ShowDialog();
                                modal.Mostrar(txtMonto.Text);

                                if (modal.Confirmar)
                                {
                                    AddItem();
                                    txtMonto.Text = "";
                                    txtJugada.Text = "";

                                }
                            }

                        }
                        else
                        {
                            if (txtMonto.Text != "" && txtJugada.Text == "")
                            {
                                if (txtMonto.IsFocused)
                                {
                                    txtJugada.Focus();
                                }

                                ((MainWindow)Window.GetWindow(this)).MensajesAlerta("El campo de jugada no puede estar vacio.", "Aviso");
                            }
                            else if (txtJugada.Text != "" && txtMonto.Text == "")
                            {
                                if (txtJugada.IsFocused)
                                {
                                    txtMonto.Focus();

                                }
                                ((MainWindow)Window.GetWindow(this)).MensajesAlerta("El campo monto no puede estar vacio.", "Aviso");
                            }

                        }
                    }
                    else
                    {
                        ((MainWindow)Window.GetWindow(this)).MensajesAlerta("El monto debe ser mayor a 0.", "Aviso");
                    }


                    if (s.Name == "txtJugada")
                    {
                        txtMonto.Focus();
                    }
                    else if (s.Name == "txtMonto")
                    {
                        txtJugada.Focus();
                    }

                }
                e.Handled = true;
            }
            //MostrarSorteos();
        }
        private void Quitar(object sender, RoutedEventArgs e)
        {
            //RemoveItem();

            ListJugadas.Clear();
            ltJugada.Items.Refresh();

            if (ltJugada.Items.Count == 0)
            {
                txtMonto.Focus();

            }

            RefreshListJugadas();
            RefrescarMonto(false, 0);
            //MostrarSorteos();
        }


        private void ResetearFormularioVenta()
        {
            listSorteo.SelectedItem = null;
            listSorteo.Items.Refresh();
            ListSorteosVender.Clear();
            CantidadSorteos.Content = "0 Sorteos seleccionados";
            txtMonto.Focus();
        }

        private bool ventaThreadIsBusy = false;


        public void CamionMillonario()
        {
            var VM = DataContext as SorteosViewModel;
            var fecha = DateTime.Now;

            if (VM != null)
            {
                var terminal = VM.Autenticador.CurrentAccount.MAR_Setting2.Sesion.Banca;

                TicketModel ticketmodel = new TicketModel();
                List<TicketDetalle> detalles = new List<TicketDetalle>();
                TicketDetalle td;

                var camionSeleccionado = ListSorteosVender.Where(x => x.SorteoNombre.Equals("Camion millonario"));

                foreach (var item in ListJugadas)
                {
                    if (item.TipoJugada.Contains("Quiniela"))
                    {
                        td = new TicketDetalle { Codigo = "CM5", SorteoID = 0, Monto = item.Monto, Jugada = item.Jugadas, TipoJugadaID = 1 };
                        detalles.Add(td);
                    }
                    else if (item.TipoJugada.Contains("Pale"))
                    {
                        td = new TicketDetalle { Codigo = "CM5", SorteoID = 0, Monto = item.Monto, Jugada = item.Jugadas, TipoJugadaID = 2 };
                        detalles.Add(td);
                    }
                    else if (item.TipoJugada.Contains("Tripleta"))
                    {
                        td = new TicketDetalle { Codigo = "CM5", SorteoID = 0, Monto = item.Monto, Jugada = item.Jugadas, TipoJugadaID = 3 };
                        detalles.Add(td);
                    }

                }

                //if (camionSeleccionado.Any())
                //{
                var cincoMinutoService = new CincoMinutosDataService();
                var setProducto = cincoMinutoService.SetProducto("CincoMinutos", VM.Autenticador.CurrentAccount);
                ticketmodel.MontoOperacion = ListJugadas.Sum(x => x.Monto);
                ticketmodel.TicketDetalles = detalles;
                ticketmodel.TerminalID = terminal;
                ticketmodel.Fecha = Convert.ToDateTime(fecha);
                Console.WriteLine(ticketmodel);
                var apuesta = cincoMinutoService.Apuesta(ticketmodel, setProducto, VM.Autenticador.CurrentAccount);

                if (apuesta == null)
                {
                    ((MainWindow)Window.GetWindow(this)).MensajesAlerta("Error interno de aplicacion. Comunique el administrador", "Aviso");
                    return;
                }
                else if (apuesta.RespuestaApi.CodigoRespuesta == "100")
                {
                    ((MainWindow)Window.GetWindow(this)).MensajesAlerta(apuesta.RespuestaApi.MensajeRespuesta, "Excelente");
                    PrintOutHelper.SendToPrinter(apuesta.PrintData);
                    ticketmodel = null;
                    //LimpiarApuesta();
                    //RefreshListJugadas();
                    //GetVendidosHoy();
                    //RefrescaBalance();
                }
                else
                {
                    ((MainWindow)Window.GetWindow(this)).MensajesAlerta(apuesta.RespuestaApi.MensajeRespuesta, "Aviso");
                    return;
                }

                Console.WriteLine(apuesta);
                //}
            }
        }


        private void Vender(object sender, RoutedEventArgs e)
        {

            int cuentaSorteos = ListSorteosVender.Count,
            cuentaJugadas = ltJugada.Items.Count;

            if (cuentaSorteos == 0)
            {
                ((MainWindow)Window.GetWindow(this)).MensajesAlerta("Debe seleccionar al menos un sorteo.", "Aviso");
                listSorteo.Focus();
                listSorteo.SelectedIndex = 0;
            }
            else if (cuentaJugadas == 0)
            {
                ((MainWindow)Window.GetWindow(this)).MensajesAlerta("No hay jugadas en la lista.", "Aviso");

                if (txtMonto.Text == "" && txtJugada.Text == "")
                {
                    txtMonto.Focus();
                    listSorteo.SelectedIndex = -1;
                }

                if (txtMonto.Text != "" && txtJugada.Text == "")
                {
                    txtJugada.Focus();
                    listSorteo.SelectedIndex = -1;
                }

                if (txtMonto.Text == "" && txtJugada.Text != "")
                {
                    txtMonto.Focus();
                    listSorteo.SelectedIndex = -1;
                }
            }
            else
            {
                if (ventaThreadIsBusy == false)
                {
                    Spinner.Visibility = Visibility.Visible;

                    Task.Factory.StartNew(() =>
                    {
                        Thread.Sleep(1000);
                        ventaThreadIsBusy = true;
                        System.Windows.Application.Current.Dispatcher.BeginInvoke(
                        DispatcherPriority.Background,
                        new Action(() =>
                        {
                            try
                            {
                                try
                                {

                                    //if (camion.Any())
                                    //{
                                    //    CamionMillonario();
                                    //    txtMontoTotal.Content = "$0.00";
                                    //    almacenandoMontos = 0;
                                    //}

                                    RegistrarVenta();
                                    txtMontoTotal.Content = "$0.00";
                                    almacenandoMontos = 0;


                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine(ex.Message);
                                }
                                CargarVendidoHoy();
                                CargarBalance();
                            }
                            catch
                            {

                            }
                            ventaThreadIsBusy = false;
                            Spinner.Visibility = Visibility.Collapsed;
                        }));
                    });
                }
            }

        }

        private void RegistrarVenta()
        {

            int cuentaSorteos = ListSorteosVender.Count,
                cuentaJugadas = ltJugada.Items.Count;

            if (cuentaSorteos > 0 && cuentaJugadas > 0)
            {

                //foreach (var sorteo in ListSorteosVender)
                //{
                var quiniela = ListJugadas.Find(x => x.TipoJugada.Contains("  Quiniela"));
                var tripleta = ListJugadas.Find(x => x.TipoJugada.Contains("  Tripleta"));
                var sorteosSeleccionadosS = ListSorteosVender.Any(b => b.Sorteo.Tipo == "S");
                var sorteosSeleccionadosT = ListSorteosVender.Any(b => b.Sorteo.Tipo == "T");

                if ((quiniela != null || tripleta != null) && sorteosSeleccionadosS && !sorteosSeleccionadosT)
                {
                    ((MainWindow)Window.GetWindow(this)).MensajesAlerta("Todas las jugadas deben ser de tipo pale, esta loteria no acepta quiniela ni tripletas.", "Aviso");
                    RefrescarMonto(true, 0);
                    return;
                }
                else
                {
                    var camion = ListSorteosVender.Where(x => x.SorteoNombre.Contains("Camion millonario"));
                    if (camion.Any())
                    {
                        CamionMillonario();
                    }

                    RealizarVenta();
                    ResetearFormularioVenta();
                }




                //if (CrearSuper.IsChecked == true)
                //{
                //    foreach (var item in ListJugadas)
                //    {
                //        foreach (var sorteo in ListSorteosVender)
                //        {
                //            var quiniela = ListJugadas.Find(x => x.TipoJugada.Contains("  Quiniela"));
                //            var tripleta = ListJugadas.Find(x => x.TipoJugada.Contains("  Tripleta"));
                //            var sorteosSeleccionados = sorteo.Sorteo.Tipo.Contains("S");

                //            if ((quiniela != null || tripleta != null) && sorteosSeleccionados)
                //            {
                //                //ventaThreadIsBusy = false;
                //                Spinner.Visibility = Visibility.Collapsed;

                //                if (MessageBox.Show("La loteria que usted eligio NO acepta Quinielas ni Tripletas. \nDesea hacer el ticket quitando esas jugadas?", "Super Pale", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                //                {

                //                    //ventaThreadIsBusy = true;
                //                    Spinner.Visibility = Visibility.Visible;

                //                    if (tripleta != null)
                //                    {
                //                        ListJugadas.Remove(tripleta);
                //                    }

                //                    if (quiniela != null)
                //                    {
                //                        ListJugadas.Remove(quiniela);
                //                    }

                //                    if (tripleta != null && quiniela != null)
                //                    {
                //                        ListJugadas.Remove(tripleta);
                //                        ListJugadas.Remove(quiniela);

                //                    }

                //                    ltJugada.Items.Refresh();
                //                    RefreshListJugadas();
                //                    RealizarVenta();
                //                    ResetearFormularioVenta();
                //                }
                //                else
                //                {
                //                    ((MainWindow)Window.GetWindow(this)).MensajesAlerta("Todas las jugadas deben ser de tipo pale.", "Aviso");
                //                    return;
                //                }

                //            }
                //            else
                //            {
                //                RealizarVenta();
                //                ResetearFormularioVenta();
                //            }
                //        }
                //    }
                //}
                //else
                //{
                //    RealizarVenta();
                //    ResetearFormularioVenta();
                //}

                //}
            }
        }


        private void RealizarVenta()
        {
            var sorteoviewmodel = DataContext as SorteosViewModel;
            if (sorteoviewmodel != null)
            {
                if (RealizarApuestaCommand != null)
                {
                    // var camionId = ListSorteosVender.Where(x => x.Sorteo.LoteriaID == 0).Select(y => y.Sorteo.Loteria).ToArray()[0];
                    //var loteriasID = ListSorteosVender.Where(y => y.Sorteo.LoteriaID != 0).Select(x => x.Sorteo.Loteria).ToArray()[0];

                    //if(ListSorteosVender.Where(x => x.SorteoNombre.Contains("Camion millonario")).Any())
                    //{
                    //    if (loteriasID != ListSorteosVender.Where(x => x.Sorteo.LoteriaID == 0).Select(y => y.Sorteo.Loteria).ToArray()[0])
                    //    {
                    sorteoviewmodel.LoteriasMultiples = ListSorteosVender.Where(y => !y.Sorteo.Loteria.Contains("Camion millonario")).Select(x => x.Sorteo.LoteriaID).ToList();
                    //    }
                    //}
                    //else
                    //{
                    //    sorteoviewmodel.LoteriasMultiples = ListSorteosVender.Select(x => x.Sorteo.LoteriaID).ToList();
                    //}

                    try
                    {
                        for (int i = 0; i < ListSorteosVender.Count; i++)
                        {

                            RealizarApuestaCommand.Execute(new ApuestaResponse { Jugadas = ListJugadas, LoteriaID = ListSorteosVender[i].Sorteo.LoteriaID });
                        }
                    }
                    catch
                    {

                    }

                    ClienteMarWPFWin7.UI.ViewModels.Commands.Sorteos.RealizarApuestaCommand.loteriasNoDisponiblesParaApuesta = new List<string>();
                    ClienteMarWPFWin7.UI.ViewModels.Commands.Sorteos.RealizarApuestaCommand.loteriasNoDisponiblesParaMostrar = String.Empty;


                }
                LimpiarApuesta();
                RefreshListJugadas();
            }
        }

        private void CargarVendidoHoy()
        {
            var vm = DataContext as SorteosViewModel;
            if (vm != null)
            {
                try
                {
                    vm.LeerVendidoHoyAsync();
                }
                catch
                {
                }
            }
        }

        private void CargarBalance()
        {
            var vm = DataContext as SorteosViewModel;
            if (vm != null)
            {
                try
                {
                    vm.LeerBalanceAsync();
                }
                catch
                {
                }
            }
        }

        //private void RealizaApuesta()
        //{
        //    var VM = DataContext as SorteosViewModel;

        //    if (ltJugada.Items.Count > 0)
        //    {
        //        var sorteos = SorteosBinding.Where(x => x.IsSelected == true).ToList();
        //        var Loteria = 0;
        //        if (sorteos.Count > 1 && CrearSuper.IsChecked == true)
        //        {
        //            int sorteoComb = FindCombinations(sorteos.Select(x => x.LoteriaID).ToList());
        //            if (sorteoComb < 0)
        //            {
        //                FindCombinationsNotExist();
        //            }
        //            else
        //            {
        //                Loteria = sorteoComb;
        //            }
        //        }
        //        else if (VM.LoteriasMultiples.Count > 0 && CrearSuper.IsChecked == false)
        //        {

        //            Loteria = sorteos.Select(x => x.LoteriaID).FirstOrDefault();
        //            if (RealizarApuestaCommand != null)
        //            {
        //                foreach (var sorteosSeleccionados in SorteosBinding.Where(x => x.IsSelected == true))
        //                {
        //                    RealizarApuestaCommand.Execute(new ApuestaResponse { Jugadas = ListJugadas, LoteriaID = sorteosSeleccionados.LoteriaID });
        //                }
        //                VM.LoteriasMultiples = new List<int>();

        //            }
        //        }




        //        LimpiarApuesta();
        //        RefreshListJugadas();
        //    }
        //    else
        //    {
        //        ((MainWindow)Window.GetWindow(this)).MensajesAlerta("No hay jugadas en la lista o debe seleccionar una loteria.", "Aviso");

        //    }

        //}



        private void btnCombinar(object sender, RoutedEventArgs e)
        {
            txtMonto.Text = "";
            txtJugada.Text = "";
            OpenCombinacion();

        }


        public void GetJugadasTicket()
        {

            var Vm = DataContext as SorteosViewModel;

            if (Vm != null)
            {
                var jugadas = Vm.ListadoJugada;
                ltJugada.ItemsSource = new List<Jugada>();
                ListJugadas = new List<Jugada>();
                if (jugadas != null && jugadas.Count() > 0)
                {
                    foreach (var jugada in jugadas)
                    {
                        txtJugada.Text = jugada.Jugadas;
                        txtMonto.Text = jugada.Monto.ToString();
                        AddItem(jugada);
                        RefreshListJugadas();
                    }
                    txtJugada.Text = "";
                    txtMonto.Text = "";
                }

                //MostrarSorteos();
            }
            else
            {
                return;
            }
        }

        private void Button_MouseEnter(object sender, MouseEventArgs e)
        {
            ticketSeleccionado = null;
        }

        private void CheckBox_OnlyLoteria(object sender, RoutedEventArgs e)
        {
            var loteria = (sender as CheckBox);
            var loteriaSeleccionada = loteria.DataContext as SorteosObservable;
            foreach (var item in SorteosBinding)
            {
                if (loteriaSeleccionada.LoteriaID == item.LoteriaID)
                {
                    if (item.IsSelected == true)
                    {
                        item.IsSelected = false;
                    }
                    else if (item.IsSelected == false)
                    {
                        item.IsSelected = true;
                    }
                }
            }
        }

        private void CheckBox_ClickOne(object sender, RoutedEventArgs e)
        {
            var VM = DataContext as SorteosViewModel;

            if (VM != null)
            {
                var objectoLoteria = (sender as Control);
                SorteosObservable sorteo = objectoLoteria.DataContext as SorteosObservable;
                var sorteoavender = new SorteosAvender() { SorteoNombre = sorteo.Loteria, Sorteo = sorteo };

                if (CrearSuper.IsChecked == false)
                {
                    var YaEstaSeleccionada = ListSorteosVender.Where(x => x.Sorteo.LoteriaID == sorteo.LoteriaID);
                    var camion = ListSorteosVender.Where(x => x.SorteoNombre.Contains("Camion millonario"));

                    if (sorteo.IsSelected == true)
                    {
                        if (YaEstaSeleccionada.Count() > 0)
                        {
                            MessageBox.Show("Esta combinacion esta en la lista o no esta disponible");
                            return;
                        }
                        else
                        {
                            ListSorteosVender.Add(new SorteosAvender()
                            {
                                SorteoNombre = sorteo.Loteria,
                                Sorteo = sorteo
                            });

                            //var camionSeleccionado = ListSorteosVender.Where(x => x.SorteoNombre.Equals("Camion millonario"));

                            //if (camionSeleccionado.Any())
                            //{
                            //    var cincoMinutoService = new CincoMinutosDataService();
                            //    var setProducto = cincoMinutoService.SetProducto("CincoMinutos", VM.Autenticador.CurrentAccount);
                            //}

                        }
                        RefrescarMonto(true, sorteo.LoteriaID);

                        if (camion != null && ListJugadas.Count > 0)
                        {
                            RefrescarMonto(true, 0);
                        }
                    }
                    else
                    {
                        var SorteoQuitar = 0;
                        sorteo.IsSelected = false;

                        for (var i = 0; i < ListSorteosVender.Count; i++)
                        {
                            if (ListSorteosVender[i].SorteoNombre == sorteo.Loteria)
                            {
                                SorteoQuitar = i;
                            }
                        }
                        ListSorteosVender.RemoveAt(SorteoQuitar);
                        RefrescarMonto(false, sorteo.LoteriaID);
                    }
                }

                //if(CrearSuper.IsChecked == true)
                //{
                //    ListSorteosVender.Add(new SorteosAvender()
                //    {
                //        SorteoNombre = nombreSuperPales
                //    });
                //}


                sorteosSeleccionados.ItemsSource = ListSorteosVender;
                SeleccionadasLista = ListSorteosVender.Count();
                CantidadSorteos.Content = $"{SeleccionadasLista} Sorteos seleccionados";

                //RefreshListJugadas();
                //if (sorteo.IsSelected==true) {
                //    var posicionLoteriaEliminar = VM.LoteriasMultiples.IndexOf(sorteo.LoteriaID);
                //    if (posicionLoteriaEliminar == -1)
                //    {
                //        VM.LoteriasMultiples.Add(sorteo.LoteriaID);
                //    }

                //}else if (sorteo.IsSelected == false)
                //{
                //    var posicionLoteriaEliminar = VM.LoteriasMultiples.IndexOf(sorteo.LoteriaID);
                //    if (posicionLoteriaEliminar != -1) { 
                //        VM.LoteriasMultiples.RemoveAt(posicionLoteriaEliminar);
                //    }
                //}

            }
            //var CheckBox = e.Source as CheckBox;
            //var item = CheckBox.DataContext as SorteosObservable;
            //if (item.IsSelected)
            //{
            //    AddLoteriaMultiples();
            //}
            //else
            //{
            //    RemoveItem(item.LoteriaID);
            //}
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            ValidateSelectOnlyTwo(sender);

            //var item = listSorteo.SelectedItem as SorteosObservable;

            //if (item != null)
            //{


            //    if (CrearSuper.IsChecked == true)
            //    {
            //        if (item.IsSelected == false)
            //        {
            //            if (SuperPales.Count(x => x.IsSelected == true) < 2)
            //            {
            //                item.IsSelected = true;

            //            }
            //        }
            //        else
            //        {
            //            item.IsSelected = false;
            //        }
            //    }
            //}
        }

        private void CheckBox_Checked_1(object sender, RoutedEventArgs e)
        {

            var objectoLoteria = (sender as Control);
            SorteosObservable sorteo = objectoLoteria.DataContext as SorteosObservable;
            var sorteoavender = new SorteosAvender() { SorteoNombre = sorteo.Loteria, Sorteo = sorteo };
            var loteriaSeleccionadas = SuperPales.Where(x => x.IsSelected == true).ToArray();


            ValidateSelectOnlyTwo(sender);

            loteriaCombinada = combinations.Where(x => (x.LoteriaID1 == loteria1 && x.LoteriaID2 == loteria2) || (x.LoteriaID1 == loteria2 && x.LoteriaID2 == loteria1)).Select(y => y.LoteriaIDDestino);

            if (loteriaSeleccionadas.Count() == 2 && loteria1 != 0 && loteria2 != 0)
            {
                if (loteriaCombinada.Count() == 1)
                {
                    var YaEstaSeleccionada = ListSorteosVender.Where(x => x.Sorteo.LoteriaID == Convert.ToInt32(loteriaCombinada.ToArray()[0]));
                    var YaEstaSeleccionadaLoteria = ListSorteosVender.Where(x => x.Sorteo.LoteriaID == Convert.ToInt32(loteriaCombinada.ToArray()[0])).Select(y => y.Sorteo.LoteriaID);

                    if (YaEstaSeleccionada.Count() > 0)
                    {
                        foreach (var i in SuperPales)
                        {
                            i.IsSelected = false;
                        }
                         ((MainWindow)Window.GetWindow(this)).MensajesAlerta("Esta combinacion ya esta en la lista.", "Aviso");

                        if (YaEstaSeleccionada != null)
                        {
                            RefrescarMonto(false, YaEstaSeleccionadaLoteria.First());
                        }

                    }
                    else
                    {
                        SorteosObservable sorteoCopia = new SorteosObservable() { Date = sorteo.Date, Loteria = sorteo.Loteria, IsSelected = true, LoteriaID = sorteo.LoteriaID };
                        sorteoCopia.LoteriaID = Convert.ToInt32(loteriaCombinada.ToArray()[0]);

                        var agrega = new SorteosAvender()
                        {
                            SorteoNombre = nombreSuperPales,
                            Sorteo = sorteoCopia
                        };

                        agrega.Sorteo.Tipo = "S";

                        ListSorteosVender.Add(agrega);

                        foreach (var i in SuperPales)
                        {
                            i.IsSelected = false;
                        }

                        foreach (var x in SorteosBinding)
                        {
                            if (x.LoteriaID == sorteoCopia.LoteriaID)
                            {
                                x.IsSelected = true;
                            }
                        }
                        //Console.WriteLine(loteriaCombinada.ToArray());
                    }
                }
                else if (loteriaCombinada.Count() == 0)
                {
                    foreach (var i in SuperPales)
                    {
                        i.IsSelected = false;
                    }

                   ((MainWindow)Window.GetWindow(this)).MensajesAlerta("Esta combinacion no esta disponible.", "Aviso");

                }

                loteria1 = 0;
                loteria2 = 0;
                listSorteo.Items.Refresh();
            }



            sorteosSeleccionados.ItemsSource = ListSorteosVender;
            SeleccionadasLista = ListSorteosVender.Count();
            CantidadSorteos.Content = $"{SeleccionadasLista} Sorteos seleccionados";

            RefreshListJugadas();
        }

        private void BorrarTodo(object sender, RoutedEventArgs e)
        {
            ListSorteosVender.Clear();
            CantidadSorteos.Content = "0 Sorteos seleccionados";
            foreach (var item in SorteosBinding)
            {
                RefrescarMonto(false, 0);
                item.IsSelected = false;
            }
            txtMonto.Focus();
            listSorteo.SelectedIndex = -1;

            RefreshListJugadas();

        }

        private void QuitarUno(object sender, RoutedEventArgs e)
        {
            var objectoLoteria = (sender as Control);
            SorteosObservable sorteo = objectoLoteria.DataContext as SorteosObservable;
            //var sorteoavender = new SorteosAvender() { SorteoNombre = sorteo.Loteria, Sorteo = sorteo };
            int loteriaid = ListSorteosVender.Last().Sorteo.LoteriaID;
            var listado = (IEnumerable<SorteosAvender>)ListSorteosVender;
            var lista = new List<SorteosAvender>(listado);
            //if (ListSorteosVender.Count > 0)
            //{
            //    int IndiceSorteoARemover = ListSorteosVender.Last().Sorteo.LoteriaID;

            //    foreach (var i in SorteosBinding)
            //    {

            //        if (i.LoteriaID == IndiceSorteoARemover)
            //        {
            //            i.IsSelected = false;
            //        }
            //    }

            //    ListSorteosVender.Remove(ListSorteosVender.Last());

            //    sorteosSeleccionados.ItemsSource = ListSorteosVender;
            //    SeleccionadasLista = ListSorteosVender.Count();
            //    CantidadSorteos.Content = $"{SeleccionadasLista} Sorteos seleccionados";

            //}

            RemoverSorteoAVender(loteriaId: loteriaid, indice: lista.FindIndex(x => x.Sorteo.LoteriaID == loteriaid));

            if (sorteosSeleccionados.Items.Count == 0)
            {
                listSorteo.SelectedIndex = -1;
                listSorteo.Items.Refresh();
                txtMonto.Focus();
            }


            RefreshListJugadas();
        }

        private void AgregarSorteoAVender(SorteosObservable sorteo)
        {
            var sorteoavender = new SorteosAvender() { SorteoNombre = sorteo.Loteria, Sorteo = sorteo };

            ListSorteosVender.Add(new SorteosAvender()
            {
                SorteoNombre = sorteo.Loteria,
                Sorteo = sorteo
            });

            sorteosSeleccionados.ItemsSource = ListSorteosVender;
            SeleccionadasLista = ListSorteosVender.Count();
            CantidadSorteos.Content = $"{SeleccionadasLista} Sorteos seleccionados";

        }

        private void RemoverSorteoAVender(int loteriaId, int indice)
        {

            if (ListSorteosVender.Count > 0)
            {

                foreach (var i in SorteosBinding)
                {
                    if (i.LoteriaID == loteriaId)
                    {
                        i.IsSelected = false;
                    }
                }


                if (indice >= 0)
                {
                    try
                    {
                        ListSorteosVender.RemoveAt(indice);
                    }
                    catch
                    {

                    }
                }

                sorteosSeleccionados.ItemsSource = ListSorteosVender;
                SeleccionadasLista = ListSorteosVender.Count();
                CantidadSorteos.Content = $"{SeleccionadasLista} Sorteos seleccionados";

            }

            RefreshListJugadas();
        }

        private void listSorteo_GotFocus(object sender, RoutedEventArgs e)
        {
            SorteoItemFocused = true;
        }

        private void listSorteo_LostFocus(object sender, RoutedEventArgs e)
        {
            SorteoItemFocused = false;
        }

        private void txtMonto_GotFocus(object sender, RoutedEventArgs e)
        {
            TxtMontoFocus = true;

            listSorteo.SelectedIndex = -1;
            listSorteo.Items.Refresh();
        }

        private void txtMonto_LostFocus(object sender, RoutedEventArgs e)
        {
            TxtMontoFocus = false;
            (sender as TextBox)?.SelectAll();

        }

        private void txtJugada_GotFocus(object sender, RoutedEventArgs e)
        {
            TxtJugadaFocus = true;
            (sender as TextBox)?.SelectAll();

            listSorteo.SelectedIndex = -1;
            listSorteo.Items.Refresh();
        }

        private void txtJugada_LostFocus(object sender, RoutedEventArgs e)
        {
            TxtJugadaFocus = false;
        }

        private void EliminarSoloUnSorteoSeleccionado(object sender, RoutedEventArgs e)
        {
            int loteriaid = int.Parse((sender as Button)?.Tag?.ToString() ?? "0");
            var listado = (IEnumerable<SorteosAvender>)ListSorteosVender;
            var lista = new List<SorteosAvender>(listado);
            RefrescarMonto(false, loteriaid);
            RemoverSorteoAVender(loteriaId: loteriaid, indice: lista.FindIndex(x => x.Sorteo.LoteriaID == loteriaid));

            if (sorteosSeleccionados.Items.Count == 0)
            {
                txtMonto.Focus();
                listSorteo.SelectedIndex = -1;
                listSorteo.Items.Refresh();
            }
        }

        private void CombinarTodo(object sender, RoutedEventArgs e)
        {
            if (CrearSuper.IsChecked == true)
            {
                var combinauno = new List<Tuple<int, string>>();
                var combinados = new List<Tuple<int, string>>();
                var combinacionesTodas = new List<Tuple<int, int, string>>();
                var combinacionesDisponibles = new List<Tuple<int, int, string, int>>();
                var combinacionesNoDisponibles = new List<Tuple<int, int, string, int>>();
                var idsCombinacionesDisponibles = new List<Tuple<int, int, int>>();

                foreach (var item in SessionGlobals.LoteriasDisponibles)
                {
                    combinauno.Add(new Tuple<int, string>(item.Numero, item.NombreResumido));
                    combinados.Add(new Tuple<int, string>(item.Numero, item.NombreResumido));
                }

                foreach (var item in combinations)
                {
                    idsCombinacionesDisponibles.Add(new Tuple<int, int, int>(item.LoteriaID1, item.LoteriaID2, item.LoteriaIDDestino));
                }


                for (int i = 0; i < combinauno.Count; i++)
                {
                    for (int j = 0; j < combinados.Count; j++)
                    {
                        if (combinauno[i].Item1 != combinados[j].Item1)
                        {
                            combinacionesTodas.Add(new Tuple<int, int, string>(
                                combinauno[i].Item1,
                                combinados[j].Item1,
                                $"SP {combinauno[i].Item2} - {combinados[j].Item2}")
                                );
                        }
                    }
                }


                for (int x = 0; x < combinacionesTodas.Count; x++)
                {
                    for (int y = 0; y < idsCombinacionesDisponibles.Count; y++)
                    {
                        if ((combinacionesTodas[x].Item1 == idsCombinacionesDisponibles[y].Item1) && (combinacionesTodas[x].Item2 == idsCombinacionesDisponibles[y].Item2))
                        {
                            combinacionesDisponibles.Add(new Tuple<int, int, string, int>(
                                combinacionesTodas[x].Item1,
                                combinacionesTodas[x].Item2,
                               combinacionesTodas[x].Item3,
                               idsCombinacionesDisponibles[y].Item3));
                            break;
                        }

                        if (y == (idsCombinacionesDisponibles.Count - 1))
                        {
                            combinacionesNoDisponibles.Add(new Tuple<int, int, string, int>(
                                 combinacionesTodas[x].Item1,
                                 combinacionesTodas[x].Item2,
                                combinacionesTodas[x].Item3,
                                0));
                        }
                    }
                }

                #region NoDisponibles

                string sorteoNoDisponible = string.Empty;

                for (int i = 0; i < combinacionesNoDisponibles.Count; i++)
                {
                    sorteoNoDisponible += $" ** {combinacionesNoDisponibles[i].Item3} **{ Environment.NewLine}";
                }

                if (combinacionesNoDisponibles.Count > 0)
                {
                    var Texto = $"{ Environment.NewLine} { Environment.NewLine} Estos y otros sorteos no estan disponibles { Environment.NewLine} Favor solicitar combinaciones a la central { Environment.NewLine} Las disponibles han sido seleccionadas, validar jugada { Environment.NewLine} { Environment.NewLine} ";
                    MessageBox.Show(sorteoNoDisponible.PadRight(800).Substring(0, 800).TrimEnd() + Texto, "Combinaciones no disponibles", MessageBoxButton.OK, MessageBoxImage.Information);
                }

                #endregion

                #region AgregandoDisponibles

                var superPaleDisponibles = new List<int>();

                for (int i = 0; i < combinacionesDisponibles.Count; i++)
                {
                    var nuevoSorteo = new SorteosObservable()
                    {
                        Date = DateTime.Today,
                        Loteria = combinacionesDisponibles[i].Item3,
                        IsSelected = true,
                        LoteriaID = combinacionesDisponibles[i].Item4
                    };


                    if (!ListSorteosVender.Any(b => b.Sorteo.LoteriaID == nuevoSorteo.LoteriaID))
                    {
                        var agrega = new SorteosAvender()
                        {
                            SorteoNombre = combinacionesDisponibles[i].Item3,
                            Sorteo = nuevoSorteo
                        };

                        agrega.Sorteo.Tipo = "S";

                        ListSorteosVender.Add(agrega);

                        superPaleDisponibles.Add(combinacionesDisponibles[i].Item4);
                        RefrescarMonto(true, agrega.Sorteo.LoteriaID);
                    }

                }

                if (superPaleDisponibles.Count > 0)
                {
                    foreach (var x in SorteosBinding)
                    {
                        if (superPaleDisponibles.Any(y => y == x.LoteriaID))
                        {
                            x.IsSelected = true;
                        }

                    }
                }

                sorteosSeleccionados.ItemsSource = ListSorteosVender;
                SeleccionadasLista = ListSorteosVender.Count();
                CantidadSorteos.Content = $"{SeleccionadasLista} Sorteos seleccionados";
                #endregion
            }
            RefreshListJugadas();

        }

        private void SeleccionarMonto(object sender, RoutedEventArgs e)
        {
            txtMonto.Focus();
            txtMonto.SelectAll();
        }

        private void SeleccionarJugada(object sender, RoutedEventArgs e)
        {
            txtJugada.Focus();
            txtJugada.SelectAll();
        }

        public void TriggerButtonClickEvent(Button boton)
        {
            try
            {

                if (boton != null)
                {
                    var peer = new ButtonAutomationPeer(boton);
                    var invokeProvider = peer.GetPattern(PatternInterface.Invoke) as IInvokeProvider;
                    invokeProvider?.Invoke();
                }
            }
            catch { }
        }

        private void QuitarDeJugadas(object sender, RoutedEventArgs e)
        {
            RemoveItem();
            txtMonto.Focus();
            if (ltJugada.Items.Count == 0)
            {
                txtMonto.Focus();

            }


        }

        //private void SeleccionarPrimerRowTablaJugada(object sender, RoutedEventArgs e)
        //{

        //    if (ltJugada.Items.Count > 0)
        //    {
        //        //ltJugada.Focus();

        //        for (var i = 0; i < ltJugada.Items.Count; i++)
        //        {
        //            DataGridRow row = ltJugada.ItemContainerGenerator.ContainerFromIndex(i) as DataGridRow;

        //            if (row != null)
        //            {
        //                ltJugada.Focus();
        //                row.IsSelected = true;
        //                row.Focus();
        //                TxtMontoFocus = false;
        //            }
        //        }

        //    }
        //}

        private void CrearSuper_GotFocus(object sender, RoutedEventArgs e)
        {
            CrearSuperFocus = true;
        }

        private void CrearSuper_LostFocus(object sender, RoutedEventArgs e)
        {
            CrearSuperFocus = false;
        }

        private void AbrirDialogoConsulta(object sender, RoutedEventArgs e)
        {
            Action focusNumeroTicket = () =>
            {
                var timer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(50) };
                timer.Tick += (s, args) =>
                {
                    timer.Stop();
                    ConsultaModal.TxtTicket.Focus();
                };
                timer.Start();
            };

            Action seleccionar = () =>
            {

                var timer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(50) };

                if (InputHelper.InputIsBlank(txtMonto.Text))
                {
                    timer.Tick += (s, args) =>
                    {
                        timer.Stop();
                        txtMonto.Focus();
                    };
                    timer.Start();

                    return;
                }

                if (InputHelper.InputIsBlank(txtJugada.Text))
                {
                    timer.Tick += (s, args) =>
                    {
                        timer.Stop();
                        txtJugada.Focus();

                    };
                    timer.Start();

                    return;
                }

                timer.Tick += (s, args) =>
                {
                    timer.Stop();
                    txtMonto.Focus();
                };
                timer.Start();
            };

            if (ValidarPagoTicketCommand != null)
            {
                var acciones = new Tuple<Action, Action>(seleccionar, focusNumeroTicket);

                ValidarPagoTicketCommand.Execute(acciones);
            }

        }

        private void ltJugada_GotFocus(object sender, RoutedEventArgs e)
        {
            listSorteo.SelectedIndex = -1;
            listSorteo.Items.Refresh();
        }

        private void Button_GotFocus(object sender, RoutedEventArgs e)
        {
            listSorteo.SelectedIndex = -1;
            listSorteo.Items.Refresh();
        }

        private void Button_GotFocus_1(object sender, RoutedEventArgs e)
        {
            listSorteo.SelectedIndex = -1;
            listSorteo.Items.Refresh();
        }

        private bool consultaThreadIsBusy = false;
        private DependencyObject listSorteoItem;

        private void Consultar(object sender, RoutedEventArgs e)
        {
            if (consultaThreadIsBusy == false)
            {
                SpinnerConsulta.Visibility = Visibility.Visible;

                Task.Factory.StartNew(() =>
                {
                    consultaThreadIsBusy = true;
                    System.Windows.Application.Current.Dispatcher.BeginInvoke(
                    DispatcherPriority.Background,
                    new Action(() =>
                    {
                        try
                        {
                            AbrirDialogoConsulta(sender, e);
                        }
                        catch
                        {

                        }
                        consultaThreadIsBusy = false;
                        SpinnerConsulta.Visibility = Visibility.Collapsed;
                    }));
                });
            }
        }

        private void vistaSorteo_MouseLeave(object sender, MouseEventArgs e)
        {
            if (txtMonto.IsFocused == false && txtJugada.IsFocused == false && listSorteo.SelectedIndex < 0 && ltJugada.IsFocused == false)
            {
                txtMonto.Focus();
            }

        }

        private void BajadaGrid_Click(object sender, RoutedEventArgs e)
        {

            if (ltJugada.Items.Count > 0)
            {
                DataGridRow row = ltJugada.ItemContainerGenerator.ContainerFromIndex(ltJugada.SelectedIndex += 1) as DataGridRow;

                if (row != null)
                {
                    row.IsSelected = true;
                    row.Focus();
                    TxtMontoFocus = false;
                }
            }


        }

        private void SubidaGrid_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ltJugada.Items.Count > 0 && !listSorteo.IsFocused)
                {
                    if (ltJugada.SelectedIndex != 0)
                    {
                        DataGridRow row = ltJugada.ItemContainerGenerator.ContainerFromIndex(ltJugada.SelectedIndex -= 1) as DataGridRow;

                        if (row != null)
                        {
                            row.IsSelected = true;
                            row.Focus();
                            TxtMontoFocus = false;
                        }
                    }
                    else
                    {
                        DataGridRow rowdos = ltJugada.ItemContainerGenerator.ContainerFromIndex(0) as DataGridRow;
                        rowdos.IsSelected = false;
                        txtMonto.Focus();
                    }

                }
            }
            catch
            {

            }

        }

        private void ConsultarCM5_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ConsultaCincoMinutos modal = new ConsultaCincoMinutos();
                modal.ShowDialog();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        //private void sorteosSeleccionados_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        //{
        //    //if (Keyboard.IsKeyDown(Key.Left))
        //    //{
        //    //    txtJugada.Focus();
        //    //}

        //    if (e.Source != null)
        //    {
        //        switch (e.ChangedButton)
        //        {
        //            case MouseButton.Left:
        //                txtJugada.Focus();
        //                break;
        //        }
        //    }

        //}





        //private void AddLoteriaMultiples()
        //{
        //    var sorteosCheck = SorteosBinding.Where(x => x.IsSelected == true).ToList();

        //    if (sorteosCheck.Count != 0 && CrearSuper.IsChecked == false)
        //    {

        //        var hasSorteo = sorteosCheck.Select(x => x.LoteriaID).ToList();
        //        if (hasSorteo.Count != 0)
        //        {
        //            foreach (var item in hasSorteo)
        //            {
        //                SorteosSave.LoteriasResponse.Add(item);
        //            }

        //        }

        //    }
        //    else if (sorteosCheck.Count != 0 && sorteosCheck.Count == 2 && CrearSuper.IsChecked == true)
        //    {
        //        var combinationID = FindCombinations(sorteosCheck.Select(x => x.LoteriaID).ToList());
        //        if (combinationID != 0)
        //        {
        //            var sorteoIsSave = SorteosSave.LoteriasResponse.Where(x => x == combinationID).Any();
        //            if (!sorteoIsSave)
        //            {
        //                SorteosSave.LoteriasResponse.Add(combinationID);
        //                RemoveAllSeletion();
        //            }

        //        }
        //        else
        //        {
        //            MessageBox.Show("Esta combinacion ya esta en la lista o no esta diponible", "Aviso", MessageBoxButton.OK, MessageBoxImage.Information);
        //        }
        //    }


        //    RefreshListSorteosSelect();

        //}

        //private void RemoveItem(int LoteriaID = 0)
        //{
        //    dynamic selectItem = listSorteo.SelectedItem;
        //    if (selectItem == null && LoteriaID == 0)
        //    {
        //        if (listSorteo.Items.Count > 0)
        //        {
        //            dynamic sorteosInList = listSorteo.ItemsSource;
        //            var LoteriasName = new List<string>();
        //            foreach (var items in sorteosInList)
        //            {
        //                LoteriasName.Add(items.Sorteo);
        //            }
        //            var item = LoteriasName.Last();

        //            if (item.Contains('+'))
        //            {
        //                var SorteosSplit = item.Split('+').ToList();
        //                SorteosSplit.Remove("+");
        //                var LoteriaIds = FindLotteryIDs(SorteosSplit);
        //                int CombinationID = FindCombinations(LoteriaIds);
        //                SorteosSave.LoteriasResponse.Remove(CombinationID);
        //            }
        //            else
        //            {
        //                int sorteoID = ConvertToSorteos(SorteosBinding).Where(x => x.Loteria == item).Select(x => x.LoteriaID).FirstOrDefault();
        //                SorteosSave.LoteriasResponse.Remove(sorteoID);
        //                var sorteo = SorteosBinding.Where(x => x.LoteriaID == sorteoID).FirstOrDefault();
        //                sorteo.IsSelected = false;
        //            }

        //        }
        //        else
        //        {
        //            MessageBox.Show("No hay sorteos en la lista.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Information);
        //        }

        //    }
        //    else if (LoteriaID != 0)
        //    {
        //        SorteosSave.LoteriasResponse.Remove(LoteriaID);
        //        var sorteo = SorteosBinding.Where(x => x.LoteriaID == LoteriaID).FirstOrDefault();
        //        sorteo.IsSelected = false;
        //    }
        //    else if (selectItem != null)
        //    {
        //        string sorteoselect = selectItem.Sorteo;
        //        if (sorteoselect.Contains('+'))
        //        {
        //            var SorteosSplit = sorteoselect.Split('+').ToList();
        //            SorteosSplit.Remove("+");
        //            var LoteriaIds = FindLotteryIDs(SorteosSplit);
        //            int CombinationID = FindCombinations(LoteriaIds);
        //            SorteosSave.LoteriasResponse.Remove(CombinationID);
        //        }
        //        else
        //        {
        //            int sorteoID = ConvertToSorteos(SorteosBinding).Where(x => x.Loteria == selectItem.Sorteo).Select(x => x.LoteriaID).FirstOrDefault();
        //            SorteosSave.LoteriasResponse.Remove(sorteoID);
        //            var sorteo = SorteosBinding.Where(x => x.LoteriaID == sorteoID).FirstOrDefault();
        //            sorteo.IsSelected = false;
        //        }

        //    }


        //    RefreshListSorteosSelect();
        //    listLoteriasSelect.Focus();
        //    listLoteriasSelect.SelectedIndex = 0;
        //}
        //private void RefreshListSorteosSelect()
        //{
        //    sorteosselect.Content = "0";
        //    listLoteriasSelect.ItemsSource = new List<string>();
        //    var Sorteos = FindNameLottery(SorteosSave.LoteriasResponse);
        //    var values = from str in Sorteos select new { Sorteo = str };
        //    listLoteriasSelect.ItemsSource = values.ToList();
        //    sorteosselect.Content = SorteosSave.LoteriasResponse.Count;
        //    listLoteriasSelect.Items.Refresh();
        //}

        //private List<string> FindNameLottery(List<int> LotteryIDs)
        //{
        //    var nameLotterys = new List<string>();
        //    var sorteos = ConvertToSorteos(SorteosBinding);
        //    if (LotteryIDs.Count != 0)
        //    {

        //        foreach (var item in LotteryIDs)
        //        {
        //            string result = sorteos.Where(x => x.LoteriaID == item).Select(x => x.Loteria).FirstOrDefault();
        //            var resultSuper = combinations.Where(x => x.LoteriaDestino == item).FirstOrDefault();

        //            if (result != null && resultSuper != null)
        //            {
        //                if (CrearSuper.IsChecked == true)
        //                {
        //                    string loteria1 = sorteos.Where(x => x.LoteriaID == resultSuper.Loteria1).Select(x => x.Loteria).FirstOrDefault();
        //                    string loteria2 = sorteos.Where(x => x.LoteriaID == resultSuper.Loteria2).Select(x => x.Loteria).FirstOrDefault();
        //                    nameLotterys.Add(loteria1 + " + " + loteria2);
        //                }
        //                else
        //                {
        //                    nameLotterys.Add(result);
        //                }

        //            }
        //            else if (result == null && resultSuper != null)
        //            {
        //                string loteria1 = sorteos.Where(x => x.LoteriaID == resultSuper.Loteria1).Select(x => x.Loteria).FirstOrDefault();
        //                string loteria2 = sorteos.Where(x => x.LoteriaID == resultSuper.Loteria2).Select(x => x.Loteria).FirstOrDefault();
        //                nameLotterys.Add(loteria1 + " + " + loteria2);

        //            }
        //            else if (result != null && resultSuper == null)
        //            {
        //                nameLotterys.Add(result);

        //            }

        //        }

        //    }

        //    return nameLotterys;
        //}

    }

}

public class SorteosAvender
{
    public string SorteoNombre { get; set; }
    public SorteosObservable Sorteo { get; set; }
}







