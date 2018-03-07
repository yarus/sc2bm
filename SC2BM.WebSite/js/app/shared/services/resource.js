(function() {
	'use strict';

	angular.module('Sc2bmApp').service('resource', resource);

	resource.$inject = ['$http'];

	function resource($http) {
		//region Methods
		function get(url, params, callback, blockUI) {
			params = params || {};
			params.noCache = new Date().getTime();

			if (_.isUndefined(blockUI)) {
				blockUI = true;
			}

			var config = {
				method: 'GET',
				cache: false,
				params: params,
				blockUI: blockUI
			};

			if (_.isFunction(callback)) {
				$http.get(url, config)
					.success(callback);
			} else {
				return $http.get(url, config);
			}

			return null;
		}

		function post(url, params, callback, blockUI) {
			if (_.isUndefined(blockUI)) {
				blockUI = true;
			}

			var config = {
				method: 'POST',
				cache: false,
				blockUI: blockUI
			};

			if (_.isFunction(callback)) {
				$http.post(url, params, config)
					.success(callback);
			} else {
				return $http.post(url, params, config);
			}

			return null;
		}
		//endregion

		return {
			get: get,
			post: post
		};
	}
})();
