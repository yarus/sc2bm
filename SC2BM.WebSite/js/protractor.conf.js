// An example configuration file.
exports.config = {
	directConnect: false,

	// Capabilities to be passed to the webdriver instance.
	capabilities: {
		'browserName': 'chrome'
	},

	// Spec patterns are relative to the current working directly when
	// protractor is called.
	specs: ['e2e/**/*.spec.js'],

	baseUrl: 'http://127.0.0.1:9000/',

	// Options to be passed to Jasmine-node.
	jasmineNodeOpts: {
		showColors: true,
		defaultTimeoutInterval: 30000
	}
};