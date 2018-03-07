using System;
using System.Web.Mvc;
using SC2BM.DomainModel;
using SC2BM.ServiceModel.BusinessServices;
using SC2BM.ServiceModel.Requests;
using SC2BM.ServiceModel.Responses;
using SC2BM.WebSite.Classes;

namespace SC2BM.WebSite.Controllers
{
    public class BlogPostController : BaseController
    {
        private readonly IBlogPostService _service;

        public BlogPostController(IBlogPostService service)
        {
            _service = service;
        }

        [HttpGet]
        public JsonResponse GetByID(int blogPostID)
        {
            var result = _service.GetByID(blogPostID);
            return new JsonResponse(result);
        }

        [HttpPost]
        public JsonResponse Search(PagedRequest<SearchBlogPostFilter> request)
        {
            var result = _service.Search(request);
            return new JsonResponse(result);
        }

        [CustomBasicAuthorize]
        [HttpPost]
        public JsonResponse AddOrUpdate(BlogPost item)
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
                var initialBlogPost = _service.GetByID(item.ID.Value);
                if (initialBlogPost.Result == null)
                {
                    return new JsonResponse(false, "Blog which user try to update does not exists");
                }

                result = _service.Update(item);

                if (result.Success)
                {
                    if (initialBlogPost.Result.LogoPath != item.LogoPath)
                    {
                        // Delete previous image
                        if (item.LogoPath != "/images/patterns/pattern-1.png" && (System.IO.File.Exists(Server.MapPath(initialBlogPost.Result.LogoPath))))
                        {
                            System.IO.File.Delete(Server.MapPath(initialBlogPost.Result.LogoPath));
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

                result = _service.Add(item);

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
        public JsonResponse Delete(BlogPost item)
        {
            if (User == null || (!User.UserData.Roles.Contains("Admin") && User.UserData.ID != item.OwnerUserID))
            {
                return new JsonResponse(false, "Unauthorized delete is not allowed!");
            }

            var result = _service.Delete(item);

            return new JsonResponse(result);
        }

        [HttpGet]
        public JsonResponse GetTopRatedBlogPosts(int blogId, int count)
        {
            var result = _service.GetTopRatedBlogPosts(blogId, count);
            return new JsonResponse(result);
        }

        [HttpGet]
        public JsonResponse GetRateForBlogPost(int blogPostId)
        {
            var result = _service.GetRateForBlogPost(blogPostId);
            return new JsonResponse(result);
        }
    }
}