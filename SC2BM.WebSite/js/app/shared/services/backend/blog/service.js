(function() {
    'use strict';

    angular.module('Sc2bmApp').service('blogService', blogService);

    blogService.$inject = ['resource', 'blogBackendUrls'];

    function blogService(resource, urls) {
        return {
            search: search,
            addOrUpdate: addOrUpdate,
            deleteBlog: deleteBlog,
            getByOwner: getByOwner,
            getTopRatedBlogs: getTopRatedBlogs,
            getBlogComments: getBlogComments,
            getRateForBlog: getRateForBlog
        };

        //region Methods
        function search(request) {
            return resource.post(urls.search, { request: request }, null, false);
        }

        function getByOwner(ownerUserName) {
            return resource.get(urls.getByOwner, { ownerUserName: ownerUserName }, null, false);
        }

        function getBlogComments(blogId) {
            return resource.get(urls.getBlogComments, { blogID: blogId }, null, false);
        }

        function getRateForBlog(blogId) {
            return resource.get(urls.getRateForBlog, { blogID: blogId }, null, false);
        }
        
        function getTopRatedBlogs(count) {
            return resource.get(urls.getTopRatedBlogs, {count: count}, null, false);
        }

        function addOrUpdate(item) {
            return resource.post(urls.addOrUpdate, { item: item }, null, false);
        }

        function deleteBlog(item) {
            return resource.post(urls.deleteBlog, { item: item }, null, false);
        }
    	//endregion
    }
})();