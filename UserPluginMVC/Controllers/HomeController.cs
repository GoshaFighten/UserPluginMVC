using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace UserPluginMVC.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetPlugin(string name) {
            var virtualPath = "~/PluginFiles/" + name;
            if (!System.IO.File.Exists(Server.MapPath(virtualPath))) {
                ViewEngineResult view = ViewEngines.Engines.FindView(this.ControllerContext, "MyPlugin", string.Empty);
                if (view.View == null)
                    return new EmptyResult();
                string htmlTextView = GetViewToString(this.ControllerContext, view, null);
                byte[] toBytes = Encoding.Default.GetBytes(htmlTextView);
                System.IO.File.WriteAllBytes(Server.MapPath(virtualPath), toBytes);
            }
            return File(virtualPath, "text/plain", name);
        }

        private string GetViewToString(ControllerContext context, ViewEngineResult result, object model) {
            string viewResult = string.Empty;
            var viewData = ViewData;
            viewData.Model = model;
            TempDataDictionary tempData = new TempDataDictionary();
            StringBuilder sb = new StringBuilder();
            using (StringWriter sw = new StringWriter(sb)) {
                using (HtmlTextWriter output = new HtmlTextWriter(sw)) {
                    ViewContext viewContext = new ViewContext(context, result.View, viewData, tempData, output);
                    result.View.Render(viewContext, output);
                }
                viewResult = sb.ToString();
            }
            return viewResult;
        }
    }
}