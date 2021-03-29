
using System;
using ClienteMarWPFWin7.Domain.Services.BancaService;
using ClienteMarWPFWin7.UI.State.Authenticators;
using ClienteMarWPFWin7.UI.Modules.FlujoEfectivo.Inicio.Modal;


namespace ClienteMarWPFWin7.UI.ViewModels.Commands.FlujoEfectivo.Inicio
{    
     
    public class CalcularBancaBalanceCommand : ActionCommand
    {
        private readonly DialogInicioViewModel _viewmodel;
        private readonly IAuthenticator _aut;
        private readonly IBancaService _banService;
        
        public CalcularBancaBalanceCommand(DialogInicioViewModel viewmodel, IAuthenticator aut, IBancaService banService) :base()
        {
            _aut = aut;
            _banService = banService;
            _viewmodel = viewmodel;
            SetAction(new Action<object>(Calcular)); 
        }
               
        public void Calcular( object parametro ) 
        {
            if (_viewmodel != null && _aut != null && _aut.BancaConfiguracion != null && _aut.BancaConfiguracion.BancaDto != null)
            {
                decimal bancaMontoReal = _banService.LeerBancaMontoReal(_aut.BancaConfiguracion.BancaDto.BancaID );

                _viewmodel.BancaBalance = bancaMontoReal;
                _viewmodel.MensajeBalance = null;
                _viewmodel.MensajeError = null;
                _viewmodel.MensajeBalance = $"Usted tiene un balance de $ {bancaMontoReal.ToString("N0")}.";

                if (bancaMontoReal < 0)
                {
                    _viewmodel.MuestroBotonAceptar = false; // No muestro boton aceptar
                    _viewmodel.MensajeError = "** Favor, Iniciar Control de Efectivo desde Central (Nivelar Balance)";
                }
                else
                {
                    _viewmodel.MuestroBotonAceptar = true; // Si muestro boton aceptar
                }
            }
        }




    }
}
