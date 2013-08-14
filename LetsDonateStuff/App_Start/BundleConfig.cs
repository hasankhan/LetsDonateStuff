using System.Web;
using System.Web.Optimization;

namespace LetsDonateStuff
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/js").Include(
                         "~/Scripts/jquery-{version}.js",
                         "~/Scripts/jquery-ui-{version}.js",
                         "~/Scripts/jquery.unobtrusive*",
                         "~/Scripts/jquery.validate*",
                         "~/Scripts/jquery.scrollintoview.js",
                         "~/Scripts/bootstrap.js",
                         "~/Scripts/tinynav.js",
                         "~/Scripts/thickbox.js",
                         "~/Scripts/site.js",
                         "~/Scripts/openid-jquery.js",
                         "~/Scripts/openid-en.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                        "~/Content/Default.css",
                        "~/Content/thickbox.css",
                        "~/Content/bootstrap.css",
                        "~/Content/bootstrap-responsive.css",
                        "~/Content/Site.css",
                        "~/Content/Site.custom.css",
                        "~/Content/ie.css",
                        "~/Content/openid.css",
                        "~/Content/openid-shadow.css"));
            
            bundles.Add(new StyleBundle("~/Content/themes/base/css").Include(
                        "~/Content/themes/base/jquery.ui.core.css",
                        "~/Content/themes/base/jquery.ui.resizable.css",
                        "~/Content/themes/base/jquery.ui.selectable.css",
                        "~/Content/themes/base/jquery.ui.accordion.css",
                        "~/Content/themes/base/jquery.ui.autocomplete.css",
                        "~/Content/themes/base/jquery.ui.button.css",
                        "~/Content/themes/base/jquery.ui.dialog.css",
                        "~/Content/themes/base/jquery.ui.slider.css",
                        "~/Content/themes/base/jquery.ui.tabs.css",
                        "~/Content/themes/base/jquery.ui.datepicker.css",
                        "~/Content/themes/base/jquery.ui.progressbar.css",
                        "~/Content/themes/base/jquery.ui.theme.css"));
        }
    }
}