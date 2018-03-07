(function() {
    'use strict';

    angular.module('Sc2bmApp').constant('blogPostBackendUrls', {
        search: 'api/BlogPost/Search',
        addOrUpdate: 'api/BlogPost/AddOrUpdate',
        deleteBlogPost: 'api/BlogPost/Delete',
        getById: 'api/BlogPost/GetByID',
        getTopRatedBlogPosts: 'api/BlogPost/GetTopRatedBlogPosts',
        getRateForBlogPost: 'api/BlogPost/GetRateForBlogPost'
    });
})();