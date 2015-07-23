using System;
using System.Collections.Generic;
using System.Text;
using System.Linq.Expressions;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;

namespace GuildfordBoroughCouncil.Web.Mvc.Html
{
    /// <summary>
    /// Represents support for help text in an ASP.NET MVC view.
    /// </summary>
    public static class LabelExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="html">The HTML helper instance that this method extends.</param>
        /// <param name="expression">An expression that identifies the property to display.</param>
        /// <param name="labelText">The label text.</param>
        /// <param name="helpText">The help text.</param>
        /// <returns>An HTML span element containing any text specified in the AdditionalMetaData attribute of the property that is represented by the expression.</returns>
        public static MvcHtmlString LabelWithHelpFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, string labelText = null, string helpText = null, object htmlAttributes = null)
        {
            var label = html.LabelFor(expression, labelText);

            var span = html.HelpFor(expression, helpText, htmlAttributes);

            return MvcHtmlString.Create(label.ToString() + span.ToString());
        }
    }
}
