
namespace MarConnectCliente.IndividualModels
{
    public partial class JuegosNuevosModel
    {
        public override string ToString()
        {
            return new
            {
                Consorcio = this.Consorcio?? "",
                Usuario = this.Usuario??"",
                Password = this.Password ?? "",
                Referencia = this.Referencia??"",
                TicketRequestModel = this.TicketRequestModel??"",
                FechaHoraSolicitud = this.FechaHoraSolicitud??"",
                DiaOperacion= this.DiaOperacion??"",
                UsarCuenta=this.UsarCuenta.ToString(),
                LocalID= this.LocalID.ToString(),
                BancaID = this.BancaID.ToString(),
                TipoDocumento = this.TipoDocumento??"",
                NumeroDocumento = this.NumeroDocumento??""

            }.ToString();
        }
    }//Fin Partial Class 'JuegosNuevosModel'

    public partial class JuegosNuevosModel
    {
        public string NumeroAutenticiacionCalculado { get; set; }
        public string Consorcio { get; set; }
        public string Usuario    { get; set; }
        public string Password { get; set; }
        public string Referencia { get; set; }
        public string TicketRequestModel { get; set; }    
        public int LocalID { get; set; }
        public int BancaID { get; set; }
        public bool UsarCuenta { get; set; }
        public string FechaHoraSolicitud { get; set; }
        public string DiaOperacion { get; set; }
        public string TipoDeOperacion = "Apuesta";
        public string TipoDocumento { get; set; }
        public string NumeroDocumento { get; set; }

    }//Fin Partial Class 'JuegosNuevosModel'
}
