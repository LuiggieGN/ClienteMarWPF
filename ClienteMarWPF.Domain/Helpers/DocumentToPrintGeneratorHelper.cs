 
using ClienteMarWPF.Domain.Enums;

namespace ClienteMarWPF.Domain.Helpers
{

    public static class DocumentToPrintGeneratorHelper
    {

        public static string GetMovimientoDocument(MovimientoToPrintHelper printDoc)
        {
            string template = "";

            if (printDoc.EsUnDeposito)
            {

                template = $@"

  --------------------------------------------
  DEPOSITO  DE FONDOS A BANCA
  --------------------------------------------
  {printDoc.BanContacto}
  {printDoc.BanDireccion}
  Transaccion: {printDoc.BanTransaccion}
  Fecha: {printDoc.FechaTransaccion}

  Monto Depositado: {printDoc.BanMonto} 
  Recibido por: 
  {printDoc.Recibido__Por}

  Firma:  ________________________

  Cedula: ________________________

" + "\n\n\n\n\n\n\n\n";


            } // Fin IF
            else
            {
                template = $@"

  --------------------------------------------
  RETIRO DE FONDOS DE BANCA
  --------------------------------------------
  {printDoc.BanContacto}
  {printDoc.BanDireccion}
  Transaccion: {printDoc.BanTransaccion}
  Fecha: {printDoc.FechaTransaccion}
  Monto Retirado: {printDoc.BanMonto} 
  Recibido por: 
  {printDoc.Recibido__Por}

  Firma:  ________________________

  Cedula: ________________________

" + "\n\n\n\n\n\n\n\n";

            } // Fin Else


            return template;


        }//fin de metodo GetMovimientoDocument()

        public static string GetCuadreDocument(CuadreToPrintHelper printDoc)
        {
            string template = "";

            if (printDoc.EsUnDeposito)
            { //Inicio IF


                //DEPOSITO//
                template = $@"

  --------------------------------------------
  DEPOSITO - CUADRE DE BANCA
  --------------------------------------------
  {printDoc.BanContacto}
  {printDoc.BanDireccion}
  Cuadre: {printDoc.CuadreTransaccion}
  Fecha: {printDoc.FechaTransaccion}
  Balance de Cuenta: {printDoc.BalanceDeCajaSegunSistema}
  Efectivo en Caja: {printDoc.EfectivoContadoEnCaja}
";

                if (printDoc.FaltanteSobrante == ArqueoDeCajaResultado.Faltante)
                {
                    //Con Faltante
                    if (printDoc.AUX_MontoFaltanteOMontoSobrante > 0)
                    {
                        template += $@"  Faltante en caja: {printDoc.MontoFaltanteOMontoSobrante}
  Responsable de Faltante:
  {printDoc.Responsable}

  Firma:  ________________________
  
  Cedula: ________________________

";
                    }

                }
                else
                {
                    //Con Sobrante
                    if (printDoc.AUX_MontoFaltanteOMontoSobrante > 0)
                    {
                        template += $@"  Sobrante en caja: {printDoc.MontoFaltanteOMontoSobrante}
  Responsable de Sobrante:
  {printDoc.Responsable}

  Firma:  ________________________
  
  Cedula: ________________________

";
                    }
                }



                template += $@"
  Monto Depositado: {printDoc.BanMontoRetiradoODepositado}
  Balance Final Caja: {printDoc.BalanceFinalCaja}
  Recibido por:
  {printDoc.RecibidoPor}

  Firma:  ________________________
  
  Cedula: ________________________

" + "\n\n\n\n\n\n\n\n";
            } // Fin IF
            else
            { // Inicio Else

                //RETIRO//

                template = $@"

  --------------------------------------------
  RETIRO - CUADRE DE BANCA
  --------------------------------------------
  {printDoc.BanContacto}
  {printDoc.BanDireccion}
  Cuadre: {printDoc.CuadreTransaccion}
  Fecha: {printDoc.FechaTransaccion}
  Balance de Cuenta: {printDoc.BalanceDeCajaSegunSistema}
  Efectivo en Caja: {printDoc.EfectivoContadoEnCaja}
";

                if (printDoc.FaltanteSobrante == ArqueoDeCajaResultado.Faltante)
                {
                    //Con Faltante
                    if (printDoc.AUX_MontoFaltanteOMontoSobrante > 0)
                    {
                        template += $@"  Faltante en caja: {printDoc.MontoFaltanteOMontoSobrante}
  Responsable de Faltante:
  {printDoc.Responsable}
  
  Firma:  ________________________
  
  Cedula: ________________________
 
";
                    }

                }
                else
                {
                    //Con Sobrante
                    if (printDoc.AUX_MontoFaltanteOMontoSobrante > 0)
                    {
                        template += $@"  Sobrante en caja: {printDoc.MontoFaltanteOMontoSobrante}
  Responsable de Sobrante:
  {printDoc.Responsable}

  Firma:  ________________________

  Cedula: ________________________

";
                    }
                }



                template += $@"
  Monto Retirado: {printDoc.BanMontoRetiradoODepositado}
  Balance Final Caja: {printDoc.BalanceFinalCaja}
  Recibido por:
  {printDoc.RecibidoPor}
  
  Firma:  ________________________
  
  Cedula: ________________________
 
" + "\n\n\n\n\n\n\n\n";
            }//Fin Else



            return template;

        }// fin metodo GetCuadreDocument()



    }// fin de clase 

}// fin de namespace
