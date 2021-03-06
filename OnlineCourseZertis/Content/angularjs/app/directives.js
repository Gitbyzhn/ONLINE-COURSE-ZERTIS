/**
 * Cube - Bootstrap Admin Theme
 * Copyright 2014 Phoonio
 */


function bsNavbar($window, $location) {

	var defaults = this.defaults = {
		activeClass: 'active',
		routeAttr: 'data-match-route',
		strict: true
	};

	return {
		restrict: 'A',
		link: function postLink(scope, element, attr, controller) {

			// Directive options
			var options = angular.copy(defaults);
			angular.forEach(Object.keys(defaults), function(key) {
				if(angular.isDefined(attr[key])) options[key] = attr[key];
			});

			// Watch for the $location
			scope.$watch(function() {

				return $location.path();

			}, function(newValue, oldValue) {

				var liElements = element[0].querySelectorAll('li[' + options.routeAttr + '],li > a[' + options.routeAttr + ']');

				angular.forEach(liElements, function(li) {

					var liElement = angular.element(li);
					var pattern = liElement.attr(options.routeAttr).replace('/', '\\/');
					if(options.strict) {
						pattern = '^' + pattern;
					}
					var regexp = new RegExp(pattern, ['i']);

					if(regexp.test(newValue)) {
						liElement.addClass(options.activeClass);
					} else {
						liElement.removeClass(options.activeClass);
					}

				});

				// Close all other opened elements
				var op = $('#sidebar-nav').find('.open:not(.active)');
				op.children('.submenu').slideUp('fast');
				op.removeClass('open');
			});

		}

	};
}

function gd(year, day, month) {
	return new Date(year, month - 1, day).getTime();
}

function showTooltip(x, y, label, data) {
	$('<div id="flot-tooltip">' + '<b>' + label + ': </b><i>' + data + '</i>' + '</div>').css({
		top: y + 5,
		left: x + 20
	}).appendTo("body").fadeIn(200);
}


function showtab() {
    return {
        link: function (scope, element, attrs) {
            element.click(function(e) {
                e.preventDefault();
                $(element).tab('show');
            });
        }
    };
}

angular
	.module('cubeWebApp')
	.directive('bsNavbar', bsNavbar)
	.directive('showtab', showtab)
