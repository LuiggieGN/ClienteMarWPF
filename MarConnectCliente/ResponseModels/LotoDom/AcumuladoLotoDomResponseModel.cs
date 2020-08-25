using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarConnectCliente.ResponseModels.LotoDom
{
    public class AcumuladoLotoDomResponseModel : BaseResponseModel
    {
    
        public double Amount { get; set; }

        public override string ToString()
        {
            return this.AcumuladoAnon().ToString();
        }

        public object AcumuladoAnon()
        {

            var ModelAnnon = new
            {
                cod = this.Cod ?? "",
                msg = this.Msg ?? "",
                Amount = this.Amount,
            };

            return ModelAnnon;
        }
    }




}
