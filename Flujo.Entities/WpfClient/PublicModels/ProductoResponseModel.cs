using Flujo.Entities.WpfClient.RequestModel;
using Flujo.Entities.WpfClient.ResponseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Flujo.Entities.WpfClient.PublicModels
{
    public class ProductoResponseModel : BaseCincoMinutosResponseModel
    {
        public class ProductoViewModelResponse : BaseCincoMinutosResponseModel
        {
            public List<ProductoViewModel> Productos { get; set; }
        }
        public class ProductoViewModel
        {
            public int ProductoID { get; set; }
            public string Nombre { get; set; }
            public double Monto { get; set; }
            public string Referencia { get; set; }
            public VP_Cuenta Cuenta { get; set; }
            public int SuplidorID { get; set; }
            public List<VP_ProductoConfig> ProductoConfig { get; set; }
            public List<VP_ProductoCampo> ProductoCampos { get; set; }
            public List<ReciboCampo> RecibosCampos { get; set; }
        }
        public class VP_ProductoConfig
        {
            public int ProductoConfigID { get; set; }
            public string ConfigKey { get; set; }
            public string ConfigValue { get; set; }
            public bool Activo { get; set; }
            public string ProductoID { get; set; }
        }
        public class VP_ProductoCampo
        {
            public int ProductoCampoID { get; set; }
            public string Nombre { get; set; }
            public string Referencia { get; set; }
            public string TipoDato { get; set; }
            public string OpcionesReferencias { get; set; }
            public string OpcionesNombres { get; set; }
            public bool Activo { get; set; }
            public int ProductoID { get; set; }
        }
        public class VP_Cuenta
        {
            public int CuentaID { get; set; }
        }

        public class ReciboCampo
        {
            public string ReciboCampoID { get; set; }
            public string Nombre { get; set; }
            public string Referencia { get; set; }
        }
    }
}
