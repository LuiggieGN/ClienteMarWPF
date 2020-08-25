using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using MAR.AppLogic.MARHelpers;
using MAR.DataAccess.Tables.DTOs;
using MAR.DataAccess.Tables.Enums;
using MAR.DataAccess.ViewModels;

namespace MAR.DataAccess.EFRepositories
{

    public class VpTransacciones
    {
        public static VP_Transaccion AgregarTransaccion(int pBancaId, int pSuplidorId, int pProductoId, int pCuentaId, decimal pMontoIngreso, decimal pMontoDescuento, string pNic, int pUsurarioId, ICollection<VP_TransaccionDetalle> vpTransaccionDetalles)
        {
            var db = new MARContext();

            var tran = new VP_Transaccion
            {
                Referencia = "0",
                ReferenciaCliente = pNic,
                ReciboID = 0,
                SolicitudID = 0,
                CuentaID = pCuentaId,
                ProductoID = pProductoId,
                SuplidorID = pSuplidorId,
                FechaIngreso = DateTime.Now,
                Descuentos = pMontoDescuento,
                Estado = "Solicitud",
                Ingresos = pMontoIngreso,
                Activo = false,
                BancaID = pBancaId,
                UsuarioID = pUsurarioId,
                VP_TransaccionDetalle = vpTransaccionDetalles
            };

            db.VP_Transaccion.Add(tran);
            db.SaveChanges();
            return tran;
        }

       

      

        public static VP_Transaccion ActualizaTransaccion(VP_Transaccion pTransaccion)
        {
            try
            {
                DbEntityEntry<VP_Transaccion> entry;

                var db = new MARContext();
                pTransaccion.FechaActualizacion = DateTime.Now;
                db.VP_Transaccion.Attach(pTransaccion);
                entry = db.Entry(pTransaccion);
                entry.State = EntityState.Modified;
                foreach (var name in entry.CurrentValues.PropertyNames)
                {
                    entry.Property(name).IsModified = true;
                }
                if (pTransaccion.VP_TransaccionDetalle != null)
                {
                    foreach (var navigationProperty in pTransaccion.VP_TransaccionDetalle)
                    {
                        var navEntry = db.Entry(navigationProperty);
                        navEntry.State = EntityState.Modified;
                        foreach (var name in navEntry.CurrentValues.PropertyNames)
                        {
                            navEntry.Property(name).IsModified = true;
                        }
                    }
                }
                db.SaveChanges();
                return pTransaccion;
            }
            catch (Exception)
            {
                throw;
            }
            
        }


        public static bool PuedeReimprimir(int pTransaccionId)
        {
            var db = new MARContext();
            var reimprime = (from t in db.VP_Transaccion
                join b in db.MBancas on t.BancaID equals b.BancaID
                             where t.TransaccionID == pTransaccionId && t.FechaIngreso < t.FechaIngreso.AddMinutes(6)
                select b.BanRePrintTicketID).FirstOrDefault();
            if (reimprime > 0)
            {
                return true;
            }
            return false;
        }

