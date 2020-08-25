using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Flujo.Entities.WebClient.POCO;

using Flujo.DataAccess.FlujoRepositories.WebClient.DapperRepositories;
using Flujo.Entities.WebClient.RequestModels;

namespace MAR.BusinessLogic.Flujo.WebClient.Code
{
    public class CajaLogic
    {

        public static decimal GetBalance(int pCajaID)
        {
            try
            {
                decimal balance = CajaRepositorio.GetCajaBalanceActual(pCajaID);
                return balance;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<Caja> GetCajasPorFiltroConsulta(ConsultaCajaRequestModel consulta)
        {
            try
            {
                List<Caja> cajas = CajaRepositorio.GetCajasPorFiltroDesdeHasta(consulta);
                return cajas;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool VerificarDisponibilidadDeCaja(int pCajaID)
        {
            try
            {
                bool disponibildad = CajaRepositorio.VerificarDisponibilidadDeCaja(pCajaID);
                return disponibildad;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public static ConsultaBalanceCaja GetCajaBalance(int pCajaID)
        {
            ConsultaBalanceCaja consultaBalance = new ConsultaBalanceCaja();
            consultaBalance.CajaID = pCajaID;
            consultaBalance.Balance = CajaRepositorio.GetCajaBalanceActual(pCajaID);
            consultaBalance.Estado = VerificarDisponibilidadDeCaja(pCajaID) ? "Disponible" : "No Disponible";
            return consultaBalance;

        }

        public static ConsultaBalanceCaja GetCajaBalance(int pCajaID, int pUsuarioLoggeado)
        {
            ConsultaBalanceCaja consulta = CajaRepositorio.GetCajaBalance(pCajaID, pUsuarioLoggeado);

            consulta.Balance = CajaRepositorio.GetCajaBalanceActual(pCajaID);

            return consulta;
        }

        public static Caja GetCajaVirtual(int pUsuarioID)
        {
            Caja c = CajaRepositorio.GetCajaVirtual(pUsuarioID);

            return c;
        }

        public static int GetCajaBalancePorUsuario(int usuarioid)
        {
            return CajaRepositorio.GetCajaBalancePorUsuario(usuarioid);
        }

        public static bool AddCaja(Caja caja)
        {
            return CajaRepositorio.AddCaja(caja);
        }

        public static bool ActualizarBalanceMinimoCajaPorUsuario(int BalanceMinimoCaja, int UsuarioID)
        {
            return CajaRepositorio.ActualizarBalanceMinimoCajaPorUsuario(BalanceMinimoCaja, UsuarioID);
        }

    }
}