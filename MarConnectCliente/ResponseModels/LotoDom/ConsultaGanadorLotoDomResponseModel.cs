using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarConnectCliente.ResponseModels.LotoDom
{
    public class ConsultaGanadorLotoDomResponseModel : BaseResponseModel
    {
    
        public double Amount { get; set; }

        public override string ToString()
        {
            return this.ConsultaGanadorAnon().ToString();
        }

        public object ConsultaGanadorAnon()
        {

            var ModelAnnon = new
            {
                cod = this.Cod ?? "",
                msg = this.Msg ?? "",
       
            };

            return ModelAnnon;
        }
    }




}
