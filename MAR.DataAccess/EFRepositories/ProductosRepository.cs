using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using MAR.AppLogic.MARHelpers;
using MAR.DataAccess.Tables.DTOs;
using static MAR.DataAccess.EFRepositories.RFRepositories.RFSorteoRepository;

namespace MAR.DataAccess.EFRepositories
{

	public class ProductosRepository
	{
		public static ProductosDisponibles GetProductosDisponibles(int pBancaId)
		{
			var db = new Tables.DTOs.MARContext();
			var productos = (from p in db.MBancas
							 where p.BancaID == pBancaId
							 select new ProductosDisponibles
							 {
								 Recargas = p.BanTarjeta,
							 }).FirstOrDefault();
			return productos;
		}
		public static IEnumerable<VP_Producto> GetVpProductos()
		{
			IEnumerable<VP_Producto> p = new List<VP_Producto>();

			using (var con = DALHelper.GetSqlConnection())
			{
				string query = "select * from VP_Producto";

				p = con.Query<VP_Producto>(query).ToList();

			}

			return p;
		}
		public static IEnumerable<VP_Producto> GetVpProductos(Expression<Func<VP_Producto, bool>> filter = null, Func<IQueryable<VP_Producto>, IOrderedQueryable<VP_Producto>> orderBy = null,
	   string includeProperties = "")
		{
			var db = new MARContext();
			IQueryable<VP_Producto> query = db.VP_Producto;

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

		public static object GetVpProductosYCampos(List<Tables.Enums.DbEnums.VP_ProductoReferencia> pProdReferencias, List<Tables.Enums.DbEnums.ReciboCampoReferencia> pReciboReferencias)
		{
			List<string> productosList = new List<string>();
			for (int i = 0; i < pProdReferencias.Count(); i++)
			{
				productosList.Add(pProdReferencias[i].ToString());
			}

			List<string> reciboReferencias = new List<string>();
			for (int i = 0; i < pReciboReferencias.Count(); i++)
			{
				reciboReferencias.Add(pReciboReferencias[i].ToString());
			}


			string sqlQuery = $@"SELECT p.ProductoID, s.SuplidorID,  p.Nombre, p.Referencia, pc.ProductoCampoID, pc.Nombre, pc.ProductoID, pc.Nombre, pc.Referencia, rc.ReciboCampoID, rc.Nombre, rc.Referencia
									, C.CuentaID, s.SuplidorID, pcf.* FROM VP_Producto p   left  join VP_ProductoCampo pc on pc.ProductoID = p.ProductoID 
									 LEFT JOIN CL_ReciboCampo rc ON rc.Activo = 1  join VP_CuentaTipo ct on ct.CuentaTipoID = p.CuentaTipoID LEFT join VP_Cuenta c on c.CuentaTipoID = ct.CuentaTipoID left join 
									 VP_Suplidor S on s.CuentaID = c.CuentaID left  join VP_ProductoConfig pcf on pcf.ProductoID = p.ProductoID
									 where p.Referencia  IN('" + String.Join("','", productosList) + "') and rc.Referencia  IN('" + String.Join("','", reciboReferencias) + "') and p.Activo = 1";




			//List<SorteoViewModel> sortesolist = new List<SorteoViewModel>();
			//List<SorteoCampo> sorteosCampos = new List<SorteoCampo>();
			//List<ReciboCampo> reciboCampos = new List<ReciboCampo>();

			List<ProductoViewModel> productoslist = new List<ProductoViewModel>();
			List<VP_ProductoCampo> productosCampos = new List<VP_ProductoCampo>();
			List<Suplidor> suplidores = new List<Suplidor>();
			List<ReciboCampo> reciboCampos = new List<ReciboCampo>();
			List<VP_ProductoConfig> productoConfig = new List<VP_ProductoConfig>();
			using (var con = DALHelper.GetSqlConnection())
			{
				var productos = con.Query<ProductoViewModel, VP_ProductoCampo, ReciboCampo, VP_Cuenta, Suplidor, VP_ProductoConfig, ProductoViewModel>(
					 sqlQuery,
					 (sor, prodCam,  recCampo,cuenta, suplidor, prodConfig) =>
					 {
						 ProductoViewModel proEntry;
						 proEntry = sor;
						 proEntry.ProductoCampos = new List<VP_ProductoCampo>();
						 proEntry.ProductoConfig = new List<VP_ProductoConfig>();
						 proEntry.RecibosCampos = new List<ReciboCampo>();
						 proEntry.Suplidores = new List<Suplidor>();

						 if (prodCam != null)
						 {
							 productosCampos.Add(prodCam);
						 }
						 if (prodConfig != null)
						 {
							 productoConfig.Add(prodConfig);
						 }
						 if (recCampo != null)
						 {
							 reciboCampos.Add(recCampo);
						 }
						 if (suplidor != null)
						 {
							 suplidores.Add(suplidor);
						 }
						 proEntry.Cuenta = cuenta;
						 return proEntry;
					 },
					 splitOn: "ProductoID,ProductoCampoID,ReciboCampoID, CuentaID, SuplidorID, ProductoConfigID", commandType: CommandType.Text)
				 .Distinct().GroupBy(x => x.ProductoID).Select(x => x.FirstOrDefault());



				List<VP_ProductoCampo> productosCamposLimpios = productosCampos.Distinct().GroupBy(x => x.ProductoCampoID).Select(x => x.FirstOrDefault()).ToList();
				List<VP_ProductoConfig> productosConfigLimpios = productoConfig.Distinct().GroupBy(x => x.ProductoConfigID).Select(x => x.FirstOrDefault()).ToList();
				List<Suplidor> suplidoresLimpios = suplidores.Distinct().GroupBy(x => x.SuplidorID).Select(x => x.FirstOrDefault()).ToList();
				foreach (var item in productos)
				{
					foreach (var ca in productosCamposLimpios.Distinct())
					{
						if (ca.ProductoID == item.ProductoID)
						{
							item.ProductoCampos.Add(ca);
						}
					}
					foreach (var ca in productosConfigLimpios.Distinct())
					{
						if (ca.ProductoID == item.ProductoID)
						{
							item.ProductoConfig.Add(ca);
						}
					}
					foreach (var s in suplidoresLimpios.Distinct())
					{

						item.Suplidores.Add(s);
					}


				}
				var result = productos;//new { Productos = productos, ReciboCampos = reciboCampos.Distinct().GroupBy(x => x.ReciboCampoID).Select(x => x.FirstOrDefault()) };
				return result;
			}
		}


		public class ProductoViewModel
		{
			public int ProductoID { get; set; }
			public string Nombre { get; set; }
			public double Monto { get; set; }
			public string Referencia { get; set; }
			public VP_Cuenta Cuenta { get; set; }
			public int SuplidorID { get; set; }
			public List<VP_ProductoCampo> ProductoCampos { get; set; }
			public List<VP_ProductoConfig> ProductoConfig { get; set; }
			public List<Suplidor> Suplidores { get; set; }
			public List<ReciboCampo> RecibosCampos { get; set; }
		}
		public class Suplidor
		{


			public int SuplidorID { get; set; }

			public string Nombre { get; set; }

			public string Referencia { get; set; }

			public double Comision { get; set; }

			public double Impuesto { get; set; }

		}

		public static IEnumerable<VP_ProductoCampo> GetVpProductoCampos(Expression<Func<VP_ProductoCampo, bool>> filter = null, Func<IQueryable<VP_ProductoCampo>, IOrderedQueryable<VP_ProductoCampo>> orderBy = null,
			string includeProperties = "")
		{
			var db = new MARContext();
			IQueryable<VP_ProductoCampo> query = db.VP_ProductoCampo;

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

		public class ProductosDisponibles
		{
			public bool? Billetes { get; set; }
			public bool Recargas { get; set; }
			public bool? Servicios { get; set; }
		}

		//El siguiente codigo garantiza que la libreria del provider the SQLClient para Entity Framework sea copiado cuando se haga Publish
		//Referencia: http://stackoverflow.com/questions/21175713/no-entity-framework-provider-found-for-the-ado-net-provider-with-invariant-name
		private volatile Type _sqlProviderDependency;
		public ProductosRepository()
		{
			_sqlProviderDependency = typeof(System.Data.Entity.SqlServer.SqlProviderServices);
		}

	}


}
