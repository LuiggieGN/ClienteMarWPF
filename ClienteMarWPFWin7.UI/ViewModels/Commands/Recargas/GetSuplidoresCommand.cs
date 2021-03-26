using ClienteMarWPFWin7.Domain.Services.RecargaService;
using ClienteMarWPFWin7.UI.Modules.Recargas;
using ClienteMarWPFWin7.UI.State.Authenticators;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace ClienteMarWPFWin7.UI.ViewModels.Commands.Recargas
{
    public class GetSuplidoresCommand : ActionCommand
    {
        enum Operador
        {
            Claro = 1,
            Orange = 2,
            Tricom = 3,
            Viva = 4,
            Digicel = 5,
            NatCom = 6,
            MarltonJuegaMas = 7,
            LoterryVIP = 8
        }


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
                    if (item != null)
                    {

                        if(item.SuplidorID != 7 && item.SuplidorID != 8)
                        {
                            ViewModel.ProveedorRecargasObservable.Add(new ModelObservable.ProveedorRecargasObservable
                            {


                                OperadorID = item.SuplidorID,
                                Operador = item.SupNombre,
                                IsSelected = false,
                                Pais = "",
                                Url = AddImageOperator(item.SuplidorID)

                            });
                        }

                     
                    }
               
                }

                ViewModel.ProveedorRecargasObservable.FirstOrDefault().IsSelected = true;
                ViewModel.Provedor = ViewModel.ProveedorRecargasObservable.FirstOrDefault();


                //RecargaService.GetRecarga(Autenticador.CurrentAccount.MAR_Setting2.Sesion,99, "10", 1, "8299617265", 30, 1500);


            }
            catch (Exception ex)
            {

            }

        }

        private string AddImageOperator(int operador)
        {
            string retorno = string.Empty;
            switch (operador)
            {
                case (int)Operador.Claro:
                    retorno =   "pack://application:,,,/ClienteMarWPFWin7.UI;component/StartUp/Images/Operador/Claro_logo.png";
                    break;

                case (int)Operador.Orange:
                    retorno = "pack://application:,,,/ClienteMarWPFWin7.UI;component/StartUp/Images/Operador/AlticeT_logo.png";
                    break;


                case (int)Operador.Tricom:
                    retorno = "pack://application:,,,/ClienteMarWPFWin7.UI;component/StartUp/Images/Operador/Tricom_logo.jpeg";
                    break;


                case (int)Operador.Viva:
                    retorno = "pack://application:,,,/ClienteMarWPFWin7.UI;component/StartUp/Images/Operador/VivaT_logo.png";
                    break;

                case (int)Operador.Digicel:
                    retorno = "pack://application:,,,/ClienteMarWPFWin7.UI;component/StartUp/Images/Operador/Digice_logo.jpg";
                    break;


                case (int)Operador.NatCom:
                    retorno = "pack://application:,,,/ClienteMarWPFWin7.UI;component/StartUp/Images/Operador/NatcomT_logo.png";
                    break;


                //case (int)Operador.MarltonJuegaMas:
                //    retorno = "pack://application:,,,/ClienteMarWPFWin7.UI;component/StartUp/Images/Operador/Claro_logo.png";
                //    break;

                //case (int)Operador.LoterryVIP:
                //    retorno = "pack://application:,,,/ClienteMarWPFWin7.UI;component/StartUp/Images/Operador/Claro_logo.png";
                //    break;

                default:
                    retorno = "default";
                    break;


            }

            return retorno;
        }
    }
}
