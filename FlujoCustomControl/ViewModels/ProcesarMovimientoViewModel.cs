using System;
using System.Windows;
using System.Windows.Input;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using System.Collections.Generic;

using FlujoCustomControl.Helpers;
using Flujo.Entities.WpfClient.POCO;
using Flujo.Entities.WpfClient.Enums;
using Flujo.Entities.WpfClient.RequestModel;
using Flujo.Entities.WpfClient.ResponseModels;

using FlujoCustomControl.ViewModels.CommonViews;
using FlujoCustomControl.ViewModels.Commands;
using FlujoCustomControl.Code.BussinessLogic;
using System.Security;
using System.Windows.Controls;
using System.Globalization;

namespace FlujoCustomControl.ViewModels
{
    public partial class ProcesarMovimientoViewModel : CommonBase
    {
        #region Inicializacion de Control de Usuario


        public void InicializarControlUsuario(int pBancaID)
        {
            this.OpcionActualTipoFlujo = "0";

            Label_Status_Content = "... Recibiendo dinero.";

            this.CONST_OPCION_TIPO_FLUJO = this.OpcionActualTipoFlujo;


            this.TheListOfTiposIngresos = new List<TipoIngresoResponseModel>();
            this.TheLitsOfTiposEgresos = new List<TipoEgresoResponseModel>();


            this.ColeccionTipoIngresoMovimientos = new ObservableCollection<TipoIngresoResponseModel>();
            this.ColeccionTipoEgresoMovimientos = new ObservableCollection<TipoEgresoResponseModel>();

            this.TheListOfTiposIngresos.AddRange(MovimientoLogic.GetTiposDeIngresos());
            this.TheLitsOfTiposEgresos.AddRange(MovimientoLogic.GetTiposDeEgresos());




            foreach (var item in TheListOfTiposIngresos) this.ColeccionTipoIngresoMovimientos.Add(item);
            foreach (var item in TheLitsOfTiposEgresos) this.ColeccionTipoEgresoMovimientos.Add(item);


            TipoIngresoSeleccionado = this.ColeccionTipoIngresoMovimientos[0];
            TipoEgresoSeleccionado = this.ColeccionTipoEgresoMovimientos[0];


            ShowTipoIngresoCombobox = true;
            ShowTipoEgresoCombobox = false;

            ShowSeccionPin = false;  // ..... ..... ..... Oculto la seccion para introducir una credencial
            IsEnableCajera = true;

            if (
                       (OpcionActualTipoFlujo.Equals("0") && TipoIngresoSeleccionado.LogicaKey == (int)TipoFlujoSubCategorias.DepositoDineroCaja)
                 || (OpcionActualTipoFlujo.Equals("1") && TipoEgresoSeleccionado.LogicaKey == (int)TipoFlujoSubCategorias.RetiroDineroCaja)
             )
            {
                ShowExternalUserSecurityPanel = true;  //... .... .... Muestro el Panel de Seguridad que contiene el numero de Documento y la credencial
                IsSeccionExternoEnable = true;         // @@ Propiedad para habilitar la (Seccion Externo Enable)
            }
            else
            {
                ShowExternalUserSecurityPanel = true; // @@ Tenia false 
                IsSeccionExternoEnable = false;       // @@ Propiedad para habilitar la (Seccion Externo Enable)
            }

            RadGroupTipoFlujoCommand = new RadioGroupCommand(onRadioGroupTipoFlujoChanged, RadioGroupPermitChangedHandler);
            Button_Procesar_Command = new RelayCommand(onBtnProcesarClick);
            Button_Restablecer_Command = new RelayCommand(onBtnRestablecerClicked);
            Button_Aplicar_Command = new RelayCommand(onBtnAplicar_Clicked);
            Button_Cancelar_Security_Selection_Command = new RelayCommand(onBtnCancelarSecuritySecuritySelecction_Clicked);
            Concepto_Combobox_Changed_Trigger_Command = new RelayCommand(onConceptoCombobox_Trigger_Changed);
            Button_CerrarModal_Command = new RelayCommand(onBtnCerrarModalClicked);

        }

