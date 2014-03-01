using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

using Staad.Domain.Abstract;
using Staad.Domain.Entities;
using Staad.Web.Models;

namespace Staad.Web.Handlers
{
    public class DictionaryHandler : IHttpHandler
    {
        private readonly IDictionaryService dictionaryService;

        private readonly IDictionaryRepository dictionaryRepository;

        private readonly JavaScriptSerializer javaScriptSerializer;

        public DictionaryHandler()
        {
            dictionaryService = DependencyResolver.Current.GetService<IDictionaryService>();
            dictionaryRepository = DependencyResolver.Current.GetService<IDictionaryRepository>();
            javaScriptSerializer = new JavaScriptSerializer();
        }

        private int[] ParseIntArray(string jsonArr)
        {
            if (string.IsNullOrEmpty(jsonArr))
            {
                return new int[0];
            }
            
            var strArray = javaScriptSerializer.Deserialize<string[]>(jsonArr);

            return strArray.Select(s => Convert.ToInt32(s)).ToArray();
        }

        private IEnumerable<Word> ParseWords(string jsonArr)
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
            catch (Exception exc)
            {
                context.Response.Clear();
                context.Response.Write(exc.Message);
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
            var idsRaw = context.Request["ids"];
            var ids = ParseIntArray(idsRaw);
            dictionaryService.DeleteDictionaries(ids);

            MakeResponse(context, ids);
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
                case "loadmore":
                    FetchMoreWords(context);
                    break;
            }
        }

        private void FetchMoreWords(HttpContext context)
        {
            var offsetRaw = context.Request["offset"];
            int offset;
            if (!int.TryParse(offsetRaw, out offset))
            {
                throw new InvalidOperationException("You should provide 'offset' parameter");
            }
            var dictIdRaw = context.Request["dictId"];
            int dictId;
            if (!int.TryParse(dictIdRaw, out dictId))
            {
                throw new InvalidOperationException("You should provide 'dictId' parameter");
            }

            var dictionary = dictionaryRepository.Read(dictId);
            var query = dictionary.Words.Skip(offset).Select(x => new WordViewModel(x));
            MakeResponse(context, query.ToArray());
        }

        private void SaveWords(HttpContext context)
        {
            var jsonArr = context.Request["words"];
            var words = ParseWords(jsonArr).ToList();
            var indexesRaw = context.Request["theirIndexes"]; // TODO: check
            var indexesInArray = ParseIntArray(indexesRaw);
            
            if (words.Count() != indexesInArray.Length)
            {
                throw new InvalidOperationException("Number of words and their indexes must be equal");
            }

            dictionaryService.SaveWords(words);

            var responseList = new List<object>();
            foreach (var selObj in words.Select((word, i) => new { word, i }))
            {
                responseList.Add(new
                    {
                        index = indexesInArray[selObj.i],
                        selObj.word.Id
                    });
            }

            MakeResponse(context, responseList);
        }

        private void DeleteWords(HttpContext context)
        {
            var idsRaw = context.Request["ids"];
            var idsList = ParseIntArray(idsRaw);
            dictionaryService.DeleteWords(idsList);

            MakeResponse(context, idsList);
        }

        private void MakeResponse(HttpContext context, object responseObject)
        {
            context.Response.Clear();
            context.Response.ContentType = "text/plain";
            context.Response.Write(javaScriptSerializer.Serialize(responseObject));
        }

        public bool IsReusable
        {
            get { return true; }
        }
    }
}