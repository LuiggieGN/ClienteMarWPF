
using ClienteMarWPFWin7.Domain.Exceptions;

using ClienteMarWPFWin7.Domain.Services.BancaService;
//using ClienteMarWPFWin7.UI.Modules.Modulo;

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ClienteMarWPFWin7.UI.ViewModels.Commands.Modulo
{

    // USO DE COMANDOS ASINCRONOS

    public class BancaBalanceConsultaCommand : ActionCommandAsync
    {
        //private readonly ModuloViewModel _moduloViewModel;
        //private readonly IBancaService   _bancaService;
        //private readonly FlujoService.MAR_Session _sesion;


        //public BancaBalanceConsultaCommand(ModuloViewModel moduloViewModel, IBancaService bancaService, FlujoService.MAR_Session sesion)
        //{
        //    _moduloViewModel = moduloViewModel;
        //    _bancaService = bancaService;
        //    _sesion = sesion;
        //}


        //public override async Task ExecuteAsync(object parameter)
        //{

        //    try
        //    {
        //        _moduloViewModel.BancaMensaje = $"Balance de Bancas Id = {_moduloViewModel.BancaId}";

        //        decimal balance = 0;//await _bancaService.GetBalance(_moduloViewModel.BancaId,_sesion);
        //        _moduloViewModel.BalanceConsultado = "Su balance es = "+ balance.ToString("#.00");



        //    }
        //    catch 
        //    {
        //        MessageBox.Show("Ha ocurrido un error consultado el balance");
        //    }

        //}
        public override Task ExecuteAsync(object parameter)
        {
            throw new NotImplementedException();
        }
    }// fin de Clase LoginCommand

}// fin de namespace 
