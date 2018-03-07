(function () {
	'use strict';

	angular.module('Sc2bmApp').controller('blogListController', blogListController);

	blogListController.$inject = ['$routeParams', '$location', '$filter', 'session', 'blogService', 'blogPostService', 'commentService'];

	function blogListController($routeParams, $location, $filter, session, blogService, blogPostService, commentService) {
		//region ViewModel declaration
		/*jshint validthis:true */
	    var vm = this;
	    vm.user = {};

	    vm.blogs = [];
	    vm.blogsRequest = {};

	    vm.blogPosts = [];
	    vm.blogPostsRequest = {};

	    vm.enableOpenBlog = false;
	    vm.enableRegisterBlog = false;
	    vm.personalBlog = null;

	    vm.latestComments = [];

	    vm.loadMorePosts = loadMorePosts;

	    var rowsStep = 5;
	    var topBlogs = 5;

		activate();
		//endregion

		//region Methods
		function activate() {
		    vm.user = session.getUser();
		    
		    vm.blogPostsRequest = { pageNumber: 1, rowsPerPage: rowsStep, totalItems: 0, orderBy: "AddedDate", orderByAscending: false, filter: { IsDeleted: false } };

		    _loadBlogs();
		    _loadBlogPosts();
		    _loadLatestComments();
		    _loadPersonalBlog();
		}

		function _loadPersonalBlog() {
		    if (vm.user == null || vm.user.ID == null) {
		        return;
		    }

		    blogService.getByOwner(vm.user.UserName)
		        .success(function(result) {
                    if (result != null) {
                        vm.personalBlog = result;
                        vm.enableOpenBlog = true;
                    } else {
                        vm.enableRegisterBlog = true;
                    }
		        });
		}
	
		function _loadBlogs() {
		    blogService.getTopRatedBlogs(topBlogs)
                .success(function (result) {
		            if (result.length > 0) {
		                vm.blogs = result;

		                vm.blogs.forEach(function(element, index, array) {
		                    var el = element;
		                    blogService.getRateForBlog(element.ID)
		                        .success(function(result) {
		                            if (result != null) {
		                                el.rate = result.Value;
		                            }
		                        });
		                });
		            }
		        });
		}

		function loadMorePosts() {
		    vm.blogPostsRequest.rowsPerPage += rowsStep;

		    _loadBlogPosts();
		}

		function _loadBlogPosts() {
		    blogPostService.search(vm.blogPostsRequest)
                .success(function (result) {
                    if (result.Items.length > 0) {
                        vm.blogPosts = result.Items;
                        vm.blogPostsRequest.totalItems = result.TotalCount;
                        vm.blogPosts.forEach(function (element, index, array) {
                            element.logoVisible = (element.LogoPath != "/images/patterns/pattern-1.png");

                            if (element.Text != null) {
                                element.Text = $filter('htmlToTextWithLines')(element.Text);

                                if (element.Text.length > 300) {
                                    element.Text = element.Text.substring(0, 300).replace(/\n/g, '<br/>') + "...";
                                } else {
                                    element.Text = element.Text.replace(/\n/g, '<br/>');
                                }
                            }

                            var el = element;
                            commentService.getComments('BlogPost', element.ID)
                                .success(function (result) {
                                    if (result != null) {
                                        el.commentsTotal = result.TotalCount;
                                    }
                                });
                        });
                    }
                });
		}

		function _loadLatestComments() {
		    commentService.getLatestComments('BlogPost')
                .success(function (result) {
                    if (result != null) {
                        vm.latestComments = result.Items;

                        vm.latestComments.forEach(function (element, index, array) {
                            if (element.Text != null) {
                                element.Text = $filter('htmlToTextWithLines')(element.Text);

                                if (element.Text.length > 100) {
                                    element.Text = element.Text.substring(0, 100).replace(/\n/g, '<br/>') + "...";
                                } else {
                                    element.Text = element.Text.replace(/\n/g, '<br/>');
                                }
                            }

                            var el = element;
                            blogPostService.getById(element.EntityID)
                                .success(function(result) {
                                    if (result != null) {
                                        el.EntityLink = "/blogs/" + result.OwnerUserName + "/" + el.EntityID;
                                    }
                                });
                        });
                    }
                });
		}
	}
})();