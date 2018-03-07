using System;
using System.Collections.Generic;
using SC2BM.DomainModel;
using SC2BM.ServiceModel.BusinessServices;
using SC2BM.ServiceModel.Repositories;
using SC2BM.ServiceModel.Requests;
using SC2BM.ServiceModel.Responses;
using System.Linq;

namespace SC2BM.BusinessFacade.Services
{
    public class BlogService : IBlogService
    {
        private readonly IBlogsRepository _repository;
        private readonly IUserRepository _userRepo;

        public BlogService(IBlogsRepository repo, IUserRepository userRepo)
        {
            _repository = repo;
            _userRepo = userRepo;
        }

        public ServiceResponse<int> Register(Blog item)
        {
            int id = _repository.Insert(item);
            item.ID = id;

            return new ServiceResponse<int>(id);
        }

        public GeneralResponse Delete(Blog item)
        {
            _repository.Delete(item);
            item.IsDeleted = true;

            return new GeneralResponse();
        }

        public GeneralResponse Update(Blog item)
        {
            _repository.Update(item);

            return new GeneralResponse();
        }

        public ServicePagedResponse<Blog> Search(PagedRequest<SearchBlogsFilter> request)
        {
            var response = _repository.Search(request);
            return new ServicePagedResponse<Blog>(response, request.PageNumber);
        }

        public ServiceResponse<Blog> GetBlogByOwner(string ownerUserName)
        {
            var userRequest = _userRepo.GetSearchRequest(userName: ownerUserName);
            var userResponse = _userRepo.SearchUsers(userRequest);

            if (userResponse.TotalCount == 0)
            {
                throw new ApplicationException("User does not exists");
            }

            var request = _repository.GetSearchRequest(ownerUserID: userResponse.Items[0].ID);

            var blogResponse = _repository.Search(request);

            Blog blogInfo = null;

            if (blogResponse.TotalCount > 0)
            {
                blogInfo = blogResponse.Items[0];
            }

            return new ServiceResponse<Blog>(blogInfo);
        }

        public ServiceResponse<EntityRate> GetRateForBlog(int blogID)
        {
            EntityRate result = null;

            var blogRates = _repository.GetRates();
            if (blogRates != null && blogRates.Count > 0)
            {
                result = blogRates.FirstOrDefault(p => p.ID == blogID);
            }

            return new ServiceResponse<EntityRate>(result);
        }

        public ServiceListResponse<Blog> GetTopRatedBlogs(int count)
        {
            var blogRates = _repository.GetRates();
            var blogsRequest = _repository.GetSearchRequest();
            var blogsResponse = _repository.Search(blogsRequest);
            
            var result = blogsResponse.Items.Where(item => blogRates.FirstOrDefault(p => p.ID == item.ID) != null).Take(count).ToList();

            return new ServiceListResponse<Blog>(result);
        }

        public ServiceListResponse<Comment> GetBlogComments(int blogID)
        {
            var result = _repository.GetBlogComments(blogID);
            return new ServiceListResponse<Comment>(result);
        } 
    }
}