﻿
using System;

using ClienteMarWPFWin7.UI.Modules.FlujoEfectivo.Inicio.Modal;

using ClienteMarWPFWin7.Domain.Enums;
using ClienteMarWPFWin7.Domain.Helpers;
using ClienteMarWPFWin7.Domain.Models.Dtos;
using ClienteMarWPFWin7.Domain.Models.Dtos.EfectivoDtos;
using ClienteMarWPFWin7.Domain.Services.CuadreService;

using ClienteMarWPFWin7.UI.State.Authenticators;
using ClienteMarWPFWin7.UI.State.Navigators;
using ClienteMarWPFWin7.UI.ViewModels.Factories;


namespace ClienteMarWPFWin7.UI.ViewModels.Commands.FlujoEfectivo.Inicio
{

    public class IniciarControlEfectivoCommand : ActionCommand
    {
        private readonly DialogInicioViewModel _viewmodel;
        private readonly INavigator _nav;
        private readonly IAuthenticator _aut;
        private readonly IViewModelFactory _vis;
        private readonly ICuadreService _cuadreService;
        private bool _puedeIniciar;

        public IniciarControlEfectivoCommand(DialogInicioViewModel viewmodel, INavigator nav, IAuthenticator aut, IViewModelFactory vistas, ICuadreService cuadreService) : base()
        {
            _nav = nav;
            _aut = aut;
            _vis = vistas;
            _cuadreService = cuadreService;
            _viewmodel = viewmodel;
            _puedeIniciar = true;
            SetAction(new Action<object>(IniciarControlEfectivo), 
                      new Predicate<object>(PuedeIniciarControlDeEfectivo));
        }

        public void IniciarControlEfectivo(object parametro)
        {
            if (_nav != null && _aut != null && _aut.BancaConfiguracion != null  && _vis != null && _cuadreService != null && _viewmodel != null)
            {
                _viewmodel.MensajeError = null;

                _puedeIniciar = false;

                var inicio = new CuadreRegistroDTO();
                inicio.Cuadre = new CuadreDTO();
                inicio.Cuadre.Tipo = CuadreHelper.Get(CuadreTipo.Inicial);
                inicio.Cuadre.BalanceMinimo = _aut.BancaConfiguracion.CajaEfectivoDto.BalanceMinimo;
                inicio.Cuadre.CajaID = _aut.BancaConfiguracion.CajaEfectivoDto.CajaID;
                inicio.Cuadre.UsuarioCaja = string.Empty;
                inicio.Cuadre.CajaOrigenID = 0;
                inicio.Cuadre.UsuarioOrigenID = 0;
                inicio.Cuadre.AuxMensajeroNombre = string.Empty;
                inicio.Cuadre.MontoContado = _viewmodel.BancaBalance;
                inicio.Cuadre.MontoDepositado = 0;
                inicio.Cuadre.MontoRetirado = 0;
                inicio.Cuadre.MontoPorPagar = 0;
                inicio.CuadreGestorAccion = CuadreGestorAccion.Depositar;
                inicio.RutaAsignacion = null;

                string toPrint;
                var inicioResult = _cuadreService.Registrar(_aut.BancaConfiguracion.BancaDto, inicio, enablePrinting: false, out toPrint);

                if (inicioResult.FueProcesado)
                {//::Control de Efectivo fue habilitado


                    _viewmodel.Ocultar();

                    var config = _aut.BancaConfiguracion;

                    config.ControlEfectivoConfigDto.BancaYaInicioControlEfectivo = true; //Activo Modulos de Control de Efectivo

                    _aut.BancaConfiguracion = config;
 
                    _nav.CurrentViewModel = _vis.CreateViewModel(Modulos.RegistrosDeMovimiento); //Redirecciona al Modulo de Registro de Movimiento

                    _aut.RefrescarBancaBalance(); //@@Actualizo el Balance de Banca

                }
                else
                {//::No fue habilitado
                    _viewmodel.MensajeError = "Ha ocurrido un error en procesar operaciòn.";
                }

                _puedeIniciar = true;
            }
        }


        private bool PuedeIniciarControlDeEfectivo(object state)
        {
            return _puedeIniciar;
        }




    }
}
