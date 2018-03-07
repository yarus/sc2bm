(function() {
	'use strict';
	angular.module('Sc2bmApp').filter('htmlToPlainText', htmlToPlainTextFilter);

	//htmlToPlainTextFilter.$inject = ['$sanitize'];

	function htmlToPlainTextFilter() {
	    return function (html) {
	        return html ? String(html).replace(/<[^>]+>/gm, '') : '';
			//return angular.element($sanitize(html)).text();
		};
	}

//	angular.module('myApp.filters', []).
//      filter('htmlToPlaintext', function () {
//          return function (text) {
//              return text ? String(text).replace(/<[^>]+>/gm, '') : '';
//          };
//      }
//    );
})();