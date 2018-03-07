(function() {
	'use strict';

	angular.module('Sc2bmApp').service('helperService', helperService);

	// helperService.$inject = [];

	function helperService() {
		/**
		 * Return MM/DD/YYYY string from ISO Date string
		 * @param dateString
		 * @returns {string}
		 * @private
		 */
		function getFormatedStringFromDateString(dateString) {
			// We need to show date without TimeZone information
			// TODO: Need to refactor using Date object
			var dateParts = dateString.substring(0, 10).split('-');
			var year = dateParts[0];
			var month = dateParts[1];
			var day = dateParts[2];
			return month + '/' + day + '/' + year;
		}

		/**
		 * Return default datePicker options
		 * @returns {{changeYear: boolean, changeMonth: boolean, yearRange: string, defaultDate: Date, dateFormat: string}}
		 */
		function getDefaultDatePickerOptions() {
			return {
				changeYear: true,
				changeMonth: true,
				yearRange: '1950:-0',
				defaultDate: new Date(),
				dateFormat: 'mm/dd/yy'
			};
		}

		function getDateFromString(value) {
			try {
				return angular.element.datepicker.parseDate(getDefaultDatePickerOptions().dateFormat, value);
			}
			catch (e) {
			}

			try {
				return angular.element.datepicker.parseDate(getDefaultDatePickerOptions().dateFormat.replace('yy', 'y'), value);
			}
			catch (e) {
			}

			return false;
		}

		function formatPhone(country, phone) {
			phone = phone || '';
			var number = phone;
			var extension = "";
			var match = phone.match(/(?:.*?([0-9])){10}/); // 10th digit will be the end.
			if (match != null && match.length > 0) {
				number = match[0];
				if (number.length < phone.length) {
					extension = phone.substring(number.length, phone.length);
					extension = extension.replace(/[^\d\s]/g, ''); // remove non-digits
					if (extension.length > 0 && extension[0] != " ") {
						extension = " " + extension;
					}
				}
			}
			var clean = cleanPhone(number);
			return formatInternational(country, clean) + extension;
		}

        function formatDigit(value) {
            return value.replace(/[^0-9]/g, '');
        }

		return {
			getFormatedStringFromDateString: getFormatedStringFromDateString,
			getDefaultDatePickerOptions: getDefaultDatePickerOptions,
			getDateFromString: getDateFromString,
			formatPhone: formatPhone,
			formatDigit: formatDigit
		};
	}
})();