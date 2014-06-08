using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Security.Policy;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

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

        public static MvcHtmlString Button(this HtmlHelper html, string buttonCaption, string actionName, string controllerName, object routeValues, object htmlAttributes)
        {
            if (string.IsNullOrEmpty(buttonCaption))
            {
                throw new ArgumentNullException("buttonCaption");
            }

            var rv = new RouteValueDictionary(routeValues);
            var htmlAttrMap = (IDictionary<string, object>)HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);

            var urlHelper = new UrlHelper(html.ViewContext.RequestContext);
            var linkUrl = urlHelper.Action(actionName, controllerName, rv);

            var tagBuilder = new TagBuilder("button")
                {
                    InnerHtml = !string.IsNullOrEmpty(buttonCaption) ? HttpUtility.HtmlEncode(buttonCaption) : string.Empty
                };
            tagBuilder.MergeAttributes(htmlAttrMap);
            tagBuilder.MergeAttribute("href", linkUrl);

            var btnSource = tagBuilder.ToString(TagRenderMode.Normal);
            return MvcHtmlString.Create(btnSource);
        }
    }
}