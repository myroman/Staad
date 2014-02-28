using System;
using System.Linq.Expressions;
using System.Web.Mvc;

namespace Staad.Web.Helpers
{
    public static class HtmlExtensions
    {
        public static MvcHtmlString FieldIdFor<TModel, TValue>(
            this HtmlHelper<TModel> html,
            Expression<Func<TModel, TValue>> expression)
        {
            var htmlFieldName = ExpressionHelper.GetExpressionText(expression);
            var inputFieldId = html.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldId(htmlFieldName);
            return MvcHtmlString.Create(inputFieldId);
        }

        public static MvcDiv BeginDiv(
            this HtmlHelper html,
            string id, 
            string @class)
        {
            var builder = new TagBuilder("div");

            var guid = Guid.NewGuid();
            builder.GenerateId(id + "_" + guid.ToString("N"));
            builder.MergeAttribute("class", @class);

            html.ViewContext.Writer.Write(builder.ToString(TagRenderMode.StartTag));

            var mvcDiv = new MvcDiv(html.ViewContext, builder.Attributes["id"]);
            return mvcDiv;
        }
    }
}