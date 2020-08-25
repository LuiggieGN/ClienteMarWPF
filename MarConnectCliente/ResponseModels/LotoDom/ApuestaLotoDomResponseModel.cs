using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarConnectCliente.ResponseModels.LotoDom
{
    public class ApuestaLotoDomResponseModel : BaseResponseModel
    {
    
        public string ticket { get; set; }
        public string no_auth { get; set; }
        public string raffle_number { get; set; }
        //public string TipoAutentificacion { get; set; } //(Online - Offline)
        //public string NumeroAutentificacion { get; set; } // naut
        //public JugadorBalance JugadorBalance { get; set; }
        //public List<int> SorteosNoDisponible { get; set; }
        //public List<TicketResponseModel.TicketDetalle> TicketDetalle { get; set; }
        public override string ToString()
        {
            return this.ApuestaAnon().ToString();
        }

        public object ApuestaAnon()
        {

            var ModelAnnon = new
            {
                no_auth = this.no_auth ?? "",
                ticket = this.ticket??"",
                cod = this.Cod??"",
                msg = this.Msg??"",
                raffle_number = this.raffle_number
            };

            return ModelAnnon;
        }
    }




}