        private void onBtnCerrarModalClicked(object obj)
        {
            ShowSeccionPin = false;
        }
        #endregion



        ///     Valida si el radio button group 'GroupTipoFlujo' posee CommandParameter
        private bool RadioGroupPermitChangedHandler(object parameter)
        {
            return (parameter != null);
        }

        ///    Maneja el cambio de la opcion del radio button group 'GroupTipoFlujo'
        private void onRadioGroupTipoFlujoChanged(object parameter)
        {
            string param = (string)parameter;

            if (param.Equals("0")) // Mostrando solo los Tipos de Ingresos
            {
                ShowTipoEgresoCombobox = false;

                TipoIngresoSeleccionado = ColeccionTipoIngresoMovimientos[0];

                ShowTipoIngresoCombobox = true;
            }
            else // Mostrando solo los Tipos de Egresos 
            {
                ShowTipoIngresoCombobox = false;

                TipoEgresoSeleccionado = ColeccionTipoEgresoMovimientos[0];

                ShowTipoEgresoCombobox = true;
            }

        }

        ///    Metodo encargado de procesar el movimiento registrado por un usuario
        private void onBtnProcesarClick(object parameter)
        {
            MovimientoInsertEstado estadoInsert = null;
            MovimientoBancaData data = new MovimientoBancaData();
            GroupedPasswordBox g = (GroupedPasswordBox)parameter;
            PasswordBox p = g.SecurePin;
            this.ColeccionDeErrores.Clear();

            try
            {
                data.BancaID = Banca;
                decimal montoTransaccion;
                bool MontoFueParseado = Decimal.TryParse(Monto_Text, NumberStyles.Any, CultureInfo.InvariantCulture, out montoTransaccion);

                if (MontoFueParseado)
                {
                    data.Monto = montoTransaccion;
                }
                else
                {
                    this.ColeccionDeErrores.Add(new ErrorMetadata() { MensajeError = " * Error: ( Monto ) inválido" });

                    return;
                }


                if (OpcionActualTipoFlujo == "0"  /*<.... Ingreso*/)
                {
                    data.TipoFlujo = TipoFlujo.Ingreso;
                    data.TipoFlujoTipoID = TipoIngresoSeleccionado.Id;
                }
                else if (OpcionActualTipoFlujo == "1" /*<.... Egreso*/)
                {
                    data.TipoFlujo = TipoFlujo.Egreso;
                    data.TipoFlujoTipoID = TipoEgresoSeleccionado.Id;

                    if (!CuadreValidacionHelper.SePermiteRetirarMonto(this.BancaBalanceActual, data.Monto))
                    {
                        this.ColeccionDeErrores.Add(new ErrorMetadata() { MensajeError = " * Error: La caja no cuenta con los fondos a retirar." });

                        return;
                    }

                }

                //Se valida el Token de usuario en caso de |DepositoDineroCaja| y |RetiroDineroCaja|
                if (
                              OpcionActualTipoFlujo.Equals("0") && TipoIngresoSeleccionado.LogicaKey != null && TipoIngresoSeleccionado.LogicaKey.Value == (int)TipoFlujoSubCategorias.DepositoDineroCaja
                        ||
                              OpcionActualTipoFlujo.Equals("1") && TipoEgresoSeleccionado.LogicaKey != null && TipoEgresoSeleccionado.LogicaKey.Value == (int)TipoFlujoSubCategorias.RetiroDineroCaja
                    )
                {

                    if (this._usuario_externo == null)
                    {

                        this.ColeccionDeErrores.Add(new ErrorMetadata() { MensajeError = " * Error   : Ingrese el (Pin) y (Token) del usuario autorizado." });

                        return;
                    }

                    if (this._usuario_externo_required_token == null || (!(this._usuario_externo_required_token.Toquen.Equals(p.Password))))
                    {

                        //Estas lineas de codigo cambia el usuario Token ID al fallar
                        //this._usuario_externo_required_token = UsuarioTokenSeguridadLogic.ConsultaUnSoloTokenDeSeguridadAleatorio(this._usuario_externo.UsuarioID);
                        //TxtUsuarioExternoOrdinalCodigoSeguridad_Text = this._usuario_externo_required_token.Posicion + "";

                        p.Clear();

                        this.ColeccionDeErrores.Add(new ErrorMetadata() { MensajeError = " * Error: token de seguridad invalido." });

                        return;
                    }


                    if (OpcionActualTipoFlujo == "0"  /*<.... Ingreso*/)
                    {

                        //Views.CajeraDialog cd = new Views.CajeraDialog();
                        //cd.Owner = this.Parent;
                        //cd.ShowDialog();
                        //this.NombreCajera = cd.NombreCajera;
                        if (Regex.Replace(NombreCajera, @"\s+", "").Equals(string.Empty))
                        {
                            this.ColeccionDeErrores.Add(new ErrorMetadata() { MensajeError = " * Error: el campo (Nombre de cajera) es requerido" });
                            return;
                        }


                        Caja cajaUsuExt = CajaLogic.GetUsuarioCaja(this._usuario_externo.UsuarioID);
                        if ((cajaUsuExt.BalanceActual - montoTransaccion) < 0)
                        {
                            this.ColeccionDeErrores.Add(new ErrorMetadata() { MensajeError = $" * Error: Usuario ext. fondos insuficientes. Usuario ext. Balance {cajaUsuExt.BalanceEnFormato}" });
                            return;
                        }


                    }


                    if (OpcionActualTipoFlujo.Equals("0") && TipoIngresoSeleccionado.LogicaKey != null && TipoIngresoSeleccionado.LogicaKey.Value == (int)TipoFlujoSubCategorias.DepositoDineroCaja)
                    {
                        data.Descripcion = $@"{TipoIngresoSeleccionado.Nombre} por usuario : {this._usuario_externo.Nombre}," + ComentarioText ?? "";
                    }
                    else
                    {
                        data.Descripcion = $@"{TipoEgresoSeleccionado.Nombre} por usuario : {this._usuario_externo.Nombre}," + ComentarioText ?? "";
                    }

                    estadoInsert = MovimientoLogic.InsertaMovimientoConUsuarioAutorizado(data, UsuarioID, this._usuario_externo);

                }//Fin IF
                else
                {

                    if (OpcionActualTipoFlujo == "0"  /*<.... Ingreso*/)
                    {

                        //Views.CajeraDialog cd = new Views.CajeraDialog();
                        //cd.Owner = this.Parent;
                        //cd.ShowDialog();

                        //this.NombreCajera = cd.NombreCajera;


                        if (Regex.Replace(NombreCajera, @"\s+", "").Equals(string.Empty))
                        {
                            this.ColeccionDeErrores.Add(new ErrorMetadata() { MensajeError = " * Error: el campo (Nombre de cajera) es requerido" });

                            return;
                        }
                    }

                    data.Descripcion = ComentarioText;
                    estadoInsert = MovimientoLogic.InsertarMovimientoSinUsuarioAutorizado(data, UsuarioID);

                }// Fin ELSE


                if (estadoInsert.FueProcesado)
                {

                    //MessageBox.Show("Se ha registrado un movimiento exitosamente.", "Transaccion exitosa", MessageBoxButton.OK, MessageBoxImage.Information);

                    this.BancaBalanceActual = CajaLogic.GetBancaBalance(Banca);
                    string strBancaBalance = "$ " + ((this.BancaBalanceActual == 0) ? "0.00" : this.BancaBalanceActual.ToString("#,##0.00", CultureInfo.CreateSpecificCulture("en-US")));
                    Set_Windows_MainLabelBancaBalanceActual?.Invoke("|Balance : " + strBancaBalance);

                    PrintDocumentTransaccionSinCuadre printDoc = new PrintDocumentTransaccionSinCuadre();
                    Banca banca = BancaLogic.GetBanca(this.Banca);

                    printDoc.BanContacto = banca.BanContacto;
                    printDoc.BanDireccion = banca.BanDireccion;
                    printDoc.BanTransaccion = estadoInsert.Referencia;
                    printDoc.FechaTransaccion = estadoInsert.MovFechaConFormato_dd_MMM_yyyy_hh_mm_tt;
                    printDoc.BanMonto = data.Monto.ToString("$#,##0.00", CultureInfo.CreateSpecificCulture("en-US"));


                    if (data.TipoFlujo == TipoFlujo.Ingreso  /*<.... Ingreso*/)
                    {
                        //Entrada de dinero en Banca

                        printDoc.Recibido__Por = NombreCajera;
                        printDoc.EsUnDeposito = true;


                    }
                    else if (data.TipoFlujo == TipoFlujo.Egreso   /*<.... Egreso*/)
                    {
                        //Salida de dinero en Banca

                        printDoc.Recibido__Por = this._usuario_externo == null ? "" : this._usuario_externo.Nombre;
                        printDoc.EsUnDeposito = false;
                    }

                    g.SecureCedula.Clear();
                    Restablecer();

                    string str__Print = ReciboTemplateHelper.Get__ReciboTemplate__SinCuadre(printDoc);  // @@@@ String generado a imprimir en el recibo @@@@                                                                            


                    PrintOutHelper.SendToPrinter(str__Print); //Descomentar



                }
                else
                {
                    MessageBox.Show(estadoInsert.MensajeError, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + " " + ex.StackTrace, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        ///     Metodo encargado de restablecer  la seleccion para re-procesar un movimiento nuevo
        private void onBtnRestablecerClicked(object parameter)
        {
            PasswordBox cedula = (PasswordBox)parameter;
            cedula.Clear();
            Restablecer();
        }


        ///    Restaura  la configuracion a los valores de la configuracion incial del control 'ProcesarMovimientoUserControl' 
        private void Restablecer()
        {
            //OpcionActualTipoFlujo = CONST_OPCION_TIPO_FLUJO;  // Restablece la opcion default del radio button group 'GroupTipoFlujo' 

            if (OpcionActualTipoFlujo == "0")
            {
                // $$ Mostrar  seleccion tipo de ingresos
                ShowTipoEgresoCombobox = false;                                                           // El combobox 'cbxTipoEgresos' es escondido
                //TipoIngresoSeleccionado = ColeccionTipoIngresoMovimientos[0];   // Restablesco el valor default del tipo de ingreso seleccionado en el combobox 'cbxTipoIngresos'
                ShowTipoIngresoCombobox = true;                                                           // Muestro el combobox 'cbxTipoIngresos'
                ComentarioText = "";                                                              // Restablesco el valor default de la propiedad Text del TextBox 'txtComentario'
                Monto_Text = "";                                                               // Restablesco el valor default de la propiedad Text del TextBox 'txtMonto'
                NombreCajera = "";
                IsEnableCajera = true;
            }
            else if (OpcionActualTipoFlujo == "1")
            {
                // $$ Mostrar seleccion tipo  de egresos
                ShowTipoIngresoCombobox = false;                                                           // El combobox 'cbxTipoIngresos' es escondido
                                                                                                           // TipoEgresoSeleccionado = ColeccionTipoEgresoMovimientos[0];   // Restablesco el valor default del tipo de egreso seleccionado en el combobox 'cbxTipoEgresos'
                ShowTipoEgresoCombobox = true;                                                           // Muestro el combobox 'cbxTipoEgresos'
                ComentarioText = "";                                                              // Restablesco el valor default de la propiedad Text del TextBox 'txtComentario'
                Monto_Text = "";                                                              // Restablesco el valor default de la propiedad Text del TextBox 'txtMonto'
                NombreCajera = "";
                IsEnableCajera = false;
            }

            TxtUsuarioExternoOrdinalCodigoSeguridad_Text = "N/A";
            ShowSeccionPin = false;  // ..... ..... ..... Oculto la seccion para introducir una credencial

            if (
                       (OpcionActualTipoFlujo.Equals("0") && TipoIngresoSeleccionado.LogicaKey == (int)TipoFlujoSubCategorias.DepositoDineroCaja)
                 || (OpcionActualTipoFlujo.Equals("1") && TipoEgresoSeleccionado.LogicaKey == (int)TipoFlujoSubCategorias.RetiroDineroCaja)
             )
            {
                ShowExternalUserSecurityPanel = true;  //... .... .... Muestro el Panel de Seguridad que contiene el numero de Documento y la credencial
                IsSeccionExternoEnable = true;         // @@ Propiedad para habilitar la (Seccion Externo Enable)
            }
            else
            {
                ShowExternalUserSecurityPanel = true;  // @@ Tenia false
                IsSeccionExternoEnable = false;         // @@ Propiedad para habilitar la (Seccion Externo Enable)
            }


            this._usuario_externo = null;
            this._usuario_externo_required_token = null;
            this.ColeccionDeErrores.Clear();

        }//Fin Restablecer( )

        private void onBtnAplicar_Clicked(object obj)
        {
            if (obj != null)
            {
                GroupedPasswordBox g = (GroupedPasswordBox)obj;

                PasswordBox p = g.SecurePin;

                PasswordBox cedula = g.SecureCedula;

                this.ColeccionDeErrores.Clear();

                //------------ Validaciones ------------//

                if (NombreCajera == "" && IsEnableCajera == true)
                {
                    this.ColeccionDeErrores.Add(new ErrorMetadata() { MensajeError = " * Error: el campo (Nombre de cajera) es requerido" });
                    return;
                }

                decimal montoTransaccion;
                bool MontoFueParseado = Decimal.TryParse(Monto_Text, NumberStyles.Any, CultureInfo.InvariantCulture, out montoTransaccion);

                if (!MontoFueParseado)
                {
                    this.ColeccionDeErrores.Add(new ErrorMetadata() { MensajeError = " * Error: ( Monto ) inválido" });
                    return;
                }

                //------------ Termina Validaciones ------------//

                if (IsSeccionExternoEnable == false)
                {
                    onBtnProcesarClick(obj);
                }
                else
                {
                    if ((cedula.Password != null && (!(Regex.Replace(cedula.Password, @"\s+", "").Equals(string.Empty)))) == false)
                    {
                        this.ColeccionDeErrores.Add(new ErrorMetadata() { MensajeError = " * Error: Debe especificar el Pin de seguridad" });
                        return;
                    }

                    if (
                           cedula.Password != null && (!(Regex.Replace(cedula.Password, @"\s+", "").Equals(string.Empty)))
                        )
                    {

                        this._usuario_externo = UsuarioLogic.BuscarUsuarioPorDocumento(cedula.Password); //@@ Pendiente permitir seleccionar el Tipo de Documento, ahora esta estatico

                        if (this._usuario_externo != null)
                        {

                            UsuarioTarjetaClave tarjeta = UsuarioLogic.GetUsuarioTarjeta(this._usuario_externo.UsuarioID);

                            if (tarjeta  == null)
                            {
                                p.Clear();
                                HideExternalUserSecurityTokenSection(g);
                                this.ColeccionDeErrores.Add(new ErrorMetadata() { MensajeError = " * Error: usuario tarjeta no asignada" });
                            }
                            else
                            {
                                this._usuario_externo_required_token = UsuarioLogic.GetTarjetaTokenAleatorio(tarjeta);  //UsuarioTokenSeguridadLogic.ConsultaUnSoloTokenDeSeguridadAleatorio(this._usuario_externo.UsuarioID);

                                if (this._usuario_externo_required_token == null)
                                {
                                    this.ColeccionDeErrores.Add(new ErrorMetadata() { MensajeError = " * Error: Codigo de seguridad no generado" });

                                    return;
                                }

                                p.Clear();                                                                                                                                // Limpia PasswordBox
                                TxtUsuarioExternoOrdinalCodigoSeguridad_Text = this._usuario_externo_required_token.Posicion + "";
                                ShowSeccionPin = true;                                                                                                                    // Muestro la seccion de introudccion de credenciales
                            }

                        }
                        else
                        {
                            p.Clear();
                            HideExternalUserSecurityTokenSection(g);
                            this.ColeccionDeErrores.Add(new ErrorMetadata() { MensajeError = " * Error: Pin inválido" });
                            //MessageBox.Show("No, Documento inválido", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

                        }
                    }
                    else
                    {
                        p.Clear();
                        HideExternalUserSecurityTokenSection(g);
                        this.ColeccionDeErrores.Add(new ErrorMetadata() { MensajeError = " * Error: Debe especificar el Pin" });
                        //MessageBox.Show("Debe especificar el No. de Documento", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }

                }


            }
        }


        private void onBtnCancelarSecuritySecuritySelecction_Clicked(object obj)
        {
            GroupedPasswordBox g = (GroupedPasswordBox)obj;
            HideExternalUserSecurityTokenSection(g);
        }

        private void onConceptoCombobox_Trigger_Changed(object obj)
        {
            GroupedPasswordBox g = (GroupedPasswordBox)obj;

            PasswordBox p = g.SecurePin;

            if (OpcionActualTipoFlujo.Equals("0")) //:Ingreso
            {
                if (
                        (TipoIngresoSeleccionado.LogicaKey == null || TipoIngresoSeleccionado.LogicaKey != (int)TipoFlujoSubCategorias.DepositoDineroCaja)
                    )
                {
                    p.Clear();
                    HideExternalUserSecurityTokenSection(g);
                    ShowExternalUserSecurityPanel = true; // @@ Tenia false
                    IsSeccionExternoEnable = false;
                }
            }
            else if (OpcionActualTipoFlujo.Equals("1"))//:Egreso
            {
                if (
                      (TipoEgresoSeleccionado.LogicaKey == null || TipoEgresoSeleccionado.LogicaKey != (int)TipoFlujoSubCategorias.RetiroDineroCaja)
                    )
                {
                    p.Clear();
                    HideExternalUserSecurityTokenSection(g);
                    ShowExternalUserSecurityPanel = true; // @@ Tenia false
                    IsSeccionExternoEnable = false;
                }
            }

        }

        private void HideExternalUserSecurityTokenSection(GroupedPasswordBox g)
        {
            ShowSeccionPin = false; // Oculto la seccion de credenciales
            TxtUsuarioExternoOrdinalCodigoSeguridad_Text = "N/A";
            if (g != null) g.SecureCedula.Clear();
            this._usuario_externo = null;
            this._usuario_externo_required_token = null;
        }

        /*** Genrando Id Aleatorio 16Digits*/
        private static string GenerateId(string pBancaID, string pAccion)
        {
            long i = 1;
            foreach (byte b in Guid.NewGuid().ToByteArray())
            {
                i *= ((int)b + 1);
            }
            return pBancaID + "-" + pAccion + string.Format("{0:x}", i - DateTime.Now.Ticks).ToUpper();
        }



    }

    public partial class ProcesarMovimientoViewModel
    {
        public ProcesarMovimientoViewModel(Window parent, int pBanca, int pUsuarioID, Action<string> accionMainTitle, Action<string> accionMainBancaBalance, string pUsuarioCaja = "")
        {

            this.Parent = parent;
            this.UsuarioCaja = pUsuarioCaja;
            Banca = pBanca;
            this.NombreCajera = "";

            this.ColeccionDeErrores = new ObservableCollection<ErrorMetadata>();

            UsuarioID = pUsuarioID;


            this.BancaBalanceActual = CajaLogic.GetBancaBalance(pBanca);

            Set_Windows_MainLabelTitle = accionMainTitle;
            Set_Windows_MainLabelBancaBalanceActual = accionMainBancaBalance;
            Set_Windows_MainLabelTitle?.Invoke("Registro Movimientos");    // Seteando el titulo principal de la pantalla principal


            //string strBancaBalance = CajaLogic.GetBancaBalanceActual(pBanca); 
            Set_Windows_MainLabelBancaBalanceActual?.Invoke("|Balance : " + this.BancaBalanceActual.ToString("#,##0.00", CultureInfo.CreateSpecificCulture("en-US"))); // Seteando el balance de la Banca desde la pantalla principal



            InicializarControlUsuario(Banca);

        }// Fin Constructor
    }

    public partial class ProcesarMovimientoViewModel
    {
        public ObservableCollection<TipoIngresoResponseModel> ColeccionTipoIngresoMovimientos { get; set; }
        public ObservableCollection<TipoEgresoResponseModel> ColeccionTipoEgresoMovimientos { get; set; }
        public ObservableCollection<ErrorMetadata> ColeccionDeErrores { get; set; }
        public TipoIngresoResponseModel TipoIngresoSeleccionado
        {
            get
            {
                return tIngresoSeleccionado;
            }
            set
            {
                tIngresoSeleccionado = value;
                OnPropertyChanged("TipoIngresoSeleccionado");

                if (
                           (OpcionActualTipoFlujo.Equals("0") && TipoIngresoSeleccionado.LogicaKey == (int)TipoFlujoSubCategorias.DepositoDineroCaja)
                 )
                {
                    ShowExternalUserSecurityPanel = true;  // 0 = Egreso y 1 = Ingreso    || Este panel de seguriadad solo se muestra caundo se va depositar o retirar dinero de una banca
                    IsSeccionExternoEnable = true;
                    IsEnableCajera = true;
                }
                else
                {
                    ShowExternalUserSecurityPanel = true; // @@ Tenia false
                    IsSeccionExternoEnable = false;
                    IsEnableCajera = true;
                }

            }
        }
        public TipoEgresoResponseModel TipoEgresoSeleccionado
        {
            get
            {
                return tEgresoSeleccionado;
            }
            set
            {
                tEgresoSeleccionado = value; OnPropertyChanged("TipoEgresoSeleccionado");

                if (
                           (OpcionActualTipoFlujo.Equals("1") && TipoEgresoSeleccionado.LogicaKey == (int)TipoFlujoSubCategorias.RetiroDineroCaja)
                 )
                {
                    ShowExternalUserSecurityPanel = true;  // 0 = Egreso y 1 = Ingreso    || Este panel de seguriadad solo se muestra caundo se va depositar o retirar dinero de una banca

                    IsSeccionExternoEnable = true;
                    IsEnableCajera = false;
                }
                else
                {
                    ShowExternalUserSecurityPanel = true; // @@ Tenia false

                    IsSeccionExternoEnable = false;
                    IsEnableCajera = false;
                }

            }
        }


        public string OpcionActualTipoFlujo
        {
            get
            {
                return opcionActualTipoFlujo;
            }
            set
            {
                if (value != null)
                {
                    if (value.Equals("0"))
                    {
                        Label_Status_Content = "... Recibiendo dinero.";


                        //Habilitando seccion ||Entrada de dinero
                        //HabilitaSeccionIngreseNombreCajera = true;
                        //NombreCajera = "";

                    }
                    else
                    {
                        Label_Status_Content = "... Entregando dinero.";

                        //Desabilitando seccion ||Salida de dinero

                        //HabilitaSeccionIngreseNombreCajera = false;
                        //NombreCajera = "";
                    }

                    opcionActualTipoFlujo = value; OnPropertyChanged("OpcionActualTipoFlujo");
                }
                else
                {
                    Label_Status_Content = "";
                }
            }
        }

        public bool ShowTipoIngresoCombobox
        {
            get
            {
                return showTipoIngresoCombobox;

            }
            set
            {
                showTipoIngresoCombobox = value; OnPropertyChanged("ShowTipoIngresoCombobox");
            }
        }

        public bool ShowTipoEgresoCombobox
        {
            get
            {
                return showTipoEgresoCombobox;
            }
            set
            {
                showTipoEgresoCombobox = value; OnPropertyChanged("ShowTipoEgresoCombobox");
            }
        }

        public bool ShowExternalUserSecurityPanel
        {
            get
            {
                return showExternalUserSecurityPanel;
            }
            set
            {
                showExternalUserSecurityPanel = value; OnPropertyChanged("ShowExternalUserSecurityPanel");
            }
        }

        public bool IsSeccionExternoEnable
        {
            get
            {
                return isSeccionExternoEnable;
            }
            set
            {
                isSeccionExternoEnable = value; OnPropertyChanged("IsSeccionExternoEnable");
            }
        }

        public bool IsEnableCajera
        {
            get
            {
                return isEnableCajera;
            }
            set
            {
                isEnableCajera = value; OnPropertyChanged("IsEnableCajera");
            }
        }

        public string ComentarioText
        {
            get
            {
                return comentario_text ?? "";
            }
            set
            {
                if (value != null)
                {
                    comentario_text = value; OnPropertyChanged("ComentarioText");
                }
            }
        }

        public string Monto_Text
        {
            get
            {
                return monto_text ?? "";
            }
            set
            {
                if (value != null)
                {
                    monto_text = value; OnPropertyChanged("Monto_Text");
                }
            }
        }


        public bool ShowSeccionPin
        {
            get
            {
                return showSeccionPin;
            }
            set
            {
                showSeccionPin = value; OnPropertyChanged("ShowSeccionPin");
            }
        }

        public string NombreCajera
        {
            get
            {
                return _nombreCajera;
            }
            set
            {
                _nombreCajera = value; OnPropertyChanged("NombreCajera");
            }
        }


        public string TxtUsuarioExternoOrdinalCodigoSeguridad_Text
        {
            get
            {
                return txtUsuarioExternoOrdinalCodigoSeguridad_Text;
            }

            set
            {
                if (value != null)
                {
                    txtUsuarioExternoOrdinalCodigoSeguridad_Text = value; OnPropertyChanged("TxtUsuarioExternoOrdinalCodigoSeguridad_Text");
                }
            }
        }

        public string Label_Status_Content
        {
            get
            {
                return lb_Status_Content;
            }
            set
            {
                if (value != null)
                {
                    lb_Status_Content = value; OnPropertyChanged("Label_Status_Content");
                }
            }
        }

        private string _nombreCajera;

        public ICommand RadGroupTipoFlujoCommand { get; set; }
        public ICommand Button_Procesar_Command { get; set; }
        public ICommand Button_Restablecer_Command { get; set; }
        public ICommand Button_Aplicar_Command { get; set; }
        public ICommand Button_Cancelar_Security_Selection_Command { get; set; }
        public ICommand Concepto_Combobox_Changed_Trigger_Command { get; set; }
        public ICommand Button_CerrarModal_Command { get; set; }

    }

    public partial class ProcesarMovimientoViewModel
    {

        private Window Parent;

        private UsuarioResponseModel _usuario_externo;
        private TokenDeSeguridadResponseModel _usuario_externo_required_token;

        private TipoIngresoResponseModel tIngresoSeleccionado;
        private TipoEgresoResponseModel tEgresoSeleccionado;


        private List<TipoIngresoResponseModel> TheListOfTiposIngresos;
        private List<TipoEgresoResponseModel> TheLitsOfTiposEgresos;





        private bool showTipoIngresoCombobox;
        private bool showTipoEgresoCombobox;
        private bool showExternalUserSecurityPanel;
        private bool isSeccionExternoEnable;
        private bool isEnableCajera;
        //private bool habilitaSeccionIngreseNombreCajera;
        private bool showSeccionPin;


        private string opcionActualTipoFlujo;
        private string comentario_text;
        private string monto_text;

        private string CONST_OPCION_TIPO_FLUJO;

        private string lb_Status_Content;

        private string txtUsuarioExternoOrdinalCodigoSeguridad_Text;
        private string UsuarioCaja;





        private readonly int Banca;
        private readonly int UsuarioID;

        private event Action<string> Set_Windows_MainLabelTitle;
        private event Action<string> Set_Windows_MainLabelBancaBalanceActual;


        private decimal BancaBalanceActual;


    }
}