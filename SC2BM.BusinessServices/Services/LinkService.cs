using System;
using System.Linq;
using SC2BM.DomainModel;
using SC2BM.ServiceModel.BusinessServices;
using SC2BM.ServiceModel.Repositories;
using SC2BM.ServiceModel.Requests;
using SC2BM.ServiceModel.Responses;

namespace SC2BM.BusinessFacade.Services
{
    public class LinkService : ILinkService
    {
        private readonly ILinkRepository _repo;

        public LinkService(ILinkRepository repo)
        {
            _repo = repo;
        }

        public GeneralResponse DeleteLink(Link link)
        {
            if (link == null)
            {
                throw new ApplicationException("Link was not provided");
            }

            var idResponse = GetByID(link.ID);
            if (idResponse.Result == null)
            {
                throw new ApplicationException("Link with ID " + link.ID + " does not exists!");
            }

            _repo.Delete(link);

            return new GeneralResponse();
        }

        public GeneralResponse UpdateLink(Link link)
        {
            if (link == null)
            {
                throw new ApplicationException("Link was not provided");
            }

            var idResponse = GetByID(link.ID);
            if (idResponse.Result == null)
            {
                throw new ApplicationException("Link with ID " + link.ID + " does not exists!");
            }

            link.AddedDate = DateTime.Now;

            _repo.Update(link);

            return new GeneralResponse();
        }

        public ServiceResponse<int> AddLink(Link link)
        {
            if (link == null)
            {
                throw new ApplicationException("Link was not provided");
            }

            link.AddedDate = DateTime.Now;

            var newID = _repo.Insert(link);
            link.ID = newID;

            return new ServiceResponse<int>(newID);
        }

        public ServiceResponse<Link> GetByID(int id)
        {
            var request = _repo.GetSearchRequest();
            request.Filter.LinkID = id;
            request.Filter.IsStrictSearch = true;

            var response = _repo.Search(request);

            Link result = null;

            if (response.Items.Count > 0)
            {
                result = response.Items[0];
            }

            return new ServiceResponse<Link>(result);
        }

        public ServicePagedResponse<Link> GetLinks(string entity, int entityID)
        {
            var request = _repo.GetSearchRequest();
            request.Filter.EntityType = entity;
            request.Filter.EntityID = entityID;
            request.OrderBy = "AddedDate";
            request.OrderByAscending = false;

            var response = _repo.Search(request);

            return new ServicePagedResponse<Link>(response, request.PageNumber);
        }

        public ServiceListResponse<Link> GetLatestLinks(string linkType, int count)
        {
            var request = _repo.GetSearchRequest();

            if (!string.IsNullOrEmpty(linkType))
            {
                request.Filter.Type = linkType;   
            }
            
            request.OrderBy = "AddedDate";
            request.OrderByAscending = false;

            var response = _repo.Search(request);

            var result = response.Items.Take(count).ToList();

            return new ServiceListResponse<Link>(result);
        }

        public ServicePagedResponse<Link> SearchLinks(PagedRequest<SearchLinksFilter> request)
        {
            var result = _repo.Search(request);
            return new ServicePagedResponse<Link>(result, request.PageNumber);
        }
    }
}