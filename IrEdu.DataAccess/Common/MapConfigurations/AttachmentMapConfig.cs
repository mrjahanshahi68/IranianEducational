using IrEdu.Common.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IrEdu.DataAccess.Common.MapConfigurations
{
	public class AttachmentMapConfig : LoggableEntityMapConfig<Attachment>
	{
		public AttachmentMapConfig()
		{
			Property(e => e.HashKey);
			Property(e => e.ObjectType);
			Property(e => e.ObjectId);
			Property(e => e.FileName);
			Property(e => e.FileSize);
			Property(e => e.FileUnit);
			Property(e => e.ContentType);

			Ignore(e => e.IsDeleted);
			//Property(e => e.IsDeleted);

			ToTable("Attachments");
		}
	}
}

