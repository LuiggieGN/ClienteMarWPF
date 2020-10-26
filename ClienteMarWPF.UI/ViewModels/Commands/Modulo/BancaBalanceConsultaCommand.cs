
using ClienteMarWPF.Domain.Exceptions;

using ClienteMarWPF.Domain.Services.BancaService;
using ClienteMarWPF.UI.Modules.Modulo;

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ClienteMarWPF.UI.ViewModels.Commands.Modulo
{

    // USO DE COMANDOS ASINCRONOS

    public class BancaBalanceConsultaCommand : AsyncCommandBase
    {
        private readonly ModuloViewModel _moduloViewModel;
        private readonly IBancaService   _bancaService;
        private readonly FlujoServices.MAR_Session _sesion;
        

        public BancaBalanceConsultaCommand(ModuloViewModel moduloViewModel, IBancaService bancaService, FlujoServices.MAR_Session sesion)
        {
            _moduloViewModel = moduloViewModel;
            _bancaService = bancaService;
            _sesion = sesion;
        }
        

        public override async Task ExecuteAsync(object parameter)
        {
            
            try
            {
                _moduloViewModel.BancaMensaje = $"Balance de Bancas Id = {_moduloViewModel.BancaId}";

                decimal balance = await _bancaService.GetBalance(_moduloViewModel.BancaId,_sesion);
                _moduloViewModel.BalanceConsultado = "Su balance es = "+ balance.ToString("#.00");



            }
            catch 
            {
                MessageBox.Show("Ha ocurrido un error consultado el balance");
            }
 
        }


          



    }// fin de Clase LoginCommand

}// fin de namespace 
