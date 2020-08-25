using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MAR.DataAccess.Tables;


namespace MAR.DataAccess.Repositories
{
    public  class mar_VP_ProductoRepository : BaseRepository
    {
        public mar_VP_ProductoRepository(int pCurrentUserID) : base(pCurrentUserID) { }

        public List<VP_ProductoRecord> GetProductos(int? SoloProductoId, string NombreProducto, string FiltroNombreProductoPor)
        {
            return _Query<VP_ProductoRecord>("mar_VP_Productos_Get_Productos", new { SoloProductoId = SoloProductoId, NombreProducto = NombreProducto, FiltroNombreProductoPor = FiltroNombreProductoPor }).ToList();
        }

    }

}
