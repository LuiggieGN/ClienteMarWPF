using ClienteMarWPF.Domain.Services.RecargaService;
using ClienteMarWPF.UI.Modules.Recargas;
using ClienteMarWPF.UI.State.Authenticators;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClienteMarWPF.UI.ViewModels.Commands.Recargas
{
    public class GetSuplidoresCommand: ActionCommand
    {
        private readonly RecargasViewModel ViewModel;
        private readonly IAuthenticator Autenticador;
        private readonly IRecargaService RecargaService;


        public GetSuplidoresCommand(RecargasViewModel viewModel, IAuthenticator autenticador, IRecargaService recargaService) : base()
        {
            ViewModel = viewModel;
            Autenticador = autenticador;
            RecargaService = recargaService;

            Action<object> comando = new Action<object>(GetSuplidores);
            base.SetAction(comando);
        }


        private void GetSuplidores(object parametro)
        {
            try
            {
                var suplidores = RecargaService.GetSuplidor(Autenticador.CurrentAccount.MAR_Setting2.Sesion);
                foreach (var item in suplidores)
                {
                    if(item != null)
                    {

                        ViewModel.ProveedorRecargasObservable.Add(new ModelObservable.ProveedorRecargasObservable
                        {
                            OperadorID = item.SuplidorID,
                            Operador = item.SupNombre,
                            IsSelected = false,
                            Pais = "",
                            Url = "pack://application:,,,/ClienteMarWPF.UI;component/StartUp/Images/Operador/Claro_logo.png"

                        });
                    }
                    
                }
               
             
            }
            catch (Exception ex)
            {

            }

        }
    }
}
