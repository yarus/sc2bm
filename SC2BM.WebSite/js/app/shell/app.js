(function() {
	'use strict';
	
	angular.module('Sc2bmApp.Templates', []);

	angular.module('Sc2bmApp', ['ngRoute', 'ngResource', 'textAngular', 'ngFileUpload',
		'angular.filter', 'ngCookies', 'ui.bootstrap', 'Sc2bmApp.Templates']);

	angular.module('Sc2bmApp').run(['session', '$rootScope', '$routeParams',
        function (session, $rootScope, $routeParams) {
	        $rootScope.$on('$routeChangeSuccess', function (event, currentRoute, previousRoute) {
		        session.title = currentRoute.title;

//		        _.each(session.navs, function(item) {
//			        // Notes Add link is available only for contact/location/company contexts
//			        if (item.Id == 'notes') {
//				        item.Add = $routeParams.contextKey != null;
//			        }
//		        });
	        });
        }
	]);
})();