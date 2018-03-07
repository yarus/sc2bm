(function() {
	'use strict';

	angular.module('Sc2bmApp').factory('templateResolver', templateResolver);

	templateResolver.$inject = ['deviceService'];

	function templateResolver(deviceService) {
		//region Methods
		/**
		 * Get template url from template resolver
		 * @param {templateDescriptor} templateDescriptor
		 * @returns {String}
		 */
		function getTemplate(templateDescriptor) {
			// return empty string if parameter undefined
			if (!templateDescriptor || !templateDescriptor.desktop) {
				return '';
			}

			var templateUrl = templateDescriptor.desktop;

			if (deviceService.isMobile) {
				// check if mobile template available, if not - check tablet, if not - return desktop
				if (templateDescriptor.mobile) {
					templateUrl = templateDescriptor.mobile;
				} else if (templateDescriptor.tablet) {
					templateUrl = templateDescriptor.tablet;
				}
			}

			if (deviceService.isTablet && templateDescriptor.tablet) {
				templateUrl = templateDescriptor.tablet;
			}

			return templateUrl;
		}
		//endregion

		return {
			getTemplate:getTemplate
		};
	}

	/**
	 * @typedef {Object} templateDescriptor
	 * @property {String} desktop
	 * @property {String} tablet
	 * @property {String} mobile
	 */
})();