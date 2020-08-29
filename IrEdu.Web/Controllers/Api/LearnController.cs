using IrEdu.Common;
using IrEdu.Common.Entities;
using IrEdu.Common.Entities.Common;
using IrEdu.Common.Entities.Learn;
using IrEdu.Common.Exceptions;
using IrEdu.DataAccess;
using IrEdu.Domain.Learn;
using IrEdu.Web.Infrastrcuture;
using IrEdu.Web.Models;
using IrEdu.Web.Models.Learn;
using IrEdu.Web.Security.Filters;
using QueryDesigner;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using static IrEdu.Common.AppConstants;
using static IrEdu.Common.AppEnums;

namespace IrEdu.Web.Controllers.Api
{
	[JwtAuthentication]
	public class LearnController : BaseApiController
	{
		private string PodcastFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, AppConfigurations.PodcastFolder);
		private string VideoFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, AppConfigurations.VideoFolder);
		#region Podcast
		[HttpPost]
		[Authorize]
		public async Task<HttpResponseMessage> SubmitPodcast()
		{

			DbContextTransaction transaction = null;
			try
			{
				var fileDataRequestParameters = HttpContext.Current.Request.GetDataFileRequestParameters<PodcastRequest, FileDataModel>();
				var parameters = fileDataRequestParameters.ViewModel;
				if (parameters == null)
					throw new ValidationModelException(MessageTemplate.ParameterIsNotDefined);
				var errors = new List<string>();
				if (string.IsNullOrWhiteSpace(parameters.Title))
					errors.Add(string.Format(MessageTemplate.Required, "عنوان"));

				if (errors.Any())
					throw new ValidationModelException(errors);

				using (var dbContext = new AppDataContext())
				{
					transaction = dbContext.Database.BeginTransaction();
					using (var uow = new UnitOfWork<AppDataContext>(dbContext))
					{
						var podcastRepo = uow.Repository<Podcast>();
						var attachmentRepo = uow.Repository<Attachment>();
						var podcastFile = fileDataRequestParameters.Files.Where(e => e.FileData.ExtraInfoString == "PodcastFile").FirstOrDefault();
						Podcast entity = null;
						string hashkey = string.Empty;
						string fileName = string.Empty;


						if (parameters.ID > 0)
						{
							var dbEntity = podcastRepo.Find(parameters.ID);
							if (dbEntity == null)
								throw new ValidationModelException(MessageTemplate.RecordNotFound);

							hashkey = dbEntity.HashKey;
							if (podcastFile == null)
								fileName = dbEntity.FileName;
							else
								fileName = podcastFile.HttpPostedFile?.FileName;

							entity = new Podcast
							{
								ID = dbEntity.ID,
								HashKey = hashkey,
								Title = parameters.Title,
								Description = parameters.Description,
								EducationalLevel=parameters.EducationalLevel,
								FileName = fileName,
								InsertDateTime = dbEntity.InsertDateTime,
								InsertUserId = dbEntity.InsertUserId,
								IsDeleted = parameters.IsDeleted,
								UpdateDateTime = DateTime.Now,
								UpdateUserId = CurrentUser.UserId,
							};

							podcastRepo.Update(entity);

							if (podcastFile != null)
							{
								var attachments = await attachmentRepo.Queryable().Where(e => e.ObjectType == ObjectType.Podcast && e.ObjectId == parameters.ID).ToListAsync();
								if (attachments.Any())
								{
									foreach (var item in attachments)
										attachmentRepo.Delete(item);

									var currentDirectory = Path.Combine(PodcastFolder, hashkey);
									if (Directory.Exists(currentDirectory))
										Directory.Delete(currentDirectory, true);
								}
							}
						}
						else
						{

							while (true)
							{
								hashkey = Guid.NewGuid().ToString();
								if (podcastRepo.Queryable().Where(e => e.HashKey == hashkey).Any()) continue;
								break;
							}

							fileName = podcastFile?.HttpPostedFile?.FileName;

							entity = new Podcast
							{
								HashKey = hashkey,
								Title = parameters.Title.Trim(),
								Description = parameters.Description,
								EducationalLevel=parameters.EducationalLevel,
								FileName = fileName,
								InsertDateTime = DateTime.Now,
								InsertUserId = CurrentUser.UserId,
								IsDeleted = parameters.IsDeleted,
							};

							podcastRepo.Insert(entity);
						}

						await uow.SaveChangeAsync();


						if (podcastFile != null && podcastFile.HttpPostedFile != null)
						{
							var file = podcastFile.HttpPostedFile;
							var attachmnet = new Attachment
							{
								HashKey = Guid.NewGuid().ToString(),
								ContentType = file.ContentType,
								FileName = file.FileName,
								FileSize = file.ContentLength / AppConstants.Kilobyte,
								ObjectType = ObjectType.Podcast,
								ObjectId = entity.ID,
								InsertUserId = CurrentUser.UserId,
								InsertDateTime = DateTime.Now,
							};

							attachmentRepo.Insert(attachmnet);

							await uow.SaveChangeAsync();

							var currentDirectory = Path.Combine(PodcastFolder, hashkey);
							if (!Directory.Exists(currentDirectory))
								Directory.CreateDirectory(currentDirectory);

							file.SaveAs($"{currentDirectory}/{file.FileName}");
						}
						transaction.Commit();

						var podcasts = await podcastRepo.Queryable().OrderByDescending(e => e.InsertDateTime).ToListAsync();
					}
					return Success();
				}

			}
			catch (Exception ex)
			{
				if (transaction != null) transaction.Rollback();
				return await HandleExceptionAsync(ex);
			}

		}
		[HttpPost]
		[Authorize]
		public async Task<HttpResponseMessage> DeletePodcast(PodcastRequest req)
		{

			DbContextTransaction transaction = null;
			List<PodCastResponse> result = null;
			try
			{
				if (req == null)
					throw new ValidationModelException(MessageTemplate.ParameterIsNotDefined);
				if (req.ID <= 0)
					throw new ValidationModelException(MessageTemplate.InvalidIdentity);

				using(var uow=new AppUnitOfWork())
				{
					uow.Repository<Podcast>().Delete(req.ID);
					await uow.SaveChangeAsync();
				}
				return Success();
			}
			catch (Exception ex)
			{
				if (transaction != null) transaction.Rollback();
				return await HandleExceptionAsync(ex);
			}

		}
		[HttpPost]
		[Authorize]
		public async Task<HttpResponseMessage> GetPodcasts(FilterContainer filter)
		{
			try
			{
				List<PodCastResponse> result = null;
				if (filter == null)
					throw new ValidationModelException(MessageTemplate.ParameterIsNotDefined);

				using (var uow = new AppUnitOfWork())
				{
					var rule = new PodcastBusinessRule(uow);
					var query = rule.Queryable();
					if (filter.OrderBy == null)
					{
						filter.OrderBy = new List<OrderFilter>
						{
							new OrderFilter
							{
								Field="ID",
							}
						};
					}

					query = query.Request(filter);
					result = (await query.ToListAsync())?.Select(e => new PodCastResponse
					{
						ID = e.ID,
						HashKey = e.HashKey,
						Title = e.Title,
						Description = e.Description,
						EducationalLevel=e.EducationalLevel,
						FileName = e.FileName,
						FilePath = $"{PodcastFolder}/{e.HashKey}/{e.FileName}",
					}).ToList();

					filter.Skip = 0;
					filter.Take = 0;
					var totalQuery = query.Request(filter);
					var totalCount = await totalQuery.CountAsync();

					return Success(new FilterQueryRsponse
					{
						TotalCount = totalCount,
						Records = result,
					});
				}
			}
			catch (Exception ex)
			{
				return await HandleExceptionAsync(ex);
			}
		}
		[HttpPost]
		[Authorize]
		public async Task<HttpResponseMessage> GetPodcast(PodcastRequest parameters)
		{
			try
			{
				PodCastResponse result = null;
				if (parameters == null)
					throw new ValidationModelException(MessageTemplate.ParameterIsNotDefined);

				using (var uow = new AppUnitOfWork())
				{
					var rule = new PodcastBusinessRule(uow);
					var entity = rule.FindEntity(parameters.ID);

					result = new PodCastResponse
					{
						ID = entity.ID,
						HashKey = entity.HashKey,
						Title = entity.Title,
						Description = entity.Description,
						EducationalLevel=entity.EducationalLevel,
						FileName = entity.FileName,
						FilePath = $"{PodcastFolder}/{entity.HashKey}/{entity.FileName}",
					};

					return Success(result);
				}
			}
			catch (Exception ex)
			{
				return await HandleExceptionAsync(ex);
			}
		}
		#endregion

		#region Video
		[HttpPost]
		[Authorize]
		public async Task<HttpResponseMessage> SubmitVideo()
		{

			DbContextTransaction transaction = null;
			try
			{
				var fileDataRequestParameters = HttpContext.Current.Request.GetDataFileRequestParameters<VideoRequest, FileDataModel>();
				var parameters = fileDataRequestParameters.ViewModel;
				if (parameters == null)
					throw new ValidationModelException(MessageTemplate.ParameterIsNotDefined);
				var errors = new List<string>();
				if (string.IsNullOrWhiteSpace(parameters.Title))
					errors.Add(string.Format(MessageTemplate.Required, "عنوان"));

				if (errors.Any())
					throw new ValidationModelException(errors);

				using (var dbContext = new AppDataContext())
				{
					transaction = dbContext.Database.BeginTransaction();
					using (var uow = new UnitOfWork<AppDataContext>(dbContext))
					{
						var videoRepo = uow.Repository<Video>();
						var attachmentRepo = uow.Repository<Attachment>();
						var videoFile = fileDataRequestParameters.Files.Where(e => e.FileData.ExtraInfoString == "VideoFile").FirstOrDefault();
						Video entity = null;
						string hashkey = string.Empty;
						string fileName = string.Empty;


						if (parameters.ID > 0)
						{
							var dbEntity = videoRepo.Find(parameters.ID);
							if (dbEntity == null)
								throw new ValidationModelException(MessageTemplate.RecordNotFound);

							hashkey = dbEntity.HashKey;
							if (videoFile == null)
								fileName = dbEntity.FileName;
							else
								fileName = videoFile.HttpPostedFile?.FileName;

							entity = new Video
							{
								ID = dbEntity.ID,
								HashKey = hashkey,
								Title = parameters.Title,
								Description = parameters.Description,
								Price=parameters.Price,
								EducationalLevel=parameters.EducationalLevel,
								FileName = fileName,
								InsertDateTime = dbEntity.InsertDateTime,
								InsertUserId = dbEntity.InsertUserId,
								IsDeleted = parameters.IsDeleted,
								UpdateDateTime = DateTime.Now,
								UpdateUserId = CurrentUser.UserId,
							};

							videoRepo.Update(entity);

							if (videoFile != null)
							{
								var attachments = await attachmentRepo.Queryable().Where(e => e.ObjectType == ObjectType.Video && e.ObjectId == parameters.ID).ToListAsync();
								if (attachments.Any())
								{
									foreach (var item in attachments)
										attachmentRepo.Delete(item);

									var currentDirectory = Path.Combine(VideoFolder, hashkey);
									if (Directory.Exists(currentDirectory))
										Directory.Delete(currentDirectory, true);
								}
							}
						}
						else
						{

							while (true)
							{
								hashkey = Guid.NewGuid().ToString();
								if (videoRepo.Queryable().Where(e => e.HashKey == hashkey).Any()) continue;
								break;
							}

							fileName = videoFile?.HttpPostedFile?.FileName;

							entity = new Video
							{
								HashKey = hashkey,
								Title = parameters.Title.Trim(),
								Description = parameters.Description,
								Price=parameters.Price,
								EducationalLevel=parameters.EducationalLevel,
								FileName = fileName,
								InsertDateTime = DateTime.Now,
								InsertUserId = CurrentUser.UserId,
								IsDeleted = parameters.IsDeleted,
							};

							videoRepo.Insert(entity);
						}

						await uow.SaveChangeAsync();


						if (videoFile != null && videoFile.HttpPostedFile != null)
						{
							var file = videoFile.HttpPostedFile;
							var attachmnet = new Attachment
							{
								HashKey = Guid.NewGuid().ToString(),
								ContentType = file.ContentType,
								FileName = file.FileName,
								FileSize = file.ContentLength / AppConstants.Kilobyte,
								ObjectType = ObjectType.Video,
								ObjectId = entity.ID,
								InsertUserId = CurrentUser.UserId,
								InsertDateTime = DateTime.Now,
							};

							attachmentRepo.Insert(attachmnet);

							await uow.SaveChangeAsync();

							var currentDirectory = Path.Combine(VideoFolder, hashkey);
							if (!Directory.Exists(currentDirectory))
								Directory.CreateDirectory(currentDirectory);

							file.SaveAs($"{currentDirectory}/{file.FileName}");
						}
						transaction.Commit();

						var podcasts = await videoRepo.Queryable().OrderByDescending(e => e.InsertDateTime).ToListAsync();
					}
					return Success();
				}

			}
			catch (Exception ex)
			{
				if (transaction != null) transaction.Rollback();
				return await HandleExceptionAsync(ex);
			}

		}
		[HttpPost]
		[Authorize]
		public async Task<HttpResponseMessage> DeleteVideo(VideoRequest req)
		{

			DbContextTransaction transaction = null;
			try
			{
				if (req == null)
					throw new ValidationModelException(MessageTemplate.ParameterIsNotDefined);
				if (req.ID <= 0)
					throw new ValidationModelException(MessageTemplate.InvalidIdentity);

				using (var uow = new AppUnitOfWork())
				{
					uow.Repository<Video>().Delete(req.ID);
					await uow.SaveChangeAsync();
				}
				return Success();
			}
			catch (Exception ex)
			{
				if (transaction != null) transaction.Rollback();
				return await HandleExceptionAsync(ex);
			}

		}
		[HttpPost]
		[Authorize]
		public async Task<HttpResponseMessage> GetVideos(FilterContainer filter)
		{
			try
			{
				List<VideoResponse> result = null;
				if (filter == null)
					throw new ValidationModelException(MessageTemplate.ParameterIsNotDefined);

				using (var uow = new AppUnitOfWork())
				{
					var rule = new VideoBusinessRule(uow);
					var query = rule.Queryable();
					if (filter.OrderBy == null)
					{
						filter.OrderBy = new List<OrderFilter>
						{
							new OrderFilter
							{
								Field="ID",
							}
						};
					}

					query = query.Request(filter);
					result = (await query.ToListAsync())?.Select(e => new VideoResponse
					{
						ID = e.ID,
						HashKey = e.HashKey,
						Title = e.Title,
						Price=e.Price,
						Description = e.Description,
						EducationalLevel=e.EducationalLevel,
						FileName = e.FileName,
						FilePath = $"{VideoFolder}/{e.HashKey}/{e.FileName}",
					}).ToList();

					filter.Skip = 0;
					filter.Take = 0;
					var totalQuery = query.Request(filter);
					var totalCount = await totalQuery.CountAsync();

					return Success(new FilterQueryRsponse
					{
						TotalCount = totalCount,
						Records = result,
					});
				}
			}
			catch (Exception ex)
			{
				return await HandleExceptionAsync(ex);
			}
		}
		[HttpPost]
		[Authorize]
		public async Task<HttpResponseMessage> GetVideo(VideoRequest parameters)
		{
			try
			{
				VideoResponse result = null;
				if (parameters == null)
					throw new ValidationModelException(MessageTemplate.ParameterIsNotDefined);

				using (var uow = new AppUnitOfWork())
				{
					var rule = new VideoBusinessRule(uow);
					var entity = rule.FindEntity(parameters.ID);

					result = new VideoResponse
					{
						ID = entity.ID,
						HashKey = entity.HashKey,
						Title = entity.Title,
						Price=entity.Price,
						Description = entity.Description,
						EducationalLevel=entity.EducationalLevel,
						FileName = entity.FileName,
						FilePath = $"{VideoFolder}/{entity.HashKey}/{entity.FileName}",
					};

					return Success(result);
				}
			}
			catch (Exception ex)
			{
				return await HandleExceptionAsync(ex);
			}
		}
		#endregion
	}
}
