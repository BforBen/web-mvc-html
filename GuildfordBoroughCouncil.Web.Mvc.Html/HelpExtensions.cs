using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Web.Mvc;
using System.Web.Routing;

namespace GuildfordBoroughCouncil.Web.Mvc.Html
{
    /// <summary>
    /// Represents support for help text in an ASP.NET MVC view.
    /// </summary>
    public static class HelpExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="html">The HTML helper instance that this method extends.</param>
        /// <param name="expression">An expression that identifies the property to display.</param>
        /// <param name="helpText">The help text.</param>
        /// <returns>An HTML span element containing any text specified in the AdditionalMetaData attribute of the property that is represented by the expression.</returns>
        public static MvcHtmlString Help(this HtmlHelper html, string modelName, string helpText = null, object htmlAttributes = null)
        {
            var span = new TagBuilder("span");

            if (!String.IsNullOrWhiteSpace(helpText))
            {
                span.SetInnerText(helpText);
                //span.Attributes.Add(new KeyValuePair<string, string>("title", helpText));
            }
            else
            {
                var MetaData = ModelMetadata.FromStringExpression(modelName, html.ViewData);

                if (MetaData.AdditionalValues.ContainsKey("help"))
                {
                    //span.Attributes.Add(new KeyValuePair<string, string>("title", MetaData.AdditionalValues["help"].ToString()));
                    span.SetInnerText(MetaData.AdditionalValues["help"].ToString());
                }
                else
                {
                    return null;
                }
            }

            var attributes = new RouteValueDictionary(htmlAttributes);

            //if (span.InnerHtml.Length > 50)
            //{
                span.Attributes.Add(new KeyValuePair<string, string>("class", "help-block"));
            /*}
            else
            {
                span.Attributes.Add(new KeyValuePair<string, string>("class", "help-inline"));
            }*/

            span.MergeAttributes(attributes);

            return new MvcHtmlString(span.ToString(TagRenderMode.Normal));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="html">The HTML helper instance that this method extends.</param>
        /// <param name="expression">An expression that identifies the property to display.</param>
        /// <param name="helpText">The help text.</param>
        /// <returns>An HTML span element containing any text specified in the AdditionalMetaData attribute of the property that is represented by the expression.</returns>
        public static MvcHtmlString HelpFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, string helpText = null, object htmlAttributes = null)
        {
            var span = new TagBuilder("span");

            if (!String.IsNullOrWhiteSpace(helpText))
            {
                span.SetInnerText(helpText);
                //span.Attributes.Add(new KeyValuePair<string, string>("title", helpText));
            }
            else
            {
                var MetaData = ModelMetadata.FromLambdaExpression(expression, html.ViewData);

                if (MetaData.AdditionalValues.ContainsKey("help"))
                {
                    //span.Attributes.Add(new KeyValuePair<string, string>("title", MetaData.AdditionalValues["help"].ToString()));
                    span.SetInnerText(MetaData.AdditionalValues["help"].ToString());
                }
                else
                {
                    return null;
                }
            }

            var attributes = new RouteValueDictionary(htmlAttributes);

            //if (span.InnerHtml.Length > 50)
            //{
                span.Attributes.Add(new KeyValuePair<string, string>("class", "help-block"));
            /*}
            else
            {
                span.Attributes.Add(new KeyValuePair<string, string>("class", "help-inline"));
            }*/

            span.MergeAttributes(attributes);

            return new MvcHtmlString(span.ToString(TagRenderMode.Normal));
        }
    }
}
