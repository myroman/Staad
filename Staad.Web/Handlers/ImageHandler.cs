namespace Staad.Web.Handlers
{
    using System.Web;

    public class ImageHandler : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
        }

        public bool IsReusable
        {
            get { return true; }
        }
    }
}