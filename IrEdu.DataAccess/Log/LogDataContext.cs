using IrEdu.DataAccess.Log.MapConfigurations;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static IrEdu.Common.AppConstants;

namespace IrEdu.DataAccess.Log
{
    public class LogDataContext : DataContext
    {
        public LogDataContext() : base("RealEstateConnectionString") { }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema(DatabaseSchema.Log);

            modelBuilder.Configurations.Add(new ExceptionMapConfig());

            base.OnModelCreating(modelBuilder);
        }
    }
}
