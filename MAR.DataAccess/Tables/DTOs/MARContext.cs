namespace MAR.DataAccess.Tables.DTOs
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class MARContext : DbContext
    {
        public MARContext()
          : base(MAR.DataAccess.UnitOfWork.GenericMethods.SetEfConnectionString(Config.ConfigEnums.DBConnection2))
        {
            base.Configuration.ProxyCreationEnabled = false;
            base.Configuration.LazyLoadingEnabled = false;
        }

        public virtual DbSet<CL_Cliente> CL_Cliente { get; set; }
        public virtual DbSet<CL_ClienteCampo> CL_ClienteCampo { get; set; }
        public virtual DbSet<CL_ClienteDetalle> CL_ClienteDetalle { get; set; }
        public virtual DbSet<CL_Pagos> CL_Pagos { get; set; }
        public virtual DbSet<CL_Recibo> CL_Recibo { get; set; }
        public virtual DbSet<CL_ReciboCampo> CL_ReciboCampo { get; set; }
        public virtual DbSet<CL_ReciboDetalle> CL_ReciboDetalle { get; set; }
        public virtual DbSet<CL_ReciboDetalle_Extra> CL_ReciboDetalle_Extra { get; set; }
        public virtual DbSet<DAlerta> DAlertas { get; set; }
        public virtual DbSet<DBilleteDetalle> DBilleteDetalles { get; set; }
        public virtual DbSet<DControlDetalle> DControlDetalles { get; set; }
        public virtual DbSet<DControle> DControles { get; set; }
        public virtual DbSet<DImpuestoRetenido> DImpuestoRetenidoes { get; set; }
        public virtual DbSet<DListaDia> DListaDias { get; set; }
        public virtual DbSet<DMensaje> DMensajes { get; set; }
        public virtual DbSet<DRiesgoDia> DRiesgoDias { get; set; }
        public virtual DbSet<DTicketDetalle> DTicketDetalles { get; set; }
        public virtual DbSet<DTicket> DTickets { get; set; }
        public virtual DbSet<dtproperty> dtproperties { get; set; }
        public virtual DbSet<DWebProductoDetalle> DWebProductoDetalles { get; set; }
        public virtual DbSet<HAlerta> HAlertas { get; set; }
        public virtual DbSet<HCertificado> HCertificados { get; set; }
        public virtual DbSet<HClaveLocal> HClaveLocals { get; set; }
        public virtual DbSet<HContabilidad> HContabilidads { get; set; }
        public virtual DbSet<HControle> HControles { get; set; }
        public virtual DbSet<HDestino> HDestinos { get; set; }
        public virtual DbSet<HEstatusDia> HEstatusDias { get; set; }
        public virtual DbSet<HImpuestoRetenido> HImpuestoRetenidoes { get; set; }
        public virtual DbSet<HLog> HLogs { get; set; }
        public virtual DbSet<HMensaje> HMensajes { get; set; }
        public virtual DbSet<HPago> HPagos { get; set; }
        public virtual DbSet<HRebote> HRebotes { get; set; }
        public virtual DbSet<HResuman> HResumen { get; set; }
        public virtual DbSet<HSecurityLog> HSecurityLogs { get; set; }
        public virtual DbSet<HTicketDetalle> HTicketDetalles { get; set; }
        public virtual DbSet<HTicketLocalDetalle> HTicketLocalDetalles { get; set; }
        public virtual DbSet<HTicket> HTickets { get; set; }
        public virtual DbSet<HTicketsLocal> HTicketsLocals { get; set; }
        public virtual DbSet<HWebProductoDetalle> HWebProductoDetalles { get; set; }
        public virtual DbSet<MBanca> MBancas { get; set; }
        public virtual DbSet<MBancasConfig> MBancasConfigs { get; set; }
        public virtual DbSet<MConsorcio> MConsorcios { get; set; }
        public virtual DbSet<MCuenta> MCuentas { get; set; }
        public virtual DbSet<MDiasDefecto> MDiasDefectoes { get; set; }
        public virtual DbSet<MImpuestoRango> MImpuestoRangoes { get; set; }
        public virtual DbSet<MNotificacione> MNotificaciones { get; set; }
        public virtual DbSet<MPrecio> MPrecios { get; set; }
        public virtual DbSet<MPremioSuperPale> MPremioSuperPales { get; set; }
        public virtual DbSet<MRifero> MRiferos { get; set; }
        public virtual DbSet<MUsuario> MUsuarios { get; set; }
        public virtual DbSet<MZonaLimite> MZonaLimites { get; set; }
        public virtual DbSet<MZona> MZonas { get; set; }
        public virtual DbSet<PDPine> PDPines { get; set; }
        public virtual DbSet<PHPine> PHPines { get; set; }
        public virtual DbSet<PHVenta> PHVentas { get; set; }
        public virtual DbSet<PMCuenta> PMCuentas { get; set; }
        public virtual DbSet<PMProducto> PMProductos { get; set; }
        public virtual DbSet<PMSuplidore> PMSuplidores { get; set; }
        public virtual DbSet<RCuentasBanca> RCuentasBancas { get; set; }
        public virtual DbSet<RF_BancaSorteo> RF_BancaSorteo { get; set; }
        public virtual DbSet<RF_EsquemaPago> RF_EsquemaPago { get; set; }
        public virtual DbSet<RF_EsquemaPagoBanca> RF_EsquemaPagoBanca { get; set; }
        public virtual DbSet<RF_EsquemaPagoPremio> RF_EsquemaPagoPremio { get; set; }
        public virtual DbSet<RF_LimiteVenta> RF_LimiteVenta { get; set; }
        public virtual DbSet<RF_LimiteVentaDia> RF_LimiteVentaDia { get; set; }
        public virtual DbSet<RF_Loteria> RF_Loteria { get; set; }
        public virtual DbSet<RF_ResumenVenta> RF_ResumenVenta { get; set; }
        public virtual DbSet<RF_Sorteo> RF_Sorteo { get; set; }
        public virtual DbSet<RF_SorteoCampo> RF_SorteoCampo { get; set; }
        public virtual DbSet<RF_SorteoConfig> RF_SorteoConfig { get; set; }
        public virtual DbSet<RF_SorteoDia> RF_SorteoDia { get; set; }
        public virtual DbSet<RF_SorteoTipo> RF_SorteoTipo { get; set; }
        public virtual DbSet<RF_SorteoTipoJugada> RF_SorteoTipoJugada { get; set; }
        public virtual DbSet<RF_SorteoTipoPremio> RF_SorteoTipoPremio { get; set; }
        public virtual DbSet<RF_Transaccion> RF_Transaccion { get; set; }
        public virtual DbSet<RF_TransaccionDetalle> RF_TransaccionDetalle { get; set; }
        public virtual DbSet<RF_TransaccionJugada> RF_TransaccionJugada { get; set; }
        public virtual DbSet<RFuncionAdminUsuario> RFuncionAdminUsuarios { get; set; }
        public virtual DbSet<RGruposUsuario> RGruposUsuarios { get; set; }
        public virtual DbSet<RRiferosUsuario> RRiferosUsuarios { get; set; }
        public virtual DbSet<RWebProductoBanca> RWebProductoBancas { get; set; }
        public virtual DbSet<RWebWindowBanca> RWebWindowBancas { get; set; }
        public virtual DbSet<SFuncionAdmin> SFuncionAdmins { get; set; }
        public virtual DbSet<SReporte> SReportes { get; set; }
        public virtual DbSet<SWebProducto> SWebProductoes { get; set; }
        public virtual DbSet<SWebProductoConfig> SWebProductoConfigs { get; set; }
        public virtual DbSet<SWebWindow> SWebWindows { get; set; }
        public virtual DbSet<sysdiagram> sysdiagrams { get; set; }
        public virtual DbSet<TEsquema> TEsquemas { get; set; }
        public virtual DbSet<TGrupos> TGrupos { get; set; }
        public virtual DbSet<TLoteria> TLoterias { get; set; }
        public virtual DbSet<VP_Comisiones> VP_Comisiones { get; set; }
        public virtual DbSet<VP_Cuenta> VP_Cuenta { get; set; }
        public virtual DbSet<VP_CuentaConfig> VP_CuentaConfig { get; set; }
        public virtual DbSet<VP_CuentaTipo> VP_CuentaTipo { get; set; }
        public virtual DbSet<VP_HTransaccion> VP_HTransaccion { get; set; }
        public virtual DbSet<VP_HTransaccionDetalle> VP_HTransaccionDetalle { get; set; }
        public virtual DbSet<VP_LimiteVenta> VP_LimiteVenta { get; set; }
        public virtual DbSet<VP_LimiteVentaDia> VP_LimiteVentaDia { get; set; }
        public virtual DbSet<VP_LogComunicacion> VP_LogComunicacion { get; set; }
        public virtual DbSet<VP_MarcaVehiculo> VP_MarcaVehiculo { get; set; }
        public virtual DbSet<VP_ModeloVehiculo> VP_ModeloVehiculo { get; set; }
        public virtual DbSet<VP_Producto> VP_Producto { get; set; }
        public virtual DbSet<VP_ProductoCampo> VP_ProductoCampo { get; set; }
        public virtual DbSet<VP_ProductoConfig> VP_ProductoConfig { get; set; }
        public virtual DbSet<VP_Suplidor> VP_Suplidor { get; set; }
        public virtual DbSet<VP_SuplidorProducto> VP_SuplidorProducto { get; set; }
        public virtual DbSet<VP_Transaccion> VP_Transaccion { get; set; }
        public virtual DbSet<VP_TransaccionDetalle> VP_TransaccionDetalle { get; set; }
        public virtual DbSet<dcontroles_bk> dcontroles_bk { get; set; }
        public virtual DbSet<PHVentas_Productos_PreRebuild> PHVentas_Productos_PreRebuild { get; set; }
        public virtual DbSet<VentaConPuto> VentaConPutoes { get; set; }
        public virtual DbSet<PVPine> PVPines { get; set; }
        public virtual DbSet<VContabilidad> VContabilidads { get; set; }
        public virtual DbSet<VDiaBillete> VDiaBilletes { get; set; }
        public virtual DbSet<VDiaConsolida> VDiaConsolidas { get; set; }
        public virtual DbSet<VDiaConsolidax> VDiaConsolidaxes { get; set; }
        public virtual DbSet<VDiaGrafico> VDiaGraficoes { get; set; }
        public virtual DbSet<VDiaTarjeta> VDiaTarjetas { get; set; }
        public virtual DbSet<VDiaVenta> VDiaVentas { get; set; }
        public virtual DbSet<VDiaVPTransaccione> VDiaVPTransacciones { get; set; }
        public virtual DbSet<VDiaVPTransaccionesWithProductoID> VDiaVPTransaccionesWithProductoIDs { get; set; }
        public virtual DbSet<VFinanzaBancaConsolida> VFinanzaBancaConsolidas { get; set; }
        public virtual DbSet<VFinanzaBancaDia> VFinanzaBancaDias { get; set; }
        public virtual DbSet<VFinanzaBancaDiaTotal> VFinanzaBancaDiaTotals { get; set; }
        public virtual DbSet<VFinanzaBancaFecha> VFinanzaBancaFechas { get; set; }
        public virtual DbSet<VFinanzaBancaTotalFecha> VFinanzaBancaTotalFechas { get; set; }
        public virtual DbSet<VFinanzaDiaConsolida> VFinanzaDiaConsolidas { get; set; }
        public virtual DbSet<VFinanzaHisConsolida> VFinanzaHisConsolidas { get; set; }
        public virtual DbSet<VFinanzasPagado> VFinanzasPagadoes { get; set; }
        public virtual DbSet<VHEstatusDia> VHEstatusDias { get; set; }
        public virtual DbSet<VHisConsolida> VHisConsolidas { get; set; }
        public virtual DbSet<VHisConsolida2> VHisConsolida2 { get; set; }
        public virtual DbSet<VHisGrafico> VHisGraficoes { get; set; }
        public virtual DbSet<VTarjetaFecBan> VTarjetaFecBans { get; set; }
        public virtual DbSet<VTarjetaPorCuentaID> VTarjetaPorCuentaIDs { get; set; }
        public virtual DbSet<VTicketsDia> VTicketsDias { get; set; }
        public virtual DbSet<VTicketsDiaGanadore> VTicketsDiaGanadores { get; set; }
        public virtual DbSet<VTicketsGanadore> VTicketsGanadores { get; set; }
        public virtual DbSet<VTicketsHi> VTicketsHis { get; set; }
        public virtual DbSet<VTicketsHisGanadore> VTicketsHisGanadores { get; set; }
        public virtual DbSet<VTicketsNoPagado> VTicketsNoPagados { get; set; }
        public virtual DbSet<VTicketsNulo> VTicketsNulos { get; set; }
        public virtual DbSet<VTicketsPagado> VTicketsPagados { get; set; }
        public virtual DbSet<VTicketsValido> VTicketsValidos { get; set; }
        public virtual DbSet<VTransacionFecBan> VTransacionFecBans { get; set; }
        public virtual DbSet<VVentaLocal> VVentaLocals { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CL_Cliente>()
                .Property(e => e.Nombres)
                .IsUnicode(false);

            modelBuilder.Entity<CL_Cliente>()
                .Property(e => e.Apellidos)
                .IsUnicode(false);

            modelBuilder.Entity<CL_Cliente>()
                .Property(e => e.Documento)
                .IsUnicode(false);

            modelBuilder.Entity<CL_ClienteDetalle>()
                .Property(e => e.Referencia)
                .IsUnicode(false);

            modelBuilder.Entity<CL_ClienteDetalle>()
                .Property(e => e.ValorText)
                .IsUnicode(false);

            modelBuilder.Entity<CL_Pagos>()
                .Property(e => e.Monto)
                .HasPrecision(19, 4);

            modelBuilder.Entity<CL_Pagos>()
                .Property(e => e.Referencia)
                .IsUnicode(false);

            modelBuilder.Entity<CL_Recibo>()
                .Property(e => e.Referencia)
                .IsUnicode(false);

            modelBuilder.Entity<CL_Recibo>()
                .Property(e => e.Serie)
                .IsUnicode(false);

            modelBuilder.Entity<CL_Recibo>()
                .Property(e => e.Ingresos)
                .HasPrecision(19, 4);

            modelBuilder.Entity<CL_Recibo>()
                .Property(e => e.Impuestos)
                .HasPrecision(19, 4);

            modelBuilder.Entity<CL_Recibo>()
                .Property(e => e.Descuentos)
                .HasPrecision(19, 4);

            modelBuilder.Entity<CL_Recibo>()
                .HasMany(e => e.CL_ReciboDetalle_Extra)
                .WithRequired(e => e.CL_Recibo)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<CL_ReciboDetalle>()
                .Property(e => e.Tipo)
                .IsUnicode(false);

            modelBuilder.Entity<CL_ReciboDetalle>()
                .Property(e => e.Estado)
                .IsUnicode(false);

            modelBuilder.Entity<CL_ReciboDetalle>()
                .Property(e => e.Ingreso)
                .HasPrecision(19, 4);

            modelBuilder.Entity<CL_ReciboDetalle>()
                .Property(e => e.Impuesto)
                .HasPrecision(19, 4);

            modelBuilder.Entity<CL_ReciboDetalle>()
                .Property(e => e.Descuento)
                .HasPrecision(19, 4);

            modelBuilder.Entity<CL_ReciboDetalle_Extra>()
                .Property(e => e.Referencia)
                .IsUnicode(false);

            modelBuilder.Entity<CL_ReciboDetalle_Extra>()
                .Property(e => e.ValorText)
                .IsUnicode(false);

            modelBuilder.Entity<CL_ReciboDetalle_Extra>()
                .Property(e => e.Ingreso)
                .HasPrecision(19, 4);

            modelBuilder.Entity<CL_ReciboDetalle_Extra>()
                .Property(e => e.Descuento)
                .HasPrecision(19, 4);

            modelBuilder.Entity<DAlerta>()
                .Property(e => e.Entidad)
                .IsUnicode(false);

            modelBuilder.Entity<DAlerta>()
                .Property(e => e.Mensaje)
                .IsUnicode(false);

            modelBuilder.Entity<DAlerta>()
                .Property(e => e.Usuario)
                .IsUnicode(false);

            modelBuilder.Entity<DAlerta>()
                .Property(e => e.Origen)
                .IsUnicode(false);

            modelBuilder.Entity<DBilleteDetalle>()
                .Property(e => e.Serial)
                .IsUnicode(false);

            modelBuilder.Entity<DControlDetalle>()
                .Property(e => e.CDeQP)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<DControlDetalle>()
                .Property(e => e.CDeNumero)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<DControle>()
                .Property(e => e.ConNombre)
                .IsUnicode(false);

            modelBuilder.Entity<DControle>()
                .Property(e => e.ConColor)
                .IsUnicode(false);

            modelBuilder.Entity<DControle>()
                .Property(e => e.ConQP)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<DImpuestoRetenido>()
                .Property(e => e.TotalSaco)
                .HasPrecision(19, 4);

            modelBuilder.Entity<DImpuestoRetenido>()
                .Property(e => e.Retenido)
                .HasPrecision(19, 4);

            modelBuilder.Entity<DImpuestoRetenido>()
                .Property(e => e.TicNumero)
                .IsUnicode(false);

            modelBuilder.Entity<DListaDia>()
                .Property(e => e.LDiNumero)
                .IsUnicode(false);

            modelBuilder.Entity<DListaDia>()
                .Property(e => e.LDiQP)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<DMensaje>()
                .Property(e => e.MenTipo)
                .IsUnicode(false);

            modelBuilder.Entity<DMensaje>()
                .Property(e => e.MenAsunto)
                .IsUnicode(false);

            modelBuilder.Entity<DMensaje>()
                .Property(e => e.MenContenido)
                .IsUnicode(false);

            modelBuilder.Entity<DMensaje>()
                .Property(e => e.MenDestino)
                .IsUnicode(false);

            modelBuilder.Entity<DMensaje>()
                .Property(e => e.MenDireccion)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<DMensaje>()
                .Property(e => e.MenOrigen)
                .IsUnicode(false);

            modelBuilder.Entity<DRiesgoDia>()
                .Property(e => e.RieQP)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<DTicketDetalle>()
                .Property(e => e.TDeQP)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<DTicketDetalle>()
                .Property(e => e.TDeNumero)
                .IsUnicode(false);

            modelBuilder.Entity<DTicketDetalle>()
                .Property(e => e.TDeCosto)
                .HasPrecision(19, 4);

            modelBuilder.Entity<DTicketDetalle>()
                .Property(e => e.TDePago)
                .HasPrecision(19, 4);

            modelBuilder.Entity<DTicketDetalle>()
                .Property(e => e.TDePagoTipo)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<DTicketDetalle>()
                .Property(e => e.TDeLlave)
                .IsUnicode(false);

            modelBuilder.Entity<DTicketDetalle>()
                .HasMany(e => e.DBilleteDetalles)
                .WithRequired(e => e.DTicketDetalle)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<DTicket>()
                .Property(e => e.TicCliente)
                .IsUnicode(false);

            modelBuilder.Entity<DTicket>()
                .Property(e => e.TicCedula)
                .IsUnicode(false);

            modelBuilder.Entity<DTicket>()
                .Property(e => e.TicNumero)
                .IsUnicode(false);

            modelBuilder.Entity<DTicket>()
                .Property(e => e.TicCosto)
                .HasPrecision(19, 4);

            modelBuilder.Entity<DTicket>()
                .Property(e => e.TicPago)
                .HasPrecision(19, 4);

            modelBuilder.Entity<DTicket>()
                .Property(e => e.TicSolicitud)
                .HasPrecision(18, 0);

            modelBuilder.Entity<dtproperty>()
                .Property(e => e.property)
                .IsUnicode(false);

            modelBuilder.Entity<dtproperty>()
                .Property(e => e.value)
                .IsUnicode(false);

            modelBuilder.Entity<DWebProductoDetalle>()
                .Property(e => e.FieldKey)
                .IsUnicode(false);

            modelBuilder.Entity<DWebProductoDetalle>()
                .Property(e => e.FieldValue)
                .IsUnicode(false);

            modelBuilder.Entity<HAlerta>()
                .Property(e => e.Entidad)
                .IsUnicode(false);

            modelBuilder.Entity<HAlerta>()
                .Property(e => e.Mensaje)
                .IsUnicode(false);

            modelBuilder.Entity<HAlerta>()
                .Property(e => e.Usuario)
                .IsUnicode(false);

            modelBuilder.Entity<HAlerta>()
                .Property(e => e.Origen)
                .IsUnicode(false);

            modelBuilder.Entity<HClaveLocal>()
                .Property(e => e.CLoLlave)
                .IsUnicode(false);

            modelBuilder.Entity<HClaveLocal>()
                .Property(e => e.CLoAutoriza)
                .IsUnicode(false);

            modelBuilder.Entity<HClaveLocal>()
                .Property(e => e.CLoIP)
                .IsUnicode(false);

            modelBuilder.Entity<HClaveLocal>()
                .Property(e => e.CLoUsuario)
                .IsUnicode(false);

            modelBuilder.Entity<HContabilidad>()
                .Property(e => e.CueNombre)
                .IsUnicode(false);

            modelBuilder.Entity<HContabilidad>()
                .Property(e => e.CueNumero)
                .IsUnicode(false);

            modelBuilder.Entity<HControle>()
                .Property(e => e.ConNombre)
                .IsUnicode(false);

            modelBuilder.Entity<HDestino>()
                .Property(e => e.DesIPAddress)
                .IsUnicode(false);

            modelBuilder.Entity<HEstatusDia>()
                .Property(e => e.PremioQ1)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<HEstatusDia>()
                .Property(e => e.PremioQ2)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<HEstatusDia>()
                .Property(e => e.PremioQ3)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<HEstatusDia>()
                .Property(e => e.EDiClave)
                .HasPrecision(18, 0);

            modelBuilder.Entity<HImpuestoRetenido>()
                .Property(e => e.TotalSaco)
                .HasPrecision(19, 4);

            modelBuilder.Entity<HImpuestoRetenido>()
                .Property(e => e.Retenido)
                .HasPrecision(19, 4);

            modelBuilder.Entity<HImpuestoRetenido>()
                .Property(e => e.TicNumero)
                .IsUnicode(false);

            modelBuilder.Entity<HLog>()
                .Property(e => e.LogTipo)
                .IsUnicode(false);

            modelBuilder.Entity<HLog>()
                .Property(e => e.LogComentario)
                .IsUnicode(false);

            modelBuilder.Entity<HLog>()
                .Property(e => e.UsuarioID)
                .IsUnicode(false);

            modelBuilder.Entity<HLog>()
                .Property(e => e.SecRemoteIP)
                .IsUnicode(false);

            modelBuilder.Entity<HMensaje>()
                .Property(e => e.MenTipo)
                .IsUnicode(false);

            modelBuilder.Entity<HMensaje>()
                .Property(e => e.MenAsunto)
                .IsUnicode(false);

            modelBuilder.Entity<HMensaje>()
                .Property(e => e.MenContenido)
                .IsUnicode(false);

            modelBuilder.Entity<HMensaje>()
                .Property(e => e.MenDestino)
                .IsUnicode(false);

            modelBuilder.Entity<HMensaje>()
                .Property(e => e.MenDireccion)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<HMensaje>()
                .Property(e => e.MenOrigen)
                .IsUnicode(false);

            modelBuilder.Entity<HPago>()
                .Property(e => e.BanContacto)
                .IsUnicode(false);

            modelBuilder.Entity<HPago>()
                .Property(e => e.PagMonto)
                .HasPrecision(19, 4);

            modelBuilder.Entity<HPago>()
                .Property(e => e.PagUsuario)
                .IsUnicode(false);

            modelBuilder.Entity<HPago>()
                .Property(e => e.TicNumero)
                .IsUnicode(false);

            modelBuilder.Entity<HRebote>()
                .Property(e => e.RebComision)
                .HasPrecision(18, 0);

            modelBuilder.Entity<HRebote>()
                .Property(e => e.RebNeto)
                .HasPrecision(18, 0);

            modelBuilder.Entity<HRebote>()
                .Property(e => e.RebTicketAqui)
                .IsUnicode(false);

            modelBuilder.Entity<HRebote>()
                .Property(e => e.RebTicketAlla)
                .IsUnicode(false);

            modelBuilder.Entity<HRebote>()
                .Property(e => e.RebSaco)
                .HasPrecision(18, 0);

            modelBuilder.Entity<HRebote>()
                .Property(e => e.RebBalance)
                .HasPrecision(18, 0);

            modelBuilder.Entity<HResuman>()
                .Property(e => e.Primero)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<HResuman>()
                .Property(e => e.Segundo)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<HResuman>()
                .Property(e => e.Tercero)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<HResuman>()
                .Property(e => e.VQuinielas)
                .HasPrecision(19, 4);

            modelBuilder.Entity<HResuman>()
                .Property(e => e.VBilletes)
                .HasPrecision(19, 4);

            modelBuilder.Entity<HResuman>()
                .Property(e => e.VPales)
                .HasPrecision(19, 4);

            modelBuilder.Entity<HResuman>()
                .Property(e => e.VTripletas)
                .HasPrecision(19, 4);

            modelBuilder.Entity<HResuman>()
                .Property(e => e.MPrimero)
                .HasPrecision(19, 4);

            modelBuilder.Entity<HResuman>()
                .Property(e => e.MSegundo)
                .HasPrecision(19, 4);

            modelBuilder.Entity<HResuman>()
                .Property(e => e.MTercero)
                .HasPrecision(19, 4);

            modelBuilder.Entity<HResuman>()
                .Property(e => e.MPales)
                .HasPrecision(19, 4);

            modelBuilder.Entity<HResuman>()
                .Property(e => e.MTripletas)
                .HasPrecision(19, 4);

            modelBuilder.Entity<HResuman>()
                .Property(e => e.MPagado)
                .HasPrecision(19, 4);

            modelBuilder.Entity<HResuman>()
                .Property(e => e.ISRRetenido)
                .HasPrecision(19, 4);

            modelBuilder.Entity<HSecurityLog>()
                .Property(e => e.SecTipo)
                .IsUnicode(false);

            modelBuilder.Entity<HSecurityLog>()
                .Property(e => e.SecComentario)
                .IsUnicode(false);

            modelBuilder.Entity<HSecurityLog>()
                .Property(e => e.UsuarioID)
                .IsUnicode(false);

            modelBuilder.Entity<HSecurityLog>()
                .Property(e => e.SecRemoteIP)
                .IsUnicode(false);

            modelBuilder.Entity<HTicketDetalle>()
                .Property(e => e.TDeQP)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<HTicketDetalle>()
                .Property(e => e.TDeNumero)
                .IsUnicode(false);

            modelBuilder.Entity<HTicketDetalle>()
                .Property(e => e.TDeCosto)
                .HasPrecision(19, 4);

            modelBuilder.Entity<HTicketDetalle>()
                .Property(e => e.TDePago)
                .HasPrecision(19, 4);

            modelBuilder.Entity<HTicketDetalle>()
                .Property(e => e.TDePagoTipo)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<HTicketDetalle>()
                .Property(e => e.TDeLlave)
                .IsUnicode(false);

            modelBuilder.Entity<HTicketLocalDetalle>()
                .Property(e => e.TLDQP)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<HTicketLocalDetalle>()
                .Property(e => e.TLDNumero)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<HTicketLocalDetalle>()
                .Property(e => e.TLDCosto)
                .HasPrecision(19, 4);

            modelBuilder.Entity<HTicketLocalDetalle>()
                .Property(e => e.TLDPago)
                .HasPrecision(19, 4);

            modelBuilder.Entity<HTicketLocalDetalle>()
                .Property(e => e.TLDPagoTipo)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<HTicket>()
                .Property(e => e.TicCliente)
                .IsUnicode(false);

            modelBuilder.Entity<HTicket>()
                .Property(e => e.TicCedula)
                .IsUnicode(false);

            modelBuilder.Entity<HTicket>()
                .Property(e => e.TicNumero)
                .IsUnicode(false);

            modelBuilder.Entity<HTicket>()
                .Property(e => e.TicCosto)
                .HasPrecision(19, 4);

            modelBuilder.Entity<HTicket>()
                .Property(e => e.TicPago)
                .HasPrecision(19, 4);

            modelBuilder.Entity<HTicket>()
                .Property(e => e.TicSolicitud)
                .HasPrecision(18, 0);

            modelBuilder.Entity<HTicketsLocal>()
                .Property(e => e.TLoCliente)
                .IsUnicode(false);

            modelBuilder.Entity<HTicketsLocal>()
                .Property(e => e.TLoCedula)
                .IsUnicode(false);

            modelBuilder.Entity<HTicketsLocal>()
                .Property(e => e.TLoNumero)
                .IsUnicode(false);

            modelBuilder.Entity<HTicketsLocal>()
                .Property(e => e.TLoCosto)
                .HasPrecision(19, 4);

            modelBuilder.Entity<HTicketsLocal>()
                .Property(e => e.TLoPago)
                .HasPrecision(19, 4);

            modelBuilder.Entity<HTicketsLocal>()
                .Property(e => e.TLoSolicitud)
                .HasPrecision(18, 0);

            modelBuilder.Entity<HWebProductoDetalle>()
                .Property(e => e.FieldKey)
                .IsUnicode(false);

            modelBuilder.Entity<HWebProductoDetalle>()
                .Property(e => e.FieldValue)
                .IsUnicode(false);

            modelBuilder.Entity<MBanca>()
                .Property(e => e.BanNombre)
                .IsUnicode(false);

            modelBuilder.Entity<MBanca>()
                .Property(e => e.BanContacto)
                .IsUnicode(false);

            modelBuilder.Entity<MBanca>()
                .Property(e => e.BanDireccion)
                .IsUnicode(false);

            modelBuilder.Entity<MBanca>()
                .Property(e => e.BanDireccionIP)
                .IsUnicode(false);

            modelBuilder.Entity<MBanca>()
                .Property(e => e.BanTelefono)
                .IsUnicode(false);

            modelBuilder.Entity<MBanca>()
                .Property(e => e.BanNumeroLinea)
                .IsUnicode(false);

            modelBuilder.Entity<MBanca>()
                .Property(e => e.BanComentario)
                .IsUnicode(false);

            modelBuilder.Entity<MBanca>()
                .Property(e => e.BanDireccionActual)
                .IsUnicode(false);

            modelBuilder.Entity<MBanca>()
                .Property(e => e.BanVersion)
                .IsUnicode(false);

            modelBuilder.Entity<MBanca>()
                .Property(e => e.BanSerieTarj)
                .IsUnicode(false);

            modelBuilder.Entity<MBanca>()
                .Property(e => e.BanRemoteCMD)
                .IsUnicode(false);

            modelBuilder.Entity<MBanca>()
                .Property(e => e.BanDireccionWeb)
                .IsUnicode(false);

            modelBuilder.Entity<MBanca>()
                .HasMany(e => e.VP_Comisiones)
                .WithRequired(e => e.MBanca)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<MConsorcio>()
                .Property(e => e.ConNombre)
                .IsUnicode(false);

            modelBuilder.Entity<MConsorcio>()
                .Property(e => e.ConContacto)
                .IsUnicode(false);

            modelBuilder.Entity<MConsorcio>()
                .Property(e => e.ConTelefonos)
                .IsUnicode(false);

            modelBuilder.Entity<MConsorcio>()
                .Property(e => e.ConServidor)
                .IsUnicode(false);

            modelBuilder.Entity<MCuenta>()
                .Property(e => e.CueNombre)
                .IsUnicode(false);

            modelBuilder.Entity<MCuenta>()
                .Property(e => e.CueNumero)
                .IsUnicode(false);

            modelBuilder.Entity<MCuenta>()
                .Property(e => e.CueComentario)
                .IsUnicode(false);

            modelBuilder.Entity<MDiasDefecto>()
                .Property(e => e.DDeCostoQ)
                .HasPrecision(19, 4);

            modelBuilder.Entity<MDiasDefecto>()
                .Property(e => e.DDeCostoP)
                .HasPrecision(19, 4);

            modelBuilder.Entity<MDiasDefecto>()
                .Property(e => e.DDeCostoT)
                .HasPrecision(19, 4);

            modelBuilder.Entity<MDiasDefecto>()
                .Property(e => e.DDePagoQ1)
                .HasPrecision(19, 4);

            modelBuilder.Entity<MDiasDefecto>()
                .Property(e => e.DDePagoQ2)
                .HasPrecision(19, 4);

            modelBuilder.Entity<MDiasDefecto>()
                .Property(e => e.DDePagoQ3)
                .HasPrecision(19, 4);

            modelBuilder.Entity<MDiasDefecto>()
                .Property(e => e.DDePagoP1)
                .HasPrecision(19, 4);

            modelBuilder.Entity<MDiasDefecto>()
                .Property(e => e.DDePagoP2)
                .HasPrecision(19, 4);

            modelBuilder.Entity<MDiasDefecto>()
                .Property(e => e.DDePagoP3)
                .HasPrecision(19, 4);

            modelBuilder.Entity<MDiasDefecto>()
                .Property(e => e.DDePagoT1)
                .HasPrecision(19, 4);

            modelBuilder.Entity<MDiasDefecto>()
                .Property(e => e.DDePagoT2)
                .HasPrecision(19, 4);

            modelBuilder.Entity<MDiasDefecto>()
                .Property(e => e.DDePagoT3)
                .HasPrecision(19, 4);

            modelBuilder.Entity<MImpuestoRango>()
                .Property(e => e.DesdeMonto)
                .HasPrecision(19, 4);

            modelBuilder.Entity<MImpuestoRango>()
                .Property(e => e.HastaMonto)
                .HasPrecision(19, 4);

            modelBuilder.Entity<MNotificacione>()
                .Property(e => e.Nombre)
                .IsUnicode(false);

            modelBuilder.Entity<MNotificacione>()
                .Property(e => e.eMail)
                .IsUnicode(false);

            modelBuilder.Entity<MRifero>()
                .Property(e => e.RifNombre)
                .IsUnicode(false);

            modelBuilder.Entity<MRifero>()
                .Property(e => e.RifContacto)
                .IsUnicode(false);

            modelBuilder.Entity<MRifero>()
                .Property(e => e.RifTelefono)
                .IsUnicode(false);

            modelBuilder.Entity<MRifero>()
                .Property(e => e.RifCelular)
                .IsUnicode(false);

            modelBuilder.Entity<MRifero>()
                .Property(e => e.RifCedula)
                .IsUnicode(false);

            modelBuilder.Entity<MRifero>()
                .Property(e => e.RifComentario)
                .IsUnicode(false);

            modelBuilder.Entity<MRifero>()
                .HasMany(e => e.MBancas)
                .WithRequired(e => e.MRifero)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<MUsuario>()
                .Property(e => e.UsuNombre)
                .IsUnicode(false);

            modelBuilder.Entity<MUsuario>()
                .Property(e => e.UsuApellido)
                .IsUnicode(false);

            modelBuilder.Entity<MUsuario>()
                .Property(e => e.UsuCedula)
                .IsUnicode(false);

            modelBuilder.Entity<MUsuario>()
                .Property(e => e.UsuUserName)
                .IsUnicode(false);

            modelBuilder.Entity<MUsuario>()
                .Property(e => e.UsuClave)
                .IsUnicode(false);

            modelBuilder.Entity<MUsuario>()
                .Property(e => e.UsuComentario)
                .IsUnicode(false);

            modelBuilder.Entity<MZona>()
                .Property(e => e.Nombre)
                .IsUnicode(false);

            modelBuilder.Entity<PDPine>()
                .Property(e => e.PinSerial)
                .IsUnicode(false);

            modelBuilder.Entity<PDPine>()
                .Property(e => e.PinNumero)
                .IsUnicode(false);

            modelBuilder.Entity<PDPine>()
                .Property(e => e.PinReferencia)
                .IsUnicode(false);

            modelBuilder.Entity<PDPine>()
                .Property(e => e.PinCodigo)
                .IsUnicode(false);

            modelBuilder.Entity<PDPine>()
                .Property(e => e.PinMensaje)
                .IsUnicode(false);

            modelBuilder.Entity<PDPine>()
                .Property(e => e.PinSecuencia)
                .HasPrecision(10, 0);

            modelBuilder.Entity<PDPine>()
                .Property(e => e.PinFlag)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<PDPine>()
                .Property(e => e.PinIPAddr)
                .IsUnicode(false);

            modelBuilder.Entity<PDPine>()
                .Property(e => e.PinCosto)
                .HasPrecision(18, 4);

            modelBuilder.Entity<PHPine>()
                .Property(e => e.PinSerial)
                .IsUnicode(false);

            modelBuilder.Entity<PHPine>()
                .Property(e => e.PinNumero)
                .IsUnicode(false);

            modelBuilder.Entity<PHPine>()
                .Property(e => e.PinReferencia)
                .IsUnicode(false);

            modelBuilder.Entity<PHPine>()
                .Property(e => e.PinCodigo)
                .IsUnicode(false);

            modelBuilder.Entity<PHPine>()
                .Property(e => e.PinMensaje)
                .IsUnicode(false);

            modelBuilder.Entity<PHPine>()
                .Property(e => e.PinSecuencia)
                .HasPrecision(10, 0);

            modelBuilder.Entity<PHPine>()
                .Property(e => e.PinFlag)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<PHPine>()
                .Property(e => e.PinIPAddr)
                .IsUnicode(false);

            modelBuilder.Entity<PHPine>()
                .Property(e => e.PinCosto)
                .HasPrecision(18, 4);

            modelBuilder.Entity<PHPine>()
                .Property(e => e.ProNombre)
                .IsUnicode(false);

            modelBuilder.Entity<PHPine>()
                .Property(e => e.SupNombre)
                .IsUnicode(false);

            modelBuilder.Entity<PHVenta>()
                .Property(e => e.SupNombre)
                .IsUnicode(false);

            modelBuilder.Entity<PHVenta>()
                .Property(e => e.Vendedor)
                .IsUnicode(false);

            modelBuilder.Entity<PHVenta>()
                .Property(e => e.VTarjetas)
                .HasPrecision(19, 4);

            modelBuilder.Entity<PMCuenta>()
                .Property(e => e.CueNombre)
                .IsUnicode(false);

            modelBuilder.Entity<PMCuenta>()
                .Property(e => e.CueComercio)
                .IsUnicode(false);

            modelBuilder.Entity<PMCuenta>()
                .Property(e => e.CueComentario)
                .IsUnicode(false);

            modelBuilder.Entity<PMCuenta>()
                .Property(e => e.CueServidor)
                .IsUnicode(false);

            modelBuilder.Entity<PMProducto>()
                .Property(e => e.ProNombre)
                .IsUnicode(false);

            modelBuilder.Entity<PMSuplidore>()
                .Property(e => e.SupNombre)
                .IsUnicode(false);

            modelBuilder.Entity<PMSuplidore>()
                .Property(e => e.SupComentario)
                .IsUnicode(false);

            modelBuilder.Entity<PMSuplidore>()
                .Property(e => e.SupInstrucciones)
                .IsUnicode(false);

            modelBuilder.Entity<PMSuplidore>()
                .HasMany(e => e.PMProductos)
                .WithRequired(e => e.PMSuplidore)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<RF_EsquemaPago>()
                .Property(e => e.Nombre)
                .IsUnicode(false);

            modelBuilder.Entity<RF_EsquemaPago>()
                .Property(e => e.Description)
                .IsUnicode(false);

            modelBuilder.Entity<RF_EsquemaPagoPremio>()
                .Property(e => e.Paga)
                .HasPrecision(19, 4);

            modelBuilder.Entity<RF_EsquemaPagoPremio>()
                .Property(e => e.DiaSemana)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<RF_LimiteVenta>()
                .Property(e => e.Limite)
                .HasPrecision(19, 4);

            modelBuilder.Entity<RF_LimiteVenta>()
                .Property(e => e.Numeros)
                .IsUnicode(false);

            modelBuilder.Entity<RF_LimiteVentaDia>()
                .Property(e => e.Vendido)
                .HasPrecision(19, 4);

            modelBuilder.Entity<RF_LimiteVentaDia>()
                .Property(e => e.Numeros)
                .IsUnicode(false);

            modelBuilder.Entity<RF_Loteria>()
                .Property(e => e.Nombre)
                .IsUnicode(false);

            modelBuilder.Entity<RF_Loteria>()
                .Property(e => e.Referencia)
                .IsUnicode(false);

            modelBuilder.Entity<RF_ResumenVenta>()
                .Property(e => e.Aposto)
                .HasPrecision(19, 4);

            modelBuilder.Entity<RF_ResumenVenta>()
                .Property(e => e.Comision)
                .HasPrecision(19, 4);

            modelBuilder.Entity<RF_ResumenVenta>()
                .Property(e => e.Saco)
                .HasPrecision(19, 4);

            modelBuilder.Entity<RF_Sorteo>()
                .Property(e => e.Nombre)
                .IsUnicode(false);

            modelBuilder.Entity<RF_Sorteo>()
                .Property(e => e.Referencia)
                .IsUnicode(false);

            modelBuilder.Entity<RF_SorteoConfig>()
                .Property(e => e.ConfigKey)
                .IsUnicode(false);

            modelBuilder.Entity<RF_SorteoConfig>()
                .Property(e => e.ConfigValue)
                .IsUnicode(false);

            modelBuilder.Entity<RF_SorteoDia>()
                .Property(e => e.Premios)
                .IsUnicode(false);

            modelBuilder.Entity<RF_SorteoDia>()
                .Property(e => e.Estado)
                .IsUnicode(false);

            modelBuilder.Entity<RF_SorteoDia>()
                .Property(e => e.Referencia)
                .IsUnicode(false);

            modelBuilder.Entity<RF_SorteoTipo>()
                .Property(e => e.Nombre)
                .IsUnicode(false);

            modelBuilder.Entity<RF_SorteoTipo>()
                .Property(e => e.Referencia)
                .IsUnicode(false);

            modelBuilder.Entity<RF_SorteoTipo>()
                .Property(e => e.Descripcion)
                .IsUnicode(false);

            modelBuilder.Entity<RF_SorteoTipo>()
                .HasMany(e => e.RF_SorteoTipoJugada)
                .WithRequired(e => e.RF_SorteoTipo)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<RF_SorteoTipoJugada>()
                .Property(e => e.Opciones)
                .IsUnicode(false);

            modelBuilder.Entity<RF_SorteoTipoJugada>()
                .Property(e => e.Instrucciones)
                .IsUnicode(false);

            modelBuilder.Entity<RF_SorteoTipoPremio>()
                .Property(e => e.Nombre)
                .IsUnicode(false);

            modelBuilder.Entity<RF_SorteoTipoPremio>()
                .Property(e => e.Descripcion)
                .IsUnicode(false);

            modelBuilder.Entity<RF_Transaccion>()
                .Property(e => e.Referencia)
                .IsUnicode(false);

            modelBuilder.Entity<RF_Transaccion>()
                .Property(e => e.Serie)
                .IsUnicode(false);

            modelBuilder.Entity<RF_Transaccion>()
                .Property(e => e.Estado)
                .IsUnicode(false);

            modelBuilder.Entity<RF_Transaccion>()
                .Property(e => e.Ingresos)
                .HasPrecision(19, 4);

            modelBuilder.Entity<RF_Transaccion>()
                .Property(e => e.Pagos)
                .HasPrecision(19, 4);

            modelBuilder.Entity<RF_Transaccion>()
                .Property(e => e.Notas)
                .IsUnicode(false);

            modelBuilder.Entity<RF_Transaccion>()
                .HasMany(e => e.RF_TransaccionDetalle)
                .WithRequired(e => e.RF_Transaccion)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<RF_TransaccionDetalle>()
                .Property(e => e.Referencia)
                .IsUnicode(false);

            modelBuilder.Entity<RF_TransaccionDetalle>()
                .Property(e => e.ValorText)
                .IsUnicode(false);

            modelBuilder.Entity<RF_TransaccionDetalle>()
                .Property(e => e.Ingreso)
                .HasPrecision(19, 4);

            modelBuilder.Entity<RF_TransaccionDetalle>()
                .Property(e => e.Descuento)
                .HasPrecision(19, 4);

            modelBuilder.Entity<RF_TransaccionJugada>()
                .Property(e => e.Numeros)
                .IsUnicode(false);

            modelBuilder.Entity<RF_TransaccionJugada>()
                .Property(e => e.Aposto)
                .HasPrecision(19, 4);

            modelBuilder.Entity<RF_TransaccionJugada>()
                .Property(e => e.Pago)
                .HasPrecision(19, 4);

            modelBuilder.Entity<RF_TransaccionJugada>()
                .Property(e => e.Opciones)
                .IsUnicode(false);

            modelBuilder.Entity<SFuncionAdmin>()
                .Property(e => e.Nombre)
                .IsUnicode(false);

            modelBuilder.Entity<SReporte>()
                .Property(e => e.RepNombre)
                .IsUnicode(false);

            modelBuilder.Entity<SReporte>()
                .Property(e => e.RepFuente)
                .IsUnicode(false);

            modelBuilder.Entity<SReporte>()
                .Property(e => e.RepArchivo)
                .IsUnicode(false);

            modelBuilder.Entity<SReporte>()
                .Property(e => e.RepOrder)
                .IsUnicode(false);

            modelBuilder.Entity<SWebProducto>()
                .Property(e => e.Nombre)
                .IsUnicode(false);

            modelBuilder.Entity<SWebProducto>()
                .Property(e => e.URL)
                .IsUnicode(false);

            modelBuilder.Entity<SWebProductoConfig>()
                .Property(e => e.Opcion)
                .IsUnicode(false);

            modelBuilder.Entity<SWebProductoConfig>()
                .Property(e => e.Valor)
                .IsUnicode(false);

            modelBuilder.Entity<SWebWindow>()
                .Property(e => e.Nombre)
                .IsUnicode(false);

            modelBuilder.Entity<SWebWindow>()
                .Property(e => e.URL)
                .IsUnicode(false);

            modelBuilder.Entity<TEsquema>()
                .Property(e => e.EsqNombre)
                .IsUnicode(false);

            modelBuilder.Entity<TGrupos>()
                .Property(e => e.GruNombre)
                .IsUnicode(false);

            modelBuilder.Entity<TGrupos>()
                .Property(e => e.GruContacto)
                .IsUnicode(false);

            modelBuilder.Entity<TGrupos>()
                .Property(e => e.GruTelefono)
                .IsUnicode(false);

            modelBuilder.Entity<TGrupos>()
                .Property(e => e.GruCelular)
                .IsUnicode(false);

            modelBuilder.Entity<TGrupos>()
                .Property(e => e.GruComentario)
                .IsUnicode(false);

            modelBuilder.Entity<TGrupos>()
                .Property(e => e.GruClientFooter)
                .IsUnicode(false);

            modelBuilder.Entity<TGrupos>()
                .Property(e => e.GruPrintHeader)
                .IsUnicode(false);

            modelBuilder.Entity<TGrupos>()
                .Property(e => e.GruPrintFooter)
                .IsUnicode(false);

            modelBuilder.Entity<TGrupos>()
                .HasMany(e => e.DControles)
                .WithRequired(e => e.TGrupos)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TGrupos>()
                .HasMany(e => e.HControles)
                .WithRequired(e => e.TGrupos)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TGrupos>()
                .HasMany(e => e.HEstatusDias)
                .WithRequired(e => e.TGrupos)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TGrupos>()
                .HasMany(e => e.HTickets)
                .WithRequired(e => e.TGrupos)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TGrupos>()
                .HasMany(e => e.MPrecios)
                .WithRequired(e => e.TGrupos)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TGrupos>()
                .HasMany(e => e.MRiferos)
                .WithRequired(e => e.TGrupos)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TLoteria>()
                .Property(e => e.LotNombre)
                .IsUnicode(false);

            modelBuilder.Entity<TLoteria>()
                .Property(e => e.LotComentario)
                .IsUnicode(false);

            modelBuilder.Entity<TLoteria>()
                .Property(e => e.NombreResumido)
                .IsUnicode(false);

            modelBuilder.Entity<VP_Cuenta>()
                .HasMany(e => e.VP_CuentaConfig)
                .WithRequired(e => e.VP_Cuenta)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<VP_Cuenta>()
                .HasMany(e => e.VP_Suplidor)
                .WithRequired(e => e.VP_Cuenta)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<VP_CuentaTipo>()
                .HasMany(e => e.VP_Cuenta)
                .WithRequired(e => e.VP_CuentaTipo)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<VP_CuentaTipo>()
                .HasMany(e => e.VP_Producto)
                .WithRequired(e => e.VP_CuentaTipo)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<VP_HTransaccion>()
                .Property(e => e.Referencia)
                .IsUnicode(false);

            modelBuilder.Entity<VP_HTransaccion>()
                .Property(e => e.ReferenciaCliente)
                .IsUnicode(false);

            modelBuilder.Entity<VP_HTransaccion>()
                .Property(e => e.Estado)
                .IsUnicode(false);

            modelBuilder.Entity<VP_HTransaccion>()
                .Property(e => e.Ingresos)
                .HasPrecision(19, 4);

            modelBuilder.Entity<VP_HTransaccion>()
                .Property(e => e.Descuentos)
                .HasPrecision(19, 4);

            modelBuilder.Entity<VP_HTransaccion>()
                .HasMany(e => e.VP_HTransaccionDetalle)
                .WithRequired(e => e.VP_HTransaccion)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<VP_HTransaccionDetalle>()
                .Property(e => e.Referencia)
                .IsUnicode(false);

            modelBuilder.Entity<VP_HTransaccionDetalle>()
                .Property(e => e.ValorText)
                .IsUnicode(false);

            modelBuilder.Entity<VP_HTransaccionDetalle>()
                .Property(e => e.Ingreso)
                .HasPrecision(19, 4);

            modelBuilder.Entity<VP_HTransaccionDetalle>()
                .Property(e => e.Descuento)
                .HasPrecision(19, 4);

            modelBuilder.Entity<VP_LimiteVenta>()
                .Property(e => e.Limite)
                .HasPrecision(19, 4);

            modelBuilder.Entity<VP_LimiteVentaDia>()
                .Property(e => e.Vendido)
                .HasPrecision(19, 4);

            modelBuilder.Entity<VP_MarcaVehiculo>()
                .Property(e => e.Marca)
                .IsUnicode(false);

            modelBuilder.Entity<VP_MarcaVehiculo>()
                .HasMany(e => e.VP_ModeloVehiculo)
                .WithRequired(e => e.VP_MarcaVehiculo)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<VP_ModeloVehiculo>()
                .Property(e => e.Modelo)
                .IsUnicode(false);

            modelBuilder.Entity<VP_Producto>()
                .HasMany(e => e.VP_Comisiones)
                .WithRequired(e => e.VP_Producto)
                .HasForeignKey(e => e.VP_ProductoID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<VP_Producto>()
                .HasMany(e => e.VP_HTransaccion)
                .WithRequired(e => e.VP_Producto)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<VP_Producto>()
                .HasMany(e => e.VP_ProductoConfig)
                .WithRequired(e => e.VP_Producto)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<VP_Producto>()
                .HasMany(e => e.VP_Transaccion)
                .WithRequired(e => e.VP_Producto)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<VP_Suplidor>()
                .HasMany(e => e.VP_HTransaccion)
                .WithRequired(e => e.VP_Suplidor)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<VP_Suplidor>()
                .HasMany(e => e.VP_Transaccion)
                .WithRequired(e => e.VP_Suplidor)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<VP_Transaccion>()
                .Property(e => e.Referencia)
                .IsUnicode(false);

            modelBuilder.Entity<VP_Transaccion>()
                .Property(e => e.ReferenciaCliente)
                .IsUnicode(false);

            modelBuilder.Entity<VP_Transaccion>()
                .Property(e => e.Estado)
                .IsUnicode(false);

            modelBuilder.Entity<VP_Transaccion>()
                .Property(e => e.Ingresos)
                .HasPrecision(19, 4);

            modelBuilder.Entity<VP_Transaccion>()
                .Property(e => e.Descuentos)
                .HasPrecision(19, 4);

            modelBuilder.Entity<VP_Transaccion>()
                .HasMany(e => e.VP_TransaccionDetalle)
                .WithRequired(e => e.VP_Transaccion)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<VP_TransaccionDetalle>()
                .Property(e => e.Referencia)
                .IsUnicode(false);

            modelBuilder.Entity<VP_TransaccionDetalle>()
                .Property(e => e.ValorText)
                .IsUnicode(false);

            modelBuilder.Entity<VP_TransaccionDetalle>()
                .Property(e => e.Ingreso)
                .HasPrecision(19, 4);

            modelBuilder.Entity<VP_TransaccionDetalle>()
                .Property(e => e.Descuento)
                .HasPrecision(19, 4);

            modelBuilder.Entity<dcontroles_bk>()
                .Property(e => e.ConNombre)
                .IsUnicode(false);

            modelBuilder.Entity<dcontroles_bk>()
                .Property(e => e.ConColor)
                .IsUnicode(false);

            modelBuilder.Entity<dcontroles_bk>()
                .Property(e => e.ConQP)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<PHVentas_Productos_PreRebuild>()
                .Property(e => e.SupNombre)
                .IsUnicode(false);

            modelBuilder.Entity<PHVentas_Productos_PreRebuild>()
                .Property(e => e.Vendedor)
                .IsUnicode(false);

            modelBuilder.Entity<PHVentas_Productos_PreRebuild>()
                .Property(e => e.VTarjetas)
                .HasPrecision(19, 4);

            modelBuilder.Entity<VentaConPuto>()
                .Property(e => e.TDeQP)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<VentaConPuto>()
                .Property(e => e.TDeNumero)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<VentaConPuto>()
                .Property(e => e.TDeCosto)
                .HasPrecision(19, 4);

            modelBuilder.Entity<VentaConPuto>()
                .Property(e => e.TDePago)
                .HasPrecision(19, 4);

            modelBuilder.Entity<VentaConPuto>()
                .Property(e => e.TDePagoTipo)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<PVPine>()
                .Property(e => e.PinSerial)
                .IsUnicode(false);

            modelBuilder.Entity<PVPine>()
                .Property(e => e.PinNumero)
                .IsUnicode(false);

            modelBuilder.Entity<PVPine>()
                .Property(e => e.PinReferencia)
                .IsUnicode(false);

            modelBuilder.Entity<PVPine>()
                .Property(e => e.PinCodigo)
                .IsUnicode(false);

            modelBuilder.Entity<PVPine>()
                .Property(e => e.PinMensaje)
                .IsUnicode(false);

            modelBuilder.Entity<PVPine>()
                .Property(e => e.PinSecuencia)
                .HasPrecision(10, 0);

            modelBuilder.Entity<PVPine>()
                .Property(e => e.PinFlag)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<PVPine>()
                .Property(e => e.PinIPAddr)
                .IsUnicode(false);

            modelBuilder.Entity<PVPine>()
                .Property(e => e.PinCosto)
                .HasPrecision(18, 4);

            modelBuilder.Entity<PVPine>()
                .Property(e => e.ProNombre)
                .IsUnicode(false);

            modelBuilder.Entity<PVPine>()
                .Property(e => e.SupNombre)
                .IsUnicode(false);

            modelBuilder.Entity<VContabilidad>()
                .Property(e => e.GruNombre)
                .IsUnicode(false);

            modelBuilder.Entity<VContabilidad>()
                .Property(e => e.RifNombre)
                .IsUnicode(false);

            modelBuilder.Entity<VContabilidad>()
                .Property(e => e.BanNombre)
                .IsUnicode(false);

            modelBuilder.Entity<VContabilidad>()
                .Property(e => e.BanContacto)
                .IsUnicode(false);

            modelBuilder.Entity<VContabilidad>()
                .Property(e => e.VQuinielas)
                .HasPrecision(19, 4);

            modelBuilder.Entity<VContabilidad>()
                .Property(e => e.VPales)
                .HasPrecision(19, 4);

            modelBuilder.Entity<VContabilidad>()
                .Property(e => e.VTripletas)
                .HasPrecision(19, 4);

            modelBuilder.Entity<VContabilidad>()
                .Property(e => e.MPrimero)
                .HasPrecision(19, 4);

            modelBuilder.Entity<VContabilidad>()
                .Property(e => e.MSegundo)
                .HasPrecision(19, 4);

            modelBuilder.Entity<VContabilidad>()
                .Property(e => e.MTercero)
                .HasPrecision(19, 4);

            modelBuilder.Entity<VContabilidad>()
                .Property(e => e.MPales)
                .HasPrecision(19, 4);

            modelBuilder.Entity<VContabilidad>()
                .Property(e => e.MTripletas)
                .HasPrecision(19, 4);

            modelBuilder.Entity<VContabilidad>()
                .Property(e => e.CtaNumero)
                .IsUnicode(false);

            modelBuilder.Entity<VContabilidad>()
                .Property(e => e.CtaNombre)
                .IsUnicode(false);

            modelBuilder.Entity<VDiaBillete>()
                .Property(e => e.SupNombre)
                .IsUnicode(false);

            modelBuilder.Entity<VDiaBillete>()
                .Property(e => e.Vendedor)
                .IsUnicode(false);

            modelBuilder.Entity<VDiaBillete>()
                .Property(e => e.VTarjetas)
                .HasPrecision(19, 4);

            modelBuilder.Entity<VDiaConsolida>()
                .Property(e => e.VTarjetas)
                .HasPrecision(38, 4);

            modelBuilder.Entity<VDiaConsolida>()
                .Property(e => e.VQuinielas)
                .HasPrecision(19, 4);

            modelBuilder.Entity<VDiaConsolida>()
                .Property(e => e.VPales)
                .HasPrecision(19, 4);

            modelBuilder.Entity<VDiaConsolida>()
                .Property(e => e.VTripletas)
                .HasPrecision(19, 4);

            modelBuilder.Entity<VDiaConsolida>()
                .Property(e => e.MPrimero)
                .HasPrecision(19, 4);

            modelBuilder.Entity<VDiaConsolida>()
                .Property(e => e.MSegundo)
                .HasPrecision(19, 4);

            modelBuilder.Entity<VDiaConsolida>()
                .Property(e => e.MTercero)
                .HasPrecision(19, 4);

            modelBuilder.Entity<VDiaConsolida>()
                .Property(e => e.MPales)
                .HasPrecision(19, 4);

            modelBuilder.Entity<VDiaConsolida>()
                .Property(e => e.MTripletas)
                .HasPrecision(19, 4);

            modelBuilder.Entity<VDiaConsolida>()
                .Property(e => e.ISRRetenido)
                .HasPrecision(19, 4);

            modelBuilder.Entity<VDiaConsolidax>()
                .Property(e => e.VTarjetas)
                .HasPrecision(38, 4);

            modelBuilder.Entity<VDiaConsolidax>()
                .Property(e => e.VQuinielas)
                .HasPrecision(19, 4);

            modelBuilder.Entity<VDiaConsolidax>()
                .Property(e => e.VPales)
                .HasPrecision(19, 4);

            modelBuilder.Entity<VDiaConsolidax>()
                .Property(e => e.VTripletas)
                .HasPrecision(19, 4);

            modelBuilder.Entity<VDiaConsolidax>()
                .Property(e => e.MPrimero)
                .HasPrecision(19, 4);

            modelBuilder.Entity<VDiaConsolidax>()
                .Property(e => e.MSegundo)
                .HasPrecision(19, 4);

            modelBuilder.Entity<VDiaConsolidax>()
                .Property(e => e.MTercero)
                .HasPrecision(19, 4);

            modelBuilder.Entity<VDiaConsolidax>()
                .Property(e => e.MPales)
                .HasPrecision(19, 4);

            modelBuilder.Entity<VDiaConsolidax>()
                .Property(e => e.MTripletas)
                .HasPrecision(19, 4);

            modelBuilder.Entity<VDiaConsolidax>()
                .Property(e => e.ISRRetenido)
                .HasPrecision(19, 4);

            modelBuilder.Entity<VDiaGrafico>()
                .Property(e => e.BanContacto)
                .IsUnicode(false);

            modelBuilder.Entity<VDiaGrafico>()
                .Property(e => e.RifNombre)
                .IsUnicode(false);

            modelBuilder.Entity<VDiaGrafico>()
                .Property(e => e.LotNombre)
                .IsUnicode(false);

            modelBuilder.Entity<VDiaGrafico>()
                .Property(e => e.Ventas)
                .HasPrecision(19, 4);

            modelBuilder.Entity<VDiaGrafico>()
                .Property(e => e.Saco)
                .HasPrecision(19, 4);

            modelBuilder.Entity<VDiaTarjeta>()
                .Property(e => e.SupNombre)
                .IsUnicode(false);

            modelBuilder.Entity<VDiaTarjeta>()
                .Property(e => e.Vendedor)
                .IsUnicode(false);

            modelBuilder.Entity<VDiaTarjeta>()
                .Property(e => e.VTarjetas)
                .HasPrecision(38, 4);

            modelBuilder.Entity<VDiaVenta>()
                .Property(e => e.Primero)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<VDiaVenta>()
                .Property(e => e.Segundo)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<VDiaVenta>()
                .Property(e => e.Tercero)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<VDiaVenta>()
                .Property(e => e.VQuinielas)
                .HasPrecision(19, 4);

            modelBuilder.Entity<VDiaVenta>()
                .Property(e => e.VBilletes)
                .HasPrecision(19, 4);

            modelBuilder.Entity<VDiaVenta>()
                .Property(e => e.VPales)
                .HasPrecision(19, 4);

            modelBuilder.Entity<VDiaVenta>()
                .Property(e => e.VTripletas)
                .HasPrecision(19, 4);

            modelBuilder.Entity<VDiaVenta>()
                .Property(e => e.MPrimero)
                .HasPrecision(19, 4);

            modelBuilder.Entity<VDiaVenta>()
                .Property(e => e.MSegundo)
                .HasPrecision(19, 4);

            modelBuilder.Entity<VDiaVenta>()
                .Property(e => e.MTercero)
                .HasPrecision(19, 4);

            modelBuilder.Entity<VDiaVenta>()
                .Property(e => e.MPales)
                .HasPrecision(19, 4);

            modelBuilder.Entity<VDiaVenta>()
                .Property(e => e.MTripletas)
                .HasPrecision(19, 4);

            modelBuilder.Entity<VDiaVenta>()
                .Property(e => e.MPagado)
                .HasPrecision(19, 4);

            modelBuilder.Entity<VDiaVenta>()
                .Property(e => e.rescierre)
                .IsUnicode(false);

            modelBuilder.Entity<VDiaVenta>()
                .Property(e => e.ISRRetenido)
                .HasPrecision(19, 4);

            modelBuilder.Entity<VDiaVPTransaccione>()
                .Property(e => e.Vendedor)
                .IsUnicode(false);

            modelBuilder.Entity<VDiaVPTransaccione>()
                .Property(e => e.VPagos)
                .HasPrecision(19, 4);

            modelBuilder.Entity<VDiaVPTransaccionesWithProductoID>()
                .Property(e => e.Vendedor)
                .IsUnicode(false);

            modelBuilder.Entity<VDiaVPTransaccionesWithProductoID>()
                .Property(e => e.VPagos)
                .HasPrecision(19, 4);

            modelBuilder.Entity<VDiaVPTransaccionesWithProductoID>()
                .Property(e => e.VDescuentos)
                .HasPrecision(19, 4);

            modelBuilder.Entity<VFinanzaBancaConsolida>()
                .Property(e => e.Dia)
                .IsUnicode(false);

            modelBuilder.Entity<VFinanzaBancaConsolida>()
                .Property(e => e.GruNombre)
                .IsUnicode(false);

            modelBuilder.Entity<VFinanzaBancaConsolida>()
                .Property(e => e.RifNombre)
                .IsUnicode(false);

            modelBuilder.Entity<VFinanzaBancaConsolida>()
                .Property(e => e.BanNombre)
                .IsUnicode(false);

            modelBuilder.Entity<VFinanzaBancaConsolida>()
                .Property(e => e.BanContacto)
                .IsUnicode(false);

            modelBuilder.Entity<VFinanzaBancaConsolida>()
                .Property(e => e.VTarjetas)
                .HasPrecision(38, 4);

            modelBuilder.Entity<VFinanzaBancaConsolida>()
                .Property(e => e.VQuinielas)
                .HasPrecision(19, 4);

            modelBuilder.Entity<VFinanzaBancaConsolida>()
                .Property(e => e.VPales)
                .HasPrecision(19, 4);

            modelBuilder.Entity<VFinanzaBancaConsolida>()
                .Property(e => e.VTripletas)
                .HasPrecision(19, 4);

            modelBuilder.Entity<VFinanzaBancaConsolida>()
                .Property(e => e.MPrimero)
                .HasPrecision(19, 4);

            modelBuilder.Entity<VFinanzaBancaConsolida>()
                .Property(e => e.MSegundo)
                .HasPrecision(19, 4);

            modelBuilder.Entity<VFinanzaBancaConsolida>()
                .Property(e => e.MTercero)
                .HasPrecision(19, 4);

            modelBuilder.Entity<VFinanzaBancaConsolida>()
                .Property(e => e.MPales)
                .HasPrecision(19, 4);

            modelBuilder.Entity<VFinanzaBancaConsolida>()
                .Property(e => e.MTripletas)
                .HasPrecision(19, 4);

            modelBuilder.Entity<VFinanzaBancaDia>()
                .Property(e => e.GruNombre)
                .IsUnicode(false);

            modelBuilder.Entity<VFinanzaBancaDia>()
                .Property(e => e.RifNombre)
                .IsUnicode(false);

            modelBuilder.Entity<VFinanzaBancaDia>()
                .Property(e => e.BanNombre)
                .IsUnicode(false);

            modelBuilder.Entity<VFinanzaBancaDia>()
                .Property(e => e.BanContacto)
                .IsUnicode(false);

            modelBuilder.Entity<VFinanzaBancaDia>()
                .Property(e => e.LotNombre)
                .IsUnicode(false);

            modelBuilder.Entity<VFinanzaBancaDia>()
                .Property(e => e.Primero)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<VFinanzaBancaDia>()
                .Property(e => e.Segundo)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<VFinanzaBancaDia>()
                .Property(e => e.Tercero)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<VFinanzaBancaDia>()
                .Property(e => e.VQuinielas)
                .HasPrecision(19, 4);

            modelBuilder.Entity<VFinanzaBancaDia>()
                .Property(e => e.VPales)
                .HasPrecision(19, 4);

            modelBuilder.Entity<VFinanzaBancaDia>()
                .Property(e => e.VTripletas)
                .HasPrecision(19, 4);

            modelBuilder.Entity<VFinanzaBancaDia>()
                .Property(e => e.MPrimero)
                .HasPrecision(19, 4);

            modelBuilder.Entity<VFinanzaBancaDia>()
                .Property(e => e.MSegundo)
                .HasPrecision(19, 4);

            modelBuilder.Entity<VFinanzaBancaDia>()
                .Property(e => e.MTercero)
                .HasPrecision(19, 4);

            modelBuilder.Entity<VFinanzaBancaDia>()
                .Property(e => e.MPales)
                .HasPrecision(19, 4);

            modelBuilder.Entity<VFinanzaBancaDia>()
                .Property(e => e.MTripletas)
                .HasPrecision(19, 4);

            modelBuilder.Entity<VFinanzaBancaDiaTotal>()
                .Property(e => e.GruNombre)
                .IsUnicode(false);

            modelBuilder.Entity<VFinanzaBancaDiaTotal>()
                .Property(e => e.RifNombre)
                .IsUnicode(false);

            modelBuilder.Entity<VFinanzaBancaDiaTotal>()
                .Property(e => e.BanNombre)
                .IsUnicode(false);

            modelBuilder.Entity<VFinanzaBancaDiaTotal>()
                .Property(e => e.BanContacto)
                .IsUnicode(false);

            modelBuilder.Entity<VFinanzaBancaDiaTotal>()
                .Property(e => e.LotNombre)
                .IsUnicode(false);

            modelBuilder.Entity<VFinanzaBancaDiaTotal>()
                .Property(e => e.Primero)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<VFinanzaBancaDiaTotal>()
                .Property(e => e.Segundo)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<VFinanzaBancaDiaTotal>()
                .Property(e => e.Tercero)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<VFinanzaBancaDiaTotal>()
                .Property(e => e.VQuinielas)
                .HasPrecision(19, 4);

            modelBuilder.Entity<VFinanzaBancaDiaTotal>()
                .Property(e => e.VPales)
                .HasPrecision(19, 4);

            modelBuilder.Entity<VFinanzaBancaDiaTotal>()
                .Property(e => e.VTripletas)
                .HasPrecision(19, 4);

            modelBuilder.Entity<VFinanzaBancaDiaTotal>()
                .Property(e => e.MPrimero)
                .HasPrecision(19, 4);

            modelBuilder.Entity<VFinanzaBancaDiaTotal>()
                .Property(e => e.MSegundo)
                .HasPrecision(19, 4);

            modelBuilder.Entity<VFinanzaBancaDiaTotal>()
                .Property(e => e.MTercero)
                .HasPrecision(19, 4);

            modelBuilder.Entity<VFinanzaBancaDiaTotal>()
                .Property(e => e.MPales)
                .HasPrecision(19, 4);

            modelBuilder.Entity<VFinanzaBancaDiaTotal>()
                .Property(e => e.MTripletas)
                .HasPrecision(19, 4);

            modelBuilder.Entity<VFinanzaBancaFecha>()
                .Property(e => e.Dia)
                .IsUnicode(false);

            modelBuilder.Entity<VFinanzaBancaFecha>()
                .Property(e => e.GruNombre)
                .IsUnicode(false);

            modelBuilder.Entity<VFinanzaBancaFecha>()
                .Property(e => e.RifNombre)
                .IsUnicode(false);

            modelBuilder.Entity<VFinanzaBancaFecha>()
                .Property(e => e.BanNombre)
                .IsUnicode(false);

            modelBuilder.Entity<VFinanzaBancaFecha>()
                .Property(e => e.BanContacto)
                .IsUnicode(false);

            modelBuilder.Entity<VFinanzaBancaFecha>()
                .Property(e => e.LotNombre)
                .IsUnicode(false);

            modelBuilder.Entity<VFinanzaBancaFecha>()
                .Property(e => e.Primero)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<VFinanzaBancaFecha>()
                .Property(e => e.Segundo)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<VFinanzaBancaFecha>()
                .Property(e => e.Tercero)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<VFinanzaBancaFecha>()
                .Property(e => e.VQuinielas)
                .HasPrecision(19, 4);

            modelBuilder.Entity<VFinanzaBancaFecha>()
                .Property(e => e.VPales)
                .HasPrecision(19, 4);

            modelBuilder.Entity<VFinanzaBancaFecha>()
                .Property(e => e.VTripletas)
                .HasPrecision(19, 4);

            modelBuilder.Entity<VFinanzaBancaFecha>()
                .Property(e => e.MPrimero)
                .HasPrecision(19, 4);

            modelBuilder.Entity<VFinanzaBancaFecha>()
                .Property(e => e.MSegundo)
                .HasPrecision(19, 4);

            modelBuilder.Entity<VFinanzaBancaFecha>()
                .Property(e => e.MTercero)
                .HasPrecision(19, 4);

            modelBuilder.Entity<VFinanzaBancaFecha>()
                .Property(e => e.MPales)
                .HasPrecision(19, 4);

            modelBuilder.Entity<VFinanzaBancaFecha>()
                .Property(e => e.MTripletas)
                .HasPrecision(19, 4);

            modelBuilder.Entity<VFinanzaBancaTotalFecha>()
                .Property(e => e.Dia)
                .IsUnicode(false);

            modelBuilder.Entity<VFinanzaBancaTotalFecha>()
                .Property(e => e.GruNombre)
                .IsUnicode(false);

            modelBuilder.Entity<VFinanzaBancaTotalFecha>()
                .Property(e => e.RifNombre)
                .IsUnicode(false);

            modelBuilder.Entity<VFinanzaBancaTotalFecha>()
                .Property(e => e.BanNombre)
                .IsUnicode(false);

            modelBuilder.Entity<VFinanzaBancaTotalFecha>()
                .Property(e => e.BanContacto)
                .IsUnicode(false);

            modelBuilder.Entity<VFinanzaBancaTotalFecha>()
                .Property(e => e.LotNombre)
                .IsUnicode(false);

            modelBuilder.Entity<VFinanzaBancaTotalFecha>()
                .Property(e => e.Primero)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<VFinanzaBancaTotalFecha>()
                .Property(e => e.Segundo)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<VFinanzaBancaTotalFecha>()
                .Property(e => e.Tercero)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<VFinanzaBancaTotalFecha>()
                .Property(e => e.VQuinielas)
                .HasPrecision(19, 4);

            modelBuilder.Entity<VFinanzaBancaTotalFecha>()
                .Property(e => e.VPales)
                .HasPrecision(19, 4);

            modelBuilder.Entity<VFinanzaBancaTotalFecha>()
                .Property(e => e.VTripletas)
                .HasPrecision(19, 4);

            modelBuilder.Entity<VFinanzaBancaTotalFecha>()
                .Property(e => e.MPrimero)
                .HasPrecision(19, 4);

            modelBuilder.Entity<VFinanzaBancaTotalFecha>()
                .Property(e => e.MSegundo)
                .HasPrecision(19, 4);

            modelBuilder.Entity<VFinanzaBancaTotalFecha>()
                .Property(e => e.MTercero)
                .HasPrecision(19, 4);

            modelBuilder.Entity<VFinanzaBancaTotalFecha>()
                .Property(e => e.MPales)
                .HasPrecision(19, 4);

            modelBuilder.Entity<VFinanzaBancaTotalFecha>()
                .Property(e => e.MTripletas)
                .HasPrecision(19, 4);

            modelBuilder.Entity<VFinanzaDiaConsolida>()
                .Property(e => e.Dia)
                .IsUnicode(false);

            modelBuilder.Entity<VFinanzaDiaConsolida>()
                .Property(e => e.GruNombre)
                .IsUnicode(false);

            modelBuilder.Entity<VFinanzaDiaConsolida>()
                .Property(e => e.RifNombre)
                .IsUnicode(false);

            modelBuilder.Entity<VFinanzaDiaConsolida>()
                .Property(e => e.BanNombre)
                .IsUnicode(false);

            modelBuilder.Entity<VFinanzaDiaConsolida>()
                .Property(e => e.BanContacto)
                .IsUnicode(false);

            modelBuilder.Entity<VFinanzaDiaConsolida>()
                .Property(e => e.VTarjetas)
                .HasPrecision(38, 4);

            modelBuilder.Entity<VFinanzaDiaConsolida>()
                .Property(e => e.VQuinielas)
                .HasPrecision(19, 4);

            modelBuilder.Entity<VFinanzaDiaConsolida>()
                .Property(e => e.VPales)
                .HasPrecision(19, 4);

            modelBuilder.Entity<VFinanzaDiaConsolida>()
                .Property(e => e.VTripletas)
                .HasPrecision(19, 4);

            modelBuilder.Entity<VFinanzaDiaConsolida>()
                .Property(e => e.MPrimero)
                .HasPrecision(19, 4);

            modelBuilder.Entity<VFinanzaDiaConsolida>()
                .Property(e => e.MSegundo)
                .HasPrecision(19, 4);

            modelBuilder.Entity<VFinanzaDiaConsolida>()
                .Property(e => e.MTercero)
                .HasPrecision(19, 4);

            modelBuilder.Entity<VFinanzaDiaConsolida>()
                .Property(e => e.MPales)
                .HasPrecision(19, 4);

            modelBuilder.Entity<VFinanzaDiaConsolida>()
                .Property(e => e.MTripletas)
                .HasPrecision(19, 4);

            modelBuilder.Entity<VFinanzaHisConsolida>()
                .Property(e => e.Dia)
                .IsUnicode(false);

            modelBuilder.Entity<VFinanzaHisConsolida>()
                .Property(e => e.GruNombre)
                .IsUnicode(false);

            modelBuilder.Entity<VFinanzaHisConsolida>()
                .Property(e => e.RifNombre)
                .IsUnicode(false);

            modelBuilder.Entity<VFinanzaHisConsolida>()
                .Property(e => e.BanNombre)
                .IsUnicode(false);

            modelBuilder.Entity<VFinanzaHisConsolida>()
                .Property(e => e.BanContacto)
                .IsUnicode(false);

            modelBuilder.Entity<VFinanzaHisConsolida>()
                .Property(e => e.VTarjetas)
                .HasPrecision(19, 4);

            modelBuilder.Entity<VFinanzaHisConsolida>()
                .Property(e => e.VQuinielas)
                .HasPrecision(19, 4);

            modelBuilder.Entity<VFinanzaHisConsolida>()
                .Property(e => e.VPales)
                .HasPrecision(19, 4);

            modelBuilder.Entity<VFinanzaHisConsolida>()
                .Property(e => e.VTripletas)
                .HasPrecision(19, 4);

            modelBuilder.Entity<VFinanzaHisConsolida>()
                .Property(e => e.MPrimero)
                .HasPrecision(19, 4);

            modelBuilder.Entity<VFinanzaHisConsolida>()
                .Property(e => e.MSegundo)
                .HasPrecision(19, 4);

            modelBuilder.Entity<VFinanzaHisConsolida>()
                .Property(e => e.MTercero)
                .HasPrecision(19, 4);

            modelBuilder.Entity<VFinanzaHisConsolida>()
                .Property(e => e.MPales)
                .HasPrecision(19, 4);

            modelBuilder.Entity<VFinanzaHisConsolida>()
                .Property(e => e.MTripletas)
                .HasPrecision(19, 4);

            modelBuilder.Entity<VFinanzasPagado>()
                .Property(e => e.Dia)
                .IsUnicode(false);

            modelBuilder.Entity<VFinanzasPagado>()
                .Property(e => e.GruNombre)
                .IsUnicode(false);

            modelBuilder.Entity<VFinanzasPagado>()
                .Property(e => e.RifNombre)
                .IsUnicode(false);

            modelBuilder.Entity<VFinanzasPagado>()
                .Property(e => e.BanNombre)
                .IsUnicode(false);

            modelBuilder.Entity<VFinanzasPagado>()
                .Property(e => e.BanContacto)
                .IsUnicode(false);

            modelBuilder.Entity<VFinanzasPagado>()
                .Property(e => e.LotNombre)
                .IsUnicode(false);

            modelBuilder.Entity<VFinanzasPagado>()
                .Property(e => e.Primero)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<VFinanzasPagado>()
                .Property(e => e.Segundo)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<VFinanzasPagado>()
                .Property(e => e.Tercero)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<VFinanzasPagado>()
                .Property(e => e.VQuinielas)
                .HasPrecision(19, 4);

            modelBuilder.Entity<VFinanzasPagado>()
                .Property(e => e.VPales)
                .HasPrecision(19, 4);

            modelBuilder.Entity<VFinanzasPagado>()
                .Property(e => e.VTripletas)
                .HasPrecision(19, 4);

            modelBuilder.Entity<VFinanzasPagado>()
                .Property(e => e.MPrimero)
                .HasPrecision(19, 4);

            modelBuilder.Entity<VFinanzasPagado>()
                .Property(e => e.MSegundo)
                .HasPrecision(19, 4);

            modelBuilder.Entity<VFinanzasPagado>()
                .Property(e => e.MTercero)
                .HasPrecision(19, 4);

            modelBuilder.Entity<VFinanzasPagado>()
                .Property(e => e.MPales)
                .HasPrecision(19, 4);

            modelBuilder.Entity<VFinanzasPagado>()
                .Property(e => e.MTripletas)
                .HasPrecision(19, 4);

            modelBuilder.Entity<VFinanzasPagado>()
                .Property(e => e.MPagado)
                .HasPrecision(19, 4);

            modelBuilder.Entity<VHEstatusDia>()
                .Property(e => e.LotNombre)
                .IsUnicode(false);

            modelBuilder.Entity<VHEstatusDia>()
                .Property(e => e.PremioQ1)
                .IsUnicode(false);

            modelBuilder.Entity<VHEstatusDia>()
                .Property(e => e.PremioQ2)
                .IsUnicode(false);

            modelBuilder.Entity<VHEstatusDia>()
                .Property(e => e.PremioQ3)
                .IsUnicode(false);

            modelBuilder.Entity<VHEstatusDia>()
                .Property(e => e.Estado)
                .IsUnicode(false);

            modelBuilder.Entity<VHEstatusDia>()
                .Property(e => e.CantidadPremios)
                .IsUnicode(false);

            modelBuilder.Entity<VHisConsolida>()
                .Property(e => e.VTarjetas)
                .HasPrecision(19, 4);

            modelBuilder.Entity<VHisConsolida>()
                .Property(e => e.VQuinielas)
                .HasPrecision(19, 4);

            modelBuilder.Entity<VHisConsolida>()
                .Property(e => e.VPales)
                .HasPrecision(19, 4);

            modelBuilder.Entity<VHisConsolida>()
                .Property(e => e.VTripletas)
                .HasPrecision(19, 4);

            modelBuilder.Entity<VHisConsolida>()
                .Property(e => e.MPrimero)
                .HasPrecision(19, 4);

            modelBuilder.Entity<VHisConsolida>()
                .Property(e => e.MSegundo)
                .HasPrecision(19, 4);

            modelBuilder.Entity<VHisConsolida>()
                .Property(e => e.MTercero)
                .HasPrecision(19, 4);

            modelBuilder.Entity<VHisConsolida>()
                .Property(e => e.MPales)
                .HasPrecision(19, 4);

            modelBuilder.Entity<VHisConsolida>()
                .Property(e => e.MTripletas)
                .HasPrecision(19, 4);

            modelBuilder.Entity<VHisConsolida>()
                .Property(e => e.ISRRetenido)
                .HasPrecision(19, 4);

            modelBuilder.Entity<VHisConsolida2>()
                .Property(e => e.VTarjetas)
                .HasPrecision(19, 4);

            modelBuilder.Entity<VHisConsolida2>()
                .Property(e => e.VQuinielas)
                .HasPrecision(19, 4);

            modelBuilder.Entity<VHisConsolida2>()
                .Property(e => e.VPales)
                .HasPrecision(19, 4);

            modelBuilder.Entity<VHisConsolida2>()
                .Property(e => e.VTripletas)
                .HasPrecision(19, 4);

            modelBuilder.Entity<VHisConsolida2>()
                .Property(e => e.MPrimero)
                .HasPrecision(19, 4);

            modelBuilder.Entity<VHisConsolida2>()
                .Property(e => e.MSegundo)
                .HasPrecision(19, 4);

            modelBuilder.Entity<VHisConsolida2>()
                .Property(e => e.MTercero)
                .HasPrecision(19, 4);

            modelBuilder.Entity<VHisConsolida2>()
                .Property(e => e.MPales)
                .HasPrecision(19, 4);

            modelBuilder.Entity<VHisConsolida2>()
                .Property(e => e.MTripletas)
                .HasPrecision(19, 4);

            modelBuilder.Entity<VHisConsolida2>()
                .Property(e => e.ISRRetenido)
                .HasPrecision(19, 4);

            modelBuilder.Entity<VHisConsolida2>()
                .Property(e => e.PagoRemoto)
                .HasPrecision(19, 4);

            modelBuilder.Entity<VHisGrafico>()
                .Property(e => e.BanContacto)
                .IsUnicode(false);

            modelBuilder.Entity<VHisGrafico>()
                .Property(e => e.RifNombre)
                .IsUnicode(false);

            modelBuilder.Entity<VHisGrafico>()
                .Property(e => e.LotNombre)
                .IsUnicode(false);

            modelBuilder.Entity<VHisGrafico>()
                .Property(e => e.Ventas)
                .HasPrecision(19, 4);

            modelBuilder.Entity<VHisGrafico>()
                .Property(e => e.Saco)
                .HasPrecision(19, 4);

            modelBuilder.Entity<VTarjetaFecBan>()
                .Property(e => e.Dia)
                .IsUnicode(false);

            modelBuilder.Entity<VTarjetaFecBan>()
                .Property(e => e.GruNombre)
                .IsUnicode(false);

            modelBuilder.Entity<VTarjetaFecBan>()
                .Property(e => e.RifNombre)
                .IsUnicode(false);

            modelBuilder.Entity<VTarjetaFecBan>()
                .Property(e => e.BanNombre)
                .IsUnicode(false);

            modelBuilder.Entity<VTarjetaFecBan>()
                .Property(e => e.BanContacto)
                .IsUnicode(false);

            modelBuilder.Entity<VTarjetaFecBan>()
                .Property(e => e.VTarjetas)
                .HasPrecision(19, 4);

            modelBuilder.Entity<VTarjetaPorCuentaID>()
                .Property(e => e.SupNombre)
                .IsUnicode(false);

            modelBuilder.Entity<VTarjetaPorCuentaID>()
                .Property(e => e.VTarjetas)
                .HasPrecision(19, 4);

            modelBuilder.Entity<VTarjetaPorCuentaID>()
                .Property(e => e.RifNombre)
                .IsUnicode(false);

            modelBuilder.Entity<VTicketsDia>()
                .Property(e => e.TicNumero)
                .IsUnicode(false);

            modelBuilder.Entity<VTicketsDia>()
                .Property(e => e.TicCosto)
                .HasPrecision(19, 4);

            modelBuilder.Entity<VTicketsDia>()
                .Property(e => e.MPales)
                .HasPrecision(19, 4);

            modelBuilder.Entity<VTicketsDia>()
                .Property(e => e.Saco)
                .HasPrecision(19, 4);

            modelBuilder.Entity<VTicketsDia>()
                .Property(e => e.BanContacto)
                .IsUnicode(false);

            modelBuilder.Entity<VTicketsDia>()
                .Property(e => e.LotNombre)
                .IsUnicode(false);

            modelBuilder.Entity<VTicketsDia>()
                .Property(e => e.GruNombre)
                .IsUnicode(false);

            modelBuilder.Entity<VTicketsDia>()
                .Property(e => e.PremioQ1)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<VTicketsDia>()
                .Property(e => e.PremioQ2)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<VTicketsDia>()
                .Property(e => e.PremioQ3)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<VTicketsDiaGanadore>()
                .Property(e => e.TicNumero)
                .IsUnicode(false);

            modelBuilder.Entity<VTicketsDiaGanadore>()
                .Property(e => e.TicCosto)
                .HasPrecision(19, 4);

            modelBuilder.Entity<VTicketsDiaGanadore>()
                .Property(e => e.MPales)
                .HasPrecision(19, 4);

            modelBuilder.Entity<VTicketsDiaGanadore>()
                .Property(e => e.Saco)
                .HasPrecision(19, 4);

            modelBuilder.Entity<VTicketsDiaGanadore>()
                .Property(e => e.BanContacto)
                .IsUnicode(false);

            modelBuilder.Entity<VTicketsDiaGanadore>()
                .Property(e => e.LotNombre)
                .IsUnicode(false);

            modelBuilder.Entity<VTicketsDiaGanadore>()
                .Property(e => e.GruNombre)
                .IsUnicode(false);

            modelBuilder.Entity<VTicketsDiaGanadore>()
                .Property(e => e.PremioQ1)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<VTicketsDiaGanadore>()
                .Property(e => e.PremioQ2)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<VTicketsDiaGanadore>()
                .Property(e => e.PremioQ3)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<VTicketsGanadore>()
                .Property(e => e.TicNumero)
                .IsUnicode(false);

            modelBuilder.Entity<VTicketsGanadore>()
                .Property(e => e.TicCosto)
                .HasPrecision(19, 4);

            modelBuilder.Entity<VTicketsGanadore>()
                .Property(e => e.MPales)
                .HasPrecision(19, 4);

            modelBuilder.Entity<VTicketsGanadore>()
                .Property(e => e.Saco)
                .HasPrecision(19, 4);

            modelBuilder.Entity<VTicketsGanadore>()
                .Property(e => e.BanContacto)
                .IsUnicode(false);

            modelBuilder.Entity<VTicketsGanadore>()
                .Property(e => e.LotNombre)
                .IsUnicode(false);

            modelBuilder.Entity<VTicketsGanadore>()
                .Property(e => e.GruNombre)
                .IsUnicode(false);

            modelBuilder.Entity<VTicketsGanadore>()
                .Property(e => e.PremioQ1)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<VTicketsGanadore>()
                .Property(e => e.PremioQ2)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<VTicketsGanadore>()
                .Property(e => e.PremioQ3)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<VTicketsHi>()
                .Property(e => e.TicNumero)
                .IsUnicode(false);

            modelBuilder.Entity<VTicketsHi>()
                .Property(e => e.TicCosto)
                .HasPrecision(19, 4);

            modelBuilder.Entity<VTicketsHi>()
                .Property(e => e.MPales)
                .HasPrecision(19, 4);

            modelBuilder.Entity<VTicketsHi>()
                .Property(e => e.Saco)
                .HasPrecision(19, 4);

            modelBuilder.Entity<VTicketsHi>()
                .Property(e => e.BanContacto)
                .IsUnicode(false);

            modelBuilder.Entity<VTicketsHi>()
                .Property(e => e.LotNombre)
                .IsUnicode(false);

            modelBuilder.Entity<VTicketsHi>()
                .Property(e => e.GruNombre)
                .IsUnicode(false);

            modelBuilder.Entity<VTicketsHi>()
                .Property(e => e.PremioQ1)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<VTicketsHi>()
                .Property(e => e.PremioQ2)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<VTicketsHi>()
                .Property(e => e.PremioQ3)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<VTicketsHisGanadore>()
                .Property(e => e.TicNumero)
                .IsUnicode(false);

            modelBuilder.Entity<VTicketsHisGanadore>()
                .Property(e => e.TicCosto)
                .HasPrecision(19, 4);

            modelBuilder.Entity<VTicketsHisGanadore>()
                .Property(e => e.MPales)
                .HasPrecision(19, 4);

            modelBuilder.Entity<VTicketsHisGanadore>()
                .Property(e => e.Saco)
                .HasPrecision(19, 4);

            modelBuilder.Entity<VTicketsHisGanadore>()
                .Property(e => e.BanContacto)
                .IsUnicode(false);

            modelBuilder.Entity<VTicketsHisGanadore>()
                .Property(e => e.LotNombre)
                .IsUnicode(false);

            modelBuilder.Entity<VTicketsHisGanadore>()
                .Property(e => e.GruNombre)
                .IsUnicode(false);

            modelBuilder.Entity<VTicketsHisGanadore>()
                .Property(e => e.PremioQ1)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<VTicketsHisGanadore>()
                .Property(e => e.PremioQ2)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<VTicketsHisGanadore>()
                .Property(e => e.PremioQ3)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<VTicketsNoPagado>()
                .Property(e => e.TicNumero)
                .IsUnicode(false);

            modelBuilder.Entity<VTicketsNoPagado>()
                .Property(e => e.TicCosto)
                .HasPrecision(19, 4);

            modelBuilder.Entity<VTicketsNoPagado>()
                .Property(e => e.MPales)
                .HasPrecision(19, 4);

            modelBuilder.Entity<VTicketsNoPagado>()
                .Property(e => e.Saco)
                .HasPrecision(19, 4);

            modelBuilder.Entity<VTicketsNoPagado>()
                .Property(e => e.BanContacto)
                .IsUnicode(false);

            modelBuilder.Entity<VTicketsNoPagado>()
                .Property(e => e.LotNombre)
                .IsUnicode(false);

            modelBuilder.Entity<VTicketsNoPagado>()
                .Property(e => e.GruNombre)
                .IsUnicode(false);

            modelBuilder.Entity<VTicketsNoPagado>()
                .Property(e => e.PremioQ1)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<VTicketsNoPagado>()
                .Property(e => e.PremioQ2)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<VTicketsNoPagado>()
                .Property(e => e.PremioQ3)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<VTicketsNulo>()
                .Property(e => e.TicNumero)
                .IsUnicode(false);

            modelBuilder.Entity<VTicketsNulo>()
                .Property(e => e.TicCosto)
                .HasPrecision(19, 4);

            modelBuilder.Entity<VTicketsNulo>()
                .Property(e => e.MPales)
                .HasPrecision(19, 4);

            modelBuilder.Entity<VTicketsNulo>()
                .Property(e => e.Saco)
                .HasPrecision(19, 4);

            modelBuilder.Entity<VTicketsNulo>()
                .Property(e => e.BanContacto)
                .IsUnicode(false);

            modelBuilder.Entity<VTicketsNulo>()
                .Property(e => e.LotNombre)
                .IsUnicode(false);

            modelBuilder.Entity<VTicketsNulo>()
                .Property(e => e.GruNombre)
                .IsUnicode(false);

            modelBuilder.Entity<VTicketsNulo>()
                .Property(e => e.PremioQ1)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<VTicketsNulo>()
                .Property(e => e.PremioQ2)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<VTicketsNulo>()
                .Property(e => e.PremioQ3)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<VTicketsPagado>()
                .Property(e => e.TicNumero)
                .IsUnicode(false);

            modelBuilder.Entity<VTicketsPagado>()
                .Property(e => e.TicCosto)
                .HasPrecision(19, 4);

            modelBuilder.Entity<VTicketsPagado>()
                .Property(e => e.MPales)
                .HasPrecision(19, 4);

            modelBuilder.Entity<VTicketsPagado>()
                .Property(e => e.Saco)
                .HasPrecision(19, 4);

            modelBuilder.Entity<VTicketsPagado>()
                .Property(e => e.BanContacto)
                .IsUnicode(false);

            modelBuilder.Entity<VTicketsPagado>()
                .Property(e => e.LotNombre)
                .IsUnicode(false);

            modelBuilder.Entity<VTicketsPagado>()
                .Property(e => e.GruNombre)
                .IsUnicode(false);

            modelBuilder.Entity<VTicketsPagado>()
                .Property(e => e.PremioQ1)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<VTicketsPagado>()
                .Property(e => e.PremioQ2)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<VTicketsPagado>()
                .Property(e => e.PremioQ3)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<VTicketsValido>()
                .Property(e => e.TicNumero)
                .IsUnicode(false);

            modelBuilder.Entity<VTicketsValido>()
                .Property(e => e.TicCosto)
                .HasPrecision(19, 4);

            modelBuilder.Entity<VTicketsValido>()
                .Property(e => e.MPales)
                .HasPrecision(19, 4);

            modelBuilder.Entity<VTicketsValido>()
                .Property(e => e.Saco)
                .HasPrecision(19, 4);

            modelBuilder.Entity<VTicketsValido>()
                .Property(e => e.BanContacto)
                .IsUnicode(false);

            modelBuilder.Entity<VTicketsValido>()
                .Property(e => e.LotNombre)
                .IsUnicode(false);

            modelBuilder.Entity<VTicketsValido>()
                .Property(e => e.GruNombre)
                .IsUnicode(false);

            modelBuilder.Entity<VTicketsValido>()
                .Property(e => e.PremioQ1)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<VTicketsValido>()
                .Property(e => e.PremioQ2)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<VTicketsValido>()
                .Property(e => e.PremioQ3)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<VTransacionFecBan>()
                .Property(e => e.Dia)
                .IsUnicode(false);

            modelBuilder.Entity<VTransacionFecBan>()
                .Property(e => e.GruNombre)
                .IsUnicode(false);

            modelBuilder.Entity<VTransacionFecBan>()
                .Property(e => e.RifNombre)
                .IsUnicode(false);

            modelBuilder.Entity<VTransacionFecBan>()
                .Property(e => e.BanNombre)
                .IsUnicode(false);

            modelBuilder.Entity<VTransacionFecBan>()
                .Property(e => e.BanContacto)
                .IsUnicode(false);

            modelBuilder.Entity<VTransacionFecBan>()
                .Property(e => e.VTarjetas)
                .HasPrecision(19, 4);

            modelBuilder.Entity<VTransacionFecBan>()
                .Property(e => e.CTarjetas)
                .HasPrecision(19, 4);

            modelBuilder.Entity<VTransacionFecBan>()
                .Property(e => e.VTarjComision)
                .HasPrecision(19, 4);

            modelBuilder.Entity<VTransacionFecBan>()
                .Property(e => e.Referencia)
                .IsUnicode(false);

            modelBuilder.Entity<VTransacionFecBan>()
                .Property(e => e.ProdComisionVenta)
                .HasPrecision(19, 4);

            modelBuilder.Entity<VTransacionFecBan>()
                .Property(e => e.ProdComisionPago)
                .HasPrecision(19, 4);

            modelBuilder.Entity<VVentaLocal>()
                .Property(e => e.Primero)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<VVentaLocal>()
                .Property(e => e.Segundo)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<VVentaLocal>()
                .Property(e => e.Tercero)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<VVentaLocal>()
                .Property(e => e.VQuinielas)
                .HasPrecision(19, 4);

            modelBuilder.Entity<VVentaLocal>()
                .Property(e => e.VPales)
                .HasPrecision(19, 4);

            modelBuilder.Entity<VVentaLocal>()
                .Property(e => e.VTripletas)
                .HasPrecision(19, 4);

            modelBuilder.Entity<VVentaLocal>()
                .Property(e => e.MPrimero)
                .HasPrecision(19, 4);

            modelBuilder.Entity<VVentaLocal>()
                .Property(e => e.MSegundo)
                .HasPrecision(19, 4);

            modelBuilder.Entity<VVentaLocal>()
                .Property(e => e.MTercero)
                .HasPrecision(19, 4);

            modelBuilder.Entity<VVentaLocal>()
                .Property(e => e.MPales)
                .HasPrecision(19, 4);

            modelBuilder.Entity<VVentaLocal>()
                .Property(e => e.MTripletas)
                .HasPrecision(19, 4);

            modelBuilder.Entity<VVentaLocal>()
                .Property(e => e.TLoSolicitud)
                .HasPrecision(18, 0);
        }
    }
}
