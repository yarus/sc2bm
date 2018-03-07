describe('shared/services/backend/note', function () {
	'use strict';

	var utils;
	var notesService;
	/** @type {noteBackendUrls} urls **/
	var urls;
	var getPatterns;
	var noteKey = 1;
	var note = {
		Text: "Text",
		TypeId: 2,
		CompletedOn: null,
		NoteKey: 1
	};

	_setup();

	describe('API interface', function () {
		it('should provide getNote function', function () {
			expect(typeof notesService.getNote).toBe('function');
		});
		it('should provide getNewNote function', function () {
			expect(typeof notesService.getNewNote).toBe('function');
		});
		it('should provide addNote function', function () {
			expect(typeof notesService.addNote).toBe('function');
		});
		it('should provide updateNote function', function () {
			expect(typeof notesService.updateNote).toBe('function');
		});
		it('should provide deleteNote function', function () {
			expect(typeof notesService.deleteNote).toBe('function');
		});
		it('should provide restoreDeletedNote function', function () {
			expect(typeof notesService.restoreDeletedNote).toBe('function');
		});
		it('should provide searchForNotes function', function () {
			expect(typeof notesService.searchForNotes).toBe('function');
		});
		it('should provide getNotes function', function () {
			expect(typeof notesService.getNotes).toBe('function');
		});
		it('should provide getTypes function', function () {
			expect(typeof notesService.getTypes).toBe('function');
		});
	});

	describe('api url validation', function () {
		it('should call getNote with valid noteKey', function () {
			utils.expectHttpCall('GET', getPatterns.getNote, function() {
				return notesService.getNote(noteKey);
			});
		});
		it('should call getNewNote with valid params', function () {
			utils.expectHttpCall('GET', getPatterns.getNewNote, function() {
				return notesService.getNewNote({entityType:'contact', entityKey: 1});
			});
		});
		it('should call getNotes with valid noteKey', function () {
			utils.expectHttpCall('GET', getPatterns.getNotes, function() {
				return notesService.getNotes({noteKey: noteKey});
			});
		});
		it('should call addNote with valid params', function () {
			utils.expectHttpCall('POST', urls.addNote, function() {
				return notesService.addNote({note: note});
			});
		});
		it('should call updateNote with valid params', function () {
			utils.expectHttpCall('POST', urls.updateNote, function() {
				return notesService.updateNote({note: note});
			});
		});
		it('should call deleteNote with valid params', function () {
			utils.expectHttpCall('POST', urls.deleteNote, function() {
				return notesService.deleteNote({noteKey: noteKey});
			});
		});
		it('should call restoreDeletedNote with valid params', function () {
			utils.expectHttpCall('POST', urls.restoreDeletedNote, function() {
				return notesService.restoreDeletedNote(noteKey);
			});
		});

		it('should call searchForNotes with valid params', function () {
			utils.expectHttpCall('POST', urls.searchForNotes, function() {
				return notesService.searchForNotes({noteKey: noteKey});
			});
		});
		it('should call getTypes with valid params', function () {
			utils.expectHttpCall('GET', getPatterns.getTypes, function () {
				return notesService.getTypes();
			});
		});
	});

	//region setup
	function _setup() {
		_loadAppModules();
		_injectDependencies();
	}

	function _loadAppModules() {
		beforeEach(module('Sc2bmApp'));
		beforeEach(module('jasmineUtils'));
	}

	function _injectDependencies() {
	    beforeEach(inject(function ($injector) {
	        urls = $injector.get('noteBackendUrls');

			notesService = $injector.get('notesService');

		    utils = $injector.get('utils');

		    getPatterns = {
                getNote: new RegExp(urls.getNote + '\\?.*noteKey=' + noteKey + '.*'),
	            getNewNote: new RegExp(urls.getNewNote + '\\?.*entityKey=' + noteKey + '.*entityType=contact' + '.*'),
			    getNotes: new RegExp(urls.getNotes + '\\?.*noteKey=' + noteKey + '.*'),
			    getTypes: new RegExp(urls.getTypes + '\\?.*')
            };
		}));
	}
	//endregion
});