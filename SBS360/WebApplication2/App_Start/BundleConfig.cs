using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;

namespace Eng360Web
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js",
                        "~/Scripts/jquery-ui.js",
                         "~/Scripts/MessageBoxs.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                         "~/Scripts/jquery.unobtrusive*",
                         "~/Scripts/jquery.validate*"));

            //        Bundle jQueryValidateBundle = new ScriptBundle("~/bundles/jqueryval")
            //.Include("~/Scripts/jquery.validate*");
            //        jQueryValidateBundle.Orderer = new JQueryValidateBundleOrderer();
            //        bundles.Add(jQueryValidateBundle);


            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/jquery-ui.css"));
                    //  "~/Content/site.css"));
        }
    }

    public class JQueryValidateBundleOrderer : IBundleOrderer
    {
        public IEnumerable<BundleFile> OrderFiles(BundleContext context, IEnumerable<BundleFile> files)
        {
            // Sort files by number of dots in file name
            return files.OrderBy(file => file.VirtualFile.Name.Count(ch => ch == '.'));
        }
    }
}
