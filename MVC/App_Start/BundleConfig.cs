using System.Web;
using System.Web.Optimization;

namespace MVC
{
    public class BundleConfig
    {
        // Para obtener más información sobre las uniones, visite https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/Js/Inicial").Include(
                "~/Scripts/jquery-{version}.js",
                "~/Scripts/bootstrap.min.js",
                "~/Scripts/modernizr-*",
                "~/Scripts/jquery.validate.min.js",
                "~/Scripts/Utilidades/Formatos.js",
                "~/Scripts/Utilidades/Util.js",
                "~/Scripts/bootbox.min.js",
                "~/Scripts/jquery.inputmask.bundle.min.js",
                "~/Scripts/moment-with-locales.min.js",
                "~/Scripts/jquery-ui-{version}.js"));
            bundles.Add(new StyleBundle("~/bundles/Css/Inicial").Include(
                "~/Content/bootstrap.min.css",
                "~/Content/site.css"));
            bundles.Add(new ScriptBundle("~/bundles/Js/DatePicker").Include(
                "~/Scripts/bootstrap-datepicker.js",
                "~/Scripts/Utilidades/Fechas.js"));
            bundles.Add(new StyleBundle("~/bundles/Css/DatePicker").Include(
                "~/Content/bootstrap-datepicker.css"));
            bundles.Add(new StyleBundle("~/bundles/Css/DataTables").Include(
                "~/Scripts/DataTables/datatables.css",
                "~/Scripts/DataTables/responsive.dataTables.min.css"));
            bundles.Add(new ScriptBundle("~/bundles/Js/DataTables").Include(
                "~/Scripts/DataTables/datatables.js",
                "~/Scripts/DataTables/dataTables.responsive.min.js"));
            bundles.Add(new ScriptBundle("~/bundles/Js/QR").Include(
                "~/Scripts/QRCode/qrcode.js",
                "~/Scripts/QRCode/qrcode.min.js"));
            bundles.Add(new ScriptBundle("~/bundles/Js/PDF").Include(
                "~/Scripts/DataTables/pdfmake-0.1.32/pdfmake.min.js",
                "~/Scripts/DataTables/pdfmake-0.1.32/vfs_fonts.js",
                "~/Scripts/DataTables/pdfmake-0.1.32/Reportes.js"));
        }
    }
}
