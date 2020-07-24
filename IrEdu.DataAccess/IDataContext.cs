using IrEdu.Common.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IrEdu.DataAccess
{
	public interface IDataContext
	{
		Database Database { get; }
		DbSet<T> Set<T>() where T : class;
		DbEntityEntry<T> Entry<T>(T entity) where T : class;
		int SaveChanges();
		Task<int> SaveChangesAsync();
		void Dispose();

	}
}
