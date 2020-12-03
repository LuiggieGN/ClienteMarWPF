
namespace ClienteMarWPF.Domain.Models.Dtos
{
    public class MultipleDTO<P, S>
    {
        public P PrimerDTO { get; set; }
        public S SegundoDTO { get; set; }
    }

    public class MultipleDTO<P, S, T>
    {
        public P PrimerDTO { get; set; }
        public S SegundoDTO { get; set; }
        public T TercerDTO { get; set; }
    }

    public class MultipleDTO<P, S, T, C>
    {
        public P PrimerDTO { get; set; }
        public S SegundoDTO { get; set; }
        public T TercerDTO { get; set; }
        public C CuartoDTO { get; set; }
    }


    public class MultipleDTO<P, S, T, C, Q>
    {
        public P PrimerDTO { get; set; }
        public S SegundoDTO { get; set; }
        public T TercerDTO { get; set; }
        public C CuartoDTO { get; set; }
        public Q QuintoDTO { get; set; }
    }
}
