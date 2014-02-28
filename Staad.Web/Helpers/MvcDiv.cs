using System;
using System.IO;
using System.Web.Mvc;

namespace Staad.Web.Helpers
{
    public class MvcDiv : IDisposable
    {
        private bool isDisposed;

        private readonly string clientId;

        private TextWriter _writer;

        public MvcDiv(ViewContext viewContext, string clientId)
        {
            if (viewContext == null)
                throw new ArgumentNullException("viewContext");
            this.clientId = clientId;
            this._writer = viewContext.Writer;
            viewContext.FormContext = new FormContext();
        }

        public string ClientId
        {
            get
            {
                return this.clientId;
            }
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize((object)this);
        }

        /// <summary>
        /// Releases unmanaged and, optionally, managed resources used by the 
        /// current instance of the <see cref="T:Staad.Web.Helpers.MvcDiv"/> class.
        /// </summary>
        /// <param name="disposing">true to release both managed and unmanaged resources; 
        /// false to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (isDisposed)
            {
                return;
            }

            isDisposed = true;
            _writer.Write("</div>");
        }
    }
}