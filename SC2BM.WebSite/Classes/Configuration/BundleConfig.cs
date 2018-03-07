using System;
using System.Web.Optimization;

namespace SC2BM.WebSite.Classes.Configuration
{
    public static class BundleConfig
    {
        public static class Scripts
        {
            public static string CoreScriptsKey = "~/js/lib/core";
        }

        public static class Styles
        {
            public const string CoreStylesKey = "~/css/core";
        }

        public static void RegisterBundles(BundleCollection bundles)
        {
#if DEBUG
            BundleTable.EnableOptimizations = false;
#else
            BundleTable.EnableOptimizations = true;
#endif
            bundles.IgnoreList.Ignore("*.spec.js");


            bundles.Add(new StyleBundle(Styles.CoreStylesKey)
                .Include(
                    "~/css/bootstrap.min.css",
                    "~/css/flexslider.css",
                    "~/css/jquery-ui.css",
                    "~/css/font-awesome.min.css",
                    "~/css/animate.min.css",
                    "~/css/ionicons.min.css",
                    "~/css/helpers.css",
                    "~/css/main_landing.css",
                    "~/css/main_community.css",
                    "~/css/main_community_theme_blue.css",
                    "~/css/textAngular.css")
                .IncludeDirectory("~/js/app/pages/", "*.css", true)
                .IncludeDirectory("~/js/app/shared/", "*.css", true));

            bundles.Add(new ScriptBundle(Scripts.CoreScriptsKey)
                .Include(
                    //"~/js/lib/jquery-2.1.1.js",
                    "~/js/lib/jquery-1.11.3.min.js",
                    "~/js/lib/jquery-ui.js",
                    "~/js/lib/jquery.ui.touch-punch.js",
                    "~/js/lib/jquery.easypiechart.min.js",
                    "~/js/lib/jquery.htmlClean.min.js",
                    "~/js/lib/jquery.isotope.min.js",
                    "~/js/lib/jquery.easing.1.3.js",
                    "~/js/lib/jquery.fitvids.js",
                    "~/js/lib/jquery.flexslider-min.js",
                    "~/js/lib/jquery.inview.min.js",
                    "~/js/lib/jquery.matchHeight-min.js",
                    "~/js/lib/jquery.placeholder.js",
                    "~/js/lib/jquery.scrollTo.min.js",
                    "~/js/lib/underscore.js",
                    "~/js/lib/underscore.string.js",
                    "~/js/lib/angular.js",
                    "~/js/lib/angular-sortable.js",
                    "~/js/lib/angular-resource.js",
                    "~/js/lib/angular-route.js",
                    "~/js/lib/angular-sanitize.js",
                    "~/js/lib/angular-filter.js",
                    "~/js/lib/angular-cookies.js",
                    "~/js/lib/phoneFormat.js",
                    "~/js/lib/clamp.js",
                    "~/js/lib/bootstrap.min.js",
                    "~/js/lib/prettify.js",
                    "~/js/lib/modernizr.custom.js",
                    "~/js/lib/ng-file-upload-shim.js",
                    "~/js/lib/ng-file-upload.js",
                    "~/js/lib/isMobile.min.js",
                    "~/js/lib/textAngular-rangy.min.js",
                    "~/js/lib/textAngular-sanitize.js",
                    "~/js/lib/textAngularSetup.js",
                    "~/js/lib/textAngular.js",
                    "~/js/lib/ui-bootstrap-tpls-0.14.3.min.js",
                    "~/js/app/shell/app.js",
                    "~/js/app/shell/routing.js",
                    "~/js/app/shell/blockUIHandler.js",
                    "~/js/app/shell/*.js")
                .IncludeDirectory("~/js/app/shared/filters/", "*.js", true)
                .IncludeDirectory("~/js/app/shared/services/", "*.js", true)
                .IncludeDirectory("~/js/app/shared/directives/", "*.js", true)
                .IncludeDirectory("~/js/app/shared/constants/", "*.js", true)
                .IncludeDirectory("~/js/app/pages/", "*.js", true));
//#else
//            BundleTable.EnableOptimizations = true;
//            bundles.Add(new StyleBundle(Styles.CoreStylesKey).Include("~/css/styles.min.css"));
//            bundles.Add(new ScriptBundle(Scripts.CoreScriptsKey).Include("~/js/vendor.min.js", "~/js/app.min.js", "~/js/templates.min.js"));
//#endif
        }
    }
}