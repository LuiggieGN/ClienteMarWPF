using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarConnectCliente.ResponseModels.LotoDom
{
    public class AnulacionLotoDomResponseModel : BaseResponseModel
    {
    
        public double Amount { get; set; }

        public override string ToString()
        {
            return this.AnulacionAnon().ToString();
        }

        public object AnulacionAnon()
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
