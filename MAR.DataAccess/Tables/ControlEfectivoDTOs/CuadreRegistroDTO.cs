namespace MAR.DataAccess.Tables.ControlEfectivoDTOs
{
    public enum CuadreGestorAccion
    {
        Depositar,
        Retirar
    }

    public class CuadreRegistroDTO
    { 
        public CuadreDTO Cuadre { get; set; }
        public CuadreGestorAccion CuadreGestorAccion { get; set; }
        public RutaAsignacionDTO RutaAsignacion { get; set; } 

    }
}
