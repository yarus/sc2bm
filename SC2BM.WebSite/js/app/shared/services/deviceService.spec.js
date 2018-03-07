describe("shared/services/deviceService", function () {
	'use strict';
	var deviceService;

	beforeEach(module("Sc2bmApp"));

	beforeEach(inject(function(_deviceService_) {
		deviceService = _deviceService_;
	}));

	it("Should return valid deviceMode object", function() {
		var mode = deviceService.getMode();
		expect(mode).toBeDefined();
		expect(mode.id).toBeGreaterThan(0);
		expect(['desktop', 'tablet', 'mobile']).toContain(mode.name);
	});

	it("Should return mobile mode for screen 360*480", function() {
		var mode = deviceService.getModeForScreen(360, 480);
		expect(mode).toBeDefined();
		expect(mode.name).toBe('mobile');
	});

	it("Should return tablet mode for screen 960*480", function() {
		var mode = deviceService.getModeForScreen(960, 480);
		expect(mode).toBeDefined();
		expect(mode.name).toBe('tablet');
	});

	it("Should return tablet mode for screen 1280*1024", function () {
	    var mode = deviceService.getModeForScreen(1280, 1024);
	    expect(mode).toBeDefined();
	    expect(mode.name).toBe('tablet');
	});

	it("Should return desktop mode for screen 1680*1050", function() {
		var mode = deviceService.getModeForScreen(1680, 1050);
		expect(mode).toBeDefined();
		expect(mode.name).toBe('desktop');
	});
});