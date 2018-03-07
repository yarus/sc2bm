(function() {
	'use strict';

	angular.module('Sc2bmApp').service('blockUI', blockUI);

	blockUI.$inject = ['$rootScope'];

	function blockUI($rootScope) {
		var requestsPending = 0;

		//region Public Api
		var publicApi = {
			block: block,
			unblock: unblock
		};

		return publicApi;
		//endregion

		function block(config) {
			if (_requiresBlock(config)) {
				requestsPending += 1;
				$rootScope.$broadcast('blockUI', config);
			}
		}

		function unblock(config) {
			if (_requiresBlock(config)) {
				if (requestsPending > 0) {
					requestsPending -= 1;
				}

				if (requestsPending === 0) {
					$rootScope.$broadcast('unblockUI', config);
				}
			}
		}

		function _requiresBlock(config) {
			return (config.blockUI && config.url.indexOf('api/') > -1);
		}
	}
})();