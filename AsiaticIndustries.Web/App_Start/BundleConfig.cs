using System.Web.Optimization;
using AsiaticIndustries.Core.Infrastructure;

namespace AsiaticIndustries.Web.App_Start
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            BundleTable.EnableOptimizations = ConfigSettings.EnableBundlingMinification;

            #region Login Bundle

            bundles.Add(new ScriptBundle("~/loginlayout/js").Include(
                "~/assets/js/sitejs/jquery-2.1.3.js",
                "~/assets/js/sitejs/jquery-migrate-1.2.1.js",
                "~/assets/js/sitejs/jquery-ui-1.11.4.js",
                "~/assets/js/sitejs/jquery.cokie.js",
                "~/assets/js/sitejs/jquery.validate*",
                "~/assets/js/sitejs/bootstrap.js",
                "~/assets/js/sitejs/bootstrap-dialog.js",
                "~/assets/js/sitejs/jquery.cokie.js",
                "~/assets/js/sitejs/toaster.js",
                "~/assets/js/sitejs/angular.js",
                "~/assets/js/sitejs/angular-route.js",
                "~/assets/js/sitejs/spin.js",
                "~/assets/js/sitejs/angular-spinner.js",
                "~/assets/js/sitejs/angular-loading-spinner.js"
                ));

            bundles.Add(new ScriptBundle("~/assets/js/viewjs/securityapp/controllerjs").Include(
                "~/assets/js/viewjs/securityapp/app.js",
                 "~/assets/js/viewjs/common.js",
                "~/assets/js/viewjs/securityapp/index.js"));

            bundles.Add(new StyleBundle("~/loginlayout/css").Include(
                "~/assets/css/bootstrap.css",
                "~/assets/css/bootstrap-dialog.css",
                "~/assets/css/font-awesome.css",
                "~/assets/css/ionicons.css",
                "~/assets/css/toaster.css",
                "~/assets/css/style.css",
                "~/assets/css/build.css",
                "~/assets/css/custom.css",
                "~/assets/css/login.css"
                ));

            #endregion

            #region Site Bundle

            bundles.Add(new ScriptBundle("~/sitelayout/js").Include(
                "~/assets/js/sitejs/jquery-2.1.3.js",
                "~/assets/js/sitejs/jquery.mousewheel.js",
                "~/assets/js/sitejs/jquery-migrate-1.2.1.js",
                "~/assets/js/sitejs/jquery-ui-1.11.4.js",
                "~/assets/js/sitejs/jquery.cokie.js",
                "~/assets/js/sitejs/jquery.validate*",
                "~/assets/js/sitejs/bootstrap.js",
                "~/assets/js/sitejs/bootstrap-dialog.js",
                "~/assets/js/sitejs/jquery.jqChart.js",
                "~/assets/js/sitejs/jquery.jqRangeSlider.js",
                "~/assets/js/viewjs/jquery.webui-popover.js",
                "~/assets/js/sitejs/toaster.js",
                "~/assets/library/token_input/src/jquery.tokeninput.js",
                "~/assets/js/sitejs/jquery.shorten.js",
                "~/assets/js/sitejs/angular.js",
                "~/assets/js/sitejs/angular-route.js",
                "~/assets/js/sitejs/angular-cookies.js",
                "~/assets/js/sitejs/spin.js",
                "~/assets/js/sitejs/angular-spinner.js",
                "~/assets/js/sitejs/angular-loading-spinner.js",
                "~/assets/js/sitejs/ui-bootstrap-tpls-0.13.4.js",
                "~/assets/js/sitejs/dirPagination.js",
                "~/assets/js/sitejs/siteapp.js"));

            bundles.Add(new ScriptBundle("~/assets/js/viewjs/siteapp/controllerjs").Include(
                "~/assets/js/viewjs/resource.js",
                "~/assets/js/viewjs/siteapp/app.js",
                "~/assets/js/viewjs/common.js",
                "~/assets/js/viewjs/siteapp/routes.js",
                "~/assets/js/viewjs/siteapp/dashboard/index.js",
                "~/assets/js/viewjs/siteapp/user/adduser.js",
                "~/assets/js/viewjs/siteapp/user/manageuser.js",
                "~/assets/js/viewjs/siteapp/user/addcustomer.js",
                "~/assets/js/viewjs/siteapp/user/managecustomer.js",
                "~/assets/js/viewjs/siteapp/user/usdmaster.js",
                "~/assets/js/viewjs/siteapp/product/manageproduct.js",
                "~/assets/js/viewjs/siteapp/product/addproduct.js",
                "~/assets/js/viewjs/siteapp/product/managesample.js",
                "~/assets/js/viewjs/siteapp/product/addsample.js",
                "~/assets/js/viewjs/siteapp/product/managequote.js",
                "~/assets/js/viewjs/siteapp/product/addquote.js",
                "~/assets/js/viewjs/siteapp/product/addsample.js",
                "~/assets/js/viewjs/siteapp/product/addrawmaterial.js",
                "~/assets/js/viewjs/siteapp/product/managerawmaterial.js"
                            ));

            bundles.Add(new StyleBundle("~/sitelayout/css").Include(
                "~/assets/css/bootstrap.css",
                "~/assets/css/bootstrap-dialog.css",
                "~/assets/css/font-awesome.css",
                "~/assets/css/ionicons.css",
                "~/assets/css/style.css",
                "~/assets/css/jquery.jqChart.css",
                "~/assets/css/jquery.jqRangeSlider.css",
                "~/assets/css/jquery-ui-1.10.4.css",
                 "~/assets/library/token_input/styles/token-input.css",
                "~/assets/css/jquery.webui-popover.css",
                "~/assets/css/toaster.css",
                "~/assets/css/build.css",
                "~/assets/css/custom.css"
                ));

            #endregion
        }
    }
}