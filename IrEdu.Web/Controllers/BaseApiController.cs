using IrEdu.Web.Infrastrcuture;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Threading.Tasks;
using IrEdu.Common.Entities;
//using System.Security.Authentication;
using IrEdu.Domain;
using static IrEdu.Common.AppEnums;
using AutoMapper;
using System.Data.Entity;
using System.Linq;
using IrEdu.Common.Exceptions;
using static IrEdu.Common.AppConstants;
using IrEdu.Web.Cache;
using IrEdu.Web.Security;

namespace IrEdu.Web.Controllers
{
	public abstract class BaseApiController : ApiController
	{
		#region Properties
		protected Mapper Mapper { get; set; }
		public bool IsAuthenticated => SecurityManager.IsAuthenticated(Token);
		public string Token => SecurityManager.GetToken(Request);
		public AppUserInfo CurrentUser
		{
			get
			{
				if (IsAuthenticated)
				{
					var principle = SecurityManager.GetPrinciple(Token);
					var userName = principle.Identity?.Name;
					var userInfo = CacheManager.GetValue(userName) as AppUserInfo;
					return userInfo;
				}
				throw new AuthenticationException("Not Athenticate");

			}
		}
		#endregion


		

		#region Response
		protected virtual HttpResponseMessage CreateResponse(ResultCode resultCode, object data = null, List<string> messages = null)
		{
			return Request.CreateResponse(HttpStatusCode.OK, new ApiResult
			{
				ResultCode = resultCode,
				Data = data,
				Messages = messages,
			});
		}
		protected virtual HttpResponseMessage CreateResponse(ResultCode resultCode, object data, string message) => CreateResponse(resultCode, data, new List<string> { message });
		protected virtual HttpResponseMessage CreateResponse(ResultCode resultCode, string message) => CreateResponse(resultCode, null, new List<string> { message });
		protected virtual HttpResponseMessage Success(object data = null) => CreateResponse(ResultCode.Success, data);
		protected virtual Task<HttpResponseMessage> HandleExceptionAsync(Exception ex) => throw ex;
		#endregion
	}
}
