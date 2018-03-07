using System;
using System.Web.Mvc;
using SC2BM.DomainModel;
using SC2BM.ServiceModel.BusinessServices;
using SC2BM.ServiceModel.Requests;
using SC2BM.ServiceModel.Responses;
using SC2BM.WebSite.Classes;

namespace SC2BM.WebSite.Controllers
{
    public class BlogController : BaseController
    {
        private readonly IBlogService _service;

        public BlogController(IBlogService service)
        {
            _service = service;
        }

        [HttpGet]
        public JsonResponse GetBlogComments(int blogID)
        {
            var result = _service.GetBlogComments(blogID);
            return new JsonResponse(result);
        }

        [HttpGet]
        public JsonResponse GetTopRatedBlogs(int count)
        {
            var result = _service.GetTopRatedBlogs(count);
            return new JsonResponse(result);
        }

        [HttpGet]
        public JsonResponse GetRateForBlog(int blogID)
        {
            var result = _service.GetRateForBlog(blogID);
            return new JsonResponse(result);
        }

        [HttpGet]
        public JsonResponse GetByOwner(string ownerUserName)
        {
            var result = _service.GetBlogByOwner(ownerUserName);
            return new JsonResponse(result);
        }

        [HttpPost]
        public JsonResponse Search(PagedRequest<SearchBlogsFilter> request)
        {
            var result = _service.Search(request);
            return new JsonResponse(result);
        }

        [CustomBasicAuthorize]
        [HttpPost]
        public JsonResponse AddOrUpdate(Blog item)
        {
            if (User == null || (!User.UserData.Roles.Contains("Admin") && User.UserData.ID != item.OwnerUserID))
            {
                return new JsonResponse(false, "Unauthorized updating is not allowed!");
            }

            GeneralResponse result;

            if (string.IsNullOrEmpty(item.LogoPath))
            {
                item.LogoPath = "/images/patterns/pattern-1.png";
            }

            if (item.ID.HasValue)
            {
                var initialBlog = _service.GetBlogByOwner(item.OwnerUserName);
                if (initialBlog.Result == null)
                {
                    return new JsonResponse(false, "Blog which user try to update does not exists");
                }

                result = _service.Update(item);

                if (result.Success)
                {
                    if (initialBlog.Result.LogoPath != item.LogoPath)
                    {
                        // Delete previous image
                        if (item.LogoPath != "/images/patterns/pattern-1.png" && (System.IO.File.Exists(Server.MapPath(initialBlog.Result.LogoPath))))
                        {
                            System.IO.File.Delete(Server.MapPath(initialBlog.Result.LogoPath));
                        }

                        // Move file from tmp folder into main upload folder
                        if (item.LogoPath.Contains("/images/upload/tmp") && System.IO.File.Exists(Server.MapPath(item.LogoPath)))
                        {
                            var normalPath = item.LogoPath.Replace("/tmp", "");

                            System.IO.File.Move(Server.MapPath(item.LogoPath), Server.MapPath(normalPath));
                            item.LogoPath = normalPath;
                            _service.Update(item);
                        }
                    }
                }
            }
            else
            {
                item.AddedDate = DateTime.Now;

                result = _service.Register(item);

                if (result.Success)
                {
                    // Move file from tmp folder into main upload folder
                    if (item.LogoPath.Contains("/images/upload/tmp") && System.IO.File.Exists(Server.MapPath(item.LogoPath)))
                    {
                        var normalPath = item.LogoPath.Replace("/tmp", "");

                        System.IO.File.Move(Server.MapPath(item.LogoPath), Server.MapPath(normalPath));

                        item.LogoPath = normalPath;
                        _service.Update(item);
                    }
                }
            }

            return new JsonResponse(result);
        }

        [CustomBasicAuthorize]
        [HttpPost]
        public JsonResponse Delete(Blog item)
        {
            if (User == null || (!User.UserData.Roles.Contains("Admin") && User.UserData.ID != item.OwnerUserID))
            {
                return new JsonResponse(false, "Unauthorized delete is not allowed!");
            }

            var result = _service.Delete(item);

            return new JsonResponse(result);
        }
    }
}