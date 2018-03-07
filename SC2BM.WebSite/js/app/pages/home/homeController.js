(function () {
	'use strict';

	angular.module('Sc2bmApp').controller('homeController', homeController);

	homeController.$inject = ['rateRepository', 'commentRepository', 'newsRepository', 'linksRepository', 'abbrService', 'userService', 'buildOrderService', 'rateService', 'newsService', 'session', 'security', '$rootScope', '$window', 'commentService', 'linkService', '$sce', 'blogPostService', 'authService', '$filter'];

	function homeController(rateRepository, commentRepository, newsRepository, linksRepository, abbrService, userService, buildOrderService, rateService, newsService, session, security, $rootScope, $window, commentService, linkService, $sce, blogPostService, authService, $filter) {
		//region ViewModel declaration
		/*jshint validthis:true */
	    var vm = this;
	    vm.user = {};
	    vm.filterLatestBuilds = filterLatestBuilds;
	    vm.filterTopRatedBuilds = filterTopRatedBuilds;
	    vm.isAdmin = false;

	    vm.loadMoreBuilds = loadMoreBuilds;
	    vm.deleteNewsItem = deleteNewsItem;

	    vm.login = login;
	    vm.logoff = logoff;
	    
	    vm.latestBuilds = [];
	    vm.latestNews = [];
	    vm.latestComments = [];
	    vm.topRatedBuilds = [];

	    vm.commentsModel = {};
	    vm.fakeEntity = { ID: -1 };

	    vm.latestVod = null;

	    var newsToShow = 3;

	    var latestBuildsToLoad = 5;

		activate();
		//endregion

		//region Methods
		function activate() {
		    vm.user = session.getUser();
		    vm.isAdmin = session.isCurrentUserAdmin();
		    
		    _loadLatestBuilds();
		    _loadTopRatedBuilds();
		    _loadLatestNews();
		    _loadLatestComments();
		    _loadLatestVod();
		    _loadLatestLinks();
		}

		function login(userName, password) {
		    if (userName == null || userName == "" || password == null || password == "") {
		        return;
		    }

		    var remember = vm.rememberMe ? "/true" : "/false";

		    $window.location.href = "/Authorization/LogIn/" + userName + "/" + password + remember;
		}

		function logoff() {
		    var user = session.getUser();
		    if (user == null || user.UserName == null) {
		        return;
		    }

		    var userName = user.UserName;

		    if (userName != null && userName != "") {
		        $window.location.href = "/Authorization/LogOff/" + userName;
		    }
		}

		function _loadLatestLinks() {
		    linksRepository.getLatestLinks(5)
		        .then(function(result) {
		            if (result != null && result.length > 0) {
		                vm.links = result;
		            }
		        });
        }

		function _loadLatestVod() {
		    linksRepository.getLatestVod()
		        .then(function(result) {
		            if (result != null) {
		                vm.latestVod = result;
		                vm.latestVod.SecondaryLinkTrust = $sce.trustAsResourceUrl(vm.latestVod.SecondaryLink);
		            }
		        });
        }
        
        function _loadLatestComments() {
            commentService.getLatestComments('')
                .success(function (result) {
                    if (result != null && result.Items != null) {
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

                            if (element.EntityType == "News") {
                                element.EntityLink = "/news/" + element.EntityID;
                            } else if (element.EntityType == "Builds") {
                                element.EntityLink = "/builds/" + element.EntityID;
                            } else if (element.EntityType == "BlogPost") {
                                var el = element;

                                blogPostService.getById(element.EntityID)
                                    .success(function (result) {
                                        if (result != null) {
                                            el.EntityLink = "/blogs/" + result.OwnerUserName + "/" + el.EntityID;
                                        }
                                    });
                            }
                        });
                    }
                });
        }

        function _loadLatestNews() {
            newsRepository.getLatestNews(newsToShow)
                .then(function (results) {
                    if (results != null) {
                        vm.latestNews = results;

                        vm.latestNews.forEach(function (element, index, array) {
                            var el = element;

                            if (el.Text != null) {
                                el.Text = $filter('htmlToTextWithLines')(el.Text);

                                if (el.Text.length > 250) {
                                    el.Text = el.Text.substring(0, 250);
                                } else {
                                    el.Text = el.Text;
                                }
                            }

                            commentRepository.getTotalComments('News', el.ID)
                                .then(function (result) {
                                    if (result != null) {
                                        el.commentsTotal = result;
                                    }
                                });
                        });
                    }
                });
		}

		function deleteNewsItem(newsItem) {
		    newsService.deleteNews(newsItem)
                .success(function (result) {
                    newsRepository.reset();

		            _loadLatestNews();
		        });
		}

		function loadMoreBuilds() {
		    latestBuildsToLoad += 5;

		    _loadLatestBuilds();
		}

		function filterLatestBuilds(race) {
		    if (race == vm.latestFaction) {
		        vm.latestFaction = '';
		    } else if (race == 'All') {
		        vm.latestFaction = null;
		    } else {
		        vm.latestFaction = race;
		    }

		    latestBuildsToLoad = 5;

		    _loadLatestBuilds();
		}

		function filterTopRatedBuilds(newFaction) {
		    if (newFaction == vm.faction) {
		        vm.faction = '';
		    } else if (newFaction == 'All') {
		        vm.faction = null;
		    } else {
		        vm.faction = newFaction;
		    }
		    
		    vm.topRatedBuilds = [];

		    _loadTopRatedBuilds();
		}

		function _loadTopRatedBuilds() {
		    buildOrderService.getTopRatedBuildOrders(null, vm.faction, null, 4)
                .success(function (result) {
                    if (result != null && result.length > 0) {
                        vm.topRatedBuilds = result;

                        vm.topRatedBuilds.forEach(function (element, index, array) {
                            var el = element;
                            _loadBuildAdditionalData(el);
                        });
                    }
                });
		}

		function _loadLatestBuilds() {
		    var request = { pageNumber: 1, rowsPerPage: latestBuildsToLoad, totalItems: 0, orderBy: "AddedDate", orderByAscending: false, filter: { IsDeleted: 0 } };

		    if (vm.latestFaction != '') {
		        request.filter.Race = vm.latestFaction;
		    } else {
		        request.filter.Race = null;
		    }

		    buildOrderService.searchBuildOrders(request)
                .success(function (result) {
                    if (result != null && result.Items != null) {
                        vm.latestBuilds = result.Items;
                        vm.latestBuildsTotal = result.TotalCount;

                        vm.latestBuilds.forEach(function (element, index, array) {
                            var el = element;
                            _loadBuildAdditionalData(el);
                        });
                    }
                });
		}

		function _loadBuildAdditionalData(build) {
		    var el = build;

		    rateRepository.getRate('Build', el.ID)
                .then(function (result) {
                    if (result != null) {
                        el.rate = parseFloat(result).toFixed(1);
                        el.rateClass = abbrService.getRateClassByRate(el.rate);
                    }
                });

		    commentRepository.getTotalComments('Builds', el.ID)
                .then(function (result) {
                    if (result != null) {
                        el.commentsTotal = result;
                    }
                });

		    if (el.Description != null) {
		        el.Description = $filter('htmlToPlainText')(el.Description);

		        if (el.Description.length > 300) {
		            el.Description = el.Description.substring(0, 300) + "...";
		        }
		    }

		    el.matchup = abbrService.getMathupLabelForBuild(el);
		    el.matchupLogo = abbrService.getImageForMatchup(el.matchup);

		    if (el.SC2VersionID != null) {
		        el.SC2VersionName = abbrService.getAddonByVersion(el.SC2VersionID);
		    }

		    el.microLabel = (el.MicroRate / 5) * 100 + "%";
		    el.microStyle = { width: el.microLabel };
		    el.macroLabel = (el.MacroRate / 5) * 100 + "%";
		    el.macroStyle = { width: el.macroLabel };
		    el.aggressionLabel = (el.AggressionRate / 5) * 100 + "%";
		    el.aggressionStyle = { width: el.aggressionLabel };
		    el.defenseLabel = (el.DefenceRate / 5) * 100 + "%";
		    el.defenseStyle = { width: el.defenseLabel };
		}
	}
})();