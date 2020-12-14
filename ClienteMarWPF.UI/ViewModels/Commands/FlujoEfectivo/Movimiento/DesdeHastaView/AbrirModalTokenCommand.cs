
#region Namespaces
using System;
using ClienteMarWPF.Domain.Services.CajaService;
using ClienteMarWPF.UI.Modules.FlujoEfectivo.Movimiento.Modal;
using ClienteMarWPF.UI.Modules.FlujoEfectivo.Movimiento.Views.View2;
#endregion


namespace ClienteMarWPF.UI.ViewModels.Commands.FlujoEfectivo.Movimiento.DesdeHastaView
{

    public class AbrirModalTokenCommand : ActionCommand
    {
        private readonly DesdeHastaViewModel _viewmodel;
        private readonly ICajaService _cajaService;

        public AbrirModalTokenCommand(DesdeHastaViewModel viewmodel, ICajaService cajaService) : base()
        {
            SetAction(new Action<object>(AbrirModalTokenConfirmacion));

            _viewmodel = viewmodel;
            _cajaService = cajaService;
        }

        public void AbrirModalTokenConfirmacion(object box)
        {
            if (_viewmodel != null &&
                _viewmodel.Gestor != null &&
                _viewmodel.Gestor.TercerDTO != null &&
                _viewmodel.Gestor.TercerDTO.InlineTokens != null &&
                _viewmodel.Gestor.TercerDTO.InlineTokens.Count > 0
                )
            {
                try
                {
                    ResetErrors();

                    Random random = new Random();

                    int totalDeTokens = _viewmodel.Gestor.TercerDTO.InlineTokens.Count;

                    int indiceToken = random.Next(totalDeTokens);

                    int tokenPosicion = indiceToken + 1;

                    string secretToken = _viewmodel.Gestor.TercerDTO.InlineTokens[indiceToken];

                    _viewmodel.MovimientoVm.Dialog = new DialogoTokenViewModel(
                        $"{tokenPosicion}",
                        secretToken,
                        new ActionCommand((object p) => _viewmodel.MovimientoVm.Dialog.Ocultar()),
                        new RegistrarTransferenciaCommand(_viewmodel, _cajaService)
                    );

                    _viewmodel.MovimientoVm.Dialog.Mostrar();
                }
                catch
                {
                    _viewmodel.Toast.ShowError("Ha ocurrido un error al procesar la operación. Verificar conexión de Internet");
                }
            }
        }

        private void ResetErrors()
        {             
            _viewmodel.Errores.EliminarError(nameof(_viewmodel.Comentario));
            _viewmodel.Errores.EliminarError(nameof(_viewmodel.InputCajera));
            _viewmodel.Errores.EliminarError(nameof(_viewmodel.Monto));
        }




    }
}
