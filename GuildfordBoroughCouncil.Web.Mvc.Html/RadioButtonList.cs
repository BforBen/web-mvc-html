using System;
using System.Collections.Generic;
using System.Text;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Text.RegularExpressions;
using GuildfordBoroughCouncil.Linq;
using System.Dynamic;

namespace GuildfordBoroughCouncil.Web.Mvc.Html
{
    public static class RadioButtonExtensions
    {

        /// <summary>
        /// Source: http://jonlanceley.blogspot.com/2011/06/mvc3-radiobuttonlist-helper.html
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="htmlHelper"></param>
        /// <param name="expression"></param>
        /// <param name="listOfValues"></param>
        /// <returns></returns>
        public static MvcHtmlString RadioButtonSelectList(
            this HtmlHelper htmlHelper,
            string name,
            IEnumerable<SelectListItem> listOfValues,
            string firstLabel = "", string lastLabel = "", string elementDiscrimator = "", object htmlAttributes = null, object selectedValue = null)
        {
            //var metaData = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            string SelectedValue = (selectedValue != null) ? selectedValue.ToString() : null;

            var sb = new StringBuilder();

            sb.AppendLine(@"<ul class=""list-unstyled radiolist"">");

            if (!String.IsNullOrWhiteSpace(firstLabel))
            {
                sb.AppendFormat(@"<li class=""radiolist-label radiolist-label-first"">{0}</li>", firstLabel);
            }

            if (listOfValues != null)
            {
                string IdName = elementDiscrimator + (!Regex.IsMatch(name, "^[a-zA-Z]") ? "A" + name : name);

                foreach (SelectListItem item in listOfValues)
                {
                    var IdDescriminator = System.Text.RegularExpressions.Regex.Replace((item.Value ?? item.Text), @"[^\w]", "_");

                    var id = string.Format("{0}_{1}", IdName, IdDescriminator);

                    var label = htmlHelper.Label(id, HttpUtility.HtmlEncode(item.Text));
                    string value = item.Value ?? HttpUtility.HtmlEncode(item.Text);


                    var dictAttributes = htmlAttributes.ToDictionary();

                    var result = new ExpandoObject();
                    var d = result as IDictionary<string, object>; //work with the Expando as a Dictionary

                    if(dictAttributes != null)
                    {
                        foreach (var pair in dictAttributes)
                        {
                            d[pair.Key] = pair.Value;
                        }
                    }

                    d.Add(new KeyValuePair<string,object>("Id", id));

                    var radio = htmlHelper.RadioButton(name, value, (!String.IsNullOrWhiteSpace(SelectedValue)) ? (SelectedValue == value) : false, d).ToHtmlString();

                    sb.AppendFormat("<li>{0}{1}</li>", radio, label);
                }
            }

            if (!String.IsNullOrWhiteSpace(lastLabel))
            {
                sb.AppendFormat(@"<li class=""radiolist-label radiolist-label-last"">{0}</li>", lastLabel);
            }

            sb.AppendLine("</ul>");

            return MvcHtmlString.Create(sb.ToString());
        }

        /// <summary>
        /// Source: http://jonlanceley.blogspot.com/2011/06/mvc3-radiobuttonlist-helper.html
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="htmlHelper"></param>
        /// <param name="expression"></param>
        /// <param name="listOfValues"></param>
        /// <returns></returns>
        public static MvcHtmlString RadioButtonForSelectList<TModel, TProperty>(
            this HtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TProperty>> expression,
            IEnumerable<SelectListItem> listOfValues,
            string firstLabel = "", string lastLabel = "")
        {
            var metaData = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            var sb = new StringBuilder();

            sb.AppendLine(@"<ul class=""list-unstyled radiolist"">");

            if (!String.IsNullOrWhiteSpace(firstLabel))
            {
                sb.AppendFormat(@"<li class=""radiolist-label radiolist-label-first"">{0}</li>", firstLabel);
            }

            if (listOfValues != null)
            {
                string IdName = metaData.PropertyName + ((metaData.Model != null) ? "_" + metaData.Model.ToString() : String.Empty);

                foreach (SelectListItem item in listOfValues)
                {
                    var IdDescriminator = System.Text.RegularExpressions.Regex.Replace((item.Value ?? item.Text), @"[^\w]", "_");

                    var id = string.Format("{0}_{1}", IdName, IdDescriminator);

                    var label = htmlHelper.Label(id, HttpUtility.HtmlEncode(item.Text));
                    var radio = htmlHelper.RadioButtonFor(expression, item.Value ?? HttpUtility.HtmlEncode(item.Text), new { Id = id }).ToHtmlString();

                    sb.AppendFormat("<li>{0}{1}</li>", radio, label);
                }
            }

            if (!String.IsNullOrWhiteSpace(lastLabel))
            {
                sb.AppendFormat(@"<li class=""radiolist-label radiolist-label-last"">{0}</li>", lastLabel);
            }

            sb.AppendLine("</ul>");

            return MvcHtmlString.Create(sb.ToString());
        }

        ///// <summary>
        ///// Based on: http://jonlanceley.blogspot.com/2011/06/mvc3-radiobuttonlist-helper.html
        ///// </summary>
        ///// <typeparam name="TModel"></typeparam>
        ///// <typeparam name="TProperty"></typeparam>
        ///// <param name="htmlHelper"></param>
        ///// <param name="expression"></param>
        ///// <param name="listOfValues"></param>
        ///// <returns></returns>
        //public static MvcHtmlString CheckButtonForSelectList<TModel, TProperty>(
        //    this HtmlHelper<TModel> htmlHelper,
        //    Expression<Func<TModel, TProperty>> expression,
        //    IEnumerable<SelectListItem> listOfValues)
        //{
        //    var metaData = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
        //    var sb = new StringBuilder();

        //    sb.AppendLine(@"<ul>");

        //    if (listOfValues != null)
        //    {
        //        foreach (SelectListItem item in listOfValues)
        //        {
        //            var id = string.Format("{0}_{1}", metaData.PropertyName, item.Value);

        //            var label = htmlHelper.Label(id, HttpUtility.HtmlEncode(item.Text));
        //            var radio = htmlHelper.CheckBoxFor(expression, item.Value, new { id = id }).ToHtmlString();

        //            sb.AppendFormat("<li>{0}{1}</li>", radio, label);
        //        }
        //    }

        //    sb.AppendLine("</ul>");

        //    return MvcHtmlString.Create(sb.ToString());
        //}
    }
}
