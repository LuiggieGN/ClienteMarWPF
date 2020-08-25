


namespace MarConnectCliente.RequestModels
{
    public class InicioDiaRequestModel : BaseRequestModel
    {
        public InicioDiaRequestModel()
        {
            TipoOperacion = "Inicio";
        }

        public override string ToString()
        {
            var _r = new
            {
                Consorcio = base.EstablecimientoID ?? "",
                Usuario = base.Usuario ?? "",
                Password = base.Password ?? "",
                TipoOperacion = "Inicio",
                CodigoOperacion = base.CodigoOperacion ?? "",
                DiaOperacion = base.DiaOperacion ?? "",
                FechaHoraSolicitud = base.FechaHoraSolicitud ?? "",
            };
            return _r.ToString();
        }

    }    

}

