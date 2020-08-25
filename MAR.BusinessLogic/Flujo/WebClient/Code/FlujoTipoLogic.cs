using System;
using System.Linq;
using System.Collections.Generic;
using Flujo.DataAccess.FlujoRepositories.WebClient.DapperRepositories;

using Flujo.Entities.WebClient.Enums;
using Flujo.Entities.WebClient.POCO;
using Flujo.Entities.WebClient.ViewModels;

namespace MAR.BusinessLogic.Flujo.WebClient.Code
{
    public static class FlujoTipoLogic
    {
        
        public static List<ComboboxTipoFlujoViewModel> GetGrupoCategoriaOperaciones()
        {
            List<ComboboxTipoFlujoViewModel> categorias = new List<ComboboxTipoFlujoViewModel>
            {              
                new ComboboxTipoFlujoViewModel{ Categoria ="Apuestas", Seleccionado = false },
                new ComboboxTipoFlujoViewModel{ Categoria ="Productos", Seleccionado = false },
                new ComboboxTipoFlujoViewModel{ Categoria ="Pagos", Seleccionado = false },
                new ComboboxTipoFlujoViewModel{ Categoria ="Faltantes", Seleccionado = false },
                new ComboboxTipoFlujoViewModel{ Categoria ="Sobrantes", Seleccionado = false },
                new ComboboxTipoFlujoViewModel{ Categoria ="Anulaciones", Seleccionado = false },
                new ComboboxTipoFlujoViewModel{ Categoria ="Entradas Manuales", Seleccionado = false },
                new ComboboxTipoFlujoViewModel{ Categoria ="Salidas Manuales", Seleccionado = false },
                new ComboboxTipoFlujoViewModel{ Categoria ="Cuadres", Seleccionado= false}

            }.OrderBy(b=> b.Categoria).ToList();

            categorias.Insert(0, new ComboboxTipoFlujoViewModel() { Categoria = "Todas las operaciones", Seleccionado = true});
            return categorias;
        }

        public static List<ComboboxTipoFlujoViewModel> GetGrupoCategoriaOperaciones(string pCategoriaASeleccionar)
        {
            List<ComboboxTipoFlujoViewModel> categorias = FlujoTipoLogic.GetGrupoCategoriaOperaciones();

            foreach (ComboboxTipoFlujoViewModel item in categorias)
            {
                if (pCategoriaASeleccionar.Equals(item.Categoria))
                {
                    item.Seleccionado = true;
                }
                else
                {
                    item.Seleccionado = false;
                }
            }

            if (!categorias.Any(b => b.Seleccionado == true))
            {
                categorias = FlujoTipoLogic.GetGrupoCategoriaOperaciones();
            }

            return categorias;
        }




        public static List<FlujoTipoCategoria> GetListadoIngreso()
        {
            try
            {
                List<FlujoTipoCategoria> LaColeccionIngresos = new List<FlujoTipoCategoria>();

                List<FlujoTipoCategoria> TResul = FlujoTipoRepositorio.GetTiposDeIngreso();

                foreach (FlujoTipoCategoria item in TResul)
                {
                    LaColeccionIngresos.Add(item);
                }

                return LaColeccionIngresos;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public static List<FlujoTipoCategoria> GetListadoIngreso(int pUsuarioID)
        {
            try
            {
                List<FlujoTipoCategoria> LaColeccionIngresos = new List<FlujoTipoCategoria>();

                List<FlujoTipoCategoria> TResul = FlujoTipoRepositorio.GetTiposDeIngreso(pUsuarioID);

                foreach (FlujoTipoCategoria item in TResul)
                {
                    LaColeccionIngresos.Add(item);
                }

                return LaColeccionIngresos;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public static List<FlujoTipoCategoria> GetListadoEgresos()
        {
            try
            {
                List<FlujoTipoCategoria> LaColeccionEgresos = new List<FlujoTipoCategoria>();

                List<FlujoTipoCategoria> TResult = FlujoTipoRepositorio.GetTiposDeEgresos();

                foreach (FlujoTipoCategoria item in TResult)
                {
                    LaColeccionEgresos.Add(item);
                }

                return LaColeccionEgresos;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public static List<FlujoTipoCategoria> GetListadoEgresos(int pUsuarioID)
        {
            try
            {
                List<FlujoTipoCategoria> LaColeccionEgresos = new List<FlujoTipoCategoria>();

                List<FlujoTipoCategoria> TResult = FlujoTipoRepositorio.GetTiposDeEgresos(pUsuarioID);

                foreach (FlujoTipoCategoria item in TResult)
                {
                    LaColeccionEgresos.Add(item);
                }

                return LaColeccionEgresos;
            }
            catch (Exception ex)
            {

                throw ex;
            }
  
        }
    }
}
