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

            //sb.AppendLine(@"<ul class=""list-unstyled radiolist"">");

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

                    var wrapper = new TagBuilder("div");
                    wrapper.AddCssClass("radio");

                    var label = new TagBuilder("label");
                    label.Attributes["for"] = id;

                    var radioAttributes = new Dictionary<string, object>();

                    radioAttributes.Add("id", id);

                    if (item.Selected)
                    {
                        radioAttributes.Add("selected", "selected");
                    }

                    if (item.Disabled)
                    {
                        radioAttributes.Add("disabled", "disabled");
                        wrapper.AddCssClass("disabled");
                    }

                    label.InnerHtml = htmlHelper.RadioButtonFor(expression, item.Value ?? HttpUtility.HtmlEncode(item.Text), radioAttributes).ToHtmlString() + " " + HttpUtility.HtmlEncode(item.Text);

                    wrapper.InnerHtml = label.ToString(TagRenderMode.Normal);

                    sb.Append(wrapper.ToString(TagRenderMode.Normal));
                }
            }

            if (!String.IsNullOrWhiteSpace(lastLabel))
            {
                sb.AppendFormat(@"<li class=""radiolist-label radiolist-label-last"">{0}</li>", lastLabel);
            }

            //sb.AppendLine("</ul>");

            return MvcHtmlString.Create(sb.ToString());
        }

        public static MvcHtmlString EnumRadioButtonListFor<TModel, TEnum>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TEnum>> expression, string firstLabel = "", string lastLabel = "", object htmlAttributes = null)
        {
            if (expression == null)
            {
                throw new ArgumentNullException("expression");
            }

            ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            if (metadata == null)
            {
                throw new ArgumentException("expression", "No metadata for " + expression.ToString());
            }

            if (metadata.ModelType == null)
            {
                throw new ArgumentException("expression", "No model type for " + expression.ToString());
            }

            if (!System.Web.Mvc.Html.EnumHelper.IsValidForEnumHelper(metadata.ModelType))
            {
                string formatString;
                if (GuildfordBoroughCouncil.Web.Mvc.Html.EnumHelper.HasFlags(metadata.ModelType))
                {
                    formatString = "Return type '{0}' is not supported. Type must not have a '{1}' attribute.";
                }
                else
                {
                    formatString = "Return type '{0}' is not supported.";
                }

                throw new ArgumentException("expression", string.Format(formatString, metadata.ModelType.FullName, "Flags" ));
            }

            // Run through same processing as SelectInternal() to determine selected value and ensure it is included
            // in the select list.
            string expressionName = System.Web.Mvc.ExpressionHelper.GetExpressionText(expression);
            string expressionFullName = htmlHelper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(expressionName);
            Enum currentValue = null;
            if (!String.IsNullOrEmpty(expressionFullName))
            {
                currentValue = htmlHelper.GetModelStateValue(expressionFullName, metadata.ModelType) as Enum;
            }

            if (currentValue == null && !String.IsNullOrEmpty(expressionName))
            {
                // Ignore any select list (enumerable with this name) in the view data
                currentValue = htmlHelper.ViewData.Eval(expressionName) as Enum;
            }

            if (currentValue == null)
            {
                currentValue = metadata.Model as Enum;
            }

            IList<SelectListItem> selectList = System.Web.Mvc.Html.EnumHelper.GetSelectList(metadata.ModelType, currentValue);
            //if (!String.IsNullOrEmpty(optionLabel) && selectList.Count != 0 && String.IsNullOrEmpty(selectList[0].Text))
            //{
            //    // Were given an optionLabel and the select list has a blank initial slot.  Combine.
            //    selectList[0].Text = optionLabel;

            //    // Use the option label just once; don't pass it down the lower-level helpers.
            //    optionLabel = null;
            //}

            //return DropDownListHelper(htmlHelper, metadata, expressionName, selectList, optionLabel, htmlAttributes);

            return RadioButtonForSelectList(htmlHelper, expression, selectList, firstLabel, lastLabel);
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
