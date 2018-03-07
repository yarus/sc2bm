(function() {
	'use strict';
	angular.module('Sc2bmApp').filter('htmlCleanup', htmlCleanupFilter);

	function htmlCleanupFilter() {
	    return function (html) {

	        if (typeof html !== 'undefined') {

                //trim anything before and after the html, get rid of "doc" in body.
	            var cleanHtml = $.htmlClean(html.toString().replace(/^[^\<]*\</, "<").replace(/\>[^\>]*\s*$/, ">").replace(/\r*\ndoc\r*\n/, ""), { format: true }); // jshint ignore:line
                //this should strip out the head/body tags...
	            var h = $("<div></div>");
	            var ele = jQuery.parseHTML(cleanHtml);
	            for (var x = 0; x < ele.length; x++ ) {
	                $(h).append( ele[x]);
	            }
	            //... but it will still leave the title. and styles, and others.
	            //... I imagine pretty much anything leftover can be chucked into here.
                //... opted for the nuclear option; most fancy html5 tags and their contents are now removed.
	            $(h).find('title,style,head,base,link,meta,script,noscript,template,body,nav,iframe,embed,object,param,video,audio,source,track,canvas,map,area,svg,form,fieldset,legend,label,input,button,select,datalist,optgroup,option,textarea,keygen,output,progress,meter,menuitem,menu,details,summary').remove();
	            return $(h).html();
	        } else {
	            return null;
	        }

	    };
	}
})();