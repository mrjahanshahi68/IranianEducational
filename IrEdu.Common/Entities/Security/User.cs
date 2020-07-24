﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static IrEdu.Common.AppEnums;

namespace IrEdu.Common.Entities.Security
{
	public class User : LoggableEntity, ILogicalDeletable
	{
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string NationalCode { get; set; }
		public string Mobile { get; set; }
		public string Email { get; set; }
		public string Address { get; set; }
		public string UserName { get; set; }
		public string Password { get; set; }
		public bool IsActive { get; set; }
		public bool IsDeleted { get; set; }
		public UserTypes UserType { get; set; }
		public DateTime RegisterDate { get; set; }
		public List<Role> Roles { get; set; }
	}
}
