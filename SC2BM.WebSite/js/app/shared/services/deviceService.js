(function() {
	'use strict';

	angular.module('Sc2bmApp').service('deviceService', deviceService);

	function deviceService() {
		var constants = {
			widgetWidth: 320,
			modes: [
				{ id: 1, name: 'desktop', appCss: 'desktop-size' },
				{ id: 2, name: 'tablet', appCss: 'tablet-size' },
				{ id: 3, name: 'mobile', appCss: 'mobile-size' }
			]
		};

		var mode = getMode();

		function getMode() {
			if (_.isUndefined(mode)) {
				mode = getModeForScreen(window.screen.width, window.screen.height, window.devicePixelRatio, window.innerWidth, window.innerHeight);
			}

			return mode;
		}

		function getModeForScreen(width, height, devicePixelRatio, innerWidth, innerHeight) {
			var result = constants.modes[0];

		    try {

		        // modified a bit.  there are inconsistencies among devices as to how to report the device size when dpr > 1.
		        // so, we now assume if dpr = 1, we can use the screen width and height.
		        // if dpr > 1, we'll use it unless we're reasonably certain we're running fullscreen (mobile, tablet)
		        // we attempt to determine full-screen status by checking to see if the inner sizes are equal either to the device
		        // sizes or the device sizes divided by the reported dpr.  If any of these are true, we assume we're at fullscreen and
		        // use the inner sizes.
                var dpr = _.isUndefined(devicePixelRatio) ? 1 : devicePixelRatio;
		        var dw = width, dh = height; // device size
		        var iw = _.isUndefined(innerWidth) ? width : innerWidth; // inner size
		        var ih = _.isUndefined(innerHeight) ? height : innerHeight; // inner sizes
		        var ew = dw, eh = dh; // effective size, default to device size

                // consolidated logic in previous version
		        if (dpr > 1 && ((dw / dpr) == iw || (dh / dpr) == ih)) {
		                ew = iw;
		                eh = ih;
    	        }
		        // otherwise, stick with dw/dh

		        var mr = Math.max(ew, eh);
		        var cols = Math.floor(mr / constants.widgetWidth);

		        if (cols <= 2) {
		            result = constants.modes[2];
		        } else if (cols <= 4) {
		            result = constants.modes[1];
		        }
			}
		    catch (e) {
			}
			
			return result;
		}

		return {
			getMode: getMode,
			getModeForScreen: getModeForScreen,
			deviceModeCss: mode.appCss,
			isDesktop: mode.id == 1,
			isTablet: mode.id == 2,
			isMobile: mode.id == 3
		};
	}
})();
