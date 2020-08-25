using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAR.BusinessLogic.Code
{
    public class ProductosLogic
    {

        public ProductosLogic()
        {

        }
        public static IEnumerable<DataAccess.Tables.DTOs.VP_Producto> VpProductos { get; set; }


        static ProductosLogic()
        {
            VpProductos = DataAccess.EFRepositories.ProductosRepository.GetVpProductos();
        }
      
        public static object GetProductosDisponibles(int pBancaId)
        {
            try
            {
                var bancaConfigs = new BancaConfigLogic();
              
                return new
                {
                    OK = true,
                    VendeBilletes = bancaConfigs.IsBancaConfigTrue(Enums.BancaConfigEnums.BancaConfigKeyEnum.BANCA_VENDE_BILLETES, pBancaId, Enums.BancaConfigEnums.BancaConfigValueEnum.TRUE),
                    VendeRecargas = bancaConfigs.IsBancaConfigTrue(Enums.BancaConfigEnums.BancaConfigKeyEnum.BANCA_VENDE_TARJETAS, pBancaId, Enums.BancaConfigEnums.BancaConfigValueEnum.TRUE),
                    VendeJuegaMas = bancaConfigs.IsBancaConfigTrue(Enums.BancaConfigEnums.BancaConfigKeyEnum.BANCA_VENDE_JUEGAMAS, pBancaId, Enums.BancaConfigEnums.BancaConfigValueEnum.TRUE),
                    VendeServicios = bancaConfigs.IsBancaConfigTrue(Enums.BancaConfigEnums.BancaConfigKeyEnum.BANCA_PAGA_SERVICIOS, pBancaId, Enums.BancaConfigEnums.BancaConfigValueEnum.TRUE),
                    VendePolizas = bancaConfigs.IsBancaConfigTrue(Enums.BancaConfigEnums.BancaConfigKeyEnum.BANCA_VENDE_POLIZAS, pBancaId, Enums.BancaConfigEnums.BancaConfigValueEnum.TRUE),
                    VendePega4 = bancaConfigs.IsBancaConfigTrue(Enums.BancaConfigEnums.BancaConfigKeyEnum.BANCA_VENDE_PEGA4, pBancaId, Enums.BancaConfigEnums.BancaConfigValueEnum.TRUE),
                    VendeJuegosNuevos = bancaConfigs.IsBancaConfigTrue(Enums.BancaConfigEnums.BancaConfigKeyEnum.BANCA_VENDE_JUEGOSNUEVOS, pBancaId, Enums.BancaConfigEnums.BancaConfigValueEnum.TRUE),
                    VendeLoteriasNuevas = bancaConfigs.IsBancaConfigTrue(Enums.BancaConfigEnums.BancaConfigKeyEnum.BANCA_VENDE_LOTERIASNUEVAS, pBancaId, Enums.BancaConfigEnums.BancaConfigValueEnum.TRUE),
                    VendeBingo = bancaConfigs.IsBancaConfigTrue(Enums.BancaConfigEnums.BancaConfigKeyEnum.BANCA_VENDE_BINGO, pBancaId, Enums.BancaConfigEnums.BancaConfigValueEnum.TRUE),
                    VendeCincoMinutos = bancaConfigs.IsBancaConfigTrue(Enums.BancaConfigEnums.BancaConfigKeyEnum.BANCA_VENDE_CINCOMINUTOS, pBancaId, Enums.BancaConfigEnums.BancaConfigValueEnum.TRUE)
                };
            }
            catch (Exception)
            {
                return new
                {
                    OK = false,
                    Err = "Fallo la transaccion intente mas tarde"
                };
            }
        }
        public static int[] GetVPProductoIds(Enums.ProductosEnum[] pReferencias)
        {
            string[] referencias = new string[pReferencias.Count()];
            for (int i = 0; i < pReferencias.Count(); i++)
            {
                referencias[i] = pReferencias[i].ToString();
            }
            //var vPproductoId = DataAccess.EFRepositories.ProductosRepository.GetVpProductos(x => referencias.Contains(x.Referencia)).Select(x => x.ProductoID).ToArray();

            var vPproductoId = VpProductos.Where(x => referencias.Contains(x.Referencia)).Select(x => x.ProductoID).ToArray();

            return vPproductoId;
        }

        //public static bool IsProductoActivoStatic(Enums.ProductosEnum pProducto)
        //{
        //    var producto = DataAccess.EFRepositories.ProductosRepository.GetVpProductos(x => x.Nombre == pProducto.ToString() && x.Activo);
        //    if (producto.Any())
        //    {
        //        return true;
        //    }
        //    return false;
        //}

        public bool IsProductoActivo(Enums.ProductosEnum pProducto)
        {
            var producto = VpProductos.Where(x => x.Nombre.Contains(pProducto.ToString()) && x.Activo);
            if (producto.Any())
            {
                return true;
            }
            return false;
        }


    }
}
