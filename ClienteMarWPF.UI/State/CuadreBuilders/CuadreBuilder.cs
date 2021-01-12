﻿using System;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

using ClienteMarWPF.Domain.Models.Dtos;
using ClienteMarWPF.Domain.Models.Dtos.EfectivoDtos;
using ClienteMarWPF.Domain.Models.Entities;


using ClienteMarWPF.Domain.Services.BancaService;
using ClienteMarWPF.Domain.Services.CajaService;
using ClienteMarWPF.Domain.Services.CuadreService;
using ClienteMarWPF.Domain.Exceptions;
using ClienteMarWPF.UI.State.Authenticators;
using ClienteMarWPF.UI.Modules.FlujoEfectivo.Cuadre.Windows.Cuadre.ViewModels;


namespace ClienteMarWPF.UI.State.CuadreBuilders
{
    public class CuadreBuilder : ICuadreBuilder
    {
        private readonly IBancaService _bancaService;
        private readonly ICajaService _cajaService;
        private readonly ICuadreService _cuadreService;

        public CuadreBuilder(
           IBancaService bancaService,
           ICajaService cajaService,
           ICuadreService cuadreService
        )
        {
            _bancaService = bancaService;
            _cajaService = cajaService;
            _cuadreService = cuadreService;
        }

        #region Metodos Cuadre Consulta Inicial 
        internal decimal LeerBancaBalance(IAuthenticator authenticator)
        {
            return _cajaService.LeerCajaBalance(authenticator.BancaConfiguracion.CajaEfectivoDto.CajaID);
        }
        internal decimal LeerBancaBalanceMinimo(IAuthenticator authenticator)
        {
            return _cajaService.LeerCajaBalanceMinimo(authenticator.BancaConfiguracion.CajaEfectivoDto.CajaID);
        }
        internal decimal LeerBancaDeuda(IAuthenticator authenticator)
        {
          return _bancaService.LeerDeudaDeBanca(authenticator.BancaConfiguracion.BancaDto.BancaID); 
        }
        public ConsultaInicialViewModel LeerCuadreConsultaInicial(IAuthenticator authenticator)
        {
            try
            {
                var consulta = new ConsultaInicialViewModel();
                var bancaBalanceMinimo = LeerBancaBalanceMinimo(authenticator);
                var bancaDeuda = LeerBancaDeuda(authenticator);
                var bancaBalance = LeerBancaBalance(authenticator);

                consulta.BancaBalanceMinimo = bancaBalanceMinimo;
                consulta.BancaDeuda = bancaDeuda;
                consulta.BancaBalance = bancaBalance;
                consulta.EstaCargando = false;

                return consulta;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
        public CuadreRegistroResultDTO BuildCuadre(BancaDTO banca, CuadreRegistroDTO ope, bool enablePrinting, out string toPrint)
        {
            try
            {
                return _cuadreService.Registrar(banca, ope, enablePrinting, out toPrint);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        

        public decimal LeerCajaBalance(int cajaid)
        {
            return _cajaService.LeerCajaBalance(cajaid);
        }


        public bool SetearCajaDisponibilidad(CajaDisponibilidadDTO disponibilidad) 
        {
            try
            {
                return _cajaService.SetearCajaDisponibilidad(disponibilidad);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



    }// fin de clase CuadreBuilder
}// fin de namespace CuadreBuilders

















