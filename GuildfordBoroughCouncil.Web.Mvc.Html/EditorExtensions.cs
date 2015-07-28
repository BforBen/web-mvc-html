using System;
using System.Linq.Expressions;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace GuildfordBoroughCouncil.Web.Mvc.Html
{
    public static partial class EditorExtensions
    {
        private static bool HasError(this HtmlHelper htmlHelper, ModelMetadata modelMetadata, string expression)
        {
            string modelName = htmlHelper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(expression);
            FormContext formContext = htmlHelper.ViewContext.FormContext;
            if (formContext == null)
                return false;

            if (!htmlHelper.ViewData.ModelState.ContainsKey(modelName))
                return false;

            ModelState modelState = htmlHelper.ViewData.ModelState[modelName];
            if (modelState == null)
                return false;

            ModelErrorCollection modelErrors = modelState.Errors;
            if (modelErrors == null)
                return false;

            return (modelErrors.Count > 0);
        }

        public static MvcHtmlString ValidationErrorFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, string error)
        {
            if (HasError(htmlHelper, ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData), ExpressionHelper.GetExpressionText(expression)))
            {
                return new MvcHtmlString(error);
            }
            else
            {
                return null;
            }
        }

        public static MvcHtmlString ValidationError(this HtmlHelper htmlHelper, string modelName, string error)
        {
            if (HasError(htmlHelper, ModelMetadata.FromStringExpression(modelName, htmlHelper.ViewData), modelName))
            {
                return new MvcHtmlString(error);
            }
            else
            {
                return null;
            }
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
        public static MvcHtmlString EditorWithHelpFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, string helpText = null, object htmlAttributes = null)
        {
            var editor = html.EditorFor(expression);

            var span = html.HelpFor(expression, helpText, htmlAttributes);

            return MvcHtmlString.Create(editor.ToString() + span.ToString());
        }
    }
}
