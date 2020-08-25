using System;
using System.Collections.Generic;

using Flujo.Entities.WebClient.Enums;
using Flujo.Entities.WebClient.POCO;

using Flujo.DataAccess.FlujoRepositories.WebClient.DapperRepositories;
using Flujo.Entities.WebClient.ViewModels;

namespace MAR.BusinessLogic.Flujo.WebClient.Code
{
    public static class UsuarioLogic
    {


        public static Usuario GetUsuarioPorCajaIDDeTipoVirtual(int pCajaID)
        {
            try
            {
                Usuario usuario = UsuarioRepositorio.GetUsuarioPorCajaIDDeTipoVirtual(pCajaID);
                return usuario;
            }
            catch (Exception ex)
            {
                return null; 
            }
        }


        public static int? AddUsuario(Usuario Usuario)
        {
            try
            {
                int? usuarioid = UsuarioRepositorio.AddUsuario(Usuario);

                return usuarioid;
            }
            catch (Exception ex)
            {
                return (int?)null;
            }           
        }

        public static bool ActualizaUsuario(Usuario Usuario)
        {
            return UsuarioRepositorio.ActualizaUsuario(Usuario);
        }

        public static void DeleteUsuario(int UsuarioID)
        {
            UsuarioRepositorio.DeleteUsuario(UsuarioID);
        }

        //Desactiva un usuario y elimina la tarjeta de tokens si el usuario la posee .  
        // True : Fue Desactivado | False: No Fue Desactivado
        public static bool DesactivarUsuario(int pUsuarioID)
        {
            try
            {
                bool fuedesactivado = UsuarioRepositorio.DesactivarUsuario(pUsuarioID);
                return fuedesactivado;
            }
            catch (Exception ex)
            {
                return false;
            }
        }



        public static bool PuedeEliminarUsuario(int pUsuarioQueIntentaEliminar, int pUsuarioAEliminar)
        {
            try
            {
                bool puedeeliminar = UsuarioRepositorio.PuedeEliminarUsuario(pUsuarioQueIntentaEliminar, pUsuarioAEliminar);
                return puedeeliminar;
            }
            catch (Exception ex)    
            {
                return false;
            }

        }


        public static List<ComboBoxModel> GetUsuariosTipo(int pTipoUsuarioID)
        {
            List<ComboBoxModel> Resultados = UsuarioRepositorio.GetUsuariosTipo(pTipoUsuarioID);

            return Resultados;
        }

        public static List<ComboBoxModel> GetDocumentosTipo()
        {
            List<ComboBoxModel> Resultados = UsuarioRepositorio.GetDocumentosTipo();

            return Resultados;
        }


        public static List<Rifero> GetRifero(string pSereach)
        {
            List<Rifero> riferos = UsuarioRepositorio.GetRifero(pSereach);

            return riferos;

        }

        public static List<MensajeroDocumento> GetMensajero(string pSereach)
        {
            List<MensajeroDocumento> mensajeros = UsuarioRepositorio.GetMensajeroDocumento(pSereach);

            return mensajeros;
        }

        public static Usuario GetUsuario(int pUsuarioID)
        {
            Usuario user = UsuarioRepositorio.GetUsuario(pUsuarioID);

            return user;

        }

        public static Usuario GetFlujoUsuarioEquivalent(int pMARUsuarioID)
        {
            Usuario user = UsuarioRepositorio.GetFlujoUsuarioEquivalent(pMARUsuarioID);

            return user;

        }

        public static ConsultaUsuarioBalance ConsultaUsuarioBalance_CajaVirtual(int pUsuarioID)
        {
            ConsultaUsuarioBalance consulta = UsuarioRepositorio.GetUsuarioBalance(pUsuarioID);

            return consulta;
        }

      public static List<ConsultaUsuarioBalance> ConsultarBalancePorUsuarioNombreODocumento( string pSearch)
      {
            try
            {

                List<ConsultaUsuarioBalance> LaColeccionDeUsuariosYBalances = UsuarioRepositorio.GetUsuariosBalances(pSearch);

                return LaColeccionDeUsuariosYBalances;

            }
            catch (Exception ex)
            {
                return new List<ConsultaUsuarioBalance>();
            }
      }

        public static List<ConsultaUsuarioBalance> ConsultarBalancePorMensajeroNombreODocumento(string pSearch)
        {
            try
            {

                List<ConsultaUsuarioBalance> LaColeccionDeUsuariosYBalances = UsuarioRepositorio.GetUsuariosTipoMensajeroBalance(pSearch);

                return LaColeccionDeUsuariosYBalances;

            }
            catch (Exception ex)
            {
                return new List<ConsultaUsuarioBalance>();
            }
        }


        public static List<ConsultaUsuarioBalance> ConsultarBalancePorUsuarioNombreODocumento (string pSearch,  TipoUsuario pTipo)
        {
            try
            {
                int TipoLogicaKey = (int)pTipo;

                List<ConsultaUsuarioBalance> laColeccionDeUsuariosConSusBalances = UsuarioRepositorio.GetUsuariosBalances(pSearch, TipoLogicaKey);

                return laColeccionDeUsuariosConSusBalances;
            }
            catch (Exception ex)
            {
                return new List<ConsultaUsuarioBalance>();                
            }

        }
    }
}
