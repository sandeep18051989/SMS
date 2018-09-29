using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;

namespace SMS.App_Start
{
	public class BundleConfig
	{
		public static void RegisterBundles(BundleCollection bundles)
		{
			bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
				 "~/Scripts/jquery-{version}.js",
				 "~/Scripts/jquery-migrate-1.2.1.min.js"));

			bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
				 "~/Scripts/jquery-ui-{version}.js"));

			bundles.Add(new ScriptBundle("~/Scripts/jqueryval").Include(
				 "~/Scripts/jquery.validate*"));

			bundles.Add(new ScriptBundle("~/Scripts/pace").Include(
							"~/Areas/Admin/Scripts/pace.min.js"));

			bundles.Add(new ScriptBundle("~/Scripts/boot").Include(
												  "~/Scripts/umd/popper.js",
												  "~/Scripts/bootstrap/bootstrap.js"));

			bundles.Add(new ScriptBundle("~/Scripts/datatable").Include(
				 "~/Scripts/DataTables/jquery.dataTables.min.js",
				"~/Scripts/DataTables/dataTables.bootstrap4.min.js"));

			bundles.Add(new ScriptBundle("~/Scripts/adminaddon").Include(
				 "~/Areas/Admin/Scripts/slim-scroll.min.js",
				 "~/Areas/Admin/Scripts/raphel.min.js",
				 "~/Areas/Admin/Scripts/morris.js",
				 "~/Areas/Admin/Scripts/jquery-jvectormap.min.js",
				 "~/Areas/Admin/Scripts/jquery-jvectormap-world-merc-en.js",
				 "~/Areas/Admin/Scripts/jquery.gritter.js",
				 "~/Areas/Admin/Scripts/apps.min.js",
				 "~/Areas/Admin/Scripts/dashboard.min.js",
				 "~/Areas/Admin/Scripts/common.Pictures.js",
				 "~/Areas/Admin/Scripts/public.common.js"));

			bundles.Add(new ScriptBundle("~/Scripts/datetimepicker").Include(
				 "~/Scripts/moment.min.js",
				 "~/Scripts/bootstrap-datetimepicker.min.js"));

			bundles.Add(new ScriptBundle("~/Scripts/home").Include(
				 "~/Scripts/public.home.js"));

			bundles.Add(new ScriptBundle("~/Scripts/dzone").Include(
				 "~/Scripts/dropzone/dropzone.min.js"));

			bundles.Add(new ScriptBundle("~/Scripts/mdb").Include(
				 "~/Scripts/mdb.min.js"));

			bundles.Add(new ScriptBundle("~/Scripts/JqGrid").Include(
				  "~/Scripts/i18n/grid.locale-en.js",
				  "~/Scripts/jquery.jqGrid.min.js"));

			bundles.Add(new StyleBundle("~/Content/jqueryuicss").Include(
			 "~/Content/themes/base/jquery-ui.css"));

			bundles.Add(new StyleBundle("~/Content/boot").Include(
				 "~/Content/bootstrap/bootstrap.min.css"));

			bundles.Add(new StyleBundle("~/Content/datatable").Include(
				"~/Content/DataTables/css/dataTables.bootstrap4.min.css",
				"~/Content/DataTables/css/responsive.dataTables.min.css"));

			bundles.Add(new StyleBundle("~/Content/mdb").Include(
				"~/Content/mdb.min.css"));

			bundles.Add(new StyleBundle("~/Content/icons").Include(
				 "~/Content/font-awesome.min.css"));

			bundles.Add(new StyleBundle("~/Content/fo").Include(
				 "~/Content/font_styles.css"));

			bundles.Add(new StyleBundle("~/Content/common").Include(
				 "~/Content/site.css"));

			bundles.Add(new StyleBundle("~/Content/datetimepicker").Include(
				 "~/Content/bootstrap-datepicker3.min.css"));
			bundles.Add(new StyleBundle("~/Content/htmleditor").Include(
				 "~/Content/bootstrap-htmleditor.css.css"));

			bundles.Add(new StyleBundle("~/Content/adminaddon").Include(
				 "~/Areas/Admin/Content/morris.css"));

			bundles.Add(new StyleBundle("~/Content/dzone").Include(
				 "~/Scripts/dropzone/dropzone.min.css"));

			bundles.Add(new StyleBundle("~/Content/JqGrid").Include(
				  "~/Content/jquery.jqGrid/ui.jqgrid.css"));

			bundles.Add(new StyleBundle("~/Areas/Admin/Content/AdminStyles").Include(
			 "~/Areas/Admin/Content/AdminStyles.css"));

			bundles.Add(new ScriptBundle("~/Scripts/addon").Include(
					 "~/Scripts/steller.min.js",
					 "~/Scripts/waypoints.min.js",
					 "~/Scripts/counter.min.js",
					 "~/Scripts/colortoggle.min.js"));
		}
	}
}