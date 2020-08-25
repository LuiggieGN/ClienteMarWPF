using System;

namespace Flujo.Entities.WebClient.POCO
{
    public class CategoriaBalance
    {
        public string Categoria { get; set; }

        public decimal BalanceTotal { get; set; }

        public string BalanceTotalEnFormato
        {
            get
            {
                return "$ " + ((BalanceTotal == 0) ? "0" : (string.Format("{0:n}", BalanceTotal)));
            }
        }

    }
}
