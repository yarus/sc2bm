using System;
using System.Collections.Generic;
using SC2BM.DomainModel;
using SC2BM.DomainModel.Routine.Paging;
using SC2BM.ServiceModel.Requests;

namespace SC2BM.ServiceModel.Repositories
{
    public interface IBlogPostRepository
    {
        DataPage<BlogPost> Search(PagedRequest<SearchBlogPostFilter> request);

        void Update(BlogPost item);

        int Insert(BlogPost item);

        void Delete(BlogPost item);

        List<EntityRate> GetRates();

        PagedRequest<SearchBlogPostFilter> GetSearchRequest(bool isStrictSearch = false, int? blogPostID = null,
            int? blogID = null,
            int? ownerUserID = null, string title = "", string text = "", DateTime? fromDate = null,
            DateTime? toDate = null, bool? isDeleted = null,
            int pageNumber = 1, int rowsPerPage = 99999);
    }
}
