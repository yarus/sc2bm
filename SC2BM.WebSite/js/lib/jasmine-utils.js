(function() {
	'use strict';

	angular.module('jasmineUtils', []);

	angular.module('jasmineUtils').service('utils', utils);

	utils.$inject = ['$httpBackend'];

	function utils($httpBackend) {
		function expectHttpCall(requestType, expectedUrl, promiseMethod) {
			var result = [];

			$httpBackend.when(requestType, expectedUrl).respond({});

			if(requestType == 'GET') {
				$httpBackend.expectGET(expectedUrl);
			} else {
				$httpBackend.expectPOST(expectedUrl);
			}

			promiseMethod().then(function (response) {
				result = response.data;
				expect(result).toBeDefined();
			});

			$httpBackend.flush();

			$httpBackend.verifyNoOutstandingExpectation();
			$httpBackend.verifyNoOutstandingRequest();
		}

		function declareHttpPromise(promise) {
			promise.success = function (fn) {
				var result = promise.then(fn);
				result.error = function (fn) { return promise.then(null, fn); };
				return result;
			};
			promise.error = function (fn) { return promise.then(null, fn); };
			return promise;
		}

		return {
			expectHttpCall: expectHttpCall,
			declareHttpPromise: declareHttpPromise
		};
	}
})();