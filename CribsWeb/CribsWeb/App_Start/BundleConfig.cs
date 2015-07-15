using System.Web;
using System.Web.Optimization;

namespace Cribs.Web
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));
            
            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/SignalR").Include("~/Scripts/jquery.signalR-2.2.0.min.js"));
          
            bundles.Add(new Bundle("~/bundles/materialize").Include("~/Scripts/materialize/materialize.js"));

          
            bundles.Add(new StyleBundle("~/Content/css/").Include("~/Content/materialize/css/materialize.css"));
           
        }
    }
}
