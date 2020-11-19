
using System;

namespace ClienteMarWPF.Domain.Models.Dtos
{
    public class BancaDTO
    {
        public int BancaID { get; set; }
        public string BanNombre { get; set; }
        public string BanContacto { get; set; }
        public string BanDireccion { get; set; }
        public string BanDireccionIP { get; set; }
        public string BanTelefono { get; set; }
        public string BanNumeroLinea { get; set; }
        public int RiferoID { get; set; }
        public bool BanActivo { get; set; }
        public string BanComentario { get; set; }
        public bool BanValidIP { get; set; }
        public int BanSesionActual { get; set; }
        public string BanDireccionActual { get; set; }
        public int BanUsuarioActual { get; set; }
        public int BanRePrintTicketID { get; set; }
        public int BanUltimoTicket { get; set; }
        public string BanVersion { get; set; }
        public float BanMaxQuiniela { get; set; }
        public float BanMaxPale { get; set; }
        public float BanMaxTriple { get; set; }
        public float BanComisionQ { get; set; }
        public float BanComisionP { get; set; }
        public float BanComisionT { get; set; }
        public int EsquemaID { get; set; }
        public DateTime BanAlive { get; set; }
        public DateTime BanFirstContact { get; set; }
        public DateTime BanLastContact { get; set; }
        public bool BanAnula { get; set; }
        public bool BanVLocal { get; set; }
        public bool BanGanadores { get; set; }
        public float BanMaxQuinielaLoc { get; set; }
        public float BanMaxPaleLoc { get; set; }
        public float BanMaxTripleLoc { get; set; }
        public bool BanTarjeta { get; set; }
        public float BanComisionTarj { get; set; }
        public int BanTerminalTarj { get; set; }
        public string BanSerieTarj { get; set; }
        public bool BanRenta { get; set; }
        public DateTime BanRegistra { get; set; }
        public string BanRemoteCMD { get; set; }
        public float BanMaxSupeLoc { get; set; }
        public float BanMaxQuinielaTic { get; set; }
        public float BanMaxPaleTic { get; set; }
        public float BanMaxTripleTic { get; set; }
        public int BanSesionWeb { get; set; }
        public string BanDireccionWeb { get; set; }
        public int BanUsuarioWeb { get; set; }
        public float BanMaxSupe { get; set; }
        public bool BanImpuesto { get; set; }
        public bool BanPrintRecarga { get; set; }
        public bool BanPrintReportes { get; set; }
        public bool BanBillete { get; set; }
        public bool BanServicios { get; set; }
        public int BanLocal { get; set; }

    }
}
