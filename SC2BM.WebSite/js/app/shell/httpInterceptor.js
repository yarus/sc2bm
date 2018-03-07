(function() {
	'use strict';

	angular.module('Sc2bmApp').service('httpInterceptor', httpInterceptor);

	angular.module('Sc2bmApp').config(['$httpProvider',
		function ($httpProvider) {
			$httpProvider.interceptors.push('httpInterceptor');
		}
	]);

	httpInterceptor.$inject = ['$q', 'blockUI', 'session'];

	function httpInterceptor($q, blockUI, session) {
		//region Public Api
		var publicApi = {
			request: request,
			response: response,
			responseError: responseError
		};

		return publicApi;
		//endregion

		//region Methods
		function request(config) {
		    blockUI.block(config);

		    var access_token = session.getAccessToken();

		    if (access_token != null && access_token != "null") {
		        config.headers["X-Access-Token"] = access_token;
		    }

			return config;
		}

		function response(responseParam) {
			blockUI.unblock(responseParam.config);

			if (response.status === 401) {
			    var i = 0;
			    //$rootScope.$broadcast('unauthorized');
			}

		    if (responseParam.headers()['content-type'] === 'application/json; charset=utf-8') {
		        var data = responseParam.data;

		        if (!_.isUndefined(data) && typeof data.Success === 'boolean') {
					if (data.Message && data.Message !== '') {
					    console.log(data.Message, data.Success ? 'Success' : 'Error');
					}

					if (!data.Success) {
					    return $q.reject(responseParam);
					}

					responseParam.data = data.Result;
				}
			}

		    return responseParam;
		}

		function responseError(responseParam) {
		    blockUI.unblock(responseParam.config);

		    if (responseParam.status == 401) {
				alert('Authorization is required.');
			}
		    else if (responseParam.status == 403) {
				alert('It appears you\'ve attempted to access data you don\'t have access to. Please contact administrator if you feel you should have access to this content.');
			}
		    else {
		        var message = 'There was an unexpected error:\n' + responseParam.status + ' - ' + responseParam.statusText + '\n\nDetails:\n' + (responseParam.data != null ? (responseParam.data.Message || responseParam.data.ExceptionMessage) : "");

		        console.log(message);
			}
		    return $q.reject(responseParam);
		}
		//endregion
	}
})();
