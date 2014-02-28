using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

using Staad.Domain.Abstract;
using Staad.Domain.Entities;

namespace Staad.Web.Handlers
{
    public class DictionaryHandler : IHttpHandler
    {
        private readonly IDictionaryService dictionaryService;

        private JavaScriptSerializer javaScriptSerializer;

        public DictionaryHandler()
        {
            dictionaryService = DependencyResolver.Current.GetService<IDictionaryService>();
        }

        private int[] ParseIntArray(string jsonArr)
        {
            if (string.IsNullOrEmpty(jsonArr))
            {
                return new int[0];
            }
            javaScriptSerializer = new JavaScriptSerializer();
            var strArray = javaScriptSerializer.Deserialize<string[]>(jsonArr);

            return strArray.Select(s => Convert.ToInt32(s)).ToArray();
        }

        private IEnumerable<Word> Parse(string jsonArr)
        {
            return new JavaScriptSerializer().Deserialize<List<Word>>(jsonArr);
        }

        public void ProcessRequest(HttpContext context)
        {
            try
            {
                var entity = context.Request["entity"];
                switch (entity)
                {
                    case "word":
                        ProcessWords(context);
                        break;
                    case "dict":
                        ProcessDictionaries(context);
                        break;
                }

                context.Response.StatusCode = (int)HttpStatusCode.OK;
            }
            catch (InvalidOperationException)
            {
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            }
        }

        private void ProcessDictionaries(HttpContext context)
        {
            switch (context.Request["action"])
            {
                case "delete":
                    DeleteDictionaries(context);
                    break;
            }
        }

        private void DeleteDictionaries(HttpContext context)
        {
            var jsonArr = context.Request["ids"];
            var list = ParseIntArray(jsonArr);
            dictionaryService.DeleteDictionaries(list);
        }

        private void ProcessWords(HttpContext context)
        {
            switch (context.Request["action"])
            {
                case "save":
                    SaveWords(context);
                    break;
                case "delete":
                    DeleteWords(context);
                    break;
            }
        }

        private void SaveWords(HttpContext context)
        {
            var jsonArr = context.Request["words"];
            var indexesRaw = context.Request["theirIndexes"]; //check
            var list = Parse(jsonArr).ToList();
            dictionaryService.SaveWords(list);

            var responseList = new List<object>();
            var index = ParseIntArray(indexesRaw);
            foreach (var i in index)
            {
                responseList.Add(new
                    {
                        index = i,
                        list[i].Id
                    });
            }

            context.Response.Clear();
            context.Response.ContentType = "text/plain";
            context.Response.Write(javaScriptSerializer.Serialize(responseList));
        }

        private void DeleteWords(HttpContext context)
        {
            var jsonArr = context.Request["ids"];
            var list = ParseIntArray(jsonArr);
            dictionaryService.DeleteWords(list);
        }

        public bool IsReusable
        {
            get { return true; }
        }
    }
}