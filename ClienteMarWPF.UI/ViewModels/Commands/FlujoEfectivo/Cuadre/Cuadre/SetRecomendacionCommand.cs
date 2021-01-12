
#region Namespaces
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using ClienteMarWPF.Domain.Enums;
using ClienteMarWPF.Domain.Models.Dtos;
using ClienteMarWPF.Domain.Models.Dtos.EfectivoDtos;
using ClienteMarWPF.UI.ViewModels.Helpers;
using ClienteMarWPF.UI.Modules.FlujoEfectivo.Cuadre.Windows.CuadreLogin;
using ClienteMarWPF.UI.Modules.FlujoEfectivo.Cuadre.Windows.Cuadre;
using ClienteMarWPF.UI.Modules.FlujoEfectivo.Cuadre.Windows.Cuadre.ViewModels;
using System.Globalization;
#endregion


namespace ClienteMarWPF.UI.ViewModels.Commands.FlujoEfectivo.Cuadre.Cuadre
{

    public class SetRecomendacionCommand : ActionCommand
    {
        private readonly CuadreViewModel _viewmodel;

        public SetRecomendacionCommand(CuadreViewModel viewmodel) : base()
        {
            SetAction(new Action<object>(SetRecomendacion));
            _viewmodel = viewmodel;
        }

        public void SetRecomendacion(object monto)
        {
            var montoContadoPorGestor = Convert.ToDecimal(monto?.ToString(), new CultureInfo("en-US"));

            if (_viewmodel != null &&
                _viewmodel.ConsultaInicial != null)
            {
                SetArqueoResultante(montoContadoPorGestor);
                SetADepositarORetirarRecomendacion(montoContadoPorGestor);
            }
        }


        public void SetArqueoResultante(decimal montoContadoPorGestor)
        {
            _viewmodel.ArqueoResultante = new ArqueoResultanteViewModel();

            var balance = _viewmodel.ConsultaInicial.BancaBalance;
            if (balance <= 0)
            {
                if (montoContadoPorGestor == 0)
                {
                    _viewmodel.ArqueoResultante.Codigo = 0;
                    _viewmodel.ArqueoResultante.Arqueo = "Monto Faltante";
                    _viewmodel.ArqueoResultante.DineroResultante = 0;
                }
                else if (montoContadoPorGestor > 0)
                {
                    _viewmodel.ArqueoResultante.Codigo = 1;
                    _viewmodel.ArqueoResultante.Arqueo = "Monto a Favor";
                    _viewmodel.ArqueoResultante.DineroResultante = montoContadoPorGestor;
                }
            }
            else
            {
                if ((balance - montoContadoPorGestor) < 0)
                {
                    _viewmodel.ArqueoResultante.Codigo = 1;
                    _viewmodel.ArqueoResultante.Arqueo = "Monto a Favor";
                    _viewmodel.ArqueoResultante.DineroResultante = (-1 * (balance - montoContadoPorGestor));
                }
                else if ((balance - montoContadoPorGestor) >= 0)
                {
                    _viewmodel.ArqueoResultante.Codigo = 0;
                    _viewmodel.ArqueoResultante.Arqueo = "Monto Faltante";
                    _viewmodel.ArqueoResultante.DineroResultante = (balance - montoContadoPorGestor);
                }
            }

            _viewmodel.ArqueoResultante.EstaCargando = Booleano.No;
        }

        public void SetADepositarORetirarRecomendacion(decimal montoContadoPorGestor)
        {
            _viewmodel.Recomendacion = new RecomendacionViewModel();

            decimal deuda = _viewmodel.ConsultaInicial.BancaDeuda;
            decimal bminimo = _viewmodel.ConsultaInicial.BancaBalanceMinimo;
            decimal recomendadoretirar = montoContadoPorGestor - bminimo - deuda;
            decimal recomendadodejar = (montoContadoPorGestor - bminimo) <= 0 ? (Math.Abs((montoContadoPorGestor - bminimo)) + deuda) :
                                                                                (
                                                                                   (deuda == 0) ? 0 : (((montoContadoPorGestor - bminimo) >= deuda) ? 0 : (deuda - (montoContadoPorGestor - bminimo)))
                                                                                );

            if (recomendadoretirar <= 0)
            {/** recomiendo DEPOSITAR */

                _viewmodel.CuadreGestorAccion = CuadreGestorAccion.Depositar;
                _viewmodel.Recomendacion.Codigo = 0;
                _viewmodel.Recomendacion.Tipo = "Depositar";
                _viewmodel.Recomendacion.MontoRecomendado = recomendadodejar;
                _viewmodel.Recomendacion.Icono = "Add";
                _viewmodel.Recomendacion.Descripcion = "Recomendado a depositar";

                _viewmodel.MontoDepositoORetiro = $"{recomendadodejar.ToString("G29")}"; // @@ Seteo Input Recomendacion
            }
            else
            {/** recomiendo RETIRAR */

                _viewmodel.CuadreGestorAccion = CuadreGestorAccion.Retirar;
                _viewmodel.Recomendacion.Codigo = 1;
                _viewmodel.Recomendacion.Tipo = "Retirar";
                _viewmodel.Recomendacion.MontoRecomendado = recomendadoretirar;
                _viewmodel.Recomendacion.Icono = "Minus";
                _viewmodel.Recomendacion.Descripcion = "Recomendado a retirar";

                _viewmodel.MontoDepositoORetiro = $"{recomendadoretirar.ToString("G29")}"; // @@ Seteo Input Recomendacion
            }

            _viewmodel.Recomendacion.EstaCargando = Booleano.No;
        }








    }//fin de clase
}//  fin de namespace




