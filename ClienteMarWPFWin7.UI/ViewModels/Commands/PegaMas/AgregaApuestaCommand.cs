
#region Namespace
using ClienteMarWPFWin7.UI.ViewModels.ModelObservable;
using ClienteMarWPFWin7.UI.ViewModels.Helpers;
using ClienteMarWPFWin7.UI.Modules.PegaMas;
using System;
#endregion

namespace ClienteMarWPFWin7.UI.ViewModels.Commands.PegaMas
{
    public class AgregaApuestaCommand : ActionCommand
    {
        private readonly PegaMasViewModel vm;

        public AgregaApuestaCommand(PegaMasViewModel viewmodel) : base()
        {
            vm = viewmodel; base.SetAction(new Action<object>(AgregarApuesta));
        }

        private void AgregarApuesta(object parametro)
        {
            try
            {
                var entradas = LeerEntradas();

                var nuevaApuesta = PegaMasApuestaObservable
                                    .NuevaApuesta(
                                        d1: entradas[0],
                                        d2: entradas[1],
                                        d3: entradas[2],
                                        d4: entradas[3],
                                        d5: entradas[4]
                                    );

                vm.Jugadas.Add(nuevaApuesta);

                ResetEntradas();

                vm.FocusEnPrimerInput?.Invoke();
            }
            catch
            {
            }
        }

        private int[] LeerEntradas()
        {
            int aux;
            var digitos = new int[5];
            var digitosEnString = new string[5] { vm.D1, vm.D2, vm.D3, vm.D4, vm.D5 };

            for (int i = 0; i < digitosEnString.Length; i++)
            {
                string da = digitosEnString[i];

                if (EstaVacio(da))
                {
                    digitos[i] = 0;
                }
                else
                {
                    if (int.TryParse(da, out aux))
                    {
                        digitos[i] = aux;
                    }
                    else
                    {
                        digitos[i] = 0;
                    }
                }
            }
            return digitos;
        }

        private bool EstaVacio(string input)
        {
            return InputHelper.InputIsBlank(input);
        }

        private void ResetEntradas()
        {
            vm.D1 = string.Empty;
            vm.D2 = string.Empty;
            vm.D3 = string.Empty;
            vm.D4 = string.Empty;
            vm.D5 = string.Empty;
        }


    }// Clase
}
