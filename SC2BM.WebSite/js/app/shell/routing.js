(function() {
	'use strict';

	angular.module('Sc2bmApp').config(routing);

	routing.$inject = ['$routeProvider', '$locationProvider'];

	function routing($routeProvider, $locationProvider) {
		$locationProvider.html5Mode(true);
	
	    $routeProvider
		    .when('/home', {
		        templateUrl: 'js/app/pages/home/home.html',
			    caseInsensitiveMatch: true,
			    title: 'SC2BM Community'
		    })
		    .when('/appguide', {
		        templateUrl: 'js/app/pages/static/appManual.html',
		        caseInsensitiveMatch: true,
		        title: 'SC2BM Community'
		    })
		    .when('/register', {
		        templateUrl: 'js/app/pages/register/register.html',
		        caseInsensitiveMatch: true,
		        title: 'Register to SC2BM Community'
		    })
            .when('/confirmRegistration/:userName/:confirmationSalt', {
                templateUrl: 'js/app/pages/register/confirmRegistration.html',
                caseInsensitiveMatch: true,
                title: 'Confirmation'
            })
            .when('/confirmRegistration/:userName', {
                templateUrl: 'js/app/pages/register/confirmRegistration.html',
                caseInsensitiveMatch: true,
                title: 'Confirmation'
            })
            .when('/news/add', {
                templateUrl: 'js/app/pages/news/newsEditor.html',
                caseInsensitiveMatch: true,
                title: 'Add News'
            })
            .when('/news/:id/edit', {
                templateUrl: 'js/app/pages/news/newsEditor.html',
                caseInsensitiveMatch: true,
                title: 'Edit News'
            })
            .when('/news/:id', {
                templateUrl: 'js/app/pages/news/newsView.html',
                caseInsensitiveMatch: true,
                title: 'News'
            })
            .when('/blogs', {
                templateUrl: 'js/app/pages/blogs/blogList.html',
                caseInsensitiveMatch: true,
                title: 'Blogs'
            })
            .when('/blogs/:owner', {
                templateUrl: 'js/app/pages/blogs/blogView.html',
                caseInsensitiveMatch: true,
                title: 'Blog'
            })
            .when('/blogs/:owner/register', {
                templateUrl: 'js/app/pages/blogs/blogEditor.html',
                caseInsensitiveMatch: true,
                title: 'Register new blog'
            })
            .when('/blogs/:owner/addpost', {
                templateUrl: 'js/app/pages/blogs/blogPostEditor.html',
                caseInsensitiveMatch: true,
                title: 'Blog Post'
            })
            .when('/blogs/:owner/:blogPostId', {
                templateUrl: 'js/app/pages/blogs/blogPostView.html',
                caseInsensitiveMatch: true,
                title: 'Blog Post'
            })
            .when('/blogs/:owner/:blogPostId/edit', {
                templateUrl: 'js/app/pages/blogs/blogPostEditor.html',
                caseInsensitiveMatch: true,
                title: 'Blog Post Editor'
            })
            .when('/builds/upload', {
                templateUrl: 'js/app/pages/builds/editBuild.html',
                caseInsensitiveMatch: true,
                title: 'Upload Build Order'
            })
            .when('/builds/:id/edit', {
                templateUrl: 'js/app/pages/builds/editBuild.html',
                caseInsensitiveMatch: true,
                title: 'Edit Build Order'
            })
            .when('/builds/:id', {
                templateUrl: 'js/app/pages/builds/buildView.html',
                caseInsensitiveMatch: true,
                title: 'Build Order'
            })
            .when('/library/:version/:race/vs/:vsRace', {
                templateUrl: 'js/app/pages/builds/builds.html',
                caseInsensitiveMatch: true,
                title: 'Build Library'
            })
            .when('/library/:version/:race', {
                templateUrl: 'js/app/pages/builds/builds.html',
                caseInsensitiveMatch: true,
                title: 'Build Library'
            })
            .when('/library/:version', {
                templateUrl: 'js/app/pages/builds/builds.html',
                caseInsensitiveMatch: true,
                title: 'Build Library'
            })
            .when('/library', {
                templateUrl: 'js/app/pages/builds/builds.html',
                caseInsensitiveMatch: true,
                title: 'Build Library'
            })
            .when('/links/:entityType/:entityId/:linkType/add', {
                templateUrl: 'js/app/pages/links/editLink.html',
                caseInsensitiveMatch: true,
                title: 'Links'
            })
            .when('/links/:id', {
                templateUrl: 'js/app/pages/links/editLink.html',
                caseInsensitiveMatch: true,
                title: 'Links'
            })
            .when('/kb/:version/:faction/:itemType', {
                templateUrl: 'js/app/pages/kb/kb.html',
                caseInsensitiveMatch: true,
                title: 'Knowladge Base'
            })
            .when('/kb/:version/:faction', {
                templateUrl: 'js/app/pages/kb/kb.html',
                caseInsensitiveMatch: true,
                title: 'Knowladge Base'
            })
            .when('/kb/:version', {
                templateUrl: 'js/app/pages/kb/kb.html',
                caseInsensitiveMatch: true,
                title: 'Knowladge Base'
            })
            .when('/kb', {
                templateUrl: 'js/app/pages/kb/kb.html',
                caseInsensitiveMatch: true,
                title: 'Knowladge Base'
            })
            .when('/videos', {
                templateUrl: 'js/app/pages/videos/videoList.html',
                caseInsensitiveMatch: true,
                title: 'Videos'
            })
            .when('/compare/:firstBuildId/:secondBuildId', {
                templateUrl: 'js/app/pages/compare/compare.html',
                caseInsensitiveMatch: true,
                title: 'Compare Build Orders'
            })
            .when('/compare/:firstBuildId', {
                templateUrl: 'js/app/pages/compare/compare.html',
                caseInsensitiveMatch: true,
                title: 'Compare Build Orders'
            })
            .when('/compare', {
                templateUrl: 'js/app/pages/compare/compare.html',
                caseInsensitiveMatch: true,
                title: 'Compare Build Orders'
            })
		    .otherwise({
			    redirectTo: '/home'
		    });
	}
})();
