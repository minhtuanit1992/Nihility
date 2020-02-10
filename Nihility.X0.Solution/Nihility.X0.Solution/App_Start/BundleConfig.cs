using System.Web;
using System.Web.Optimization;

namespace Nihility.X0.Solution
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            // Gói dữ liệu Style Sheet và Script dành riêng cho Global
            bundles.Add(new StyleBundle("~/Content/css").Include(
                        "~/Content/Vendor/Reset.css",
                        "~/Content/bootstrap.css",
                        "~/Content/Vendor/animate.min.css",
                        "~/Content/Vendor/datatables.min.css"
                        ));

            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"
                        ));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                        "~/Scripts/bootstrap.js"
                        ));

            // Sử dụng phiên bản phát triển của Modernizr để phát triển và học hỏi.
            // Sau khi, bạn đã sẵn sàng để sản xuất, sử dụng công cụ xây dựng tại http://modernizr.com để chỉ chọn các thử nghiệm bạn cần.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            // Gói dữ liệu Style Sheet dành riêng cho Main Page
            bundles.Add(new StyleBundle("~/Content/main").Include(
                        "~/Content/Css/MainStyle.css",
                        "~/Content/Vendor/pretty-checkbox.css"
                        ));

            // Gói dữ liệu Javascript dành riêng cho Main Page
            bundles.Add(new ScriptBundle("~/bundles/main").Include(

                        ));

            // Gói dữ liệu Style Sheet dành riêng cho Admin Page
            bundles.Add(new StyleBundle("~/Content/admin").Include(
                         "~/Areas/Administrator/Content/Css/hyper.css",
                         "~/Areas/Administrator/Content/Css/admin-style.css"
                        ));

            // Gói dữ liệu Javascript dành riêng cho Admin Page
            bundles.Add(new ScriptBundle("~/bundles/admin").Include(
                         "~/Areas/Administrator/Scripts/Hyper/vendor.js",
                         "~/Areas/Administrator/Scripts/Hyper/app.js"
                        ));
        }
    }
}
