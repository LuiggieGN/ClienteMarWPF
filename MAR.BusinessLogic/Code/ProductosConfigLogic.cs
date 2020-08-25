using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAR.BusinessLogic.Code
{
    public class ProductosConfigLogic
    {

       
        public static void SetLimiteMinutosReImprision(int pLimiteMinutos)
        {
            try
            {
                DataAccess.EFRepositories.ProductosConfigRepository.SetLimiteMinutosReImpresion(pLimiteMinutos);
            }
            catch (Exception e)
            {
                //ignore
            }
        }
        public static int GetLimiteMinutosReImprision()
        {
            try
            {
               return DataAccess.EFRepositories.ProductosConfigRepository.GetLimiteMinutosReImpresion();
            }
            catch (Exception e)
            {
                return 5;
            }
        }
        public static bool GetRecargasEncendidas()
        {
            try
            {
               return DataAccess.EFRepositories.ProductosConfigRepository.GetRecargasEncendidas();
            }
            catch (Exception e)
            {
                return e.ToString().Length>0;
                //return true;
            }
        }

        public static void SetRecargasEncendidas(bool Encendido)
        {
            try
            {
                DataAccess.EFRepositories.ProductosConfigRepository.SetRecargasEncendidas(Encendido);
            }
            catch (Exception e)
            {
                // Pending log exception to DLog table
            }
        }

       public static string GetLoteriaCierreRecargas()
        {
            try
            {
               return DataAccess.EFRepositories.ProductosConfigRepository.GetLoteriaCierreRecargas();
            }
            catch (Exception e)
            {
                return "2";
            }
        }

        public static void SetLoteriaCierreRecargas(string LoteriaID)
        {
            try
            {
                DataAccess.EFRepositories.ProductosConfigRepository.SetLoteriaCierreRecargas(LoteriaID);
            }
            catch (Exception e)
            {
                // Pending log exception to DLog table
            }
        }

        public static bool PuedeReimprimirTicketLoteria(DateTime pTicFecha)
        {
            try
            {
                DateTime fechaLimite = pTicFecha.AddMinutes(DataAccess.EFRepositories.ProductosConfigRepository.GetLimiteMinutosReImpresion());
                if (fechaLimite < DateTime.Now)
                {
                    return false;
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public static bool RecargaEstaEnHorario()
        {
            try
            {
                var enHorario = DataAccess.EFRepositories.ProductosConfigRepository.RecargaEstaEnHorarioDeVentas();
                return enHorario;
            }
            catch (Exception)
            {
                return true;
            }

        }
        public static bool PuedePagarRemoto(int pBancaId, int pBancaDelTicket)
        {
            try
            {
                var pagoRemoto = DataAccess.EFRepositories.ProductosConfigRepository.PuedePagarRemoto(pBancaId, pBancaDelTicket);
                return pagoRemoto;
            }
            catch (Exception)
            {
                return true;
            }
        }

    }
}
