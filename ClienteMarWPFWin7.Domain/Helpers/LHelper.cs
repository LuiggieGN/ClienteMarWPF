namespace ClienteMarWPFWin7.Domain.Helpers
{
    public static class LHelper
    {
        public static string GetIdenidadCalculada(string cernumero)
        {
            if (cernumero == null || cernumero.Trim().Equals(string.Empty))
            {
                return string.Empty;
            }

            int valor;

            bool fueParceado = int.TryParse(cernumero, out valor);

            if (!fueParceado)
            {
                return string.Empty;
            }

            if (valor <= 8)
            {
                return string.Empty;
            }

            int calculado = (valor - 8) * 3;

            return $"{calculado}";
        } 

    }// LocalHelper
}
