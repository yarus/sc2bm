using System;
using System.Collections.Generic;
using SC2BM.DomainModel;
using SC2BM.DomainModel.Routine.Paging;
using SC2BM.ServiceModel.Requests;

namespace SC2BM.ServiceModel.Repositories
{
    public interface IBlogsRepository
    {
        int Insert(Blog item);

        void Delete(Blog item);

        void Update(Blog item);

        DataPage<Blog> Search(PagedRequest<SearchBlogsFilter> request);

        List<EntityRate> GetRates();

        List<Comment> GetBlogComments(int blogID);

        PagedRequest<SearchBlogsFilter> GetSearchRequest(bool isStrictSearch = false, int? blogID = null,
            int? ownerUserID = null, string title = "", DateTime? fromDate = null, DateTime? toDate = null, bool? isDeleted = null,
            int pageNumber = 1, int rowsPerPage = 99999);
    }
}