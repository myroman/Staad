using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

using System.Xml.Linq;

namespace Staad.Web.Controllers
{
    public class HomeController : Controller
    {
        public JavaScriptResult UserScripts()
        {
            var scripts = GetScriptsFromConfig();
            return JavaScript(scripts);
        }

        private string GetScriptsFromConfig()
        {
            var path = Server.MapPath("~/ScriptsBundle.xml");
            var doc = XDocument.Load(path);

            var scriptsContent = new StringBuilder();
            var root = doc.Element("scripts");
            if (root != null)
            {
                var scriptElems = root.Elements("add");
                foreach (var scriptElem in scriptElems)
                {
                    scriptsContent.Append(GetScriptContent(scriptElem));
                }
            }

            return scriptsContent.ToString();
        }

        private string GetScriptContent(XElement scriptElem)
        {
            var relPath = scriptElem.Attribute(XName.Get("src"));
            if (relPath == null)
            {
                return string.Empty;
            }
            var pathToScript = Url.Content(relPath.Value);
            
            using (var reader = System.IO.File.OpenText(Server.MapPath(pathToScript)))
            {
                var content = reader.ReadToEnd();
                return content;
            }
        }
    }
}