        public static bool AnulaVPTransaccion(int pTransaccionId, string pEstado)
        {
            try
            {
                string query = $@"UPDATE  VP_Transaccion   SET Activo = 0   ,Estado = @Estado  WHERE TransaccionID = @TrannsaccionId";

                using (var con = DALHelper.GetSqlConnection())
                {
                    var p = new DynamicParameters();
                    p.Add("@TrannsaccionId", pTransaccionId);
                    p.Add("@Estado", pEstado);
                    con.Query(query, p, commandType: CommandType.Text);
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
          
        }
        public static bool PremioYaPagado(string pReferencia, DbEnums.Productos pReferenciaCliente)
        {

            string query = "SELECT  * FROM VP_Transaccion WHERE Referencia = '" + pReferencia + "' and ReferenciaCliente ='" +  pReferenciaCliente + "'";

            using (var con = DALHelper.GetSqlConnection())
            {
                var p = new DynamicParameters();
                p.Add("@ReferenciaCliente", pReferenciaCliente);
                p.Add("@Referencia", pReferencia);
                var pago = con.Query(query, p, commandType: CommandType.Text).ToList();
                if (pago.Any())
                {
                    return true;
                }
                return false;
            }
        }

        public static IEnumerable<VP_Transaccion> GetTransacciones(Expression<Func<VP_Transaccion, bool>> filter = null, Func<IQueryable<VP_Transaccion>, IOrderedQueryable<VP_Transaccion>> orderBy = null,
        string includeProperties = "")
        {
            var db = new MARContext();
            IQueryable<VP_Transaccion> query = db.VP_Transaccion;

            if (filter != null)
            {
                query = query.Where(filter);
            }
            query = includeProperties.Split(new char[] {','}, StringSplitOptions.RemoveEmptyEntries).Aggregate(query, (current, includeProperty) => current.Include(includeProperty));
            if (orderBy != null)
            {
                return orderBy(query).ToList();
            }
            return query.ToList();
        }

        public static IEnumerable<VP_HTransaccion> GetHTransacciones(Expression<Func<VP_HTransaccion, bool>> filter = null, Func<IQueryable<VP_HTransaccion>, IOrderedQueryable<VP_HTransaccion>> orderBy = null, string includeProperties = "")
        {
            var db = new MARContext();
            IQueryable<VP_HTransaccion> query = db.VP_HTransaccion;

            if (filter != null)
            {
                query = query.Where(filter);
            }
            query = includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Aggregate(query, (current, includeProperty) => current.Include(includeProperty));
            if (orderBy != null)
            {
                return orderBy(query).ToList();
            }
            return query.ToList();
        }

      
        public static IEnumerable<VP_Transaccion> BuscarTransaccionPorFecha(DateTime pFechaDesde, DateTime pFechaHasta)
        {
            var db = new MARContext();
            var transaccion = db.VP_Transaccion.Where(x=> x.FechaIngreso >= pFechaDesde.Date && x.FechaIngreso <= pFechaHasta.Date).Include(x => x.VP_Suplidor);
            return transaccion;
        }

        public static ReportesViewModels.SumaVentasTransacciones GetTransaccionesSumaVentasFechasProducto(int pBancaId, DateTime pFechaDesde, DateTime pFechaHasta, Tables.Enums.DbEnums.Productos pProducto)
        {
            var values = new ReportesViewModels.SumaVentasTransacciones();
            var db = new MARContext();
            var transacciones = from t in db.VP_Transaccion
                                where t.Activo && t.FechaIngreso >= pFechaDesde.Date && t.FechaIngreso <= pFechaHasta.Date
                                join s in db.VP_Suplidor on t.SuplidorID equals s.SuplidorID
                                join p in db.VP_Producto on t.ProductoID equals p.ProductoID
                                where p.Nombre == pProducto.ToString() && t.BancaID == pBancaId
                                select new { t, s };
            var hTransacciones = from t in db.VP_HTransaccion
                                where t.Activo && t.FechaIngreso >= pFechaDesde.Date && t.FechaIngreso <= pFechaHasta.Date
                                join s in db.VP_Suplidor on t.SuplidorID equals s.SuplidorID
                                join p in db.VP_Producto on t.ProductoID equals p.ProductoID
                                where p.Nombre == pProducto.ToString() && t.BancaID == pBancaId
                                select new { t, s };

            values.Venta = 0;
            values.Comision = 0;
            if (transacciones.Any())
            {
                values.Venta = (double)transacciones.Sum(x => x.t.Ingresos);
                values.Comision = (double)transacciones.Select(s => s.s.Comision).FirstOrDefault() * values.Venta;  
            }
            if (hTransacciones.Any())
            {
                values.Venta += (double)hTransacciones.Sum(x => x.t.Ingresos);
                values.Comision += (double)hTransacciones.Select(s => s.s.Comision).FirstOrDefault() * values.Venta;
            }

            return values;
        }

 public static TransaccionBalance ObtenerBalanceFiltro(DateTime FechaInicial, DateTime FechaFinal, TransaccionFiltro t)
        {

            try
            {
                string query = @"
  

    select sum(cast(r.Ingresos as money)) as Ingreso, sum(cast(r.Descuentos as money)) as Descuento from (
                                select
                                s.Producto,
                                s.Referencia,
                                s.ReferenciaCliente,
                                s.Ingresos    as Ingresos,
                                s.Descuentos  as Descuentos,
                                s.Estado,
                                s.BancaID,
                                s.FechaIngreso,
                                s.Activo
                               from(
                        
                                 select 
                        	         p.Nombre as Producto , u.ProductoID, u.TransaccionID ,u.Referencia, u.ReferenciaCliente, u.Ingresos, u.Descuentos, u.Estado, u.BancaID, u.FechaIngreso, u.Activo
                                     from (
                             
                                      select h.ProductoID, h.TransaccionID ,h.Referencia, h.ReferenciaCliente, h.Ingresos, h.Descuentos, h.Estado, h.BancaID, h.FechaIngreso, h.Activo  from VP_HTransaccion h
                                       union
                                      select t.ProductoID, t.TransaccionID  ,t.Referencia, t.ReferenciaCliente, t.Ingresos, t.Descuentos, t.Estado, t.BancaID, t.FechaIngreso, t.Activo  from VP_Transaccion t
                             
                                   ) as u left join VP_Producto as p on u.ProductoID = p.ProductoID 
                        
                               ) AS s 
                              where ( CONVERT(date,s.FechaIngreso) >= Convert(date,@FechaInicial) and convert(date,s.FechaIngreso) <= convert(date,@FechaFinal )
							   
                                    and  ( @Producto is null or s.Producto LIKE '%'+@Producto+'%' )
                                    and ( @Estado is null or s.Estado LIKE '%'+@Estado+'%' )
                                    and ( @Referencia is null or  s.Referencia LIKE '%'+@Referencia+'%' )
                                    and ( @ReferenciaCliente is null or s.ReferenciaCliente  LIKE '%'+@ReferenciaCliente+'%' )
                               
									 and (
								 
								   (@BancaID = 0 )
								   
								   
								 or  
								      (@BancaID > 0 and s.BancaID = @BancaID)
								 ) 
								  and (
									
									( @Ingresos = 0)

									or

									 (@Ingresos > 0 and s.Ingresos = @Ingresos)
								)
								and (
								
								( @Descuentos = 0)

								or
								(@Descuentos > 0 and s.Descuentos = @Descuentos)
                                
                                )
                            
                        and(
								( @ProductoID = 0)

									or

									 (@ProductoID > 0 and s.ProductoID = @ProductoID)
									 )

                                    ) and s.Activo = 1
						 
						
						       ) as r";

                using (var con = DALHelper.GetSqlConnection())
                {
                    var p = new DynamicParameters();
                    p.Add("@FechaInicial", FechaInicial);
                    p.Add("@FechaFinal", FechaFinal);
                    p.Add("@Producto", t.producto);
                    p.Add("@Estado", t.Estado);
                    p.Add("@Referencia", t.Referencia);
                    p.Add("@ReferenciaCliente", t.ReferenciaCliente);
                    p.Add("@BancaID", t.BancaID);
                    p.Add("@Ingresos", t.Ingresos);
                    p.Add("@Descuentos", t.Descuentos);
                    p.Add("@fechaIngreso", t.FechaIngreso);
                    p.Add("@ProductoID", t.ProductoID);
                    var pago = con.Query<TransaccionBalance>(query, p, commandType: CommandType.Text).First();

                    return pago;

                }
            }
            catch (Exception ex)
            {

                throw ex;
            }




        }
        public class TransaccionBalance
        {
            public double Ingreso { get; set; }
            public double Descuento { get; set; }
        }

        public class TransaccionFiltro
        {
            public string producto { get; set; }
            public string Referencia { get; set; }
            public string ReferenciaCliente { get; set; }
            public string Estado { get; set; }
            public int BancaID { get; set; }
            public double Ingresos { get; set; }
            public double Descuentos { get; set; }
            public string FechaIngreso { get; set; }
            public int ProductoID { get; set; }

        }

        public static VP_Transaccion BuscarTransaccionPorId(int pTransaccionId)
        {
            var db = new MARContext();
            var transaccion = db.VP_Transaccion.FirstOrDefault(x => x.TransaccionID == pTransaccionId);
            return transaccion;
        }

        //El siguiente codigo garantiza que la libreria del provider the SQLClient para Entity Framework sea copiado cuando se haga Publish
        //Referencia: http://stackoverflow.com/questions/21175713/no-entity-framework-provider-found-for-the-ado-net-provider-with-invariant-name
        private volatile Type _sqlProviderDependency;
        public VpTransacciones()
        {
            _sqlProviderDependency = typeof(System.Data.Entity.SqlServer.SqlProviderServices);
        }


    }
}
