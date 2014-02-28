namespace Staad.Web.Handlers
{
    using System;
    using System.Net;
    using System.Web;
    using System.Web.Mvc;

    using Staad.Domain.Abstract;
    using Staad.Domain.Entities;
    using Staad.Web.Helpers;

    public class WordHandler : IHttpHandler
    {
        private readonly IWordRepository wordRepository;

        private readonly IDictionaryRepository dictionaryRepository;

        private readonly IDictionaryService dictionaryService;

        public WordHandler()
        {
            wordRepository = DependencyResolver.Current.GetService<IWordRepository>();
            dictionaryRepository = DependencyResolver.Current.GetService<IDictionaryRepository>();
            dictionaryService = DependencyResolver.Current.GetService<IDictionaryService>();
        }

        public void ProcessRequest(HttpContext context)
        {
            var action = context.Request["action"];
            if (action == "save")
            {
                var validationReport = CheckWord(context);
                if (!string.IsNullOrEmpty(validationReport))
                {
                    MakeErrorResponse(context, validationReport);
                    return;
                }

                try
                {
                    var wordId = GetWordId(context);
                    if (wordId > 0)
                    {
                        SaveWord(context);
                    }
                    else
                    {
                        wordId = AddNewWord(context);
                    }

                    var updateResult = new WordUpdateResult { WordId = wordId };

                    context.Response.ContentType = "text/plain";
                    context.Response.Write(updateResult.ToJson());
                }
                catch (Exception exc)
                {
                    MakeErrorResponse(context, exc.Message);
                }
            }
        }

        private string CheckWord(HttpContext context)
        {
            var originalWord = GetOriginalWord(context);
            var definition = GetDefinition(context);

            var result = string.Empty;
            if (string.IsNullOrEmpty(originalWord))
            {
                result += "Original word must not be empty. ";
            }
            if(string.IsNullOrEmpty(definition))
            {
                result += "Definition must not be empty. ";
            }
            return result;
        }

        private void SaveWord(HttpContext context)
        {
            var word = wordRepository.Read(GetWordId(context));
            
            word.Original = GetOriginalWord(context);
            word.Definition = GetDefinition(context);
            word.Example = GetExample(context);
            
            wordRepository.Update(word);
        }

        private int AddNewWord(HttpContext context)
        {
            var word = new Word
                {
                    Original = GetOriginalWord(context),
                    Definition = GetDefinition(context),
                    Example = GetExample(context)
                };

            var dictionary = dictionaryRepository.Read(GetDictionaryId(context));

            dictionaryService.AddWordToDictionary(word, dictionary);

            return word.Id;
        }

        private int GetDictionaryId(HttpContext context)
        {
            var dictionaryId = context.Request["dictId"];
            var id = Convert.ToInt32(dictionaryId);
            return id;
        }

        private string GetExample(HttpContext context)
        {
            return context.Request["example"].Trim();
        }

        private static int GetWordId(HttpContext context)
        {
            string wordId = context.Request["wordId"];
            var id = Convert.ToInt32(wordId);
            return id;
        }

        private static string GetDefinition(HttpContext context)
        {
            return context.Request["definition"].Trim();
        }

        private static string GetOriginalWord(HttpContext context)
        {
            return context.Request["original"].Trim();
        }

        private void MakeErrorResponse(HttpContext context, string errorText)
        {
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            context.Response.ContentType = "text/plain";
            context.Response.Write(new WordUpdateResult { ErrorMsg = errorText }.ToJson());

            context.Response.End();
        }

        public bool IsReusable
        {
            get
            {
                return true;
            }
        }
    }
}