(function() {
    'use strict';

    angular.module('Sc2bmApp').service('blogPostService', blogPostService);

    blogPostService.$inject = ['resource', 'blogPostBackendUrls'];

    function blogPostService(resource, urls) {
        return {
            search: search,
            addOrUpdate: addOrUpdate,
            deleteBlogPost: deleteBlogPost,
            getById: getById,
            getTopRatedBlogPosts: getTopRatedBlogPosts,
            getRateForBlogPost: getRateForBlogPost
        };

        //region Methods
        function search(request) {
            return resource.post(urls.search, { request: request }, null, false);
        }

        function addOrUpdate(item) {
            return resource.post(urls.addOrUpdate, { item: item }, null, false);
        }

        function deleteBlogPost(item) {
            return resource.post(urls.deleteBlogPost, { item: item }, null, false);
        }

        function getById(blogPostId) {
            return resource.get(urls.getById, { blogPostId: blogPostId }, null, false);
        }

        function getTopRatedBlogPosts(blogId, count) {
            return resource.get(urls.getTopRatedBlogPosts, { blogId: blogId, count: count }, null, false);
        }

        function getRateForBlogPost(blogPostId) {
            return resource.get(urls.getRateForBlogPost, { blogPostId: blogPostId }, null, false);
        }
    	//endregion
    }
})();