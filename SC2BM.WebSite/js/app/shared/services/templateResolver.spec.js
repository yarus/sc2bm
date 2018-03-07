describe('shared/services/templateResolver', function () {
    'use strict';

	beforeEach(module('Sc2bmApp'));

	var templateDescriptor = {
		desktop: "desktop",
		tablet: "tablet",
		mobile: "mobile"
	};

	describe('mobile', function() {
		beforeEach(module(function($provide) {
			$provide.value('deviceService', {
				isMobile: true
			});
		}));

		it("Should return mobile url from descriptor on mobile environment", inject(function(templateResolver) {
			var template = templateResolver.getTemplate(templateDescriptor);
			expect(template).toBeDefined();
			expect(template).toBe("mobile");
		}));
	});

	describe('tablet', function() {
		beforeEach(module(function($provide) {
			$provide.value('deviceService', {
				isTablet: true
			});
		}));

		it("Should return tablet url from descriptor on tablet environment", inject(function(templateResolver) {
			var template = templateResolver.getTemplate(templateDescriptor);
			expect(template).toBeDefined();
			expect(template).toBe("tablet");
		}));
	});

	describe('desktop', function() {
		beforeEach(module(function($provide) {
			$provide.value('deviceService', {
				isDesktop: true
			});
		}));

		it("Should return desktop url from descriptor on desktop environment", inject(function(templateResolver) {
			var template = templateResolver.getTemplate(templateDescriptor);
			expect(template).toBeDefined();
			expect(template).toBe("desktop");
		}));
	});
});