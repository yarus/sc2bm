(function() {
    'use strict';

    angular.module('Sc2bmApp').constant('blogBackendUrls', {
        search: 'api/Blog/Search',
        addOrUpdate: 'api/Blog/AddOrUpdate',
        deleteBlog: 'api/Blog/Delete',
        getByOwner: 'api/Blog/GetByOwner',
        getTopRatedBlogs: 'api/Blog/GetTopRatedBlogs',
        getBlogComments: 'api/Blog/GetBlogComments',
        getRateForBlog: 'api/Blog/GetRateForBlog'
    });
})();